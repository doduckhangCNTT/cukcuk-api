using CukCuk.WebFresher032023.BL.DTO.MenuGroups;
using CukCuk.WebFresher032023.BL.Service.Bases;
using CukCuk.WebFresher032023.BL.Service.MenuGroups;
using CukCuk.WebFresher032023.DL.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CukCuk.WebFresher032023.Practice.Controllers
{
    [Route("api/v1/[controller]")]
    public class MenuGroupsController : BaseController<MenuGroup, MenuGroupDto, MenuGroupUpdateDto, MenuGroupCreateDto>
    {

        private readonly string _connectionString;

        public MenuGroupsController(IConfiguration configuration, IMenuGroupService menuGroupService) : base(menuGroupService)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }
    }
}
