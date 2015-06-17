namespace InfrastructureLayer.Model
{
    public class ResponseMessage
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
        public string UrlToGo { get; set; }
        public string Title { get; set; }
        public object Data { get; set; }
    }
}
