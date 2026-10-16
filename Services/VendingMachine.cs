
using MyConsoleApp.Models;
using MyConsoleApp.Controllers;

namespace MyConsoleApp.Services;

//simuliajome aparata kuris valdo monetas ir prekes
public class VendingMachine
{
    private Dictionary<int, Coin> _cashRegister;
    private Dictionary<string, Product> _products;

    // Sukuria naujÄ… prekybos automata
    public VendingMachine()
    {
        _cashRegister = new Dictionary<int, Coin>();
        _products = new Dictionary<string, Product>(StringComparer.OrdinalIgnoreCase);
    }
        public Product? GetProduct(string productName)
    {
        _products.TryGetValue(productName, out var product);
        return product;
    }
    public string GetCashRegisterStatus()
    {
        var lines = new List<string>{"Kasa: "};
        foreach(var kvp in _cashRegister.OrderByDescending(x => x.Key))
        {
            lines.Add($"{kvp.Value}");
        }
        return string.Join("\n", lines);
    }
    public string GetProductsStatus()
    {
        var lines = new List<string>{"PrekÄ—s: "};
        foreach(var product in _products.Values)
        {
            lines.Add($"{product}");
        }
        return string.Join("\n", lines);
    }
    //prideti monetas
        public void AddCoin(List<Coin> coins)
        {
            foreach (var coin in coins)
            {
                if (_cashRegister.ContainsKey(coin.Denomination))
                {
                    _cashRegister[coin.Denomination].Add(coin.Quantity);
                }
                else
                {
                    _cashRegister.Add(coin.Denomination, coin);
                }
            }
        }

    //prideti prekes
    public void AddProduct(List<Product> products)
    {
        foreach (var product in products)
        {
            _products.Add(product.Name, product);
        }
    }
    private void RefundPayment(List<Coin> payment)
    {
        foreach (var coin in payment)
        {
           if(_cashRegister.ContainsKey(coin.Denomination))
           {
               _cashRegister[coin.Denomination].Remove(coin.Quantity);
           }
        }
    }
    public List<Product> GetProducts()
    {
        return _products.Values.ToList();
    }
    public List<Coin> GetCoins()
    {
        return _cashRegister.Values.OrderByDescending(coin=>coin.Denomination).ToList();
    }
    //pirkimo metodas
    public PurchaseResult Purchase(string productName, List<Coin> payment)
    {
        //1. Patikra ar yra preke
        if (!_products.TryGetValue(productName, out var product))
        {
            return PurchaseResult.Failed("PrekÄ— nerasta");
        }
        //2 patikrinti ar prekes quantity ne 0
        if (product.Quantity == 0)
        {
            return PurchaseResult.Failed("PrekÄ— baigÄ—si");
        }
        //3 skaiciuojam suma kiek reikia sumoketi
        int totalPayment = payment.Sum(c => c.Denomination * c.Quantity);
        //4 tikrinam ar uztenka pinigu
        if (totalPayment < product.Price)
        {
            return PurchaseResult.Failed("SumokÄ—ta suma nepakanka");
        }
        //5 pridedam mokejima
        foreach (var coin in payment)
        {
            if (_cashRegister.ContainsKey(coin.Denomination))
            {
                _cashRegister[coin.Denomination].Add(coin.Quantity);
            }
            else
            {
                _cashRegister[coin.Denomination] = new Coin(coin.Denomination, coin.Quantity);
            }
        }
        // panaikinama preke
        product.Purchase();

        //skaiciuojame graza
        int changeAmount = totalPayment - product.Price;
        var change = GetChange(changeAmount);

        if(change == null || change.Count ==0 && changeAmount > 0)
        {
            RefundPayment(payment);
            return PurchaseResult.Failed("Nepavyko suskaiciuoti grazo");
        }

        // Atimame grÄ…Å¾Ä… iÅ¡ kasos
        foreach (var coin in change)
        {
            _cashRegister[coin.Denomination].Remove(coin.Quantity);
        }

        //Grazinti true
        return PurchaseResult.Successful(product, change);
    }
    //skaiciuojama kiek reikia grazinti
    private List<Coin> GetChange(int amount)
    {
        if (amount == 0)
        
            return new List<Coin>();
        var change = new List<Coin>();
        int remaining = amount;
        
        //rusiuojam monetas 
        var denominations = _cashRegister.Keys.OrderByDescending(d => d).ToList();

        foreach( var denomination in denominations)
        {
            if(remaining <= 0)
            break;
            
            if(!_cashRegister.ContainsKey(denomination))
            continue;

            int maxPossible = remaining / denomination;
            int available = _cashRegister[denomination].Quantity;
            int count = Math.Min(maxPossible, available);
            
            if(count > 0)
            {
                change.Add(new Coin(denomination, count));
                remaining -= denomination * count;
            }
        }
        //JEIGU NEPAVYKO SUSKAICIUOTI TIKSLIOS GRAZOS

        if(remaining != 0)
        {
            return null;
        }
        
        return change;
    }


}
