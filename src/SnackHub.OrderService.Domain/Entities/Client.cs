using System;

namespace SnackHub.OrderService.Domain.Entities;

public class Client
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public string Email { get; set; }

    public static Client Create(Guid id, string name, string email)
        => new Client
        {
            Id = id,
            Name = name,
            Email = email
        };
}