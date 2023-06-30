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

        [HttpPost("Ids")]
        public virtual async Task<ActionResult<List<TEntityDto>>> GetAsync(string ids)
        {
            List<TEntityDto> foods = await _baseService.GetAsync(ids);
            return Ok(foods);
        }

        [HttpPost("filter")]
        public virtual async Task<ActionResult<FilterEntity<TEntityDto>>> EntitysFilter(EntityFilter entityFilter)
        {
            var entityFilterDto = await _baseService.EntitysFilterAsync(entityFilter);
            return Ok(entityFilterDto);
        }

        [HttpPost()]
        public virtual async Task<ActionResult<int>> CreateAsync(TEntityCreateDto entityCreateDto)
        {
            int qualityRecordsCreate = await _baseService.CreateAsync(entityCreateDto);
            return Ok(1);
        }

        [HttpPut]
        public virtual async Task<ActionResult<int>> UpdateAsync(TEntityUpdateDto entityUpdateDto)
        {
            int qualityRecordsUpdate = await _baseService.UpdateAsync(entityUpdateDto);
            return Ok(qualityRecordsUpdate);
        }

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
