using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.DL.Model
{
    /// <summary>
    /// - Thực thể lọc
    /// - Author: DDKhang (3/7/2023)
    /// </summary>
    public class EntityFilter
    {
        /// <summary>
        ///  Số trang
        /// - Author: DDKhang (3/7/2023)
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// - Trang bắt đầu
        /// - Author: DDKhang (3/7/2023)
        /// </summary>
        public int? Start { get; set; }

        /// <summary>
        /// - Giới hạn số bản ghi trên trang
        /// - Author: DDKhang (3/7/2023)
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// - Danh sách các lựa chọn sắp xếp
        /// - Author: DDKhang (3/7/2023)
        /// </summary>
        public List<EntitySort>? Sorts { get; set; }

        /// <summary>
        /// - Danh sách các lựa chọn filter
        /// - Author: DDKhang (3/7/2023)
        /// </summary>
        public List<EntityItemFilter>? Filters { get; set; }
    }
}
