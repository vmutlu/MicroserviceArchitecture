namespace DotnetMicroserviceArchitecture.Order.Application.DTOs
{
    public  class AdressDTO
    {
        public string City { get; private set; }
        public string Town { get; private set; }
        public string Street { get; private set; }
        public string ZipCode { get; private set; }
        public string Line { get; private set; }
    }
}
