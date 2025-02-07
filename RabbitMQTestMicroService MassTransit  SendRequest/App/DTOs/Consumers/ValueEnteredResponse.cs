namespace App.DTOs.Consumers
{
    public class ValueEnteredResponse
    {
        public string Result { get; set; }

        public string ReceivedValue { get; set; }
        public DateTime ProcessedAt { get; set; }
    }
}
