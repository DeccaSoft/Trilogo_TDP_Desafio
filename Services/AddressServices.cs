using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Treinando.Data;
using Treinando.Models;

namespace Aula6.Services
{
    public class AddressServices
    {
        private readonly DBContext _dbContext;
        public AddressServices(DBContext dbContext)
        {
            _dbContext = dbContext;
            //_dbContext.Database.EnsureCreated();
        }
        public List<Address> ListAddress()        //Lista Todos os Endereços
        {
            return _dbContext.Adresses.ToList();
        }

        public Address GetAddressById(int id)    //Retorna Endereço pelo Id
        {
            return _dbContext.Adresses.Find(id);
        }

        public List<Address> GetAddressByStreet(string street)    //Retorna Endereços pelo Nome da Rua
        {
            return _dbContext.Adresses.Where(a => a.Street.Contains(street)).ToList();
        }

        public List<Address> GetAddressByNeighborhood(string neighborhood)    //Retorna Endereços pelo Bairro
        {
            return _dbContext.Adresses.Where(n => n.Neighborhood.Contains(neighborhood)).ToList();
        }

        public Address CreateAddress(Address address)
        {
            var addressModel = _dbContext.Adresses.FirstOrDefault(a => a.Id.Equals(address.Id));
            if (addressModel != null)
            {
                return address; // BadRequest("Endereço já Cadastrado !");
            }
            _dbContext.Adresses.Add(address);
            _dbContext.SaveChanges();
            return address;
        }

        public Address UpdateAddress(Address address)
        {
            _dbContext.Adresses.Update(address);
            _dbContext.SaveChanges();
            return address;
        }

        public bool DeleteAddress(int id)
        {
            var address = _dbContext.Adresses.Find(id);
            if (address != null)
            {
                _dbContext.Adresses.Remove(address);
                _dbContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }

        //Lista endereços que contenham o 'term' em sua Rua, Número, Bairro, Cidade ou Estado... Paginando a partir do endereço 'offset', listando de 'limit' em 'limit' endereços
        public List<Address> SearchAddress(string term, int offset, int limit)
        {
            List<Address> adresses = _dbContext.Adresses.Where(a => a.Street.Contains(term) || a.Number.Contains(term) || a.Neighborhood.Contains(term) || a.City.Contains(term) || a.State.Contains(term)).Skip(offset).Take(limit).ToList();
            return adresses;
        }
    }
}
