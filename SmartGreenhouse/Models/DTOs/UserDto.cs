using SmartGreenhouse.Models.Entities;

namespace SmartGreenhouse.Models.DTOs
{
    public class UserDto
    {
        public int Id { get; set; } // ID користувача, якщо потрібно
        public string UserName { get; set; } // Ім'я користувача
        public string Email { get; set; } // Електронна пошта користувача
        public ICollection<Greenhouse> Greenhouses { get; set; } // Якщо потрібно відправляти теплиці користувача
        public ICollection<UserSetting> UserSettings { get; set; } // Якщо потрібно відправляти налаштування користувача

    }
}
