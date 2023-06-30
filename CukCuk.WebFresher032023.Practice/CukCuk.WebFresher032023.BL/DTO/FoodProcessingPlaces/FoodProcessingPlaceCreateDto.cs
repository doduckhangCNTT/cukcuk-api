using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.BL.DTO.FoodProcessingPlaces
{
    public class FoodProcessingPlaceCreateDto
    {
        /// <summary>
        /// - Mã chế biến tại
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public Guid FoodProcessingPlaceId { get; set; }

        /// <summary>
        /// - Tên nơi chế biến
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public string FoodProcessingPlaceName { get; set; }
    }
}
