using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using WebsiteTester.Application.Models;

namespace WebsiteTester.WebApi.Extensions
{
    public static class ResultExtention
    {
        public static ActionResult<TContract> ToApiResponseResult<TResult, TContract>(this Result<TResult> result, Func<TResult, TContract> mapper)
        {
            return result.Match<ActionResult<TContract>>(
            onSuccess: value =>
            {
                var result = mapper(value);
                return new OkObjectResult(result);
            },
            onFailure: error =>
            {
                var code = HttpStatusCode.InternalServerError;
                var results = string.Empty;
                switch (error)
                {
                    case ValidationException validationException:
                        results = JsonSerializer.Serialize(validationException.Message);
                        return new BadRequestObjectResult(results);

                    default:
                        return new ObjectResult(JsonSerializer.Serialize(error))
                        {
                            StatusCode = (int)code,
                        }; ;
                }
            });
        }
    }
}
