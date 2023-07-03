using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.BL.DTO.ServiceHobbes
{
    public class ServiceHobbyUpdateDto
    {
        /// <summary>
        /// - Mã dịch vụ sở thích
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public Guid ServiceHobbyId { get; set; }

        /// <summary>
        /// - Tên dịch vụ sở thích
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public string? ServiceHobbyName { get; set; }

        /// <summary>
        /// - Thêm tiền
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public int? MoreMoney { get; set; }
    }
}
