namespace OpenVid.Models
{
    public class AjaxResponse
    {
        public object Result { get; set; }
        public string Message { get; set; }
        public bool IsSuccess => string.IsNullOrWhiteSpace(Message);
    }
}
