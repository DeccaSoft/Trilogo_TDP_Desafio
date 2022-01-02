using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Treinando.Data;
using Treinando.Models;

namespace Aula6.Services
{
    public class UserServices
    {
        private readonly DBContext _dbContext;
        public UserServices(DBContext dbContext)
        {
            _dbContext = dbContext;
            //_dbContext.Database.EnsureCreated();
        }

        public List<User> GetListUsers()
        {
            return _dbContext.Users.Include(u => u.Address).ToList();
    }

        public User GetUserById(int id)
        {
            return _dbContext.Users.Include(u => u.Address).Include(u => u.Orders).FirstOrDefault(u => u.Id == id);
        }

        public User GetUserByLogin(string login)
        {
            return _dbContext.Users.Include(u => u.Address).Include(u => u.Orders).FirstOrDefault(u => u.Login == login);
        }

        public List<Order> GetUserWithOrders(int userId)
        {

            //return _dbContext.Users.Include(u => u.Orders).Where(u => u.Login == login).ToList();
            return _dbContext.Orders.Where(u => u.UserId == userId).ToList();
        }

        public bool CreateUser(User user)
        {
            if (_dbContext.Users.FirstOrDefault(u => u.Id.Equals(user.Id)) != null
                || _dbContext.Users.FirstOrDefault(u => u.Name.Equals(user.Name) && u.Login.Equals(user.Login)
                    || u.CPF.Equals(user.CPF) || u.Email.Equals(user.Email) && u.Login.Equals(user.Login)) != null)
            {
                return false;
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return true;
        }

        public User UpdateUser(User user)
        {
            var userModel = _dbContext.Users.Find(user.Id);
            //var addressModel = _dbContext.Adresses.Find(user.Address.Id);
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            //_dbContext.Users.Update(user);
            _dbContext.Entry(userModel).CurrentValues.SetValues(user);
            //_dbContext.Entry(addressModel).CurrentValues.SetValues(user.Address);
            _dbContext.SaveChanges();
            return user;
        }
         

        public bool UpdateUserAddress(int userId, int addressId)
        {
            var user = _dbContext.Users.Find(userId);
            var address = _dbContext.Adresses.Find(addressId);
            if(user is null || address is null) { return false; }
            user.Address = address;
            _dbContext.Users.Update(user);
            _dbContext.Adresses.Update(address);
            _dbContext.SaveChanges();
            return true;
        }
        

        public bool DeleteUser(int id)        
        {
            var user = _dbContext.Users.Find(id);
            //Checa se usuario existe e se Existe Algum Pedido Cadastrado para o Usuário
            bool hasOrderRegistered = _dbContext.Orders.Any(o => o.UserId == id);
            if (user != null && !hasOrderRegistered)
            {
                _dbContext.Users.Remove(user);
                _dbContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        //Lista usuários que contenham o 'term' em seu Nome, Login ou Email... Paginando a partir do usuário 'offset', listando de 'limit' em 'limit' usuários
        public List<User> SearchUsers([FromQuery] string searchTerm, int initialRecord, int limitPerPage)
        {
            List<User> users = _dbContext.Users.Where(u => u.Name.Contains(searchTerm) || u.Login.Contains(searchTerm) || u.Email.Contains(searchTerm)).Skip(initialRecord).Take(limitPerPage).ToList();
            return users;
        }

        public User GetUserByCPF(string cpf)
        {
            return _dbContext.Users.Include(u => u.Address).Include(u => u.Orders).FirstOrDefault(u => u.CPF == cpf);
        }

        public User GetUserByEmail(string email)
        {
            return _dbContext.Users.Include(u => u.Address).Include(u => u.Orders).FirstOrDefault(u => u.Email == email);
        }

        public List<User> GetUsersByBirthday(string birthday)
        {
            return _dbContext.Users.Include(u => u.Address).Include(u => u.Orders).Where(u => u.Birthday.ToString().Contains(birthday)).ToList();
        }
    }
}
