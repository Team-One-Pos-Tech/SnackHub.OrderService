using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MassTransit;
using Moq;
using Reqnroll;
using SnackHub.OrderService.Application.Contracts;
using SnackHub.OrderService.Application.Models.Order;
using SnackHub.OrderService.Application.UseCases;
using SnackHub.OrderService.Behavior.Tests.Fixtures;
using SnackHub.OrderService.Domain.Contracts;
using SnackHub.OrderService.Domain.Entities;
using SnackHub.OrderService.Domain.ValueObjects;
using SnackHub.OrderService.Infra.Repositories.MongoDB;

namespace SnackHub.OrderService.Behavior.Tests.StepDefinition;

[Binding]
public class ConfirmOrderUseCaseStepDefinition : MongoDbFixture
{
    private IClientRepository _clientRepository;
    private IProductRepository _productRepository;
    private IOrderRepository _orderRepository;
    
    private Mock<IPublishEndpoint> _publishEndpointMock;
    
    private IConfirmOrderUseCase _confirmOrderUseCase;
    private ICancelOrderUseCase _cancelOrderUseCase;
    private IListOrderUseCase _listOrderUseCase;
    private ICheckPaymentStatusUseCase _checkPaymentStatusUseCase;
    
    private Guid? _confirmOrderId;
    
    [BeforeScenario]
    public async Task Setup()
    {
        await BaseSetUp();
        
        _publishEndpointMock = new Mock<IPublishEndpoint>();
        
        _productRepository = new ProductRepository(MongoDatabase);
        _clientRepository = new ClientRepository(MongoDatabase);
        _orderRepository = new OrderRepository(MongoDatabase);

        _confirmOrderUseCase = new ConfirmOrderUseCase(_orderRepository, _clientRepository, _productRepository,
            _publishEndpointMock.Object);
        _cancelOrderUseCase = new CancelOrderUseCase(_orderRepository);
        _listOrderUseCase = new ListOrderUseCase(_orderRepository);
        _checkPaymentStatusUseCase = new CheckPaymentStatusUseCase(_orderRepository);
    }

    [Given("a valid client with id '(.*)' and name '(.*)'")]
    public async Task GivenAValidClientWithIdAndName(Guid id, string name)
    {
        var client = Client.Create(id, name, "test@mail.com");
        await _clientRepository.AddAsync(client);
    }

    [Given("a table of products with")]
    public async Task GivenATableOfProductsWith(Table table)
    {
        var products = table.CreateSet<Product>();
        await _productRepository.AddManyAsync(products);
    }

    [When("confirming the order with client id '(.*)' and product details table:")]
    public async Task WhenConfirmingTheOrder(string clientId, Table productDetailsTable)
    {
        var confirmRequestItems = productDetailsTable.CreateSet<Item>();
        var confirmRequest = ConfirmOrderRequest.Create(clientId, confirmRequestItems);
        
        var orderResponse = await _confirmOrderUseCase.Execute(confirmRequest);
        
        orderResponse
            .Should()
            .NotBeNull();
        
        orderResponse
            .OrderId
            .Should()
            .NotBeEmpty();
        
        _confirmOrderId = orderResponse.OrderId;
    }

    [Then("the order status should be '(.*)'")]
    public async Task ThenTheOrderStatusShouldBe(string status)
    {
        Enum.TryParse(status, out OrderStatus expectedStatus);
        
        var orders = await _listOrderUseCase.Execute();
        var order = orders.SingleOrDefault();

        order.Should().NotBeNull();
        order.Status.Should().Be(expectedStatus.ToString());
    }
    
    [When("cancelling the order")]
    public async Task WhenCancellingTheOrder()
    {
        var cancelRequest = new CancelOrderRequest
        {
            OrderId = _confirmOrderId!.Value
        };
        
        await _cancelOrderUseCase.Execute(cancelRequest);
    }

    [Given(@"confirming the order with client id '(.*)' and product details table:")]
    public async Task GivenConfirmingTheOrderWithClientIdAndProductDetailsTable(string clientId, Table productDetailsTable)
    {
        await WhenConfirmingTheOrder(clientId, productDetailsTable);
    }

    [Then(@"the payment status should be '(.*)'")]
    public async Task ThenThePaymentStatusShouldBe(string status)
    {
        Enum.TryParse(status, out OrderStatus expectedStatus);
        
        var checkPaymentStatusRequest = new CheckPaymentStatusRequest()
        {
            OrderId = _confirmOrderId!.Value
        };
        
        var response = await _checkPaymentStatusUseCase.Execute(checkPaymentStatusRequest);
        response
            .PaymentStatus
            .Should()
            .Be(expectedStatus);
    }
}