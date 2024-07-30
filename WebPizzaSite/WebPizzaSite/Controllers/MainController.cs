using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPizzaSite.Data;
using WebPizzaSite.Data.Entities;
using WebPizzaSite.Models.Category;

namespace WebPizzaSite.Controllers
{
    public class MainController : Controller
    {
        private readonly PizzaDbContext _pizzaDbContext;
        private readonly IMapper _mapper;
        private readonly string imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");


        public MainController(PizzaDbContext pizzaDbContext, IMapper mapper)
        {
            _pizzaDbContext = pizzaDbContext;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var list = _pizzaDbContext.Categories.ProjectTo<CategoryItemViewModel>(_mapper.ConfigurationProvider)
                .ToList();

            return View(list);
        }

        /// <summary>
        /// Виводить сторінку для додавання нової категорії
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CategoryCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string filePath = null;
                if (model.Image != null && model.Image.Length > 0)
                {
                    var fileName = Path.GetFileName(model.Image.FileName);
                    filePath = Path.Combine("uploads", fileName); // Save path

                    // Ensure the upload directory exists
                    if (!Directory.Exists(imgPath))
                    {
                        Directory.CreateDirectory(imgPath);
                    }

                    using (var stream = new FileStream(Path.Combine(imgPath, fileName), FileMode.Create))
                    {
                        model.Image.CopyTo(stream);
                    }
                }

                var category = new CategoryEntity
                {
                    Name = model.Name,
                    Description = model.Description,
                    Image = filePath  // Store the relative file path
                };

                _pizzaDbContext.Categories.Add(category);
                _pizzaDbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var pizza = _pizzaDbContext.Categories.SingleOrDefault(c => c.Id == id);
            if (pizza == null) return NotFound();

            var item = new CategoryEditViewModel
            {
                Id = pizza.Id,
                Name = pizza.Name,
                Description = pizza.Description,
                ExistingImagePath = pizza.Image
            };
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(CategoryEditViewModel updatedItem)
        {
            if (ModelState.IsValid)
            {
                var pizza = _pizzaDbContext.Categories.SingleOrDefault(c => c.Id == updatedItem.Id);
                if (pizza == null) return NotFound();

                pizza.Name = updatedItem.Name;
                pizza.Description = updatedItem.Description;

                if (updatedItem.Image != null && updatedItem.Image.Length > 0)
                {
                    var fileName = Path.GetFileName(updatedItem.Image.FileName);
                    var filePath = Path.Combine("uploads", fileName);

                    if (!Directory.Exists(imgPath))
                    {
                        Directory.CreateDirectory(imgPath);
                    }

                    using (var stream = new FileStream(Path.Combine(imgPath, fileName), FileMode.Create))
                    {
                        updatedItem.Image.CopyTo(stream);
                    }

                    // Delete the old image if it exists
                    if (!string.IsNullOrEmpty(updatedItem.ExistingImagePath))
                    {
                        var oldImagePath = Path.Combine(imgPath, updatedItem.ExistingImagePath);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    pizza.Image = filePath; // Update the image path
                }

                _pizzaDbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(updatedItem);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var entity = _pizzaDbContext.Categories.SingleOrDefault(c => c.Id == id);
            if (entity == null)
                return NotFound();
            _pizzaDbContext.Categories.Remove(entity);
            _pizzaDbContext.SaveChanges();
            return Ok();
        }
    }
}
