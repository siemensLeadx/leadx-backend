using System;

namespace Application.Features.UserAccount.Queries
{
    public class UserDetailsResponseDTO
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public Guid role { get; set; }
    }
}
