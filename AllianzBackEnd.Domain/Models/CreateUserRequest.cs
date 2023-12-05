using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllianzBackEnd.Domain.Models
{
    public record CreateUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string DateOfBirth { get; set; }

    }

    public record LoginRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
