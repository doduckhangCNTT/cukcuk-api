using AutoMapper;
using CukCuk.WebFresher032023.BL.DTO.MenuGroups;
using CukCuk.WebFresher032023.BL.Service.Bases;
using CukCuk.WebFresher032023.DL.Entity;
using CukCuk.WebFresher032023.DL.Repository.Bases;
using CukCuk.WebFresher032023.DL.Repository.MenuGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.BL.Service.MenuGroups
{
    public class MenuGroupService : BaseService<MenuGroup, MenuGroupDto, MenuGroupUpdateDto, MenuGroupCreateDto>, IMenuGroupService
    {
        public MenuGroupService(IMenuGroupRepository menuGroupRepository, IMapper mapper) : base(menuGroupRepository, mapper)
        {
        }
    }
}
