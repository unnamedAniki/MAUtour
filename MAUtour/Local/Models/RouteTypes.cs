namespace MAUtour.Local.Models
{
    public class RouteTypes
    {
        public RouteTypes() 
        {
            Routes = new HashSet<Routes>();
            Pins = new HashSet<Pins>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Routes> Routes { get; set; }
        public ICollection<Pins> Pins { get; set; }
    }
}
