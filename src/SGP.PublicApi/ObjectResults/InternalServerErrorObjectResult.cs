using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace SGP.PublicApi.ObjectResults;

[DefaultStatusCode(StatusCodes.Status500InternalServerError)]
public class InternalServerErrorObjectResult : ObjectResult
{
    public InternalServerErrorObjectResult([ActionResultObjectValue] object value) : base(value)
    {
        StatusCode = StatusCodes.Status500InternalServerError;
    }
}