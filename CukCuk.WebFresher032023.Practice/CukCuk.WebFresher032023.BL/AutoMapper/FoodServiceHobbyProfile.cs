using AutoMapper;
using CukCuk.WebFresher032023.BL.DTO.FoodServiceHobbes;
using CukCuk.WebFresher032023.DL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.BL.AutoMapper
{
    public class FoodServiceHobbyProfile : Profile
    {
        public FoodServiceHobbyProfile()
        {
            /*
             - Thực hiện ánh xạ các thuộc tính từ Employee thành các trường tương ứng trong EmployeeDTO (những trường ko được 
            khai báo trong EmployeeDTO thì không được ánh xạ vào)
             */
            CreateMap<FoodServiceHobby, FoodServiceHobbyDto>();
            CreateMap<FoodServiceHobbyDto, FoodServiceHobby>();
            CreateMap<FilterEntity<FoodServiceHobby>, FilterEntity<FoodServiceHobbyDto>>();
            CreateMap<FoodServiceHobbyCreateDto, FoodServiceHobby>();
            CreateMap<FoodServiceHobbyUpdateDto, FoodServiceHobby>();
        }
    }
}
