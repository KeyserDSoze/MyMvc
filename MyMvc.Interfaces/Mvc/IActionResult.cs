namespace MyMvc.Interfaces
{
    public interface IActionResult
    {
        string Response { get; }
        string ContentType { get; }
    }
}
