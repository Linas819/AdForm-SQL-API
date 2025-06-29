using AdForm_SQL_API.Models;
using AdForm_SQL_API.Services;
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
        /// <summary>
        /// Get details about a given order's total price and products: name, category, quantity, price
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /AdForm
        ///     {
        ///         "orderId": "ord11766"
        ///     }
        /// </remarks>
        /// <param name="orderId"></param>
        /// <response code="400">Missing order id</response>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetOrder(string orderId)
        {
            // One object that has all the nessecary information the can then be returned to the frontend
            InvoiceResponse invoice = _adFormService.GetOrders(orderId);
            return (Ok(new
            {
                Success = invoice.Success,
                Products = invoice.Products,
                TotalPrice = invoice.TotalPrice,
                Messege = invoice.Message
            }));
        }
        /// <summary>
        /// Get order distrubution details by a given city or all cities if no city is provided
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /AdForm/OrderDistribution

        ///     {
        ///         "city": "British",
        ///         "order": true
        ///     }
        /// </remarks>
        /// <param name="city"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("OrderDistribution")]
        public IActionResult GetOrdersByCustomer(string city="", bool order=false)
        {
            // One object that has all the nessecary information the can then be returned to the frontend
            OrderDistributionResponse response = _adFormService.GetOrderDistribution(city, order);
            return (Ok(new
            {
                Success = response.Success,
                OrderDistribution = response.OrderDistributions,
                Messege = response.Message
            }));
        }
    }
}
