using Colo_Shop.Models;
using Newtonsoft.Json;

namespace Colo_Shop.Services
{
    public static class SessionServices // Lớp này dùng để lưu trữ dữ liệu vào session
    {
        // Lấy dữ liệu từ session trả về 1 list sản phẩm
        public static List<Role> GetObjFromSession(ISession session, string key)
        {
            // Bước 1: Lấy string data từ session ở dạng json
            string jsonData = session.GetString(key);
            if (jsonData == null) return new List<Role>();
            // Nếu dữ liệu null thì tạo mới 1 list rỗng
            // bước 2: Convert về List
            var role = JsonConvert.DeserializeObject<List<Role>>(jsonData);
            return role; // Trả về list
        }
        // Ghi dữ liệu từ 1 list vào session
        public static void SetObjToSession(ISession session, string key, object values)
        {
            var jsonData = JsonConvert.SerializeObject(values);
            session.SetString(key, jsonData);
        }
        public static bool CheckExistProduct(Guid id, List<Role> roles)
        {
            return roles.Any(x => x.Id == id);
        }
        // Kiểm tra sự tồn tại của sp trong List
    }
}
