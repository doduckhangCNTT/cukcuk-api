using AutoMapper;
using CukCuk.WebFresher032023.BL.DTO.FoodProcessingPlaces;
using CukCuk.WebFresher032023.BL.Service.Bases;
using CukCuk.WebFresher032023.DL.Entity;
using CukCuk.WebFresher032023.DL.Repository.Bases;
using CukCuk.WebFresher032023.DL.Repository.FoodProcessingPlaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.BL.Service.FoodProcessingPlaces
{
    public class FoodProcessingPlaceService : BaseService<FoodProcessingPlace, FoodProcessingPlaceDto, FoodProcessingPlaceUpdateDto, FoodProcessingPlaceCreateDto>, IFoodProcessingPlaceService
    {
        private readonly IFoodProcessingPlaceRepository _foodProcessingPlaceRepository;
        public FoodProcessingPlaceService(IFoodProcessingPlaceRepository foodProcessingPlaceRepository, IMapper mapper) : base(foodProcessingPlaceRepository, mapper)
        {
            _foodProcessingPlaceRepository = foodProcessingPlaceRepository;
        }
    }
}
