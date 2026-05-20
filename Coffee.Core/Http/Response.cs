namespace Coffee.Core.Http;

public class Response<T>
{
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
    public bool IsSuccess => Errors.Count == 0;
}