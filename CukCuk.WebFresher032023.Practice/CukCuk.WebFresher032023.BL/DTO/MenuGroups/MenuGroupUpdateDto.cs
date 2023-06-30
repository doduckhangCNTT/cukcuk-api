using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.BL.DTO.MenuGroups
{
    public class MenuGroupUpdateDto
    {
        /// <summary>
        /// - Mã nhóm đồ ăn
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public Guid MenuGroupId { get; set; }

        /// <summary>
        /// - Tên nhóm đồ ăn
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public string MenuGroupName { get; set; }
    }
}
