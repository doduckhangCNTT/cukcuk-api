using AutoMapper;
using CukCuk.WebFresher032023.BL.DTO.Foods;
using CukCuk.WebFresher032023.BL.Service.Bases;
using CukCuk.WebFresher032023.BL.Service.FoodServiceHobbes;
using CukCuk.WebFresher032023.BL.Service.FoodUnits;
using CukCuk.WebFresher032023.DL.Entity;
using CukCuk.WebFresher032023.DL.Model;
using CukCuk.WebFresher032023.DL.Repository.Bases;
using CukCuk.WebFresher032023.DL.Repository.Foods;
using CukCuk.WebFresher032023.DL.Repository.FoodServiceHobbes;
using CukCuk.WebFresher032023.DL.Repository.FoodUnits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.BL.Service.Foods
{
    public class FoodService : BaseService<Food, FoodDto, FoodUpdateDto, FoodCreateDto>, IFoodService
    {
        #region Filed
        private readonly IFoodRepository _foodRepository;
        private readonly IFoodServiceHobbyRepository _foodServiceHobbyRepository;
        private IFoodServiceHobbyService _foodServiceHobby;
        private IMapper _mapper;
        #endregion

        #region Constructor
        public FoodService(IFoodRepository foodRepository, IFoodServiceHobbyService foodServiceHobbyService, IMapper mapper) : base(foodRepository, mapper)
        {
            _foodRepository = foodRepository;
            _mapper = mapper;
            _foodServiceHobby = foodServiceHobbyService; // Truyền DI 
        } 
        #endregion

        /// <summary>
        /// - Thực hiện lấy thông tin food theo id
        /// </summary>
        /// <param name="ids">Danh sách Id Food</param>
        /// <returns>Thông tin food</returns>
        /// - Author: DDKhang (30/6/2023)
        public override async Task<List<FoodDto>> GetAsync(string ids)
        {
            // Lấy thông tin food theo id
            List<Food> foods = await _foodRepository.GetAsync(ids);
            Food food = foods[0];
            // Lấy danh sách id các sở thích phục vụ thuộc foodId tương ứng
            List<FoodServiceHobby> foodServiceHobbes = await _foodServiceHobby.GetFoodServiceHobby(ids);

            if(foodServiceHobbes != null)
            {
                // Thêm thông tin các dịch vụ sở thích cho food
                food.FoodServiceHobby = foodServiceHobbes;
            }

            // Ánh xạ các trường -> Dto
            List<FoodDto> foodssDto = _mapper.Map<List<FoodDto>>(foods);
            return foodssDto;
        }
    }
}
