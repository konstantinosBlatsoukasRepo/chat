namespace Chat.Models
{
    public class MessageIn
    {
        public string Type { get; set; }

        public string Nickname { get; set; }

        public int Timestamp { get; set; }

        public string Message { get; set; }
    }
}
