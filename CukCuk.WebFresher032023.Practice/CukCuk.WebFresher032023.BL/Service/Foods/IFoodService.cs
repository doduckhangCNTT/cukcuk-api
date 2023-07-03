using CukCuk.WebFresher032023.BL.DTO.Foods;
using CukCuk.WebFresher032023.BL.Service.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.BL.Service.Foods
{
    public interface IFoodService : IBaseService<FoodDto, FoodUpdateDto, FoodCreateDto>
    {
        //Task<int> CreateAsyncFood(FoodDto foodDto);
    }
}
