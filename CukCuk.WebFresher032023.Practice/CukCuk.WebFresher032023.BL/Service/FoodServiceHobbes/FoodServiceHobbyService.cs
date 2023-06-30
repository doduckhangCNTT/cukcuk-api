using AutoMapper;
using CukCuk.WebFresher032023.BL.DTO.FoodServiceHobbes;
using CukCuk.WebFresher032023.BL.Service.Bases;
using CukCuk.WebFresher032023.DL.Entity;
using CukCuk.WebFresher032023.DL.Repository.Bases;
using CukCuk.WebFresher032023.DL.Repository.FoodServiceHobbes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.BL.Service.FoodServiceHobbes
{
    public class FoodServiceHobbyService : BaseService<FoodServiceHobby, FoodServiceHobbyDto, FoodServiceHobbyUpdateDto, FoodServiceHobbyCreateDto>, IFoodServiceHobbyService
    {
        private readonly IFoodServiceHobbyRepository _foodServiceHobbyRepository;

        public FoodServiceHobbyService(IFoodServiceHobbyRepository foodServiceHobbyRepository, IMapper mapper) : base(foodServiceHobbyRepository, mapper)
        {
            _foodServiceHobbyRepository = foodServiceHobbyRepository;
        }

        public async Task<List<FoodServiceHobby>> GetFoodServiceHobby(string foodId)
        {
            List<FoodServiceHobby> foodServiceHobbies = await _foodServiceHobbyRepository.GetAsync(foodId);
            return foodServiceHobbies;
        }
    }
}
