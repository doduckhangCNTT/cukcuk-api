using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.Common.Enum
{
    public enum HttpStatusCodeEnum
    {
        Success = 200,
        CreatedSuccess = 201,
        NoContent = 204,
        BadRequest = 400,
        NoAuthentication = 401,
        ServerError = 500,
    }
}
