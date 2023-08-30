namespace MAUtour.Local.Models
{
    public class Routes
    {
        public Routes() 
        { 
            Pins = new HashSet<RoutePins>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public RouteTypes RouteType { get; set; }
        public int RouteTypeId { get; set; }
        public ICollection<RoutePins> Pins { get; set; }
    }
}
