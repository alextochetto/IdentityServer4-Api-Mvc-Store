using System;
using Newtonsoft.Json;

namespace Register.Api.Dto.Request
{
    public class ClientShippingAddressRequestDto
    {
        [JsonIgnore]
        public string Name { get; set; }
        [JsonProperty("street_address")]
        public string Address1 { get; set; }
        [JsonIgnore]
        public string Address2 { get; set; }
        [JsonProperty("locality")]
        public string City { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("postal_code")]
        public string Zip { get; set; }
        [JsonIgnore]
        public string Phone { get; set; }
    }
}
