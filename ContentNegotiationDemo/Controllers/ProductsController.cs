using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Text.Json;
using System.Xml.Serialization;
using System.IO;
using ContentNegotiationDemo.Models;

namespace ContentNegotiationDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = new Product()
            {
                Id = id,
                Name = "Gizmo",
                Category = "Widgets",
                Price = 1.99M
            };

            // Inspect the Accept header manually
            var acceptHeader = HttpContext.Request.Headers["Accept"].ToString();

            if (acceptHeader.Contains("application/xml"))
            {
                // Return as XML if requested
                var xmlSerializer = new XmlSerializer(typeof(Product));
                using (var stringWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(stringWriter, product);
                    return Content(stringWriter.ToString(), "application/xml");
                }
            }
            else if (acceptHeader.Contains("application/json") || string.IsNullOrEmpty(acceptHeader))
            {
                // Return as JSON by default or if requested
                var json = JsonSerializer.Serialize(product);
                return Content(json, "application/json");
            }
            else
            {
                // If the requested media type is not supported, return 406 Not Acceptable
                return StatusCode(StatusCodes.Status406NotAcceptable);
            }
        }
    }
}