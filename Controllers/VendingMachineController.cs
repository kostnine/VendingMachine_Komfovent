using Microsoft.AspNetCore.Mvc;
using MyConsoleApp.Models;
using MyConsoleApp.Services;

namespace MyConsoleApp.Controllers;
    [ApiController]
    [Route("api/[controller]")]

    public class VendingMachineController : ControllerBase
    {

        private readonly VendingMachine _vendingMachine;

        public VendingMachineController(VendingMachine vendingMachine)
        {
            _vendingMachine = vendingMachine;
        }

        [HttpGet("products")]
        public IActionResult GetProducts()
        {
            return Ok(_vendingMachine.GetProducts());
        }
        

        [HttpGet("cash-register")]
        public IActionResult GetCashRegister()
        {
            return Ok(_vendingMachine.GetCoins());
        }

        [HttpPost("purchase")]
        public IActionResult Purchase([FromBody] PurchaseRequest request)
        {
            var payment = request.Coins.Select(c => new Coin(c.Denomination, c.Quantity)).ToList();
            var result = _vendingMachine.Purchase(request.ProductName, payment);

            return Ok(new
            {
                success = result.Success,
                product = result.Product?.Name,
                change = result.Change.Select(c => new { c.Denomination, c.Quantity }),
                errorMessage = result.ErrorMessage
            });
        }
    }

    public class PurchaseRequest
    {
        public string ProductName { get; set; }
        public List<CoinDto> Coins { get; set; }
    }

    public class CoinDto
    {
        public int Denomination { get; set; }
        public int Quantity { get; set; }
    }




