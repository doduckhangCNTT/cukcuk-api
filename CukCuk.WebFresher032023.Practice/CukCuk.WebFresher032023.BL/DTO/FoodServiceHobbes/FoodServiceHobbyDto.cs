using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.BL.DTO.FoodServiceHobbes
{
    public class FoodServiceHobbyDto
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
