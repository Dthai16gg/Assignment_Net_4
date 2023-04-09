namespace Colo_Shop.Models;

public class Role
{
    public string Description { get; set; }

    public Guid Id { get; set; }

    public string RoleName { get; set; }

    public int Status { get; set; }

    public virtual IEnumerable<User> Users { get; set; }
}