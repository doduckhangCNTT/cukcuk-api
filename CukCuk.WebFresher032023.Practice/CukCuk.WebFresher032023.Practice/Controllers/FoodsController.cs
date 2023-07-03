using CukCuk.WebFresher032023.BL.DTO.Foods;
using CukCuk.WebFresher032023.BL.Service.Bases;
using CukCuk.WebFresher032023.BL.Service.Foods;
using CukCuk.WebFresher032023.BL.Service.FoodServiceHobbes;
using CukCuk.WebFresher032023.Common.Model;
using CukCuk.WebFresher032023.DL.Entity;
using CukCuk.WebFresher032023.Practice.Model;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Text;

namespace CukCuk.WebFresher032023.Practice.Controllers
{
    [Route("api/v1/[controller]")]
    public class FoodsController : BaseController<Food, FoodDto, FoodUpdateDto, FoodCreateDto>
    {
        private readonly string _connectionString;

        public FoodsController(IConfiguration configuration, IFoodService foodService) : base(foodService)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        //[HttpGet]
        //public IActionResult GetFoods()
        //{
        //    using (var connection = new MySqlConnection(_connectionString))
        //    {
        //        connection.Open();

        //        string sqlQuery = "SELECT * FROM Food";

        //        var employees = connection.Query<Food>(sqlQuery);

        //        return Ok(employees);
        //    }
        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult<Food>> GetFoodById(Guid id)
        //{
        //    using (var connection = new MySqlConnection(_connectionString))
        //    {
        //        connection.Open();
        //        string sqlCommand = $"SELECT * FROM Food Where FoodId = '{id}'";
        //        var foods = connection.Query<Food>(sqlCommand);
        //        return Ok(foods);
        //    }
        //}

        //[HttpPost("image")]
        //public IActionResult Add([FromForm] Product model)
        //{
        //    var status = new Status();
        //    if (!ModelState.IsValid)
        //    {
        //        status.StatusCode = 0;
        //        status.Message = "Please pass the valid data";
        //        return Ok(status);
        //    }
        //    if (model.ImageFile != null)
        //    {
        //        var fileResult = _fileService.SaveImage(model.ImageFile);
        //        if (fileResult.Item1 == 1)
        //        {
        //            model.ProductImage = fileResult.Item2; // getting name of image
        //        }
        //        var productResult = _productRepo.Add(model);
        //        if (productResult)
        //        {
        //            status.StatusCode = 1;
        //            status.Message = "Added successfully";
        //        }
        //        else
        //        {
        //            status.StatusCode = 0;
        //            status.Message = "Error on adding product";

        //        }
        //    }
        //    return Ok(status);

        //}

        //[HttpDelete("image")]
        //public IActionResult Delete(string imageFileName)
        //{
        //    var fileResult = _fileService.DeleteImage(imageFileName);
        //    //var productResult = _productRepo.DeleteImage(imageFileName);
        //    return Ok();
        //}

    }
}
