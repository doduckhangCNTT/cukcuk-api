using CukCuk.WebFresher032023.BL.DTO.ServiceHobbes;
using CukCuk.WebFresher032023.BL.Service.Bases;
using CukCuk.WebFresher032023.DL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.BL.Service.ServiceHobbes
{
    public interface IServiceHobbyService : IBaseService<ServiceHobbyDto, ServiceHobbyUpdateDto, ServiceHobbyCreateDto>
    {
        Task<int> CreateServiceHobby(ServiceHobby serviceHobby);
        Task<List<ServiceHobby>> GetAsyncServiceHobby(string ids);
    }
}
