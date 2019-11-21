namespace iwor.api.DTOs
{
    public class PriceRaiseDto : NewPriceRaiseDto
    {
        public string RaisedUserId { get; set; }
        public string Date { get; set; }
    }
}