using CukCuk.WebFresher032023.BL.Service.Bases;
using CukCuk.WebFresher032023.BL.Service.FoodServiceHobbes;
using CukCuk.WebFresher032023.Common.Enum;
using CukCuk.WebFresher032023.DL.Entity;
using CukCuk.WebFresher032023.DL.Model;
using Microsoft.AspNetCore.Mvc;

namespace CukCuk.WebFresher032023.Practice.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public abstract class BaseController<TEntity, TEntityDto, TEntityUpdateDto, TEntityCreateDto> : ControllerBase
    {
        #region Field
        // Khai báo đối tượng gọi lên tầng service
        protected readonly IBaseService<TEntityDto, TEntityUpdateDto, TEntityCreateDto> _baseService;
        protected readonly IFoodServiceHobbyService _foodServiceHobbyService;
        #endregion

        #region Constructor
        protected BaseController(IBaseService<TEntityDto, TEntityUpdateDto, TEntityCreateDto> baseService)
        {
            _baseService = baseService;
        }
        #endregion

        /// <summary>
        /// - Lấy thông tin thực thể theo id
        /// </summary>
        /// <param name="ids">Danh sách id</param>
        /// <returns>List<TEntityDto>></returns>
        /// CreatedBy: DDKhang (27/6/2023)
        [HttpPost("Ids")]
        public virtual async Task<ActionResult<List<TEntityDto>>> GetAsync([FromBody]EntityGet entityGet)
        {
            List<TEntityDto> foods = await _baseService.GetAsync(entityGet.Ids);
            return Ok(foods);
        }

        /// <summary>
        /// - Lọc thông tin các thực thể
        /// </summary>
        /// <param name="entityFilter">Thông tin thực thể muốn lọc</param>
        /// <returns>ActionResult<FilterEntity<TEntityDto>></returns>
        /// CreatedBy: DDKhang (27/6/2023)
        [HttpPost("filter")]
        public virtual async Task<ActionResult<FilterEntity<TEntityDto>>> EntitysFilter(EntityFilter entityFilter)
        {
            var entityFilterDto = await _baseService.EntitysFilterAsync(entityFilter);
            return Ok(entityFilterDto);
        }

        /// <summary>
        /// - Thực hiện thêm thông tin thực thể
        /// </summary>
        /// <param name="entityCreateDto">Thông tin thực thể muốn thêm</param>
        /// <returns>Số bản ghi đã thêm</returns>
        /// CreatedBy: DDKhang (27/6/2023)
        [HttpPost()]
        public virtual async Task<ActionResult<int>> CreateAsync([FromForm]TEntityCreateDto entityCreateDto)
        {
            int qualityRecordsCreate = await _baseService.CreateAsync(entityCreateDto);
            return Ok(1);
        }

        /// <summary>
        /// - Thực hiện cập nhật thông tin thực thể
        /// </summary>
        /// <param name="entityUpdateDto">Thông tin thực thể muốn cập nhật</param>
        /// <returns>Số lượng thực thể đã cập nhật</returns>
        /// CreatedBy: DDKhang (27/6/2023)
        [HttpPut]
        public virtual async Task<ActionResult<int>> UpdateAsync([FromForm]TEntityUpdateDto entityUpdateDto)
        {
            int qualityRecordsUpdate = await _baseService.UpdateAsync(entityUpdateDto);
            return Ok(qualityRecordsUpdate);
        }

        /// <summary>
        /// - Thực hiện xóa thông tin theo entityId
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>Số bản ghi đã xóa</returns>
        /// CreatedBy: DDKhang (27/6/2023)
        [HttpDelete("{entityId}")]
        public virtual async Task<ActionResult<int>> DeleteAsync(Guid entityId)
        {
            int qualityDelete = await _baseService.DeleteAsync(entityId);
            return StatusCode((int)HttpStatusCodeEnum.NoContent, qualityDelete);
        }

        /// <summary>
        /// - Xóa nhiều bản ghi
        /// </summary>
        /// <param name="listEntityId">Danh sách mã bản ghi được nối bằng ","</param>
        /// <returns>Số bản ghi được xóa</returns>
        /// CreatedBy: DDKhang (27/6/2023)
        [HttpDelete("delete-multiple")]
        public virtual async Task<IActionResult> DeleteMutilEntityAsync(string listEntityId)
        {
            int result = await _baseService.DeleteMutilEntityAsync(listEntityId);
            return StatusCode((int)HttpStatusCodeEnum.NoContent, result);
        }
    }
}
