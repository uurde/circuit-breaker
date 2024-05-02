using Main.Extensions;
using Microsoft.AspNetCore.Mvc;
using Polly.CircuitBreaker;

namespace Main.Controllers
{
    [Route("api/[controller]")]
    public class MainController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MainController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseModel>> Get()
        {
            var result = GetVideoDetail();
            return result;
        }

        private ResponseModel GetVideoDetail()
        {
            //response from DB
            ResponseModel model = new ResponseModel()
            {
                VideoId = 1,
                VideoTitle = "test",
                PresenterInfo = new PresenterInfo
                {
                    PresenterId = 1000
                }
            };

            //response from Presenter Service
            model.PresenterInfo = GetPresenterInfo(model.PresenterInfo.PresenterId).GetAwaiter().GetResult();

            return model;
        }

        private async Task<PresenterInfo> GetPresenterInfo(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("providerClient");
                var response = await client.GetAsync($"/api/Presenter/{id}");

                var result = response.Content.ReadFromJsonAsync<PresenterInfo>().GetAwaiter().GetResult();

                return result;
            }
            catch (BrokenCircuitException ex)
            {

            }

            return null;
        }
    }
}

