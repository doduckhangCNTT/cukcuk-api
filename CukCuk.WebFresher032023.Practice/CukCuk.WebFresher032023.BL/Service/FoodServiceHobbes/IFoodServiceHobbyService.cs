using CukCuk.WebFresher032023.BL.DTO.FoodServiceHobbes;
using CukCuk.WebFresher032023.BL.Service.Bases;
using CukCuk.WebFresher032023.DL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.BL.Service.FoodServiceHobbes
{
    public interface IFoodServiceHobbyService : IBaseService<FoodServiceHobbyDto, FoodServiceHobbyUpdateDto, FoodServiceHobbyCreateDto>
    {
        Task<List<FoodServiceHobby>> GetFoodServiceHobby(string foodId);
        Task<int> DeleteFoodServiceHobby(string foodId);
    }
}
