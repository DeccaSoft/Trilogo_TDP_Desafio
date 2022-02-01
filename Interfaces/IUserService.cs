using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Treinando.Models;

namespace Aula6.Interfaces
{
    public interface IUserService
    {
        List<User> GetListUsers();
        User GetUserById(int id);
        User GetUserByLogin(string login);
        List<Order> GetUserWithOrders(int userId);
        bool CreateUser(User user);
        User UpdateUser(User user);
        bool UpdateUserAddress(int userId, int addressId);
        bool DeleteUser(int id);
        List<User> SearchUsers([FromQuery] string searchTerm, int initialRecord, int limitPerPage);
        User GetUserByCPF(string cpf);
        User GetUserByEmail(string email);
        List<User> GetUsersByBirthday(string birthday);

    }
}
