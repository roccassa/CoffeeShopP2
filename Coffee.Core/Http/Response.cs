namespace Coffee.Core.Http;

public class Response <T>
{
    public T Data { get; set; } //la T es de tipo generico, puede ser de cualquier tipo
    public string Message { get; set; } = ""; //inicializado vacio
    public List<string> Errors { get; set; } = new List<string>(); //inicializado 
    public bool Success { get; set; }
}