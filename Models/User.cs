using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Treinando.Data;
using Treinando.Controllers;

namespace Treinando.Models
{
    public class User
    {
        public int Id { get; set; }

        //[Required(ErrorMessage = "O Campo Nome é Obrigatório.")]
        public string Name { get; set; }

        //[Required(ErrorMessage = "O Campo Login é Obrigatório.")]
        public string Login { get; set; }

        //[Required(ErrorMessage = "O Campo Senha é Obrigatório.")]
        public string Password { get; set; }

        //[Required(ErrorMessage = "O Campo CPF é Obrigatório.")]
        public string CPF { get; set; }

        //[Required(ErrorMessage = "O Campo E-Mail é Obrigatório.")]
        public string Email { get; set; }

        public DateTime Birthday { get; set; }

        public Address? Address { get; set; }
        public List<Order>? Orders { get; set; }

    }
}