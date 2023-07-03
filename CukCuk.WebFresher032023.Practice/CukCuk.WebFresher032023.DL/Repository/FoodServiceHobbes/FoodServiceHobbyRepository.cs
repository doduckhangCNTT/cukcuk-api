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

namespace CukCuk.WebFresher032023.DL.Repository.FoodServiceHobbes
{
    public class FoodServiceHobbyRepository : BaseRepository<FoodServiceHobby>, IFoodServiceHobbyRepository
    {
        public FoodServiceHobbyRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<int> CreateFoodServiceHobby(string foodId, string serviceHobbyIds)
        {
            try
            {
                var sqlConnectionCommand = await GetOpenConnectionAsync();
                // Khởi tạo đối tượng DynamicParameters để chứa các tham số đầu vào
                var parameters = new DynamicParameters();
                parameters.Add("@foodId", foodId, DbType.String, ParameterDirection.Input);
                parameters.Add("@serviceHobbyIds", serviceHobbyIds, DbType.String, ParameterDirection.Input);

                // Gọi stored procedure bằng phương thức Execute của Dapper
                int qualityCreate = sqlConnectionCommand.Execute("Proc_InsertFoodServiceHobby", parameters, commandType: CommandType.StoredProcedure);
                return qualityCreate;

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
