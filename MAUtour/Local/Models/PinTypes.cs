namespace MAUtour.Local.Models
{
    public class PinTypes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Pins> Pins { get; set; }
    }
}
