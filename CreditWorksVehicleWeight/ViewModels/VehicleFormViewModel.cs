using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CreditWorksVehicleWeight.ViewModels
{
    public sealed class VehicleFormViewModel
    {
        [Required, StringLength(120)]
        [Display(Name = "Owner's name")]
        public string OwnerName { get; set; } = string.Empty;

        [Required, Display(Name = "Manufacturer")]
        public int? ManufacturerId { get; set; }

        [Required, Range(1886, 2100)]
        [Display(Name = "Year of manufacture")]
        public int YearOfManufacture { get; set; } = DateTime.UtcNow.Year;

        [Required, Range(typeof(decimal), "0.01", "99999999.99")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Weight must be positive and have no more than two decimal places.")]
        [Display(Name = "Weight (kg)")]
        public decimal WeightKg { get; set; }

        public IEnumerable<SelectListItem> Manufacturers { get; set; } = [];
    }
}
