using System.Collections;
using Microsoft.AspNetCore.Mvc;
namespace orders_api.Controllers;
[ApiController]
[Route("[controller]")]
public class OrderController: ControllerBase
{
    public ActionResult<IList<string>> GetOrdersByCustomerID(string customerID)
    {
        return new string[] { "Order 1", "Order 2", "Order 3" };
    }
}