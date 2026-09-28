using DataAccess.Models;
using DataAccess.Repository.Interface;

namespace CreditWorksVehicleWeight.Services
{
    public sealed record CategoryInput(string Name, decimal MinimumWeightKg, decimal? MaximumWeightKg, string Icon);

    public sealed class CategoryConfigurationService(IVehicleCategoryRepository categoryRepository)
    {
        private static readonly HashSet<string> AllowedIcons = ["🛵", "🚙", "🚚", "🚗", "🏎️", "🚐", "🚜", "🚛"];

        public static IReadOnlyList<string> IconChoices => AllowedIcons.ToList();

        public static IReadOnlyList<string> Validate(IReadOnlyCollection<CategoryInput> categories)
        {
            var errors = new List<string>();
            if (categories.Count == 0) return ["At least one category is required."];
            var ordered = categories.OrderBy(x => x.MinimumWeightKg).ToList();
            if (ordered.Any(x => string.IsNullOrWhiteSpace(x.Name))) errors.Add("Every category needs a name.");
            if (ordered.Any(x => x.Name is null || x.Name.Length > 80)) errors.Add("Category names must be present and 80 characters or fewer.");
            if (ordered.Where(x => !string.IsNullOrWhiteSpace(x.Name)).Select(x => x.Name.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).Count() != ordered.Count(x => !string.IsNullOrWhiteSpace(x.Name)))
                errors.Add("Category names must be unique.");
            if (ordered.Any(x => !AllowedIcons.Contains(x.Icon))) errors.Add("Choose a valid icon for every category.");
            if (ordered[0].MinimumWeightKg != 0) errors.Add("The first range must start at 0 kg to cover every positive vehicle weight.");
            for (var i = 0; i < ordered.Count; i++)
            {
                var current = ordered[i];
                if (current.MinimumWeightKg < 0 || decimal.Round(current.MinimumWeightKg, 2) != current.MinimumWeightKg)
                    errors.Add($"{current.Name}: minimum must be 0 or greater and use at most two decimal places.");
                if (i < ordered.Count - 1)
                {
                    if (current.MaximumWeightKg is null || current.MaximumWeightKg <= current.MinimumWeightKg)
                        errors.Add($"{current.Name}: enter a maximum greater than its minimum.");
                    else if (current.MaximumWeightKg != ordered[i + 1].MinimumWeightKg)
                        errors.Add($"There must be no gap or overlap between {current.Name} and {ordered[i + 1].Name}; the maximum must equal the next minimum.");
                }
                else if (current.MaximumWeightKg is not null)
                    errors.Add($"{current.Name}: the final category must have no upper limit.");
            }
            return errors;
        }

        public async Task<IReadOnlyList<string>> SaveAsync(IReadOnlyCollection<CategoryInput> inputs, CancellationToken cancellationToken)
        {
            var errors = Validate(inputs);
            if (errors.Count > 0) return errors;

            var categories = inputs.OrderBy(x => x.MinimumWeightKg).Select(x => new VehicleCategory
            {
                Name = x.Name.Trim(),
                MinimumWeightKg = x.MinimumWeightKg,
                MaximumWeightKg = x.MaximumWeightKg,
                Icon = x.Icon
            }).ToList();
            await categoryRepository.ReplaceConfigurationAsync(categories, cancellationToken);
            return [];
        }

        public static VehicleCategory? FindCategory(IEnumerable<VehicleCategory> categories, decimal weightKg) =>
            categories.SingleOrDefault(x => weightKg >= x.MinimumWeightKg &&
                (x.MaximumWeightKg is null || weightKg < x.MaximumWeightKg));
    }
}
