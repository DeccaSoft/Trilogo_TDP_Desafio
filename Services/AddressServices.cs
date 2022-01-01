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

        public bool CreateAddress(Address address)
        {
            if (_dbContext.Adresses.FirstOrDefault(a => a.Id.Equals(address.Id)) != null
                || _dbContext.Adresses.FirstOrDefault(a => a.Street.Equals(address.Street) && a.Neighborhood.Equals(address.Neighborhood) 
                    && a.Number.Equals(address.Number) && a.City.Equals(address.City) && a.State.Equals(address.State)) != null )
            {
                return false;
            }
            _dbContext.Adresses.Add(address);
            _dbContext.SaveChanges();
            return true;
        }

        
        public Address UpdateAddress(Address address)  //Address
        {
            var addressModel = _dbContext.Adresses.Find(address.Id);
            //_dbContext.Products.Update(product);
            _dbContext.Entry(addressModel).CurrentValues.SetValues(address);
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
        public List<Address> SearchAddress(string searchTerm, int initialRecord, int limitPerPage)
        {
            List<Address> adresses = _dbContext.Adresses.Where(a => a.Street.Contains(searchTerm) || a.Number.Contains(searchTerm) || a.Neighborhood.Contains(searchTerm) || a.City.Contains(searchTerm) || a.State.Contains(searchTerm)).Skip(initialRecord).Take(limitPerPage).ToList();
            return adresses;
        }
    }
}
