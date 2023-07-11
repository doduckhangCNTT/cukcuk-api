using AutoMapper;
using CukCuk.WebFresher032023.BL.DTO.Foods;
using CukCuk.WebFresher032023.BL.Service.Bases;
using CukCuk.WebFresher032023.BL.Service.FilesImage;
using CukCuk.WebFresher032023.BL.Service.FoodServiceHobbes;
using CukCuk.WebFresher032023.BL.Service.FoodUnits;
using CukCuk.WebFresher032023.BL.Service.ServiceHobbes;
using CukCuk.WebFresher032023.Common.ExceptionsError;
using CukCuk.WebFresher032023.Common.Resources;
using CukCuk.WebFresher032023.DL.Entity;
using CukCuk.WebFresher032023.DL.Model;
using CukCuk.WebFresher032023.DL.Repository.Bases;
using CukCuk.WebFresher032023.DL.Repository.Foods;
using CukCuk.WebFresher032023.DL.Repository.FoodServiceHobbes;
using CukCuk.WebFresher032023.DL.Repository.FoodUnits;
using CukCuk.WebFresher032023.DL.Repository.ServiceHobbes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.BL.Service.Foods
{
    public class FoodService : BaseService<Food, FoodDto, FoodUpdateDto, FoodCreateDto>, IFoodService
    {
        #region Filed
        private readonly IFoodRepository _foodRepository;
        private readonly IServiceHobbyRepository _serviceHobbyRepository;
        private readonly IFoodServiceHobbyRepository _foodServiceHobbyRepository;
        private IFoodServiceHobbyService _foodServiceHobby;
        private IServiceHobbyService _serviceHobby;
        private IMapper _mapper;
        private IFileService _fileService;

        #endregion

        #region Constructor
        public FoodService(IFoodRepository foodRepository, IFileService fileService, IServiceHobbyRepository serviceHobbyRepository, IFoodServiceHobbyService foodServiceHobbyService, IServiceHobbyService serviceHobbyService, IFoodServiceHobbyRepository foodServiceHobbyRepository, IMapper mapper) : base(foodRepository, mapper)
        {
            _foodRepository = foodRepository;
            _mapper = mapper;
            _foodServiceHobby = foodServiceHobbyService; // Truyền DI 
            _serviceHobby = serviceHobbyService; // Truyền DI 
            _serviceHobbyRepository = serviceHobbyRepository; // Truyền DI 
            _foodServiceHobbyRepository = foodServiceHobbyRepository; // Truyền DI 
            _fileService = fileService;


        }
        #endregion

        /// <summary>
        /// - Thực hiện lấy thông tin food theo id
        /// </summary>
        /// <param name="ids">Danh sách Id Food</param>
        /// <returns>Thông tin food</returns>
        /// - Author: DDKhang (30/6/2023)
        public override async Task<List<FoodDto>> GetAsync(string ids)
        {
            try
            {
                // Lấy thông tin food theo id
                List<Food> foods = await _foodRepository.GetAsync(ids);
                Food food = foods[0];
                // Lấy danh sách id các sở thích phục vụ thuộc foodId tương ứng
                List<FoodServiceHobby> foodServiceHobbes = await _foodServiceHobby.GetFoodServiceHobby(ids);

                if (foodServiceHobbes != null)
                {
                    // Thêm thông tin các dịch vụ sở thích cho food
                    food.FoodServiceHobby = foodServiceHobbes;
                }

                // Ánh xạ các trường -> Dto
                List<FoodDto> foodsDto = _mapper.Map<List<FoodDto>>(foods);
                return foodsDto;
            }
            catch (Exception ex)
            {
                throw new InternalException(ex.Message);
            }

        }

        /// <summary>
        /// - Thực hiện thêm mới food, thêm các thông tin vào các bảng liên quan
        /// </summary>
        /// <param name="food"></param>
        /// <returns>Số bản ghi được thêm</returns>
        /// - Author: DDKhang (28/6/2023)
        public override async Task<int> CreateAsync(FoodCreateDto foodCreateDto)
        {

            await ValidateCreate(foodCreateDto);
            List<string> validateErrors = new List<string>();

            //using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            //{
            try
            {
                Food food = _mapper.Map<Food>(foodCreateDto);
                // Danh sách chứa toàn bộ mảng id serviceHobby
                List<string> serviceHobbyIds = new();

                // Thực hiện thêm giá trị bảng Food
                Guid newGuidFood = Guid.NewGuid();
                food.FoodId = newGuidFood;

                // Kiểm tra hình ảnh có được đẩy lên
                if (food.ImageFile != null && (food.Image == null || food.Image == ""))
                {
                    var fileResult = _fileService.SaveImage(foodCreateDto.ImageFile);

                    if (fileResult.Item1 == 1)
                    {
                        food.Image = fileResult.Item2; // getting name of image
                    }
                }

                // Thực hiện thêm mới food
                await _foodRepository.CreateAsync(food);

                if (food.ServiceHobbes?.Count > 0)
                {
                    // Thưc hiện thêm các giá trị vào bảng ServiceHobby
                    food.ServiceHobbes?.ForEach(async sh =>
                    {
                        if (sh.ServiceHobbyId != Guid.Empty)
                        {
                            // Nếu mà trong danh sách service hobby có id thì "là đã chọn"
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
                            if(sh.MoreMoney < 0 ) {
                                validateErrors.Add("Trường thêm tiền phải là một số dương.");
                            }
                            int qualityCreated = await _serviceHobby.CreateServiceHobby(sh);
                            if (qualityCreated <= 0)
                            {
                                return;
                            }

                            if(validateErrors.Count > 0)
                            {
                                throw new ValidateException(validateErrors, ResourceVN.Validate_DevMessage);
                            }

                        }
                    });

                    // Chứa toàn bộ các id sở thích dịch vụ của food tương ứng
                    string serviceHobbesIds = string.Join(',', serviceHobbyIds);

                    return await _serviceHobbyRepository.CreateAsyncServiceHobby(newGuidFood, serviceHobbesIds);
                }
                return 1;
            }
            catch (ValidateException ex)
            {
                throw ex;
                //throw new InternalException(ex.Message);
            }
        }

        /// <summary>
        /// - Thực hiện cập nhật thông tin thực thể
        /// </summary>
        /// <param name="entityUpdateDto">Thông tin thực thể cập nhật</param>
        /// <returns>Số bản ghi đã cập nhật</returns>
        /// - Author: DDKhang (28/6/2023)
        public override async Task<int> UpdateAsync(FoodUpdateDto entityUpdateDto)
        {
            try
            {
                await ValidateUpdate(entityUpdateDto);

                Food food = _mapper.Map<Food>(entityUpdateDto);

                // Kiểm tra hình ảnh có được đẩy lên
                if (food.ImageFile != null)
                {
                    if ((entityUpdateDto.Image?.Trim() == "" || entityUpdateDto.Image == null))
                    {
                        // Cập nhật hình ảnh mới
                        var fileResult = _fileService.SaveImage(entityUpdateDto.ImageFile);

                        if (fileResult.Item1 == 1)
                        {
                            food.Image = fileResult.Item2; // getting name of image
                        }
                    }
                    else if (entityUpdateDto.Image?.Trim() != "" && entityUpdateDto.Image != null)
                    {
                        // Xóa hình ảnh trong thư mục Uploads
                        bool isDeleteImg = _fileService.DeleteImage(entityUpdateDto.Image);
                        if (isDeleteImg)
                        {
                            // Cập nhật hình ảnh mới
                            var fileResult = _fileService.SaveImage(entityUpdateDto.ImageFile);

                            if (fileResult.Item1 == 1)
                            {
                                food.Image = fileResult.Item2; // getting name of image
                            }
                        }
                    }
                }
                else
                {
                    if (entityUpdateDto.Image?.Trim() == "" && entityUpdateDto.Image == null)
                    {
                        bool isDeleteImg = _fileService.DeleteImage(entityUpdateDto.Image);
                        if (isDeleteImg)
                        {
                            food.Image = "";
                        }
                    }
                }

                // Cập nhật thông tin trên bảng Food
                int qualityUpdate = await _foodRepository.UpdateAsync(food);

                // Cập nhật thông tin trên bảng FoodServiceHobby (bảng chung)
                Guid foodId = (Guid)food.FoodId;
                // Kiểm foodId có tồn tại trong bảng chung gian
                List<FoodServiceHobby> foods = await _foodServiceHobby.GetFoodServiceHobby(foodId.ToString());
                if (foods.Count > 0)
                {
                    await _foodServiceHobby.DeleteFoodServiceHobby(foodId.ToString());
                }

                // Kiểm tra hình ảnh có được cập nhật


                // Nếu có giá trị của mảng servicesHobby truyền lên -> thực hiện thêm vào bảng chung gian
                if (food.ServiceHobbes?.Count > 0)
                {
                    // Danh sách chứa toàn bộ mảng id serviceHobby
                    List<string> serviceHobbyIds = new();

                    food.ServiceHobbes?.ForEach(async sh =>
                    {
                        if (sh.ServiceHobbyId != Guid.Empty)
                        {
                            //// Kiểm tra giá trị các trường có sự thay đổi không
                            //List<ServiceHobby> serviceHobbies = await _serviceHobby.GetAsyncServiceHobby(sh.ServiceHobbyId.ToString());
                            //ServiceHobby serviceHobbyOld = serviceHobbies[0];

                            //if (serviceHobbyOld.ServiceHobbyName != sh.ServiceHobbyName || serviceHobbyOld.MoreMoney != sh.MoreMoney)
                            //{
                            //    // --- Nếu có thay đổi
                            //    // 1. Tạo mới serviceHobbyId
                            //    Guid newGuidServiceHobby = Guid.NewGuid();

                            //    // Xóa giá trị cũ
                            //    await _serviceHobby.DeleteMutilEntityAsync(serviceHobbyOld.ServiceHobbyId.ToString());
                            //    // 2. Cập nhật id mới
                            //    sh.ServiceHobbyId = newGuidServiceHobby;

                            //    // 3. Thêm vào mảng chứa toàn bộ id serviceHobby
                            //    serviceHobbyIds.Add(newGuidServiceHobby.ToString());
                            //    // Thêm mới giá trị cập nhật
                            //    int test = await _serviceHobby.CreateServiceHobby(sh);
                            //}
                            //else
                            //{
                            //    // --- Nếu không có thay đổi
                            //    // Kiểm tra xem đối tượng sở thích dịch vụ có chứa Id không -> chỉ lấy id đó
                            //    serviceHobbyIds.Add(sh.ServiceHobbyId.ToString());
                            //}

                            serviceHobbyIds.Add(sh.ServiceHobbyId.ToString());
                        }
                        else
                        {
                            // Thực hiện tạo serviceHobby mới
                            // 1. Tạo mới serviceHobbyId
                            Guid newGuidServiceHobby = Guid.NewGuid();

                            // 2. Cập nhật id mới
                            sh.ServiceHobbyId = newGuidServiceHobby;

                            // 3. Thêm vào mảng chứa toàn bộ id serviceHobby
                            serviceHobbyIds.Add(newGuidServiceHobby.ToString());
                            int test = await _serviceHobby.CreateServiceHobby(sh);
                        }
                    });

                    // Chứa toàn bộ các id sở thích dịch vụ của food tương ứng
                    string serviceHobbesIds = string.Join(',', serviceHobbyIds);
                    // Thêm các id vào bảng chung
                    int qualityCreated = await _foodServiceHobbyRepository.CreateFoodServiceHobby(foodId.ToString(), serviceHobbesIds);
                }

                return qualityUpdate;
            }
            catch (ValidateException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// - Xóa nhiều bản ghi (xóa hình ảnh nếu có)
        /// </summary>
        /// <param name="listEntityId">Danh sách mã thực thể</param>
        /// <returns>Số bản ghi đã xóa</returns>
        /// - Author: DDKhang (28/6/2023)
        public override async Task<int> DeleteMutilEntityAsync(string listEntityId)
        {
            try
            {
                // Thực hiện xóa toàn bộ hình ảnh đã được lưu
                List<Food> foods = await _foodRepository.GetMultiAsync(listEntityId);
                foods.ForEach(f =>
                {
                    if (f.Image != null && f.Image.Trim() != "")
                    {
                        string imageName = f.Image;
                        bool isDeleteImg = _fileService.DeleteImage(imageName);
                    }
                });

                // Xóa các bản ghi
                int qualityDelete = await _foodRepository.DeleteMutilEntityAsync(listEntityId);
                return qualityDelete;

            }
            catch (Exception ex)
            {
                throw new InternalException(ex.Message);
            }
        }

        /// <summary>
        /// - Thực hiện xác thực dữ liệu cho những trường khi thêm mới
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Task<bool></returns>
        /// Author: DDKhang (10/6/2023)
        public override async Task<bool> ValidateCreate(FoodCreateDto entity)
        {
            try
            {
                List<string> validateErrors = new List<string>();

                // Kiểm tra null
                if (entity == null)
                {
                    // Middleware
                    throw new InternalException(ResourceVN.Validate_NotFoundAssests);
                }

                int qualityDupEmployeeCode = await IsDuplicateCode(entity.FoodCode);
                // Thực hiện xác thực thông tin
                // 1. Kiểm tra rỗng (các trường yêu cầu bắt buộc nhập)
                // 1.1 Kiểm tra mã món ăn
                if (string.IsNullOrEmpty(entity.FoodCode))
                {
                    validateErrors.Add(ResourceVN.Validate_FoodCodeRequired);
                }
                else if (qualityDupEmployeeCode >= 1)
                {
                    //validateErrors.Add(ResourceVN.Validate_DuplicateFoodCode);
                    validateErrors.Add($"Mã <{entity.FoodCode}> đã tồn tại trên một trong các danh sách sau: Món ăn, đồ uống, combo, món khác, dịch vụ tính tiền theo thời gian.");
                }

                // 1.2 Kiểm tra tên món ăn
                if (string.IsNullOrEmpty(entity.FoodName))
                {
                    validateErrors.Add(ResourceVN.Validate_FoodNameRequired);
                }
                else if (entity.FoodName.Length > 50)
                {
                    validateErrors.Add(ResourceVN.Validate_FullNameLengthMax + "cần nhỏ hơn " + 50 + "kí tự.");
                }

                // 1.3 Kiểm tra đơn vị tính
                if (entity.FoodUnitId == Guid.Empty || string.IsNullOrEmpty(entity.FoodUnitId.ToString()))
                {
                    validateErrors.Add(ResourceVN.Validate_FoodUnitRequired);
                }

                // 1.4 Kiểm tra giá bán
                if (string.IsNullOrEmpty(entity.Price.ToString()))
                {
                    validateErrors.Add(ResourceVN.Validate_FoodPriceRequired);
                }
                else if (entity.Price < 0)
                {
                    validateErrors.Add(ResourceVN.Validate_PricePositive);
                }

                if (validateErrors.Count > 0)
                {
                    string devMessage = ResourceVN.Validate_DevMessage;
                    throw new ValidateException(validateErrors, devMessage);
                }
                return true;
            }
            catch (ValidateException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// - Thực hiện xác thực thông itn cho những trường khi cập nhật
        /// </summary>
        /// <param name="entity">Thông tin thực thể</param>
        /// <returns>Bool</returns>
        /// <exception cref="InternalException"></exception>
        /// Created By: DDKhang (24/6/2023)
        public override async Task<bool> ValidateUpdate(FoodUpdateDto entity)
        {
            try
            {
                List<string> validateErrors = new List<string>();

                // Kiểm tra null
                if (entity == null)
                {
                    // Middleware
                    throw new InternalException(ResourceVN.Validate_NotFoundAssests);
                }

                int qualityDupEmployeeCode = await IsDuplicateCode(entity.FoodCode);
                // Thực hiện xác thực thông tin
                // 1. Kiểm tra rỗng (các trường yêu cầu bắt buộc nhập)
                // 1.1 Kiểm tra mã món ăn
                if (string.IsNullOrEmpty(entity.FoodCode))
                {
                    validateErrors.Add(ResourceVN.Validate_FoodCodeRequired);
                }
                else if (qualityDupEmployeeCode > 1)
                {
                    validateErrors.Add(ResourceVN.Validate_DuplicateFoodCode);
                }

                // 1.2 Kiểm tra tên món ăn
                if (string.IsNullOrEmpty(entity.FoodName))
                {
                    validateErrors.Add(ResourceVN.Validate_FoodNameRequired);
                }
                else if (entity.FoodName.Length > 50)
                {
                    validateErrors.Add(ResourceVN.Validate_FullNameLengthMax + "cần nhỏ hơn " + 50 + "kí tự.");
                }

                // 1.3 Kiểm tra đơn vị tính
                if (entity.FoodUnitId == Guid.Empty || string.IsNullOrEmpty(entity.FoodUnitId.ToString()))
                {
                    validateErrors.Add(ResourceVN.Validate_FoodUnitRequired);
                }

                // 1.4 Kiểm tra giá bán
                if (string.IsNullOrEmpty(entity.Price.ToString()))
                {
                    validateErrors.Add(ResourceVN.Validate_FoodPriceRequired);
                }
                else if (entity.Price < 0)
                {
                    validateErrors.Add(ResourceVN.Validate_PricePositive);
                }

                // 4. Kiểm tra mã món ăn có trùng khớp với món ăn hiện tại
                if (entity.FoodId != Guid.Empty)
                {
                    List<Food> foods = await _foodRepository.GetAsync(entity.FoodId.ToString());
                    Food food = foods[0];
                    if (!food.FoodCode.Equals(entity.FoodCode))
                    {
                        validateErrors.Add(ResourceVN.Validate_DuplicateFoodCode);
                    }
                }

                if (validateErrors.Count > 0)
                {
                    string devMessage = ResourceVN.Validate_DevMessage;
                    throw new ValidateException(validateErrors, devMessage);
                }
                return true;
            }
            catch (ValidateException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// - Kiểm tra trùng lặp mã
        /// </summary>
        /// <param name="code">Mã kiểm tra</param>
        /// <returns>Task<bool></returns>
        /// - Author: DDKhang (10/6/2023)
        public async Task<int> IsDuplicateCode(string code)
        {
            int check = await _foodRepository.CheckDuplicateCode(code);
            return check;
        }
    }
}
