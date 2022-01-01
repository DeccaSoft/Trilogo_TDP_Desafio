using Treinando.Data;
using Treinando.Controllers;

namespace Treinando.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; } = "";
        public string Neighborhood { get; set; } = "";
        public string Number { get; set; } = "";  //por ser um campo string, aceita o complemento junto
        public string City { get; set; } = "";
        public string State { get; set; } = "";
    }
}