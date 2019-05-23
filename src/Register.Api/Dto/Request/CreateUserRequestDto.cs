using System;
namespace Register.Api.Dto.Request
{
    public class CreateUserRequestDto
    {
        public int BusinessId { get; set; }
        public string Subject { get; set; }
        public string Name { get; set; }
        public string GivenName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Document { get; set; }
    }
}
