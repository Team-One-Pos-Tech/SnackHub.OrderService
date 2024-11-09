using System;

namespace SnackHub.OrderService.Domain.Entities;

public record Product(Guid Id, string Name, decimal Price, string Description);