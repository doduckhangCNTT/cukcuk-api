using CukCuk.WebFresher032023.BL.Service.FoodProcessingPlaces;
using CukCuk.WebFresher032023.BL.Service.Foods;
using CukCuk.WebFresher032023.BL.Service.FoodServiceHobbes;
using CukCuk.WebFresher032023.BL.Service.FoodUnits;
using CukCuk.WebFresher032023.BL.Service.MenuGroups;
using CukCuk.WebFresher032023.DL.Repository.FoodProcessingPlaces;
using CukCuk.WebFresher032023.DL.Repository.Foods;
using CukCuk.WebFresher032023.DL.Repository.FoodServiceHobbes;
using CukCuk.WebFresher032023.DL.Repository.FoodUnits;
using CukCuk.WebFresher032023.DL.Repository.MenuGroups;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Thực hiện cấu hình viết PascalCase khi dữ liệu trả về từ Json
builder.Services.AddControllers().AddJsonOptions(option =>
{
    option.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddControllers();

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Tiêm phụ thuộc của AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

/*
 - Mỗi lần thực hiện một IEmployeeRepository thì tương ứng sẽ có lớp thể hiện của các phương thức đó
 - Khi mà mình có gọi những cái Repository này ở các tầng nào khác (Controller, BL, DL) thì đều chỉ khởi tạo 1 instant
 */
builder.Services.AddScoped<IFoodRepository, FoodRepository>();
builder.Services.AddScoped<IFoodService, FoodService>();

builder.Services.AddScoped<IFoodServiceHobbyRepository, FoodServiceHobbyRepository>();
builder.Services.AddScoped<IFoodServiceHobbyService, FoodServiceHobbyService>();

builder.Services.AddScoped<IFoodProcessingPlaceRepository, FoodProcessingPlaceRepository>();
builder.Services.AddScoped<IFoodProcessingPlaceService, FoodProcessingPlaceService>();

builder.Services.AddScoped<IFoodUnitRepository, FoodUnitRepository>();
builder.Services.AddScoped<IFoodUnitService, FoodUnitService>();

builder.Services.AddScoped<IMenuGroupRepository, MenuGroupRepository>();
builder.Services.AddScoped<IMenuGroupService, MenuGroupService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(builder => builder
    .WithOrigins("http://127.0.0.1:5173")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
