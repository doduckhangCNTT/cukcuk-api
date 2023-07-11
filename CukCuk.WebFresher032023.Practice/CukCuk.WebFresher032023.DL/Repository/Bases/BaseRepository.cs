using CukCuk.WebFresher032023.Common.ExceptionsError;
using CukCuk.WebFresher032023.DL.Entity;
using CukCuk.WebFresher032023.DL.Funtions;
using CukCuk.WebFresher032023.DL.Model;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CukCuk.WebFresher032023.DL.Repository.Bases
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
    {
        #region Field
        private readonly string _connectionString; // biến kết nối db
        #endregion

        #region Constructor
        /// <summary>
        /// - Hàm khởi tạo thực hiện cung cấp chuỗi kết nối db
        /// </summary>
        /// <param name="configuration"></param>
        /// CreatedBy: DDKhang (23/5/2023)
        public BaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }
        #endregion

        /// <summary>
        /// - Thực hiện mở kết nối đến database
        /// </summary>
        /// <returns>DbConnection</returns>
        /// CreatedBy: DDKhang (23/5/2023)
        public virtual async Task<DbConnection> GetOpenConnectionAsync()
        {
            var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        /// <summary>
        /// - Lấy thông tin của enity theo id
        /// </summary>
        /// <param name="id">Mã entity</param>
        /// <returns>TEntity</returns>
        /// <exception cref="InternalException"></exception>
        /// Created By: DDKhang (24/5/2023)
        public virtual async Task<List<TEntity>> GetAsync(string ids)
        {
            try
            {
                List<TEntity> entities = await GetEntityById(ids);
                return entities;
            }
            catch (Exception ex)
            {
                throw new InternalException(ex.Message);
            }
        }

        /// <summary>
        /// - Thực hiện thêm thông tin
        /// - Để sử dụng:
        ///     + Tạo Proc mới cho entity muốn thêm ("Proc_Insert" + entityName)
        ///     + Các tham số của các Proc phải giống nhau (m_Name)
        /// </summary>
        /// <param name="entity">Thông tin của thực thể</param>
        /// <returns>Số bản ghi được thêm</returns>
        /// <exception cref="InternalException"></exception>
        /// Created By: DDKhang (24/6/2023)
        public virtual async Task<int> CreateAsync(TEntity entity)
        {
            int qualityAdd = await CreateEntity(entity);
            return qualityAdd;
        }

        /// <summary>
        /// - Cập nhật thông tin thực thể
        /// </summary>
        /// <param name="entity">Thông tin thực thể cập nhật</param>
        /// <returns>Số bản ghi đã cập nhật</returns>
        /// Created By: DDKhang (24/6/2023)
        public virtual async Task<int> UpdateAsync(TEntity entity)
        {
            int qualityUpdate = await UpdateEntity(entity);
            return qualityUpdate;
        }

        /// <summary>
        /// - Thực hiện xóa thông tin thực thể
        /// </summary>
        /// <param name="entityId">Mã thực thể muốn xóa</param>
        /// <returns>Số bản ghi đã xóa</returns>
        /// <exception cref="Exception"></exception>
        /// Created By: DDKhang (24/6/2023)
        public virtual async Task<int> DeleteAsync(Guid entityId)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    // Lấy tên của entity
                    string tableName = typeof(TEntity).Name;
                    // Khởi tạo kết nối với MariaDb
                    using var sqlConnection = await GetOpenConnectionAsync();

                    //// === Cách 1: Sử dụng truy vấn 
                    //// Thực hiện xóa
                    //string sqlCommandDelete = $"DELETE FROM {tableName} WHERE {tableName}Id = '{id}'";
                    //// Thực hiện chạy sql
                    //int result = await sqlConnection.ExecuteAsync(sqlCommandDelete);

                    // === Cách 2: Sử dụng Proc
                    // 1. Khởi tạo lệnh sql gọi đến Stored Procedure
                    string sqlCommandProc = "Proc_Delete" + tableName + "ById";

                    // 2. Thực hiện thêm tham số cho proc
                    MySqlCommand command = new MySqlCommand(sqlCommandProc, (MySqlConnection?)sqlConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue($"@m_{tableName}Id", entityId);

                    // 3. Thực thi proc
                    int result = await command.ExecuteNonQueryAsync();

                    // Đóng kết nối db
                    await sqlConnection.CloseAsync();
                    // Trả về số bản ghi xóa

                    scope.Complete();
                    return result;
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new InternalException(ex.Message);
                }
            }
        }

        /// <summary>
        /// - Xóa nhiều bản ghi
        /// </summary>
        /// <param name="listEntityId">Danh sách mã bản ghi được nối bằng ","</param>
        /// <returns>Số bản ghi được xóa</returns>
        /// CreatedBy: DDKhang (27/6/2023)
        public virtual async Task<int> DeleteMutilEntityAsync(string listEntityId)
        {
            int qualityDelete = await DeleteMultiEntity(listEntityId);
            return qualityDelete;
        }

        /// <summary>
        /// - Thực hiện lọc thông tin thực thể
        /// </summary>
        /// <param name="entityFilter">Thông tin thực thể lọc</param>
        /// <returns>FilterEntity<TEntity></returns>
        /// Created By: DDKhang (24/6/2023)
        public virtual async Task<FilterEntity<TEntity>> EntityFilterAsync(EntityFilter entityFilter)
        {
            try
            {
                int page, limit = 1, skip = 0;

                // Thực hiện xử lí pagination
                if (entityFilter.Page == null)
                {
                    page = 1;
                }
                else
                {
                    page = (int)entityFilter.Page;
                }

                if (entityFilter.Limit > 0 && page > 0)
                {
                    skip = (int)((page - 1) * entityFilter.Limit);
                }

                // Lấy tên của thực thể
                string tableName = typeof(TEntity).Name;
                // Khởi tạo đối tượng lọc
                var entityFilterResult = new FilterEntity<TEntity>();
                // Khởi tạo danh sách thực thể để lưu lại toàn bộ thông tin được lọc
                List<TEntity> entities = new();
                int totalEntities = 0; // Tổng tất cả bản ghi
                int totalRecordsResult = 0; // Tổng số bảng ghi đã lọc

                // Khởi tạo kết nối với MariaDb
                using var sqlConnection = await GetOpenConnectionAsync();

                // Tạo câu lệnh sql
                string sqlQuery = ConvertQuery.BuildSqlQuery(entityFilter.Filters);

                string sqlQuerySort = "";
                if(entityFilter.Sorts != null && entityFilter.Sorts.Count > 0)
                {
                    List<string> textSorts = new();
                    entityFilter.Sorts.ForEach(ef =>
                    {
                        if(ef.Direction?.Trim() != "")
                        {
                        string sqlQuerySort = "";
                        sqlQuerySort = $"{ef.Property} {ef.Direction}";
                        textSorts.Add(sqlQuerySort);
                        }
                    });

                    sqlQuerySort = string.Join(", ", textSorts);
                }

                // Thực hiện truy vấn
                // List<TEntity> foods = (List<TEntity>)sqlConnection.Query<TEntity>(sqlQuery);

                if (sqlQuery.Trim() == "")
                {
                    sqlQuery = "true";
                }

                // Kiểm tra có giá trị truy vấn sắp xếp
                if(sqlQuerySort.Trim() != "")
                {
                    sqlQuerySort = $"ORDER BY {sqlQuerySort}";
                }

                // Kết nối StoredProcedure - Thực hiện lọc 
                string sqlCommandProcFilter = "Proc_Filter" + tableName;

                using (MySqlCommand command = new MySqlCommand(sqlCommandProcFilter, (MySqlConnection?)sqlConnection))
                {
                    // Khai báo sử dụng stored procedure
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    // Thêm các tham số vào stored procedure (nếu có)
                    command.Parameters.AddWithValue($"@{tableName.ToLower()}FilterQuery", sqlQuery);
                    command.Parameters.AddWithValue($"@{tableName.ToLower()}SortQuery", sqlQuerySort);
                    command.Parameters.AddWithValue("@limitFilter", entityFilter.Limit == null ? null : entityFilter.Limit);
                    command.Parameters.AddWithValue("@skip", skip);

                    // Thực thi stored procedure
                    using MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        totalEntities = (int)(Int64)reader.GetValue(0);
                        if (entityFilter.Limit == null)
                        {
                            // Nếu limit = null or < 0 thì limit sẽ là toàn bộ giá trị trong bảng
                            limit = totalEntities;
                        }
                        else
                        {
                            limit = (int)entityFilter.Limit;
                        }
                    }

                    reader.NextResult();
                    // Khởi tạo đối tượng chung TEntity
                    TEntity entity = Activator.CreateInstance<TEntity>();
                    // Danh sách các thuộc tính không hỗ trợ trong TEntity
                    List<string> propertyNoSupport = new() { "ServiceHobbes", "FoodServiceHobby", "ImageFile" };

                    // Sử dụng reflection để đặt giá trị cho các thuộc tính của TEntity lấy các thuộc tính của entity
                    PropertyInfo[] properties = typeof(TEntity).GetProperties();
                    if (reader.HasRows)
                    {
                        while (reader.Read()) // Đọc từng bản ghi trong MySqlDataReader
                        {
                            for (int i = 0; i < properties.Length; i++)
                            {
                                // Lấy tên của thuộc tính (của từng cột) từ dữ liệu trả về từ proc
                                PropertyInfo property = properties[i];
                                string propertyName = property.Name;

                                /* Vì trường GenderName không không là thuộc tính trong database mà chỉ là trường bổ sung thêm 
                                 * - Việc kiểm tra thuộc tính "GenderName" là bởi giá trị trả về từ Proc không có trường đó -> không lấy ra được "columnIndex"
                                 * */
                                if (!propertyNoSupport.Contains(propertyName))
                                {
                                    // Kiểm tra xem cột có tồn tại trong dữ liệu đọc được từ Proc hay không
                                    int columnIndex = reader.GetOrdinal(propertyName);
                                    if (columnIndex >= 0 && !reader.IsDBNull(columnIndex))
                                    {
                                        object value = reader.GetValue(columnIndex);
                                        property.SetValue(entity, value);
                                    }
                                }
                            }

                            // Thêm TEntity vào danh sách
                            entities.Add(entity);

                            // Tạo instance mới của TEntity để đọc bản ghi tiếp theo
                            entity = Activator.CreateInstance<TEntity>();
                        }
                    }

                    reader.NextResult();
                    if (reader.Read())
                    {
                        totalRecordsResult = (int)(Int64)reader.GetValue(0);
                    }
                }

                int totalPageByCondition = 1;
                if (sqlQuery.Trim() != "")
                {
                    totalPageByCondition = (int)Math.Ceiling((float)((float)totalRecordsResult / limit));
                }

                // Cấu hình dữ liệu trả về cho client
                entityFilterResult.TotalRecord = (int)(totalEntities);
                entityFilterResult.TotalPage = (int)Math.Ceiling((float)((float)totalEntities / limit));
                entityFilterResult.TotalPageByCondition = totalPageByCondition;
                entityFilterResult.TotalRecordsResult = totalRecordsResult;
                entityFilterResult.CurrentPage = (int)page;
                entityFilterResult.CurrentPageRecords = (int)limit;
                entityFilterResult.Data = entities.ToArray();

                await sqlConnection.CloseAsync();
                return entityFilterResult;
            }
            catch (Exception ex)
            {
                throw new InternalException(ex.Message);
            }
        }

        /// <summary>
        /// - Lấy thông tin thực thể theo id
        /// </summary>
        /// <param name="ids">Danh sách id</param>
        /// <returns>List<TEntity></returns>
        /// Created By: DDKhang (24/6/2023)
        public async Task<List<TEntity>> GetEntityById(string ids)
        {
            try
            {
                string tableName = typeof(TEntity).Name;
                // Khởi tạo danh sách thực thể để lưu lại toàn bộ thông tin được lọc theo id
                List<TEntity> entities = new();

                // Khởi tạo kết nối với MariaDb
                using var sqlConnection = await GetOpenConnectionAsync();

                // Lấy dữ liệu từ database
                //// === Cách 1: Sử dụng câu truy vấn ===
                //// 1. Câu lệnh truy vấn database
                //string sqlCommand = $"SELECT * FROM {tableName} WHERE {tableName}Id = @Id";
                //DynamicParameters parameters = new DynamicParameters();
                //parameters.Add("@Id", id);

                //// 2. Thực hiện lấy dữ liệu
                //var entity = await sqlConnection.QueryFirstOrDefaultAsync<TEntity>(sqlCommand, param: parameters);

                // === Cách 2: Gọi Stored Procedure ===
                // 1. Khởi tạo lệnh sql gọi đến Stored Procedure
                string sqlCommandProc = "Proc_Get" + tableName + "ByIds";

                // 2. Thực hiện thêm tham số cho proc
                MySqlCommand command = new MySqlCommand(sqlCommandProc, (MySqlConnection?)sqlConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue($"@m_{tableName}Ids", ids);

                // 3. Thực thi proc
                using MySqlDataReader reader = await command.ExecuteReaderAsync();

                // Khởi tạo đối tượng chung TEntity
                TEntity entity1 = Activator.CreateInstance<TEntity>();

                // Sử dụng reflection để đặt giá trị cho các thuộc tính của TEntity từ dữ liệu đọc được từ Proc
                PropertyInfo[] properties = typeof(TEntity).GetProperties();
                // Danh sách các thuộc tính không hỗ trợ trong TEntity
                //List<string> propertyNoSupport = new() { "ServiceHobbes", "FoodServiceHobby", "MenuGroupName", "FoodUnitName", "TypeFoodName" };
                List<string> propertyNoSupport = new() { "ServiceHobbes", "FoodServiceHobby", "ImageFile" };

                while (reader.Read())
                {
                    for (int i = 0; i < properties.Length; i++)
                    {
                        // Lấy tên của thuộc tính
                        PropertyInfo property = properties[i];
                        string propertyName = property.Name;

                        if (!propertyNoSupport.Contains(propertyName))
                        {
                            // Kiểm tra xem cột có tồn tại trong dữ liệu đọc được hay không
                            int columnIndex = reader.GetOrdinal(propertyName);
                            if (columnIndex >= 0 && !reader.IsDBNull(columnIndex))
                            {
                                object value = reader.GetValue(columnIndex);
                                property.SetValue(entity1, value);
                            }
                        }
                    }

                    // Thêm TEntity vào danh sách
                    entities.Add(entity1);

                    // Tạo instance mới của TEntity để đọc bản ghi tiếp theo
                    entity1 = Activator.CreateInstance<TEntity>();
                }
                // Đóng kết nối sql
                await sqlConnection.CloseAsync();
                // Trả về kết quả truy vấn cho client
                return await Task.FromResult(entities);
            }
            catch (Exception ex)
            {
                throw new InternalException(ex.Message);
            }
        }

        /// <summary>
        /// - Thực hiện tạo mã mới cho entity
        /// </summary>
        /// <returns>String</returns>
        /// <exception cref="InternalException"></exception>
        /// Created By: DDKhang (24/5/2023)
        public virtual async Task<string> NewEntityCode(string prefixEntity)
        {
            try
            {
                // Lấy tên của thực thể
                string tableName = typeof(TEntity).Name;
                // Khởi tạo kết nối với MariaDb
                using var sqlConnection = await GetOpenConnectionAsync();

                string sqlCommandProc = "Proc_New" + tableName + "Code";
                MySqlCommand command = new MySqlCommand(sqlCommandProc, (MySqlConnection?)sqlConnection);
                command.CommandType = CommandType.StoredProcedure;

                // Add the output parameter

                //command.Parameters.Add(new MySqlParameter("@prefix", MySqlDbType.VarChar, 255));
                //command.Parameters["@prefix"].Direction = ParameterDirection.Input;
                //command.Parameters.Add(new MySqlParameter("@newCode", MySqlDbType.VarChar, 255));
                //command.Parameters["@newCode"].Direction = ParameterDirection.Output;

                var parameters = new DynamicParameters();
                parameters.Add("@prefix", prefixEntity);
                parameters.Add("@newCode", dbType: DbType.String, direction: ParameterDirection.Output);

                //// Thực thi stored Procedure
                //command.ExecuteNonQuery();


                // Gọi stored procedure và lấy kết quả
                sqlConnection.Execute(sqlCommandProc, parameters, commandType: CommandType.StoredProcedure);

                //string newEntityCode = command.Parameters["@newCode"].Value?.ToString() ?? "";

                // Lấy giá trị từ tham số đầu ra
                string newEntityCode = parameters.Get<string>("@newCode");

                // Đóng kết nối db
                await sqlConnection.CloseAsync();
                return newEntityCode;
            }
            catch (Exception ex)
            {
                throw new InternalException(ex.Message);
            }
        }

        /// <summary>
        /// - Hàm thực thể việc thêm thực thể
        /// </summary>
        /// <param name="entity">Thông tin thực thể thêm</param>
        /// <returns>Số lượng bản ghi đã thêm</returns>
        /// <exception cref="Exception"></exception>
        /// Created By: DDKhang (24/6/2023)
        public async Task<int> CreateEntity(TEntity entity)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    // Tạo id mới cho nhân viên mới
                    //entity.EmployeeId = Guid.NewGuid();
                    //entity.CreatedBy = "abc";
                    //string inputDate = entity.IdentityDate;

                    // Lấy tên của entity
                    string tableName = typeof(TEntity).Name;

                    // Kết nối database
                    using var sqlConnection = await GetOpenConnectionAsync();
                    //var transaction = sqlConnection.BeginTransaction();
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
                    scope.Dispose();
                    throw new InternalException(ex.Message);
                }
            }
        }

        /// <summary>
        /// - Hàm cập nhật thông tin thực thể
        /// </summary>
        /// <param name="entity">Thông tin thực thể cập nhật</param>
        /// <returns>Số bản ghi đã cập nhật</returns>
        /// <exception cref="Exception"></exception>
        /// Created By: DDKhang (24/6/2023)
        public async Task<int> UpdateEntity(TEntity entity)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string tableName = typeof(TEntity).Name;

                    // Kết nối database
                    using var mySqlConnection = await GetOpenConnectionAsync();

                    string sqlCommandProc = "Proc_Update" + tableName;
                    // Đọc các tham số đầu vào của stor
                    var sqlCommand = mySqlConnection.CreateCommand();
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
                    // Lấy số lượng bản ghi được cập nhật
                    var result = mySqlConnection.Execute(sql: sqlCommandProc, param: dynamicParam, commandType: System.Data.CommandType.StoredProcedure);
                    scope.Complete();
                    return await Task.FromResult(result);
                }
                catch (Exception ex)
                {
                    throw new InternalException(ex.Message);
                }
            }
        }

        /// <summary>
        /// - Thực hiện xóa nhiều bản ghi theo mã thực thể
        /// </summary>
        /// <param name="listEntityId">Danh sách mã thực thể</param>
        /// <returns>Số lượng đã xóa</returns>
        /// <exception cref="InternalException"></exception>
        /// Created By: DDKhang (24/6/2023)
        public async Task<int> DeleteMultiEntity(string listEntityId)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    // Thực hiện cấu trúc lại chuỗi xóa theo dạng "a,b"
                    StringBuilder formatString = new StringBuilder();
                    List<string> listId = listEntityId.Split(',').Select(s => s.Trim()).ToList();
                    formatString = formatString.Append(string.Join(",", listId));
                    string formatResult = formatString.ToString();

                    // Lấy tên của entity
                    string tableName = typeof(TEntity).Name;
                    // Khởi tạo kết nối với MariaDb
                    using var sqlConnection = await GetOpenConnectionAsync();

                    // Khởi tạo lệnh sql
                    string sqlCommandProc = "Proc_Delete" + tableName + "MultiById";

                    MySqlCommand command = new MySqlCommand(sqlCommandProc, (MySqlConnection?)sqlConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue($"@m_List{tableName}Id", formatResult.Trim());
                    // Thực thi proc
                    int result = await command.ExecuteNonQueryAsync();

                    // Commit Transaction
                    scope.Complete();
                    return result;
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new InternalException(ex.Message);
                }
                finally
                {

                }
            }
        }

        /// <summary>
        /// - Thực hiện kiểm tra mã trùng lặp
        /// </summary>
        /// <param name="entityCode">Mã thực thể</param>
        /// <returns>Bool</returns>
        /// Created By: DDKhang (24/6/2023)
        public virtual async Task<int> CheckDuplicateCode(string entityCode)
        {
            return 0;
        }
    }
}
