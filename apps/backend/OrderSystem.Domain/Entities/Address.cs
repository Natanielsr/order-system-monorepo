using System;
using System.Net.Cache;

namespace OrderSystem.Domain.Entities;

public class Address : Entity
{
    // Propriedades principais
    public required string FullName { get; set; }
    public required string Cpf { get; set; }
    public required string Street { get; set; }
    public required string Number { get; set; }
    public string Complement { get; set; } = string.Empty;
    public required string Neighborhood { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string ZipCode { get; set; }
    public required string Country { get; set; } = "Brasil"; // Valor padrão
    public required Guid UserId { get; init; }
    public User? User { get; init; }
    public List<Order>? Orders { get; init; }
    public required bool IsDefault { get; set; }

    // Construtor vazio (importante para serialização/EF)
    protected Address() { }

    // Método útil para exibir o endereço formatado
    public override string ToString()
    {
        return $"{FullName} {Cpf} {Street}, {Number} - {Neighborhood}, {City}/{State} - CEP: {ZipCode}";
    }

}
