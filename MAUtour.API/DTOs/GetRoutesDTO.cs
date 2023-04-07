namespace MAUtour.API.DTOs
{
    public class GetRoutesDTO
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string? Description { get; set; }
        public PinsDTO Pins { get; set; }
    }
}
