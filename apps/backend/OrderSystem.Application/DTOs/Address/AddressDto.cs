namespace OrderSystem.Application.DTOs.Address;

public record class AddressDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Complement { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = "Brasil"; // Valor padrão
    public Guid UserId { get; set; }
    public bool IsDefault { get; set; }
}
