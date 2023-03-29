namespace Colo_Shop.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string NumberPhone { get; set; }
        public string Password { get; set; }
        public int Status { get; set; }
        public virtual IEnumerable<Bill> Bills { get; set; }
        public virtual Role Role { get; set; }
    }
}
