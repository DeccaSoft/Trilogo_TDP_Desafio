using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aula6.Requests
{
    public class LoginRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
