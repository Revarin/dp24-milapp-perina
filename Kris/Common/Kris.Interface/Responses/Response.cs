using Microsoft.AspNetCore.Http;

namespace Kris.Interface.Responses;

public class Response
{
    public int Status { get; set; }
    public string? Message { set; get; }

    public bool IsSuccess() => Status == StatusCodes.Status200OK;
    public bool IsBadRequest() => Status == StatusCodes.Status400BadRequest;
    public bool IsNotFound() => Status == StatusCodes.Status404NotFound;
    public bool IsUnauthorized() => Status == StatusCodes.Status401Unauthorized;
    public bool IsInternalError() => Status == StatusCodes.Status500InternalServerError;
}
