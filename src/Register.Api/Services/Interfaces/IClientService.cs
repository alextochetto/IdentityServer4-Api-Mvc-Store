using System;
using System.Threading.Tasks;
using Register.Api.Dto.Request;
using Register.Api.Dto.Response;

namespace Register.Api.Services.Interfaces
{
    public interface IClientService
    {
        bool CreateAccountUser(CreateUserRequestDto createUserRequestDto);

        bool UpdateAccountWithAddress(ClientShippingAddressRequestDto clientShippingAddressRequestDto);

        UserResponse GetClient();
    }
}
