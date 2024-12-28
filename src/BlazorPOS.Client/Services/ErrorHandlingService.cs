namespace BlazorPOS.Client.Services
{
    public class ErrorHandlingService
    {
        public string HandleHttpError(HttpResponseMessage response)
        {
            return response.StatusCode switch
            {
                HttpStatusCode.Unauthorized => "You are not authorized to perform this action.",
                HttpStatusCode.Forbidden => "You do not have permission to access this resource.",
                HttpStatusCode.NotFound => "The requested resource could not be found.",
                HttpStatusCode.BadRequest => "Invalid request. Please check your input.",
                HttpStatusCode.InternalServerError => "An unexpected server error occurred.",
                _ => "An unknown error occurred."
            };
        }
    }