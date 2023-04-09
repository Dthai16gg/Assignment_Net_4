namespace Colo_Shop.IServices;

using Colo_Shop.Models;

public interface IUserServices
{
    public bool CreateNewUsers(User User);

    public bool DeleteUser(Guid id);

    public IEnumerable<User> GetAllUsers(Guid? currentUserId = null);

    public List<User> GetAllUsers();

    public User GetUserById(Guid id);

    public List<User> GetUserByUserName(string name);

    public bool UpdateUser(User User);
}