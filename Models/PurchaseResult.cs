using MyConsoleApp.Models;
namespace MyConsoleApp.Models;

//Pirkimo rezultato klasė
public class PurchaseResult
{
    public bool Success { get; }
    public Product? Product { get; }
    public List<Coin> Change { get; }
    public string? ErrorMessage { get; }

    private PurchaseResult(bool success, Product? product, List<Coin> change, string? errorMessage)
    {
        Success = success;
        Product = product;
        Change = change;
        ErrorMessage = errorMessage;
    }

    // Sėkmingas pirkimas
    public static PurchaseResult Successful(Product product, List<Coin> change)
    {
        return new PurchaseResult(true, product, change, null);
    }

    // Nepavykęs pirkimas
    public static PurchaseResult Failed(string errorMessage)
    {
        return new PurchaseResult(false, null, new List<Coin>(), errorMessage);
    }
}

