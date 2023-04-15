namespace Colo_Shop.Services;

using Colo_Shop.Models;

using Newtonsoft.Json;

public static class SessionServices
{
    public static bool CheckExistProduct(Guid id, List<Role> products)
    {
        return products.Any(x => x.Id == id);
    }

    public static List<Role> GetObjFromSession(ISession session, string key)
    {
        // Bước 1: Lấy string data từ session ở dạng json
        var jsonData = session.GetString(key);
        if (jsonData == null) return new List<Role>();

        // Nếu dữ liệu null thì tạo mới 1 list rỗng
        // bước 2: Convert về List
        var products = JsonConvert.DeserializeObject<List<Role>>(jsonData);
        return products;
    }

    // Ghi dữ liệu từ 1 list vào session
    public static void SetObjToSession(ISession session, string key, object values)
    {
        var jsonData = JsonConvert.SerializeObject(values);
        session.SetString(key, jsonData);
    }
}