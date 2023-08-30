namespace MAUtour.Local.Models
{
    public class RoutePins
    {
        public int Id { get; set; }
        public Routes Routes { get; set; }
        public int RoutesId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
