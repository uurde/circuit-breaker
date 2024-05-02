using Microsoft.AspNetCore.Mvc;

namespace Provider.Controllers
{
    [Route("api/[controller]")]
    public class PresenterController : Controller
    {
        [HttpGet("{id}")]
        public PresenterInfo Get(int id)
        {
            var result = new PresenterInfo
            {
                PresenterId = id,
                Name = "Uğur",
                Lastname = "Değirmenci"
            };

            return result;
        }
    }

    public class PresenterInfo
    {
        public int PresenterId { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
    }
}

