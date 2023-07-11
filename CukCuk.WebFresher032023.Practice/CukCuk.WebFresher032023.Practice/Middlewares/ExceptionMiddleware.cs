using CukCuk.WebFresher032023.Common.ExceptionsError;
using CukCuk.WebFresher032023.Common.Resources;
using System.Net;

namespace CukCuk.WebFresher032023.Practice.Middlewares
{
    public class ExceptionMiddleware
    {
        #region Field
        private readonly RequestDelegate _next;
        #endregion

        #region Constructor
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        #endregion

        /// <summary>
        /// - Hàm sẽ được trong tất cả các trường hợp 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// CreatedBy: DDKhang (28/5/2023)
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // Tiếp tục đến các nhiệm vụ tiếp theo
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex); // Xảy ra lỗi
            }
        }

        /// <summary>
        /// - Thực hiện xử lí các ngoại lệ
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Thực hiện bắt các "Exceoption" tương ứng
            if (exception is NotFoundException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                await context.Response.WriteAsync(
                text: new BaseException()
                {
                    ErrorCode = context.Response.StatusCode,
                    //ErrorCode = ((NotFoundException)exception).ErrorCode,
                    UserMessage = ResourceVN.Validate_NotFoundAssests,
                    UserMessages = new List<string>() { ResourceVN.Validate_NotFoundAssests },
                    DevMessage = exception.Message,
                    TraceId = context.TraceIdentifier,
                    MoreInfor = exception.HelpLink
                }.ToString() ?? ""
                );
            }
            else if (exception is ValidateException validateException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                await context.Response.WriteAsync(
                text: new BaseException()
                {
                    ErrorCode = context.Response.StatusCode,
                    UserMessage = string.Join(", ", validateException.ValidateErrors),
                    UserMessages = validateException.ValidateErrors,
                    DevMessage = exception.Message,
                    TraceId = context.TraceIdentifier,
                    MoreInfor = exception.HelpLink
                }.ToString() ?? ""
                );
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                await context.Response.WriteAsync(
                text: new BaseException()
                {
                    ErrorCode = context.Response.StatusCode,
                    UserMessage = ResourceVN.Error_Exception,
                    UserMessages = new List<string>() { ResourceVN.Error_Exception },
                    DevMessage = exception.Message,
                    TraceId = context.TraceIdentifier,
                    MoreInfor = exception.HelpLink
                }.ToString() ?? ""
                );
            }
        }
    }
}
