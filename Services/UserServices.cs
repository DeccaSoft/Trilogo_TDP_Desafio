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
            return _dbContext.Users.Include(u => u.Address).FirstOrDefault(u => u.Id == id);
        }

        public User GetUserByLogin(string login)
        {
            return _dbContext.Users.Include(u => u.Address).FirstOrDefault(u => u.Login == login);
        }

        public User GetUserWithOrders(string login)
        {
            return _dbContext.Users.Include(u => u.Address).Include(u => u.Orders).FirstOrDefault(u => u.Login == login);
        }

        public User CreateUser(User user)
        {
            var userModel = _dbContext.Users.FirstOrDefault(u => u.CPF.Equals(user.CPF) || u.Email.Equals(user.Email) || (u.Login.Equals(user.Login)));
            if (userModel != null)
            {
                return user; // BadRequest("Usuário já Cadastrado !");
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return user;
        }

        public User UpdateUser(User user)
        {
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
            return user;
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
        public List<User> SearchUsers([FromQuery] string term, int offset, int limit)
        {
            List<User> users = _dbContext.Users.Where(u => u.Name.Contains(term) || u.Login.Contains(term) || u.Email.Contains(term)).Skip(offset).Take(limit).ToList();
            return users;
        }
    }
}
