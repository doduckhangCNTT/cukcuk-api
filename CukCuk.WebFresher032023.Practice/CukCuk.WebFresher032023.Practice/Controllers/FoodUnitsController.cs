using CukCuk.WebFresher032023.BL.DTO.FoodUnits;
using CukCuk.WebFresher032023.BL.Service.Bases;
using CukCuk.WebFresher032023.BL.Service.FoodUnits;
using CukCuk.WebFresher032023.DL.Entity;

namespace CukCuk.WebFresher032023.Practice.Controllers
{
    public class FoodUnitsController : BaseController<FoodUnit, FoodUnitDto, FoodUnitUpdateDto, FoodUnitCreateDto>
    {
        private readonly string _connectionString;
        public FoodUnitsController(IConfiguration configuration, IFoodUnitService foodUnitService) : base(foodUnitService)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }
    }
}
