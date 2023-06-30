using AutoMapper;
using CukCuk.WebFresher032023.BL.DTO.FoodUnits;
using CukCuk.WebFresher032023.BL.Service.Bases;
using CukCuk.WebFresher032023.DL.Entity;
using CukCuk.WebFresher032023.DL.Repository.Bases;
using CukCuk.WebFresher032023.DL.Repository.FoodUnits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.BL.Service.FoodUnits
{
    public class FoodUnitService : BaseService<FoodUnit, FoodUnitDto, FoodUnitUpdateDto, FoodUnitCreateDto>, IFoodUnitService
    {
        public FoodUnitService(IFoodUnitRepository foodUnitRepository, IMapper mapper) : base(foodUnitRepository, mapper)
        {
        }

        public async Task<int> GetAsyncAbc()
        {
            return 1;

        }
    }
}
