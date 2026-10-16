using MyConsoleApp.Services;
using MyConsoleApp.Models;
using MyConsoleApp.Controllers;
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();
    var machine=new VendingMachine();
    machine.AddCoin(new List<Coin>
    {
        new Coin(1, 10),
        new Coin(5, 10),
        new Coin(10, 10),
        new Coin(20, 10),
        new Coin(50, 10)
    });
    machine.AddProduct(new List<Product>
    {
        new Product("Kava", 27, 10),
        new Product("Sokoladas", 25, 10),
        new Product("Vanduo", 13, 10)
    });

    builder.Services.AddSingleton(machine);


    var app = builder.Build();
    app.UseDefaultFiles();
    app.UseStaticFiles();
    app.MapControllers();

    app.Run();

/*
public class Program
{
    public static void Main(string[] args)
    {
        var machine = new VendingMachine();
        
        // UÅ¾krauname monetas
        machine.AddCoin(new List<Coin>
        {
            new Coin(1, 10),
            new Coin(5, 10),
            new Coin(10, 10),
            new Coin(20, 10),
            new Coin(50, 10)
        });
        
        // Pridedame prekÄ™
        machine.AddProduct(new List<Product> 
        { new Product("Kava", 27, 10), 
          new Product("Sokoladas", 25, 10), 
          new Product("Vanduo", 13, 10) 
        });
        
        Console.WriteLine("Prekybos automatas");
        Console.WriteLine();

        while(true)
        {
            Console.WriteLine("Prekes:");
            Console.WriteLine(machine.GetProductsStatus());
            Console.WriteLine();

            Console.Write("Isveskite prekes pavadinima: arba 0 jei norite iseiti: ");
            string? productName = Console.ReadLine();
            if (productName == "0")
            {
                break;
            }
            if (string.IsNullOrWhiteSpace(productName))
            {
                Console.WriteLine("Prekes pavadinimas negali buti tuscias");
                Console.WriteLine();
                continue;
            }
            productName = productName.Trim();
            // Randame prekÄ™ ir jos kaina
                var selectedProduct = machine.GetProduct(productName);
                
                if (selectedProduct == null)
                {
                    Console.WriteLine("PrekÄ— nerasta!");
                    Console.WriteLine();
                    continue;
                }
                
                Console.WriteLine($"PrekÄ—: {selectedProduct.Name}, Kaina: {selectedProduct.Price}ct");
                Console.WriteLine();
                Console.WriteLine("Kasos busena pries pirkima:");
                Console.WriteLine(machine.GetCashRegisterStatus());
                Console.WriteLine();


            // LeidÅ¾iami nominalai
            var allowedDenominations = new List<int> { 1, 5, 10, 20, 50 };

            //ivedam monetas
             var payment = new List<Coin>();
             int totalPaid= 0;
            while (totalPaid < selectedProduct.Price)
            {
                Console.Write($"Ä®veskite monetÄ… (nominalas: 1, 5, 10, 20, 50). SumokÄ—ta: {totalPaid}ct, Reikia: {selectedProduct.Price}ct: ");
                string? coinInput = Console.ReadLine();
                
                if (string.IsNullOrWhiteSpace(coinInput))
                {
                    Console.WriteLine("Neteisingas Ä¯vedimas!");
                    continue;
                }
                
                if (!int.TryParse(coinInput, out int denomination))
                {
                    Console.WriteLine("Neteisingas nominalas!");
                    continue;
                }
                //validacija
                if(!allowedDenominations.Contains(denomination))
                {
                    Console.WriteLine("Neteisingas nominalas!");
                    continue;
                }
                payment.Add(new Coin(denomination, 1));
                totalPaid += denomination;
                Console.WriteLine($"SumokÄ—ta: {totalPaid}ct");
            }
                    Console.WriteLine();
        
        var result = machine.Purchase(productName, payment);
        
        if (result.Success)
        {
            Console.WriteLine($"Sekmingai nupirkta: {result.Product?.Name}");
            if (result.Change.Any())
            {
                Console.WriteLine($"GrÄ…Å¾a: {string.Join(", ", result.Change)}");
            }
            else
            {
                Console.WriteLine("Grazos: nereikia");
            }
            Console.WriteLine();
            Console.WriteLine("Kasos busena:");
            Console.WriteLine(machine.GetCashRegisterStatus());
        }
            else
            {
                Console.WriteLine($" Pirkimas nepavyko: {result.ErrorMessage}");
            }
            
            
            Console.WriteLine();
            Console.WriteLine("---");
            Console.WriteLine();
        }

        // Console.WriteLine("Testas 1: Pirkimas su grÄ…za");
        // // Kava kainuoja 27ct, mokame 30ct (3 x 10ct)
        // // GrÄ…Å¾a turi bÅ«ti 3ct (galim variantai: 3 x 1ct arba 1 x 1ct + 1 x 2ct - bet 2ct neturime)
        // var result = machine.Purchase("Kava", new List<Coin> { new Coin(10, 3) });
        // Console.WriteLine($"Pirkimas: {result.Success}");
        // if (result.Success)
        // {
        //     Console.WriteLine($"PrekÄ—: {result.Product?.Name}");
        //     Console.WriteLine($"GrÄ…Å¾a: {string.Join(", ", result.Change)}");
        // }
        // else
        // {
        //     Console.WriteLine($"Klaida: {result.ErrorMessage}");
        // }
        
        // Console.WriteLine();
        // Console.WriteLine("Testas 2: Pirkimas be grazos" );
        // machine.AddProduct(new List<Product> { new Product("Sokoladas", 25, 10) });
        // var result2 = machine.Purchase("Sokoladas", new List<Coin> { new Coin(5, 5) });
        // Console.WriteLine($"Pirkimas: {result2.Success}");
        // if (result2.Success)
        // {
        //     Console.WriteLine($"PrekÄ—: {result2.Product?.Name}");
        //     Console.WriteLine($"GrÄ…Å¾a: {string.Join(", ", result2.Change)}");
        // }
        // else
        // {
        //     Console.WriteLine($"Klaida: {result2.ErrorMessage}");
        // }
        
        // Console.WriteLine();
        // Console.WriteLine("Testas 3: Pirkimas su nepakankama suma ");
        // var result3 = machine.Purchase("Kava", new List<Coin> { new Coin(10, 2) });
        // Console.WriteLine($"Pirkimas: {result3.Success}");
        // if (!result3.Success)
        // {
        //     Console.WriteLine($"Klaida: {result3.ErrorMessage}");
        // }
        
        // Console.WriteLine();
        // Console.WriteLine("Testas 4: Prekes nera ");
        // var result4 = machine.Purchase("TruskalÄ—s", new List<Coin> { new Coin(10, 5) });
        // Console.WriteLine($"Pirkimas: {result4.Success}");
        // if (!result4.Success)
        // {
        //     Console.WriteLine($"Klaida: {result4.ErrorMessage}");
        // }

        // Console.WriteLine();
        // Console.WriteLine(" Testas 5: Nepavyko suskaiciuoti grazos");
        // // PridÄ—sime prekÄ™, kuri kainuoja 3ct, bet turime tik 5ct monetas
        // machine.AddProduct(new List<Product> { new Product("Guma", 3, 10) });
        // // Mokame 5ct, reikia grÄ…Å¾inti 2ct, bet 2ct monetÅ³ neturime
        // var result5 = machine.Purchase("Guma", new List<Coin> { new Coin(5, 1) });
        // Console.WriteLine($"Pirkimas: {result5.Success}");
        // if (!result5.Success)
        //     Console.WriteLine($"Klaida: {result5.ErrorMessage}");
        Console.WriteLine();
        Console.WriteLine("=== GalutinÄ— bÅ«sena ===");
        Console.WriteLine(machine.GetCashRegisterStatus());
        Console.WriteLine();
        Console.WriteLine(machine.GetProductsStatus());
        }


}
*/
