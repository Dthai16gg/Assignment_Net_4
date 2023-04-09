namespace Colo_Shop.Services;

using Colo_Shop.IServices;
using Colo_Shop.Models;

public class UserServices : IUserServices
{
    private readonly ShopDbContext _dbConText;

    public UserServices()
    {
        this._dbConText = new ShopDbContext();
    }

    public bool CreateNewUsers(User User)
    {
        try
        {
            this._dbConText.Users.Add(User);
            this._dbConText.SaveChanges();
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
            var _User = this._dbConText.Users.Find(id);
            this._dbConText.Users.Remove(_User);
            this._dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public List<User> GetAllUsers()
    {
        return this._dbConText.Users.ToList();
    }

    public IEnumerable<User> GetAllUsers(Guid? currentUserId = null)
    {
        if (currentUserId.HasValue)
            return this._dbConText.Users.Where(u => u.Id != currentUserId.Value);
        return this._dbConText.Users;
    }

    public User GetUserById(Guid id)
    {
        return this._dbConText.Users.FirstOrDefault(p => p.Id == id);
    }

    public List<User> GetUserByUserName(string username)
    {
        return this._dbConText.Users.Where(p => p.Username == username).ToList();
    }

    public bool UpdateUser(User User)
    {
        try
        {
            var _User = this._dbConText.Users.Find(User.Id);
            _User.Username = User.Username;
            _User.Password = User.Password;
            _User.RoleId = User.RoleId;
            _User.Status = User.Status;
            _User.Name = User.Name;
            _User.Email = User.Email;
            _User.NumberPhone = User.NumberPhone;
            this._dbConText.Users.Update(_User);
            this._dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}