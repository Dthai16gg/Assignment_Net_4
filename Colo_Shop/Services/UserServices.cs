using Colo_Shop.IServices;
using Colo_Shop.Models;

namespace Colo_Shop.Services;

public class UserServices : IUserServices
{
    private readonly ShopDbContext _dbConText;

    public UserServices()
    {
        _dbConText = new ShopDbContext();
    }

    public bool CreateNewUsers(User User)
    {
        try
        {
            _dbConText.Users.Add(User);
            _dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool DeleteUser(Guid id)
    {
        try
        {
            var _User = _dbConText.Users.Find(id);
            _dbConText.Users.Remove(_User);
            _dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool UpdateUser(User User)
    {
        try
        {
            var _User = _dbConText.Users.Find(User.Id);
            _User.Username = User.Username;
            _User.Password = User.Password;
            _User.RoleId = User.RoleId;
            _User.Status = User.Status;
            _User.Name = User.Name;
            _User.Email = User.Email;
            _User.NumberPhone = User.NumberPhone;
            _dbConText.Users.Update(_User);
            _dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public List<User> GetAllUsers()
    {
        return _dbConText.Users.ToList();
    }

    public User GetUserById(Guid id)
    {
        return _dbConText.Users.FirstOrDefault(p => p.Id == id);
    }

    public List<User> GetUserByUserName(string username)
    {
        return _dbConText.Users.Where(p => p.Username == username).ToList();
    }

    public IEnumerable<User> GetAllUsers(Guid? currentUserId = null)
    {
        if (currentUserId.HasValue)
            return _dbConText.Users.Where(u => u.Id != currentUserId.Value);
        return _dbConText.Users;
    }
}