namespace Colo_Shop.Services;

using Colo_Shop.IServices;
using Colo_Shop.Models;

public class RoleServices : IRoleServices
{
    private readonly ShopDbContext _dbConText;

    public RoleServices()
    {
        this._dbConText = new ShopDbContext();
    }

    public bool CreateNewRoles(Role Role)
    {
        try
        {
            this._dbConText.Roles.Add(Role);
            this._dbConText.SaveChanges();
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
            var _Role = this._dbConText.Roles.Find(id);
            this._dbConText.Roles.Remove(_Role);
            this._dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public List<Role> GetAllRoles()
    {
        return this._dbConText.Roles.ToList();
    }

    public Role GetRoleById(Guid id)
    {
        return this._dbConText.Roles.FirstOrDefault(p => p.Id == id);
    }

    public List<Role> GetRoleByName(string name)
    {
        return this._dbConText.Roles.Where(p => p.RoleName.Contains(name)).ToList();
    }

    public bool UpdateRole(Role role)
    {
        try
        {
            var _Role = this._dbConText.Roles.Find(role.Id);
            _Role.RoleName = role.RoleName;
            _Role.Description = role.Description;
            _Role.Status = role.Status;
            this._dbConText.Roles.Update(_Role);
            this._dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}