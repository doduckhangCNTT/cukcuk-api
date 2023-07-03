﻿using CukCuk.WebFresher032023.DL.Entity;
using CukCuk.WebFresher032023.DL.Repository.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.DL.Repository.FoodServiceHobbes
{
    public interface IFoodServiceHobbyRepository : IBaseRepository<FoodServiceHobby>
    {
        Task<int> CreateFoodServiceHobby(string foodId, string serviceHobbyIds);
    }
}
