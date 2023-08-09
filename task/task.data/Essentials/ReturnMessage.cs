namespace task.data.Essentials
{
    public class ReturnMessage
    {
        public ReturnMessage(bool IsDefault = false) { Success = IsDefault; }
        public string Message { get; set; } = "";
        public bool Success { get; set; }
        public object? Obj { get; set; }
       
    }
}
