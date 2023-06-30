using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.DL.Entity
{
    /// <summary>
    /// - Mã món ăn
    /// </summary>
    /// Created By: DDKhang (25/6/2023)
    public class FoodUnit : BaseEntity
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
    }
}
