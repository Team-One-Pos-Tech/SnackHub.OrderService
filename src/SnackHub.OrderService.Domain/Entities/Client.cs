using System;

namespace SnackHub.OrderService.Domain.Entities;

public record Client(Guid Identifier, string Name, string Email);