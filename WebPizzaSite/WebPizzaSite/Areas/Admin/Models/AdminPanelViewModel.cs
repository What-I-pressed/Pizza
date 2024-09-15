namespace WebPizzaSite.Areas.Admin.Models
{
    public class AdminPanelViewModel
    {
        public int Id { get; set; } // Обов'язкове поле для ідентифікації користувача при видаленні

        public string FirstName { get; set; } = string.Empty; // Ім'я користувача для відображення
        public string LastName { get; set; } = string.Empty; // Прізвище користувача для відображення
        public string Email { get; set; } = string.Empty; // Електронна пошта користувача для відображення
        public string UserName { get; set; } = string.Empty; // Ім'я користувача для відображення
    }
}