using CukCuk.WebFresher032023.DL.Entity;
using CukCuk.WebFresher032023.DL.Model;
using CukCuk.WebFresher032023.DL.Repository.Bases;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;

namespace CukCuk.WebFresher032023.DL.Repository.Foods
{
    public class FoodRepository : BaseRepository<Food>, IFoodRepository
    {
        public FoodRepository(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// - Thực hiện thêm mới food, thêm các thông tin vào các bảng liên quan
        /// </summary>
        /// <param name="food"></param>
        /// <returns>Số bản ghi được thêm</returns>
        /// - Author: DDKhang (28/6/2023)
        public override async Task<int> CreateAsync(Food food)
        {
            //using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            //{
                try
                {
                    // Danh sách chứa toàn bộ mảng id serviceHobby
                    List<string> serviceHobbyIds = new();

                    // Thực hiện thêm giá trị bảng Food
                    Guid newGuidFood = Guid.NewGuid();
                    food.FoodId = newGuidFood;
                    // Thực hiện thêm mới food
                    await CreateEntity(food);

                    if(food.ServiceHobbes?.Count > 0)
                    {
                        // Thưc hiện thêm các giá trị vào bảng ServiceHobby
                        food.ServiceHobbes?.ForEach(async sh =>
                        {
                            if (sh.ServiceHobbyId != Guid.Empty)
                            {
                                // Kiểm tra xem đối tượng sở thích dịch vụ có chứa Id không -> chỉ lấy id đó
                                serviceHobbyIds.Add(sh.ServiceHobbyId.ToString());
                            }
                            else
                            {
                                // Thực hiện tạo serviceHobby mới
                                Guid newGuidServiceHobby = Guid.NewGuid();
                                sh.ServiceHobbyId = newGuidServiceHobby;
                                // Thêm vào mảng chứa toàn bộ id serviceHobby
                                serviceHobbyIds.Add(newGuidServiceHobby.ToString());
                                await CreateEntity<ServiceHobby>(sh, "ServiceHobby");
                            }
                        });

                        // Chứa toàn bộ các id sở thích dịch vụ của food tương ứng
                        string serviceHobbesIds = string.Join(',', serviceHobbyIds);

                        // Chuỗi kết nối tới MySQL
                        using var sqlConnectionCommand = await GetOpenConnectionAsync();

                        // Khởi tạo đối tượng DynamicParameters để chứa các tham số đầu vào
                        var parameters = new DynamicParameters();
                        parameters.Add("@foodId", newGuidFood, DbType.String, ParameterDirection.Input);
                        parameters.Add("@serviceHobbyIds", serviceHobbesIds, DbType.String, ParameterDirection.Input);

                        // Gọi stored procedure bằng phương thức Execute của Dapper
                        int qualityRecordsInsert = sqlConnectionCommand.Execute("Proc_InsertFoodServiceHobby", parameters, commandType: CommandType.StoredProcedure);

                        return qualityRecordsInsert;
                    }
                    return 1;
                }
                catch (Exception ex)
                {
                    throw;
                }
            //}
        }

        /// <summary>
        /// - Thực hiện việc thêm thông tin thực thể
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">Thông tin thực thể muốn thêm</param>
        /// <param name="tableNameEntity">Tên bảng thực thể trong Database</param>
        /// <returns>Số bản ghi được thêm</returns>
        /// <exception cref="Exception"></exception>
        /// - Author: DDKhang (28/6/2023)
        public async Task<int> CreateEntity<T>(T entity, string tableNameEntity)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    // Lấy tên của entity
                    //string tableName = "ServiceHobby";
                    string tableName = tableNameEntity;

                    // Kết nối database
                    using var sqlConnection = await GetOpenConnectionAsync();
                    //using var mySqlConnection = new MySqlConnection(sqlConnection);
                    //mySqlConnection.Open();

                    string sqlCommandProc = "Proc_Insert" + tableName;
                    // Đọc các tham số đầu vào của store procedure
                    var sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandText = sqlCommandProc;
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    // Lấy các tham số của Stored Procedure
                    MySqlCommandBuilder.DeriveParameters((MySqlCommand)sqlCommand);

                    var dynamicParam = new DynamicParameters();
                    foreach (MySqlParameter parameter in sqlCommand.Parameters)
                    {
                        // Tên tham số
                        var paramName = parameter.ParameterName;
                        // Bỏ tiền tố "m_" trong tham số
                        var propName = paramName.Replace("@m_", "");
                        // Lấy thuộc tính theo tên trong entity -> kiểm sự tồn tại của thuộc tính trong entity đó
                        var entityProperty = entity?.GetType().GetProperty(propName);
                        if (entityProperty != null)
                        {
                            var propValue = entity?.GetType().GetProperty(propName)?.GetValue(entity);
                            dynamicParam.Add(paramName, propValue);
                        }
                        else
                        {
                            dynamicParam.Add(paramName, null);
                        }
                    }
                    // Số bản ghi được thêm vào
                    var res = sqlConnection.Execute(sql: sqlCommandProc, param: dynamicParam, commandType: System.Data.CommandType.StoredProcedure);

                    if (res > 0)
                    {
                        // Commit Transaction
                        scope.Complete();
                    }
                    return await Task.FromResult(res);
                }
                catch (Exception ex)
                {
                    //throw new InternalException(ex.Message);
                    throw new Exception();
                    // Rollback Trasaction
                    scope.Dispose();
                }
            }
        }

        //public async Task<List<FoodServiceHobby>> GetFoodServiceHobbes()
        //{

        //}

        public override async Task<int> UpdateAsync(Food food)
        {
            // Cập nhật thông tin trên bảng Food
            await UpdateEntity(food);

            // Chuỗi kết nối tới MySQL
            using var sqlConnectionCommand = await GetOpenConnectionAsync();

            // Cập nhật thông tin trên bảng FoodServiceHobby (bảng chung)
            Guid foodId = (Guid)food.FoodId;
            // Kiểm foodId có tồn tại trong bảng chung gian
            string sqlCommand = $"SELECT * FROM FoodServiceHobby WHERE FoodId = '{foodId}'";
            List<Food> foods = (List<Food>)await sqlConnectionCommand.QueryAsync<Food>(sqlCommand);

            if (foods.Count > 0)
            {
                // Nếu food tồn tại trong bảng chung gian -> thực hiện xóa các giá trị cũ
                string sqlCommandDelete = $"DELETE FROM FoodServiceHobby WHERE FoodId = '{foodId}'";
                var test = await sqlConnectionCommand.QueryAsync<Food>(sqlCommandDelete);
            }

            // Nếu  có giá trị của mảng servicesHobby truyền lên -> thực hiện thêm vào bảng chung gian
            if(food.ServiceHobbes?.Count > 0)
            {
                // Danh sách chứa toàn bộ mảng id serviceHobby
                List<string> serviceHobbyIds = new List<string>();

                food.ServiceHobbes?.ForEach(async sh =>
                {
                    if (sh.ServiceHobbyId != Guid.Empty)
                    {
                        // Kiểm tra xem đối tượng sở thích dịch vụ có chứa Id không -> chỉ lấy id đó
                        serviceHobbyIds.Add(sh.ServiceHobbyId.ToString());
                    }
                    else
                    {
                        // Thực hiện tạo serviceHobby mới
                        Guid newGuidServiceHobby = Guid.NewGuid();
                        sh.ServiceHobbyId = newGuidServiceHobby;
                        // Thêm vào mảng chứa toàn bộ id serviceHobby
                        serviceHobbyIds.Add(newGuidServiceHobby.ToString());
                        await CreateEntity<ServiceHobby>(sh, "ServiceHobby");
                    }
                });


                // Chứa toàn bộ các id sở thích dịch vụ của food tương ứng
                string serviceHobbesIds = string.Join(',', serviceHobbyIds);

                // Khởi tạo đối tượng DynamicParameters để chứa các tham số đầu vào
                var parameters = new DynamicParameters();
                parameters.Add("@foodId", foodId, DbType.String, ParameterDirection.Input);
                parameters.Add("@serviceHobbyIds", serviceHobbesIds, DbType.String, ParameterDirection.Input);

                // Gọi stored procedure bằng phương thức Execute của Dapper
                sqlConnectionCommand.Execute("Proc_InsertFoodServiceHobby", parameters, commandType: CommandType.StoredProcedure);
            }
            return 1;
        }
    }
}
