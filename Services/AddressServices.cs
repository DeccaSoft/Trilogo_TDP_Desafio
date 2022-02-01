using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aula6.Interfaces;
using Treinando.Data;
using Treinando.Models;

namespace Aula6.Services
{
    public class AddressServices : IAddressService
    {
        private readonly DBContext _dbContext;
        public AddressServices(DBContext dbContext)
        {
            _dbContext = dbContext;
            
        }
        public List<Address> ListAddress()        
        {
            return _dbContext.Adresses.ToList();
        }

        public Address GetAddressById(int id)    
        {
            return _dbContext.Adresses.Find(id);
        }

        public List<Address> GetAddressByStreet(string street)    
        {
            return _dbContext.Adresses.Where(a => a.Street.Contains(street)).ToList();
        }

        public List<Address> GetAddressByNeighborhood(string neighborhood)   
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

        
        public Address UpdateAddress(Address address)  
        {
            var addressModel = _dbContext.Adresses.Find(address.Id);
            
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
