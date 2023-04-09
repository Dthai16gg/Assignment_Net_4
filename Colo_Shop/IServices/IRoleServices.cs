namespace Colo_Shop.IServices;

using Colo_Shop.Models;

public interface IRoleServices
{
    public bool CreateNewRoles(Role Role);

    public bool DeleteRole(Guid id);

    public List<Role> GetAllRoles();

    public Role GetRoleById(Guid id);

    public List<Role> GetRoleByName(string name);

    public bool UpdateRole(Role Role);
}