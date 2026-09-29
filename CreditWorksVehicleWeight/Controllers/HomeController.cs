using CreditWorksVehicleWeight.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CreditWorksVehicleWeight.Controllers
{
    public sealed class HomeController : Controller
    {
        [Route("Home/Error")]
        public IActionResult Error() => View();
    }
}
