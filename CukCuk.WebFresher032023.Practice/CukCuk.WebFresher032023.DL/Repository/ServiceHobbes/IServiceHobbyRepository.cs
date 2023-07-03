using CukCuk.WebFresher032023.DL.Entity;
using CukCuk.WebFresher032023.DL.Repository.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.DL.Repository.ServiceHobbes
{
    public interface IServiceHobbyRepository : IBaseRepository<ServiceHobby>
    {
        Task<int> CreateAsyncServiceHobby(Guid foodId, string serviceHobbesIds);
        Task<List<ServiceHobby>> GetAsyncServiceHobby(string ids);
    }
}
