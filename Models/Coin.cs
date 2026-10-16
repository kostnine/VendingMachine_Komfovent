
using MyConsoleApp.Models;

namespace MyConsoleApp.Models;

//musu monetu klase

public class Coin
{
    public int Denomination { get; }
    public int Quantity { get; private set; }

    /// <param name="denomination">Monetos nominalo verte</param>
    /// <param name="quantity">Monetos kiekis</param>

    public Coin(int denomination, int quantity)
    {
       //validacija
        if(denomination <= 0 || quantity < 0)
        {
            throw new ArgumentException("Monetos verte ir kiekis turi buti teigiami");
        }
        Denomination = denomination;
        Quantity = quantity;
    }
    //prideti monetu
    public void Add(int count)
    {
        Quantity += count;
    }
    public void Remove(int count)
    {
        Quantity -= count;
    }
    public override string ToString()
    {
        return $" {Quantity} x {Denomination}ct";
    }
}

