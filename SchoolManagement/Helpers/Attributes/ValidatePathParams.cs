using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Helpers.DTOs;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace SchoolManagement.Helpers.Attributes
{
    public class ValidatePathParams : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.ContainsKey("courseId"))
            {
                var courseId = context.ActionArguments["courseId"] as string;

                if (string.IsNullOrEmpty(courseId) || !Guid.TryParse(courseId, out _))
                {
                    context.Result = new BadRequestObjectResult(new BaseResponse
                    {
                        ResponseCode = "01",
                        ResponseMessage = "Invalid Course ID"
                    });
                }
            }
            else if (context.ActionArguments.ContainsKey("studentId"))
            {
                var courseId = context.ActionArguments["studentId"] as string;
                string pattern = @"[<>&'$=]|(\bOR\b)";

                if (string.IsNullOrEmpty(courseId) || Regex.IsMatch(courseId, pattern, RegexOptions.IgnoreCase))
                {
                    context.Result = new BadRequestObjectResult(new BaseResponse
                    {
                        ResponseCode = "01",
                        ResponseMessage = "Invalid Student ID"
                    });
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
