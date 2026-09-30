using CreditWorksVehicleWeight.Services;
using CreditWorksVehicleWeight.ViewModels;
using DataAccess.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CreditWorksVehicleWeight.Controllers
{
    public sealed class CategoriesController(IVehicleCategoryRepository categoryRepository, CategoryConfigurationService configuration) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var categories = await categoryRepository.GetAllAsync(cancellationToken);
            var rows = categories.OrderBy(x => x.MinimumWeightKg)
                .Select(x => new CategoryRowViewModel { Name = x.Name, MinimumWeightKg = x.MinimumWeightKg, MaximumWeightKg = x.MaximumWeightKg, Icon = x.Icon })
                .ToList();
            return View(BuildModel(rows));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(CategoryConfigurationViewModel model, CancellationToken cancellationToken)
        {
            model.Categories ??= [];
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Check the highlighted fields and try again.");
                return View("Index", BuildModel(model.Categories));
            }
            var inputs = model.Categories.Select(x => new CategoryInput(x.Name ?? string.Empty, x.MinimumWeightKg, x.MaximumWeightKg, x.Icon ?? string.Empty)).ToList();
            var errors = await configuration.SaveAsync(inputs, cancellationToken);
            if (errors.Count > 0)
            {
                foreach (var error in errors) ModelState.AddModelError(string.Empty, error);
                return View("Index", BuildModel(model.Categories));
            }
            TempData["Success"] = "Category configuration saved. Existing vehicles now use the updated ranges.";
            return RedirectToAction(nameof(Index));
        }

        private static CategoryConfigurationViewModel BuildModel(List<CategoryRowViewModel> rows) => new()
        {
            Categories = rows,
            IconChoices = CategoryConfigurationService.IconChoices.Select(x => new SelectListItem(x, x)).ToList()
        };
    }
}
