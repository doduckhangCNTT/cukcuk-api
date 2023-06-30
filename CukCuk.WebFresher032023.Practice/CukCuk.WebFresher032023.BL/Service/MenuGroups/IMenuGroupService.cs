using CukCuk.WebFresher032023.BL.DTO.MenuGroups;
using CukCuk.WebFresher032023.BL.Service.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.BL.Service.MenuGroups
{
    public interface IMenuGroupService : IBaseService<MenuGroupDto, MenuGroupUpdateDto, MenuGroupCreateDto>
    {
    }
}
