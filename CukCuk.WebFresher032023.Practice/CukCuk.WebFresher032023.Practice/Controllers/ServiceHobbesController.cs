using CukCuk.WebFresher032023.BL.DTO.ServiceHobbes;
using CukCuk.WebFresher032023.BL.Service.Bases;
using CukCuk.WebFresher032023.BL.Service.ServiceHobbes;
using CukCuk.WebFresher032023.DL.Entity;

namespace CukCuk.WebFresher032023.Practice.Controllers
{
    public class ServiceHobbesController : BaseController<ServiceHobby, ServiceHobbyDto, ServiceHobbyUpdateDto, ServiceHobbyCreateDto>
    {
        public ServiceHobbesController(IConfiguration configuration, IServiceHobbyService serviceHobbyService) : base(serviceHobbyService)
        {
        }
    }
}
