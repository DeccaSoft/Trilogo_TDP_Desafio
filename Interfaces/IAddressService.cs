using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Treinando.Models;

namespace Aula6.Interfaces
{
    public interface IAddressService
    {
        List<Address> ListAddress();
        Address GetAddressById(int id);
        List<Address> GetAddressByStreet(string street);
        List<Address> GetAddressByNeighborhood(string neighborhood);
        bool CreateAddress(Address address);
        Address UpdateAddress(Address address);
        bool DeleteAddress(int id);
        List<Address> SearchAddress(string searchTerm, int initialRecord, int limitPerPage);
    }
}
