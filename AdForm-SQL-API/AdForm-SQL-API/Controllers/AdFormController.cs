using AdForm_SQL_API.AdFormDB;
using AdForm_SQL_API.Models;
using AdForm_SQL_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdForm_SQL_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdFormController : ControllerBase
    {
        private AdFormService _adFormService;
        public AdFormController(AdFormService adFormService)
        {
            _adFormService = adFormService;
        }

        [HttpGet]
        public IActionResult GetOrder(string orderId)
        {
            Invoice invoice = _adFormService.GetOrders(orderId);
            return (Ok(new
            {
                Success = true,
                Data = invoice
            }));
        }
        [HttpGet]
        [Route("orderDistribution")]
        public IActionResult GetOrdersByCustomer(string city="", bool order=false)
        {
            List<OrderDistribution> orderDistributions = _adFormService.GetOrderDistributio(city, order);
            return (Ok(new
            {
                Success = true,
                Data = orderDistributions
            }));
        }
    }
}
