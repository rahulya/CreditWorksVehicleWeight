using CreditWorksVehicleWeight.Services;
using CreditWorksVehicleWeight.ViewModels;
using DataAccess.Models;
using DataAccess.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CreditWorksVehicleWeight.Controllers
{
    public sealed class VehiclesController(
    IVehicleRepository vehicleRepository,
    IManufacturerRepository manufacturerRepository,
    IVehicleCategoryRepository categoryRepository) : Controller
    {
        public async Task<IActionResult> Index(string sortBy = "owner", string direction = "asc", CancellationToken cancellationToken = default)
        {
            sortBy = new[] { "owner", "manufacturer", "year", "weight" }.Contains(sortBy) ? sortBy : "owner";
            direction = direction == "desc" ? "desc" : "asc";
            var vehicles = await vehicleRepository.GetSortedAsync(sortBy, direction, cancellationToken);
            var categories = await categoryRepository.GetAllAsync(cancellationToken);
            var rows = vehicles.Select(v =>
            {
                var category = CategoryConfigurationService.FindCategory(categories, v.WeightKg);
                return new VehicleListItemViewModel(v.Id, v.OwnerName, v.Manufacturer.Name, v.YearOfManufacture,
                    v.WeightKg, category?.Name ?? "Uncategorised", category?.Icon ?? "⚠️");
            }).ToList();
            return View(new VehicleListViewModel { Vehicles = rows, SortBy = sortBy, Direction = direction });
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken) => View(await BuildFormAsync(new VehicleFormViewModel(), cancellationToken));

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleFormViewModel model, CancellationToken cancellationToken)
        {
            await ValidateManufacturerAndCategoriesAsync(model, cancellationToken);
            if (!ModelState.IsValid) return View(await BuildFormAsync(model, cancellationToken));
            var vehicle = new Vehicle
            {
                OwnerName = model.OwnerName.Trim(),
                ManufacturerId = model.ManufacturerId!.Value,
                YearOfManufacture = model.YearOfManufacture,
                WeightKg = model.WeightKg
            };
            await vehicleRepository.AddAsync(vehicle, cancellationToken);
            await vehicleRepository.SaveChangesAsync(cancellationToken);
            TempData["Success"] = "Vehicle added.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var vehicle = await vehicleRepository.GetByIdAsync(id, cancellationToken);
            if (vehicle is null) return NotFound();
            return View(await BuildFormAsync(new VehicleFormViewModel
            {
                OwnerName = vehicle.OwnerName,
                ManufacturerId = vehicle.ManufacturerId,
                YearOfManufacture = vehicle.YearOfManufacture,
                WeightKg = vehicle.WeightKg
            }, cancellationToken));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VehicleFormViewModel model, CancellationToken cancellationToken)
        {
            var vehicle = await vehicleRepository.GetByIdAsync(id, cancellationToken);
            if (vehicle is null) return NotFound();
            await ValidateManufacturerAndCategoriesAsync(model, cancellationToken);
            if (!ModelState.IsValid) return View(await BuildFormAsync(model, cancellationToken));
            vehicle.OwnerName = model.OwnerName.Trim(); vehicle.ManufacturerId = model.ManufacturerId!.Value;
            vehicle.YearOfManufacture = model.YearOfManufacture; vehicle.WeightKg = model.WeightKg;
            vehicleRepository.Update(vehicle);
            await vehicleRepository.SaveChangesAsync(cancellationToken);
            TempData["Success"] = "Vehicle updated.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var vehicle = await vehicleRepository.GetByIdAsync(id, cancellationToken);
            if (vehicle is null) return NotFound();
            vehicleRepository.Delete(vehicle);
            await vehicleRepository.SaveChangesAsync(cancellationToken);
            TempData["Success"] = "Vehicle deleted.";
            return RedirectToAction(nameof(Index));
        }

        private async Task<VehicleFormViewModel> BuildFormAsync(VehicleFormViewModel model, CancellationToken cancellationToken)
        {
            var manufacturers = await manufacturerRepository.GetAllAsync(cancellationToken);
            model.Manufacturers = manufacturers.OrderBy(x => x.Name)
                .Select(x => new SelectListItem(x.Name, x.Id.ToString(), x.Id == model.ManufacturerId)).ToList();
            return model;
        }

        private async Task ValidateManufacturerAndCategoriesAsync(VehicleFormViewModel model, CancellationToken cancellationToken)
        {
            if (model.ManufacturerId is not null && !await vehicleRepository.ManufacturerExistsAsync(model.ManufacturerId.Value, cancellationToken))
                ModelState.AddModelError(nameof(model.ManufacturerId), "Select a manufacturer from the list.");
            var categories = await categoryRepository.GetAllAsync(cancellationToken);
            if (CategoryConfigurationService.FindCategory(categories, model.WeightKg) is null)
                ModelState.AddModelError(nameof(model.WeightKg), "The current category configuration does not cover this weight. Update the categories first.");
        }
    }
}
