using Colo_Shop.Models;

namespace Colo_Shop.IServices;

public interface IRoleServices
{
    public bool CreateNewRoles(Role Role);
    public bool UpdateRole(Role Role);
    public bool DeleteRole(Guid id);
    public Role GetRoleById(Guid id);
    public List<Role> GetRoleByName(string name);
    public List<Role> GetAllRoles();
}