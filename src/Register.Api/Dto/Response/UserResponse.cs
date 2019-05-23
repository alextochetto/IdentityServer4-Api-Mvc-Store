using System;
namespace Register.Api.Dto.Response
{
    public class UserResponse
    {
        public string BusinessId { get; set; }
        public string Subject { get; set; }
        public string Name { get; set; }
        public string GivenName { get; set; }
        public string ClientSecret { get; set; }
        public string Email { get; set; }
        public string Document { get; set; }
        public ShippingAddressResponse ClientShippingAddressApp { get; set; }
    }

    public class ShippingAddressResponse 
    {
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
    }
}
