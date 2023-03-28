using Colo_Shop.IServices;
using Colo_Shop.Models;

namespace Colo_Shop.Services
{
  public class UserServices : IUserServices
  {
    ShopDbContext _dbConText;

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
        _User.ImageUser = User.ImageUser;
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

    public List<User> GetUserByName(string name)
    {
      return _dbConText.Users.Where(p => p.Username == name).ToList();
    }

  }
}
