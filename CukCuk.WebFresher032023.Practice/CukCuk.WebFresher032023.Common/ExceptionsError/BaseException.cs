using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.Common.ExceptionsError
{
    /// <summary>
    /// - Chứa các thông tin lỗi trả về 
    /// - Author: DDKhang (24/5/2023)
    /// </summary>
    public class BaseException : Exception
    {
        #region Properties
        /// <summary>
        /// - Mã lỗi
        /// - Author: DDKhang (24/5/2023)
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// - Thông báo lỗi cho nhà phát triển
        /// - Author: DDKhang (24/5/2023)
        /// </summary>
        public string? DevMessage { get; set; }

        /// <summary>
        /// - Thông báo lỗi cho người dùng
        /// - Author: DDKhang (24/5/2023)
        /// </summary>
        public string? UserMessage { get; set; }

        /// <summary>
        /// - Chứa nhiều thông báo lỗi cho người dùng
        /// - Author: DDKhang (24/5/2023)
        /// </summary>
        public List<string>? UserMessages { get; set; }

        /// <summary>
        /// - Truy vẫn lỗi
        /// - Author: DDKhang (24/5/2023)
        /// </summary>
        public string? TraceId { get; set; }

        /// <summary>
        /// - Thông tin thêm 
        /// - Author: DDKhang (24/5/2023)
        /// </summary>
        public string? MoreInfor { get; set; }
        #endregion

        #region Methods
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
        #endregion
    }
}
