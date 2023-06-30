using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.BL.DTO.FoodUnits
{
    /// <summary>
    /// - Đối tượng chuyển đổi đơn vị món ăn
    /// </summary>
    /// Created By: DDKhang (25/6/2023)
    public class FoodUnitDto
    {
        /// <summary>
        /// - Mã đơn vị đồ ăn
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public Guid FoodUnitId { get; set; }

        /// <summary>
        /// - Mã đơn vị đồ ăn
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public string FoodUnitName { get; set; }

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
