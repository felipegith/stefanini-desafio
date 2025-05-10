namespace Stefanini.Application.Models.Response;

public class ResponseOutputModel
{
    public bool Status { get; init; }
    public string Message { get; init; }

    public ResponseOutputModel(bool status, string message)
    {
        Status = status;
        Message = message;
    }
}