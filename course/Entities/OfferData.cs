namespace course.Entities
{
    public class OfferData
    {
        public int Id { get; set; }
        public string PriceCurrency { get; set; }
        public string Price { get; set; }
        public override string ToString() => $"({Price}, {PriceCurrency})";
    }
}
