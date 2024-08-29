using System.ComponentModel.DataAnnotations;

namespace WebPizzaSite.Models.Category
{
    public class CategoryEditViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Назва категорії")]
        [Required(ErrorMessage = "Вкажіть назву категорії")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Опис категорії")]
        [Required(ErrorMessage = "Вкажіть опис категорії")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Фото url")]
        public IFormFile? Image { get; set; }  // Made nullable to remove validation

        public string ExistingImagePath { get; set; }
    }
}

