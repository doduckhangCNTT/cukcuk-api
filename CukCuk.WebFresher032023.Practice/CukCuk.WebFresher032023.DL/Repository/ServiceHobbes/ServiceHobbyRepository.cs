using CukCuk.WebFresher032023.DL.Entity;
using CukCuk.WebFresher032023.DL.Repository.Bases;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.DL.Repository.ServiceHobbes
{
    public class ServiceHobbyRepository : BaseRepository<ServiceHobby>, IServiceHobbyRepository
    {
        public ServiceHobbyRepository(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// - Thực hiện thêm mới món ăn và dịch vụ sở thích vào bảng chung gian
        /// </summary>
        /// <param name="foodId">Mã món ăn</param>
        /// <param name="serviceHobbesIds">Danh sách mã dịch vụ sở thích</param>
        /// <returns>Số lượng bản ghi đã thêm</returns>
        /// Author: DDKhang (30/6/2023)
        public async Task<int> CreateAsyncServiceHobby(Guid foodId, string serviceHobbesIds)
        {
            // Chuỗi kết nối tới MySQL
            using var sqlConnectionCommand = await GetOpenConnectionAsync();

            // Khởi tạo đối tượng DynamicParameters để chứa các tham số đầu vào
            var parameters = new DynamicParameters();
            parameters.Add("@foodId", foodId, DbType.String, ParameterDirection.Input);
            parameters.Add("@serviceHobbyIds", serviceHobbesIds, DbType.String, ParameterDirection.Input);

            // Gọi stored procedure bằng phương thức Execute của Dapper
            int qualityRecordsInsert = sqlConnectionCommand.Execute("Proc_InsertFoodServiceHobby", parameters, commandType: CommandType.StoredProcedure);

            return qualityRecordsInsert;
        }

        public async Task<List<ServiceHobby>> GetAsyncServiceHobby(string ids)
        {
            List<ServiceHobby> serviceHobbies = await GetEntityById(ids);
            return serviceHobbies;
        }
    }
}
