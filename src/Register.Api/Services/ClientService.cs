using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Register.Api.Dto.Request;
using Register.Api.Dto.Response;
using Register.Api.Services.Interfaces;

namespace Register.Api.Services
{
    public class ClientService : IClientService
    {
        readonly UserManager<ApplicationUser> _userManager;

        readonly ClaimsPrincipal _claimsPrincipal;

        public ClientService(UserManager<ApplicationUser> userManager, IPrincipal principal) 
        {
            _userManager = userManager;
            _claimsPrincipal = principal as ClaimsPrincipal;
        }

        public bool CreateAccountUser(CreateUserRequestDto createUserRequestDto)
        {
            IdentityResult result = null;

            var newUser = new ApplicationUser
            {
                Email = createUserRequestDto.Email,
                UserName = createUserRequestDto.Email
            };

            result = _userManager.CreateAsync(newUser, createUserRequestDto.Password).Result;

            var user = _userManager.FindByEmailAsync(createUserRequestDto.Email).Result;

            result = _userManager.AddClaimsAsync(newUser, new Claim[]
            {
                new Claim(JwtClaimTypes.IdentityProvider, user?.Id),
                new Claim(JwtClaimTypes.Subject, createUserRequestDto.Subject),
                new Claim(JwtClaimTypes.Name, createUserRequestDto.Name),
                new Claim(JwtClaimTypes.GivenName, createUserRequestDto.GivenName),
                new Claim(JwtClaimTypes.FamilyName, createUserRequestDto.GivenName),
                new Claim(JwtClaimTypes.Email, createUserRequestDto.Email),
                new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                new Claim(JwtClaimTypes.WebSite, "http://zonaazul.com"),
                //new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                new Claim(JwtClaimTypes.AuthenticationTime, DateTimeOffset.UtcNow.ToEpochTime().ToString(), ClaimValueTypes.Integer),
                new Claim("business_id", createUserRequestDto.BusinessId.ToString()),
                new Claim(JwtClaimTypes.ZoneInfo, "E. South America Standard Time"),
                new Claim("document", createUserRequestDto.Document)
            }).Result;

            return result != null && result.Succeeded;
        }

        public bool UpdateAccountWithAddress(ClientShippingAddressRequestDto clientShippingAddressRequestDto)
        {
            IdentityResult result = null;

            var user = _userManager.GetUserAsync(_claimsPrincipal).Result;

            var jsonAddress = JsonConvert.SerializeObject(clientShippingAddressRequestDto);

            result = _userManager.AddClaimsAsync(user, new Claim[]
            {
                new Claim(JwtClaimTypes.PhoneNumber, clientShippingAddressRequestDto.Phone),
                new Claim(JwtClaimTypes.Address, jsonAddress, IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
            }).Result;

            return result != null && result.Succeeded;
        }

        public UserResponse GetClient() {
            var addressJson = _claimsPrincipal.FindFirstValue(JwtClaimTypes.Address);

            ShippingAddressResponse address = null;

            if (!string.IsNullOrEmpty(addressJson))
                address = JsonConvert.DeserializeObject<ShippingAddressResponse>(addressJson);

            var userResponse = new UserResponse
            {
                BusinessId = _claimsPrincipal.FindFirstValue("business_id"),
                Subject = _claimsPrincipal.FindFirstValue(JwtClaimTypes.Subject),
                Name = _claimsPrincipal.FindFirstValue(JwtClaimTypes.Name),
                GivenName = _claimsPrincipal.FindFirstValue(JwtClaimTypes.GivenName),
                Email = _claimsPrincipal.FindFirstValue(JwtClaimTypes.Email),
                Document = _claimsPrincipal.FindFirstValue("document"),
                ClientShippingAddressApp = address
            };

            return userResponse;
        }
    }
}
