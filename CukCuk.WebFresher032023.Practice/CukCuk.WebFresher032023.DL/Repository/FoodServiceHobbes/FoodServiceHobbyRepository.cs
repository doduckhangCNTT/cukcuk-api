using CukCuk.WebFresher032023.DL.Entity;
using CukCuk.WebFresher032023.DL.Repository.Bases;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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
    }
}
