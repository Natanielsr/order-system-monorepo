using OrderSystem.Domain.Entities;
using OrderSystem.Application.DTOs.Order;

namespace OrderSystem.Tests.Application.DTOs.Order;

public class PaymentInfoDtoTest
{
    [Fact]
    public void PaymentInfoDto_DefaultValues_AreCorrect()
    {
        // Arrange & Act
        var paymentInfo = new PaymentInfoDto();

        // Assert
        Assert.Equal(Guid.Empty, paymentInfo.Id);
        Assert.Equal(PaymentMethod.Pix, paymentInfo.Method); // Default enum value
        Assert.Equal(0m, paymentInfo.PaidAmount);
        Assert.Equal(string.Empty, paymentInfo.TransactionReference);
        Assert.Equal(string.Empty, paymentInfo.LastFourDigits);
        Assert.Equal(string.Empty, paymentInfo.ProviderName);
        Assert.Equal(PaymentStatus.Pending, paymentInfo.Status); // Default enum value
    }

    [Fact]
    public void PaymentInfoDto_SetProperties_WorksCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var method = PaymentMethod.CreditCard;
        var paidAmount = 150.75m;
        var reference = "txn_123456";
        var lastFour = "4242";
        var provider = "Stripe";
        var status = PaymentStatus.Captured;

        // Act
        var paymentInfo = new PaymentInfoDto
        {
            Id = id,
            Method = method,
            PaidAmount = paidAmount,
            TransactionReference = reference,
            LastFourDigits = lastFour,
            ProviderName = provider,
            Status = status
        };

        // Assert
        Assert.Equal(id, paymentInfo.Id);
        Assert.Equal(method, paymentInfo.Method);
        Assert.Equal(paidAmount, paymentInfo.PaidAmount);
        Assert.Equal(reference, paymentInfo.TransactionReference);
        Assert.Equal(lastFour, paymentInfo.LastFourDigits);
        Assert.Equal(provider, paymentInfo.ProviderName);
        Assert.Equal(status, paymentInfo.Status);
    }

    [Fact]
    public void PaymentInfoDto_Equality_SameValues_AreEqual()
    {
        // Arrange
        var id = Guid.NewGuid();
        var paymentInfo1 = new PaymentInfoDto
        {
            Id = id,
            Method = PaymentMethod.Boleto,
            PaidAmount = 99.99m,
            TransactionReference = "ref_001",
            LastFourDigits = "1122",
            ProviderName = "PagSeguro",
            Status = PaymentStatus.Authorized
        };
        var paymentInfo2 = new PaymentInfoDto
        {
            Id = id,
            Method = PaymentMethod.Boleto,
            PaidAmount = 99.99m,
            TransactionReference = "ref_001",
            LastFourDigits = "1122",
            ProviderName = "PagSeguro",
            Status = PaymentStatus.Authorized
        };

        // Act & Assert
        Assert.Equal(paymentInfo1, paymentInfo2);
        Assert.True(paymentInfo1 == paymentInfo2);
        Assert.False(paymentInfo1 != paymentInfo2);
        Assert.Equal(paymentInfo1.GetHashCode(), paymentInfo2.GetHashCode());
    }

    [Fact]
    public void PaymentInfoDto_Equality_DifferentValues_AreNotEqual()
    {
        // Arrange
        var paymentInfo1 = new PaymentInfoDto
        {
            Id = Guid.NewGuid(),
            Method = PaymentMethod.Pix,
            PaidAmount = 50m,
            TransactionReference = "ref_A",
            LastFourDigits = "3333",
            ProviderName = "Provider1",
            Status = PaymentStatus.Pending
        };
        var paymentInfo2 = new PaymentInfoDto
        {
            Id = Guid.NewGuid(),
            Method = PaymentMethod.CreditCard,
            PaidAmount = 100m,
            TransactionReference = "ref_B",
            LastFourDigits = "4444",
            ProviderName = "Provider2",
            Status = PaymentStatus.Failed
        };

        // Act & Assert
        Assert.NotEqual(paymentInfo1, paymentInfo2);
        Assert.False(paymentInfo1 == paymentInfo2);
        Assert.True(paymentInfo1 != paymentInfo2);
    }

    [Fact]
    public void PaymentInfoDto_WithExpression_CreatesModifiedCopy()
    {
        // Arrange
        var original = new PaymentInfoDto
        {
            Id = Guid.NewGuid(),
            Method = PaymentMethod.CreditCard,
            PaidAmount = 200m,
            TransactionReference = "original_ref",
            LastFourDigits = "1111",
            ProviderName = "OriginalProvider",
            Status = PaymentStatus.Pending
        };

        // Act
        var modified = original with
        {
            PaidAmount = 250m,
            Status = PaymentStatus.Authorized,
            ProviderName = "UpdatedProvider"
        };

        // Assert
        Assert.Equal(original.Id, modified.Id);
        Assert.Equal(original.Method, modified.Method);
        Assert.Equal(original.TransactionReference, modified.TransactionReference);
        Assert.Equal(original.LastFourDigits, modified.LastFourDigits);
        Assert.NotEqual(original.PaidAmount, modified.PaidAmount);
        Assert.Equal(250m, modified.PaidAmount);
        Assert.NotEqual(original.Status, modified.Status);
        Assert.Equal(PaymentStatus.Authorized, modified.Status);
        Assert.NotEqual(original.ProviderName, modified.ProviderName);
        Assert.Equal("UpdatedProvider", modified.ProviderName);
    }

    [Fact]
    public void PaymentInfoDto_WithExpression_PreservesUnmodifiedProperties()
    {
        // Arrange
        var original = new PaymentInfoDto
        {
            Id = Guid.NewGuid(),
            Method = PaymentMethod.Pix,
            PaidAmount = 75.5m,
            TransactionReference = "txn_xyz",
            LastFourDigits = "9999",
            ProviderName = "PixProvider",
            Status = PaymentStatus.Captured
        };

        // Act
        var copy = original with { };

        // Assert
        Assert.Equal(original.Id, copy.Id);
        Assert.Equal(original.Method, copy.Method);
        Assert.Equal(original.PaidAmount, copy.PaidAmount);
        Assert.Equal(original.TransactionReference, copy.TransactionReference);
        Assert.Equal(original.LastFourDigits, copy.LastFourDigits);
        Assert.Equal(original.ProviderName, copy.ProviderName);
        Assert.Equal(original.Status, copy.Status);
        Assert.True(original == copy);
        Assert.False(ReferenceEquals(original, copy));
    }

    [Fact]
    public void PaymentInfoDto_EnumValues_AreHandledCorrectly()
    {
        // Arrange & Act
        var allMethods = Enum.GetValues<PaymentMethod>().Cast<PaymentMethod>().ToList();
        var allStatuses = Enum.GetValues<PaymentStatus>().Cast<PaymentStatus>().ToList();

        // Assert
        Assert.Contains(PaymentMethod.Pix, allMethods);
        Assert.Contains(PaymentMethod.CreditCard, allMethods);
        Assert.Contains(PaymentMethod.Boleto, allMethods);
        Assert.Equal(3, allMethods.Count);

        Assert.Contains(PaymentStatus.Pending, allStatuses);
        Assert.Contains(PaymentStatus.Authorized, allStatuses);
        Assert.Contains(PaymentStatus.Captured, allStatuses);
        Assert.Contains(PaymentStatus.Refunded, allStatuses);
        Assert.Contains(PaymentStatus.Failed, allStatuses);
        Assert.Equal(5, allStatuses.Count);
    }
}
