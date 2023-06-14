namespace CampCorr.Models
{
    public class Logs
    {
        public int Id { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Login { get; set; }
        public string Ip { get; set; }
        public DateTime Horario { get; set; }
        public bool IsError { get; set; }
    }
}
