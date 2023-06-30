using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.DL.Entity
{
    /// <summary>
    /// - Lớp đồ ăn 
    /// Created By: DDKhang (25/6/2023)
    /// </summary>
    public class Food : BaseEntity
    {
        /// <summary>
        /// - Mã món ăn
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public Guid? FoodId { get; set; }

        /// <summary>
        /// - Mã code món ăn
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public string? FoodCode { get; set; }

        /// <summary>
        /// - Tên món ăn
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public string? FoodName { get; set; }

        /// <summary>
        /// - Gía bán
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public int? Price { get; set; }

        /// <summary>
        /// - Gía vốn
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public int? InitialPrice { get; set; }

        /// <summary>
        /// - Hình ảnh
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public string? Image { get; set; }

        /// <summary>
        /// - Mô tả
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public string? Description { get; set; }

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

        /// <summary>
        /// - Ngừng bán
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public int? StopSelling { get; set; }

        /// <summary>
        /// - Mã đơn vị món ăn
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public Guid? FoodUnitId { get; set; }

        /// <summary>
        /// - Mã nhóm thực đơn
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public Guid? MenuGroupId { get; set; }

        /// <summary>
        /// - Mã kiểu đồ ăn
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public Guid? TypeFoodId { get; set; }

        /// <summary>
        /// - Mã vị trí chế biến đồ ăn
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public Guid? FoodProcessingPlaceId { get; set; }

        /// <summary>
        /// - Tên nhóm đồ ăn
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public virtual string? MenuGroupName { get; set; }

        /// <summary>
        /// - Tên đơn vị đồ ăn
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public virtual string? FoodUnitName { get; set; }

        /// <summary>
        /// - Tên kiểu đồ ăn
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public virtual string? TypeFoodName { get; set; }

        /// <summary>
        /// - Danh sách sở thích phục vụ
        /// </summary>
        /// Created By: DDKhang (25/6/2023)
        public virtual List<ServiceHobby>? ServiceHobbes { get; set; }
        public virtual List<FoodServiceHobby>? FoodServiceHobby { get; set; }
    }
}
