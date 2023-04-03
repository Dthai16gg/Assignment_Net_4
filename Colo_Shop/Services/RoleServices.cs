using Colo_Shop.IServices;
using Colo_Shop.Models;

namespace Colo_Shop.Services;

public class RoleServices : IRoleServices
{
    private readonly ShopDbContext _dbConText;

    public RoleServices()
    {
        _dbConText = new ShopDbContext();
    }

    public bool CreateNewRoles(Role Role)
    {
        try
        {
            _dbConText.Roles.Add(Role);
            _dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool DeleteRole(Guid id)
    {
        try
        {
            var _Role = _dbConText.Roles.Find(id);
            _dbConText.Roles.Remove(_Role);
            _dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool UpdateRole(Role role)
    {
        try
        {
            var _Role = _dbConText.Roles.Find(role.Id);
            _Role.RoleName = role.RoleName;
            _Role.Description = role.Description;
            _Role.Status = role.Status;
            _dbConText.Roles.Update(_Role);
            _dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public List<Role> GetAllRoles()
    {
        return _dbConText.Roles.ToList();
    }

    public Role GetRoleById(Guid id)
    {
        return _dbConText.Roles.FirstOrDefault(p => p.Id == id);
    }

    public List<Role> GetRoleByName(string name)
    {
        return _dbConText.Roles.Where(p => p.RoleName.Contains(name)).ToList();
    }
}