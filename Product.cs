//musu prekiu klase
public class Product
{
    public string Name { get; }
    public int Price { get; }
    public int Quantity { get; private set; }

    public Product(string name, int price, int quantity)
    {
       //validacija
        if(string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Prekes pavadinimas negali buti tuscia");
        }
        if(price <= 0 || quantity < 0)
        {
            throw new ArgumentException("Prekes kaina ir kiekis turi buti teigiami");
        }
        Name = name;
        Price = price;
        Quantity = quantity;
    }

    internal void Purchase()
    {
        Quantity--;
    }
    public override string ToString()
    {
        return $" {Name}: {Price} ct  x {Quantity}";
    }
}

