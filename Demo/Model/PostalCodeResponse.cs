namespace Demo.Model
{
    public class PostalCodeResponse
    {
        public string Message { get; set; }
        public string Status { get; set; }
        public PostOffice[] PostOffice { get; set; }
    }
}
