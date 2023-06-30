using CukCuk.WebFresher032023.BL.DTO.FoodServiceHobbes;
using CukCuk.WebFresher032023.BL.Service.FoodServiceHobbes;
using CukCuk.WebFresher032023.DL.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CukCuk.WebFresher032023.Practice.Controllers
{
    [Route("api/v1/[controller]")]
    public class FoodServiceHobbesController : BaseController<FoodServiceHobby, FoodServiceHobbyDto, FoodServiceHobbyUpdateDto, FoodServiceHobbyCreateDto>
    {
        private readonly string _connectionString;

        public FoodServiceHobbesController(IConfiguration configuration, IFoodServiceHobbyService foodServiceHobbyService) : base (foodServiceHobbyService)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }
    }
}
