namespace Colo_Shop.Models
{
    public class Role
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string RoleName { get; set; }
        public virtual IEnumerable<User> Users { get; set; }
    }
}
