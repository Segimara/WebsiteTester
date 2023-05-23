using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using WebsiteTester.Application.Models;

namespace WebsiteTester.WebApi.Extensions
{
    public static class ResultExtention
    {
        public static ActionResult<TResult> ToApiResponseResult<TResult>(this Result<TResult> result)
        {
            return result.Match<ActionResult<TResult>>(
            onSuccess: value => new OkObjectResult(value),
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
