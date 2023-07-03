using AutoMapper;
using CukCuk.WebFresher032023.DL.Entity;
using CukCuk.WebFresher032023.DL.Model;
using CukCuk.WebFresher032023.DL.Repository.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.BL.Service.Bases
{
    public class BaseService<TEntity, TEntityDto, TEntityUpdateDto, TEntityCreateDto> : IBaseService<TEntityDto, TEntityUpdateDto, TEntityCreateDto>
    {
        #region Field
        // Khai báo đối tượng DL
        protected readonly IBaseRepository<TEntity> _baseRepository;
        protected readonly IMapper _mapper;
        #endregion

        #region Constructor
        /// <summary>
        /// - Hàm khởi tạo của base service, thực hiện tạo đối tượng gọi lên repository, và tạo đối tượng mapper
        /// </summary>
        /// <param name="baseRepository"></param>
        /// <param name="mapper"></param>
        /// Author: DDKhang (27/6/2023)
        public BaseService(IBaseRepository<TEntity> baseRepository, IMapper mapper)
        {
            _baseRepository = baseRepository;
            _mapper = mapper;
        }
        #endregion

        /// <summary>
        /// - Thực hiện lọc thông tin thực thể theo điều kiện lọc
        /// </summary>
        /// <param name="entityFilter">Thông tin điều kiện lọc</param>
        /// <returns>FilterEntity<TEntityDto></returns>
        /// - Author: DDKhang (30/6/2023)
        public virtual async Task<FilterEntity<TEntityDto>> EntitysFilterAsync(EntityFilter entityFilter)
        {
            FilterEntity<TEntity> entities = await _baseRepository.EntityFilterAsync(entityFilter);

            // Ánh xạ các trường -> Dto
            FilterEntity<TEntityDto> entitiesDto = _mapper.Map<FilterEntity<TEntityDto>>(entities);

            return entitiesDto;
        }

        /// <summary>
        /// - Thực hiện lấy danh sách các thực thể theo id
        /// - Mục đích: Phục vụ cho việc lấy 1 và nhiều id
        /// </summary>
        /// <param name="ids">Danh sách id</param>
        /// <returns>List<TEntityDto</returns>
        /// - Author: DDKhang (30/6/2023)
        public virtual async Task<List<TEntityDto>> GetAsync(string ids)
        {
            List<TEntity> entities = await _baseRepository.GetAsync(ids);

            // Ánh xạ các trường -> Dto
            List<TEntityDto> entitiesDto = _mapper.Map<List<TEntityDto>>(entities);
            return entitiesDto;
        }

        /// <summary>
        /// - Thực hiện tạo mới thực thể
        /// </summary>
        /// <param name="entityCreateDto">Thông tin đối tượng muốn thêm mới</param>
        /// <returns>Số bản ghi đã thêm mới</returns>
        /// - Author: DDKhang (30/6/2023)
        public virtual async Task<int> CreateAsync(TEntityCreateDto entityCreateDto)
        {
            var entityCreate = _mapper.Map<TEntity>(entityCreateDto);

            int qualityRecordsCreate = await _baseRepository.CreateAsync(entityCreate);
            return 1;
        }

        /// <summary>
        /// - Thực hiện cập nhật thông tin thực thể
        /// </summary>
        /// <param name="entityUpdateDto">Thông tin thực thể muốn cập nhật</param>
        /// <returns>Số bản ghi đã được cập nhật</returns>
        /// - Author: DDKhang (30/6/2023)
        public virtual async Task<int> UpdateAsync(TEntityUpdateDto entityUpdateDto)
        {
            var entityUpdate = _mapper.Map<TEntity>(entityUpdateDto);

            int qualityRecordsUpdate = await _baseRepository.UpdateAsync(entityUpdate);
            return qualityRecordsUpdate;
        }

        /// <summary>
        /// - Thực hiện xóa thông tin thực thể theo id
        /// </summary>
        /// <param name="entityId">id thực thể muốn xóa</param>
        /// <returns>Số thực thể đã xóa</returns>
        /// - Author: DDKhang (30/6/2023)
        public async Task<int> DeleteAsync(Guid entityId)
        {
            return await _baseRepository.DeleteAsync(entityId);
        }

        /// <summary>
        /// - Xóa nhiều bản ghi
        /// </summary>
        /// <param name="listEntityId">Danh sách mã bản ghi được nối bằng ","</param>
        /// <returns>Số bản ghi được xóa</returns>
        /// CreatedBy: DDKhang (27/5/2023)
        public virtual async Task<int> DeleteMutilEntityAsync(string listEntityId)
        {
            int result = await _baseRepository.DeleteMutilEntityAsync(listEntityId);
            return result;
        }
    }
}
