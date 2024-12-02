using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Reqnroll;
using SnackHub.OrderService.Application.Contracts;
using SnackHub.OrderService.Application.EventConsumers.Payment;
using SnackHub.OrderService.Application.Models.Payment;
using SnackHub.OrderService.Application.UseCases;
using SnackHub.OrderService.Behavior.Tests.Fixtures;
using SnackHub.OrderService.Domain.Contracts;
using SnackHub.OrderService.Domain.Entities;
using SnackHub.OrderService.Domain.ValueObjects;
using SnackHub.OrderService.Infra.Repositories.MongoDB;

namespace SnackHub.OrderService.Behavior.Tests.StepDefinition;

[Binding]
public class PaymentStatusUpdatedConsumerStepDefinition : MongoDbFixture
{
    private IOrderRepository _orderRepository;
    private Mock<IPublishEndpoint> _publishEndpointMock;
    
    private IGetOrderUseCase _getOrderUseCase;
    
    private PaymentApprovedConsumer _paymentApprovedConsumer;
    private PaymentRejectedConsumer _paymentRejectedConsumer;
    
    private Order _order;
    
    [BeforeScenario]
    public async Task Setup()
    {
        await BaseSetUp();
        
        _publishEndpointMock = new Mock<IPublishEndpoint>();
        _orderRepository = new OrderRepository(MongoDatabase);

        var loggerFactory = new NullLoggerFactory();
        
        _getOrderUseCase = new GetOrderUseCase(_orderRepository);
        
        _paymentApprovedConsumer = new PaymentApprovedConsumer(loggerFactory.CreateLogger<PaymentApprovedConsumer>(), _orderRepository, _publishEndpointMock.Object);
        _paymentRejectedConsumer = new PaymentRejectedConsumer(loggerFactory.CreateLogger<PaymentRejectedConsumer>(), _orderRepository);
    }

    [Given("a valid order with product details:")]
    public async Task GivenAValidOrderWithIdAndProductDetails(Table productDetails)
    {
        var products = productDetails.CreateSet<OrderItem>().ToImmutableArray();
        
        _order = Order.Factory.Create(Guid.NewGuid(), products);
        _order.Confirm();

        _order
            .Should()
            .NotBeNull();

        _order
            .Id
            .Should()
            .NotBeEmpty();
        
        await _orderRepository.AddAsync(_order);
    }
    
    [When("receiving an approved update event for that order")]
    public async Task WhenReceivingAnApprovedUpdateEventWithOrderId()
    {
        var approve = new PaymentApproved(_order.Id, Guid.NewGuid());
        var contextMock = new Mock<ConsumeContext<PaymentApproved>>();
        contextMock.Setup(consumeContext => consumeContext.Message).Returns(approve);
        
        await _paymentApprovedConsumer.Consume(contextMock.Object);
    }

    [When("receiving a reject update event for that order")]
    public async Task WhenReceivingARejectUpdateEventWithOrderId()
    {
        var rejected = new PaymentRejected(_order.Id, Guid.NewGuid());
        var contextMock = new Mock<ConsumeContext<PaymentRejected>>();
        contextMock.Setup(consumeContext => consumeContext.Message).Returns(rejected);
        
        await _paymentRejectedConsumer.Consume(contextMock.Object);
    }
    
    [Then("the order should have status '(.*)'")]
    public async Task ThenTheOrderWithIdShouldHaveStatus(string status)
    {
        Enum.TryParse(status, out OrderStatus expectedStatus);
        
        var order = await _getOrderUseCase.Execute(_order.Id);
        order
            .Should()
            .NotBeNull();
        
        order
            .Status
            .Should()
            .Be(expectedStatus.ToString());
    }
}