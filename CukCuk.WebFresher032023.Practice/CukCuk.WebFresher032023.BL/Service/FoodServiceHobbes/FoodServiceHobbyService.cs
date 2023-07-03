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
        #region Field
        private readonly IFoodServiceHobbyRepository _foodServiceHobbyRepository; 
        #endregion

        #region Constructor
        public FoodServiceHobbyService(IFoodServiceHobbyRepository foodServiceHobbyRepository, IMapper mapper) : base(foodServiceHobbyRepository, mapper)
        {
            _foodServiceHobbyRepository = foodServiceHobbyRepository;
        } 
        #endregion

        /// <summary>
        /// - Thực hiện lấy thông các id sở thích phục vụ theo foodId tương ứng
        /// </summary>
        /// <param name="foodId">Mã đồ ăn</param>
        /// <returns>List<FoodServiceHobby></returns>
        /// Author: DDKhang (30/6/2023)
        public async Task<List<FoodServiceHobby>> GetFoodServiceHobby(string foodId)
        {
            List<FoodServiceHobby> foodServiceHobbies = await _foodServiceHobbyRepository.GetAsync(foodId);
            return foodServiceHobbies;
        }

        /// <summary>
        /// - Thực hiện xóa foodId trong bảng chung
        /// </summary>
        /// <param name="foodId">Mã đồ ăn</param>
        /// <returns>Số lượng bản ghi đã xóa</returns>
        /// Author: DDKhang (1/7/2023)
        public async Task<int> DeleteFoodServiceHobby(string foodId)
        {
            int qualityDelte = await _foodServiceHobbyRepository.DeleteMutilEntityAsync(foodId);
            return qualityDelte;
        }

        /// <summary>
        /// - Thực hiện thêm mới thông tin foodServiceHobby (bảng chung)
        /// </summary>
        /// <param name="foodId">Mã đồ ăn</param>
        /// <param name="serviceHobbyIds">Danh sách các sở thích</param>
        /// <returns>Số lượng đã thêm</returns>
        /// Author: DDKhang (1/7/2023)
        public async Task<int> CreateFoodServiceHobby(string foodId, string serviceHobbyIds)
        {
            int qualityCreate = await _foodServiceHobbyRepository.CreateFoodServiceHobby(foodId, serviceHobbyIds);
            return qualityCreate;
        }
    }
}
