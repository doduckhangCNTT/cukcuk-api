using AutoMapper;
using CukCuk.WebFresher032023.BL.DTO.FoodProcessingPlaces;
using CukCuk.WebFresher032023.DL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.BL.AutoMapper
{
    public class FoodProcessingPlaceProfile : Profile
    {
        public FoodProcessingPlaceProfile()
        {
            /*
             - Thực hiện ánh xạ các thuộc tính từ Employee thành các trường tương ứng trong EmployeeDTO (những trường ko được 
            khai báo trong EmployeeDTO thì không được ánh xạ vào)
             */
            CreateMap<FoodProcessingPlace, FoodProcessingPlaceDto>();
            CreateMap<FoodProcessingPlaceDto, FoodProcessingPlace>();
            CreateMap<FilterEntity<FoodProcessingPlace>, FilterEntity<FoodProcessingPlaceDto>>();
            CreateMap<FoodProcessingPlaceCreateDto, FoodProcessingPlace>();
            CreateMap<FoodProcessingPlaceUpdateDto, FoodProcessingPlace>();
        }
    }
}
