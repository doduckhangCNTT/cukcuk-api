using CukCuk.WebFresher032023.BL.DTO.FoodProcessingPlaces;
using CukCuk.WebFresher032023.BL.Service.Bases;
using CukCuk.WebFresher032023.BL.Service.FoodProcessingPlaces;
using CukCuk.WebFresher032023.DL.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CukCuk.WebFresher032023.Practice.Controllers
{
    [Route("api/v1/[controller]")]
    public class FoodProcessingPlacesController : BaseController<FoodProcessingPlace, FoodProcessingPlaceDto, FoodProcessingPlaceUpdateDto, FoodProcessingPlaceCreateDto>
    {
        private readonly string _connectionString;

        public FoodProcessingPlacesController(IConfiguration configuration, IFoodProcessingPlaceService foodProcessingPlaceService) : base(foodProcessingPlaceService)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }
    }
}
