using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.DL.Entity
{
    /// <summary>
    /// - Lớp loại đồ ăn
    /// </summary>
    /// Created By: DDKhang (25/6/2023)
    public class TypeFood : BaseEntity
    {
        /// <summary>
        /// - Mã loại đồ ăn
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public Guid TypeFoodId { get; set; }

        /// <summary>
        /// - Tên loại đồ ăn
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public string TypeFoodName { get; set; }
    }
}
