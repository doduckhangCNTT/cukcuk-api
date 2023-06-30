using AutoMapper;
using CukCuk.WebFresher032023.BL.DTO.Foods;
using CukCuk.WebFresher032023.DL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.BL.AutoMapper
{
    public class FoodProfile : Profile
    {
        public FoodProfile()
        {
            /*
             - Thực hiện ánh xạ các thuộc tính từ Employee thành các trường tương ứng trong EmployeeDTO (những trường ko được 
            khai báo trong EmployeeDTO thì không được ánh xạ vào)
             */
            CreateMap<Food, FoodDto>();
            CreateMap<FoodDto, Food>();
            CreateMap<FilterEntity<Food>, FilterEntity<FoodDto>>();
            CreateMap<FoodCreateDto, Food>();
            CreateMap<FoodUpdateDto, Food>();
        }
    }
}
