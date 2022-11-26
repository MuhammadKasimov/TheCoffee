namespace StarBucks.Service.DTOs
{
    public class UserForViewDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public AddressForCreationDTO Address { get; set; }
    }
}
