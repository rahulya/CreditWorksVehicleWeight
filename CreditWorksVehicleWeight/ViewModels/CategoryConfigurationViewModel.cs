using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CreditWorksVehicleWeight.ViewModels
{
    public sealed class CategoryConfigurationViewModel
    {
        public List<CategoryRowViewModel> Categories { get; set; } = [];
        public IEnumerable<SelectListItem> IconChoices { get; set; } = [];
    }

    public sealed class CategoryRowViewModel
    {
        [Required, StringLength(80)] public string Name { get; set; } = string.Empty;
        [Range(typeof(decimal), "0", "99999999.99")] public decimal MinimumWeightKg { get; set; }
        public decimal? MaximumWeightKg { get; set; }
        [Required] public string Icon { get; set; } = string.Empty;
    }
}
