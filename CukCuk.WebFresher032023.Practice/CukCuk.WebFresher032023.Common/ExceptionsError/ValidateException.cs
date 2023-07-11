using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.Common.ExceptionsError
{
    /// <summary>
    /// - Thực thực xác thực dữ liệu
    /// </summary>
    /// - Author: DDKhang (1/6/2023)
    public class ValidateException : Exception
    {
        #region Field
        public List<string> ValidateErrors;
        #endregion

        #region Constructor
        public ValidateException()
        {
        }

        public ValidateException(List<string> validateErrors, string devMessage) : base(devMessage)
        {
            ValidateErrors = validateErrors;
            DevMessage = devMessage;
        }
        #endregion

        public string? DevMessage { get; set; }

        //public int? ErrorCode { get; set; }
        //public List<string>? UserMessage { get; set; }
        //public string? TraceId { get; set; }
        //public string? MoreInfor { get; set; }
    }
}
