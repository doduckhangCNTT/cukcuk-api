using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.BL.DTO.FoodServiceHobbes
{
    public class FoodServiceHobbyCreateDto
    {
        /// <summary>
        /// - Mã đồ ăn
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public Guid FoodId { get; set; }

        /// <summary>
        /// - Mã dịch vụ sở thích
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public Guid ServiceHobbyId { get; set; }
    }
}
