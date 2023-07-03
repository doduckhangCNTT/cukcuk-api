using AutoMapper;
using CukCuk.WebFresher032023.BL.DTO.ServiceHobbes;
using CukCuk.WebFresher032023.BL.Service.Bases;
using CukCuk.WebFresher032023.DL.Entity;
using CukCuk.WebFresher032023.DL.Repository.Bases;
using CukCuk.WebFresher032023.DL.Repository.ServiceHobbes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.BL.Service.ServiceHobbes
{
    public class ServiceHobbyService : BaseService<ServiceHobby, ServiceHobbyDto, ServiceHobbyUpdateDto, ServiceHobbyCreateDto>, IServiceHobbyService
    {

        #region Field
        private readonly IServiceHobbyRepository _serviceHobbyRepository;
        #endregion

        #region Constructor
        public ServiceHobbyService(IServiceHobbyRepository serviceHobbyRepository, IMapper mapper) : base(serviceHobbyRepository, mapper)
        {
            _serviceHobbyRepository = serviceHobbyRepository;
        } 
        #endregion

        /// <summary>
        /// - Thực hiện thêm mới sở thích dịch vụ 
        /// </summary>
        /// <param name="serviceHobby">Thông tin sở thích dịch vụ</param>
        /// <returns>Số lượng bản ghi đã thêm mới</returns>
        /// Author: DDKhang (30/6/2023)
        public async Task<int> CreateServiceHobby(ServiceHobby serviceHobby)
        {
            int qualityCreated = await _serviceHobbyRepository.CreateAsync(serviceHobby);
            return qualityCreated;
        }

        public async Task<List<ServiceHobby>> GetAsyncServiceHobby(string ids)
        {
            List<ServiceHobby> serviceHobbies =  await _serviceHobbyRepository.GetAsyncServiceHobby(ids);

            return serviceHobbies;
        }
    }
}
