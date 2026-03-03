namespace OrderSystem.Domain.Entities;

public enum PaymentStatus
{
    Pending,
    Authorized,
    Captured,
    Refunded,
    Failed
}
public class PaymentInfo : Entity
{
    // Dados para o histórico (Snapshot)
    public required PaymentMethod Method { get; init; } // "CreditCard", "Pix", "Boleto"
    public required decimal PaidAmount { get; init; }
    // Rastreabilidade Externa
    public required string TransactionReference { get; init; } // O ID que vem do Gateway
    public required string LastFourDigits { get; init; }       // Apenas para o cliente identificar o cartão
    public required string ProviderName { get; init; }        // Ex: "Stripe", "Adyen"

    // Status do Pagamento (usando a dica da string que vimos antes)
    public required PaymentStatus Status { get; init; } // "Authorized", "Captured", "Refunded", "Failed"
    public required Guid OrderId { get; init; }
    public Order? Order { get; set; }

    public PaymentInfo()
    {

    }
}
