﻿using CukCuk.WebFresher032023.DL.Entity;
using CukCuk.WebFresher032023.DL.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.DL.Repository.Bases
{
    public interface IBaseRepository<TEntity>
    {

        /// <summary>
        /// - Thực hiện lọc thông tin thực thể
        /// </summary>
        /// <param name="entityFilter">Thông tin lọc</param>
        /// <returns>Danh sách thực thể đã lọc</returns>
        /// CreatedBy: DDKhang (27/6/2023)
        Task<FilterEntity<TEntity>> EntityFilterAsync(EntityFilter entityFilter);

        /// <summary>
        /// - Thực hiện lấy thông tin các thực thể theo id
        /// </summary>
        /// <param name="ids">(GUID) Danh sách các id của thực thể</param>
        /// <returns>Danh sách các thực thể theo id</returns>
        /// CreatedBy: DDKhang (27/6/2023)
        Task<List<TEntity>> GetAsync(string ids);

        /// <summary>
        /// - Thực hiện tạo thông tin thực thể mới
        /// </summary>
        /// <param name="entityCreateDto">Thông tin thực thể mới</param>
        /// <returns>Số lượng thực thể đã thêm vào</returns>
        /// CreatedBy: DDKhang (27/6/2023)
        Task<int> CreateAsync(TEntity entity);
        //Task<int> CreateAsync(TEntity entity, DbTransaction transaction);

        /// <summary>
        /// - Thực hiện cập nhật thông tin thực thể
        /// </summary>
        /// <param name="entityUpdateDto">Thông tin thực thể muốn cập nhật</param>
        /// <returns>Số lượng thực thể được cập nhật</returns>
        /// CreatedBy: DDKhang (27/6/2023)
        Task<int> UpdateAsync(TEntity entity);

        /// <summary>
        /// - Thực hiện xóa thông tin thực thể theo id
        /// </summary>
        /// <param name="entityId">Mã thực thể muốn xóa</param>
        /// <returns>Số lượng thực thể đã xóa</returns>
        /// CreatedBy: DDKhang (27/6/2023)
        Task<int> DeleteAsync(Guid entityId);

        /// <summary>
        /// - Xóa nhiều bản ghi
        /// </summary>
        /// <param name="listEntityId">Danh sách mã bản ghi được nối bằng ","</param>
        /// <returns>Số bản ghi được xóa</returns>
        /// CreatedBy: DDKhang (27/6/2023)
        Task<int> DeleteMutilEntityAsync(string listEntityId);

        /// <summary>
        /// - Tạo mới mã món ăn mới
        /// </summary>
        /// <param name="prefixEntity">Tiền tố món ăn</param>
        /// <returns>Mã code mới</returns>
        Task<string> NewEntityCode(string prefixEntity);

        /// <summary>
        /// - Kiểm tra trùng lặp mã
        /// </summary>
        /// <param name="entityCode">Mã thực thể</param>
        /// <returns>Boolean</returns>
        Task<int> CheckDuplicateCode(string entityCode);
    }
}
