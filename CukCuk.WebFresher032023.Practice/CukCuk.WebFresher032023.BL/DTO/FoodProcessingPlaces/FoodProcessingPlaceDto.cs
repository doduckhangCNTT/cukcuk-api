using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.BL.DTO.FoodProcessingPlaces
{
    public class FoodProcessingPlaceDto
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

        /// <summary>
        /// - Ngày tạo
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// - Người tạo
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public string? CreatedBy { get; set; }

        /// <summary>
        /// - Ngày sửa
        /// </summary>
        /// Created By: DDKhang (25/6/2023
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// - Người sửa
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public string? ModifiedBy { get; set; }
    }
}
