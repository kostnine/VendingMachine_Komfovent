public class Program
{
    public static void Main(string[] args)
    {
        var machine = new VendingMachine();
        
        // Užkrauname monetas
        machine.AddCoin(new List<Coin>
        {
            new Coin(1, 10),
            new Coin(5, 10),
            new Coin(10, 10),
            new Coin(20, 10),
            new Coin(50, 10)
        });
        
        // Pridedame prekę
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
            // Randame prekę ir jos kaina
                var selectedProduct = machine.GetProduct(productName);
                
                if (selectedProduct == null)
                {
                    Console.WriteLine("Prekė nerasta!");
                    Console.WriteLine();
                    continue;
                }
                
                Console.WriteLine($"Prekė: {selectedProduct.Name}, Kaina: {selectedProduct.Price}ct");
                Console.WriteLine();
                Console.WriteLine("Kasos busena pries pirkima:");
                Console.WriteLine(machine.GetCashRegisterStatus());
                Console.WriteLine();


            // Leidžiami nominalai
            var allowedDenominations = new List<int> { 1, 5, 10, 20, 50 };

            //ivedam monetas
             var payment = new List<Coin>();
             int totalPaid= 0;
            while (totalPaid < selectedProduct.Price)
            {
                Console.Write($"Įveskite monetą (nominalas: 1, 5, 10, 20, 50). Sumokėta: {totalPaid}ct, Reikia: {selectedProduct.Price}ct: ");
                string? coinInput = Console.ReadLine();
                
                if (string.IsNullOrWhiteSpace(coinInput))
                {
                    Console.WriteLine("Neteisingas įvedimas!");
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
                Console.WriteLine($"Sumokėta: {totalPaid}ct");
            }
                    Console.WriteLine();
        
        var result = machine.Purchase(productName, payment);
        
        if (result.Success)
        {
            Console.WriteLine($"Sekmingai nupirkta: {result.Product?.Name}");
            if (result.Change.Any())
            {
                Console.WriteLine($"Grąža: {string.Join(", ", result.Change)}");
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

        // Console.WriteLine("Testas 1: Pirkimas su grąza");
        // // Kava kainuoja 27ct, mokame 30ct (3 x 10ct)
        // // Grąža turi būti 3ct (galim variantai: 3 x 1ct arba 1 x 1ct + 1 x 2ct - bet 2ct neturime)
        // var result = machine.Purchase("Kava", new List<Coin> { new Coin(10, 3) });
        // Console.WriteLine($"Pirkimas: {result.Success}");
        // if (result.Success)
        // {
        //     Console.WriteLine($"Prekė: {result.Product?.Name}");
        //     Console.WriteLine($"Grąža: {string.Join(", ", result.Change)}");
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
        //     Console.WriteLine($"Prekė: {result2.Product?.Name}");
        //     Console.WriteLine($"Grąža: {string.Join(", ", result2.Change)}");
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
        // var result4 = machine.Purchase("Truskalės", new List<Coin> { new Coin(10, 5) });
        // Console.WriteLine($"Pirkimas: {result4.Success}");
        // if (!result4.Success)
        // {
        //     Console.WriteLine($"Klaida: {result4.ErrorMessage}");
        // }

        // Console.WriteLine();
        // Console.WriteLine(" Testas 5: Nepavyko suskaiciuoti grazos");
        // // Pridėsime prekę, kuri kainuoja 3ct, bet turime tik 5ct monetas
        // machine.AddProduct(new List<Product> { new Product("Guma", 3, 10) });
        // // Mokame 5ct, reikia grąžinti 2ct, bet 2ct monetų neturime
        // var result5 = machine.Purchase("Guma", new List<Coin> { new Coin(5, 1) });
        // Console.WriteLine($"Pirkimas: {result5.Success}");
        // if (!result5.Success)
        //     Console.WriteLine($"Klaida: {result5.ErrorMessage}");
        Console.WriteLine();
        Console.WriteLine("=== Galutinė būsena ===");
        Console.WriteLine(machine.GetCashRegisterStatus());
        Console.WriteLine();
        Console.WriteLine(machine.GetProductsStatus());
        }


}

