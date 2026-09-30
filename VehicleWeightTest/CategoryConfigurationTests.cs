using CreditWorksVehicleWeight.Services;
using CreditWorksVehicleWeight.Models;
using DataAccess.Models;

namespace VehicleWeightTest
{
    public sealed class CategoryConfigurationTests
    {
        private static List<CategoryInput> Defaults() =>
            new List<CategoryInput>
            {
                new("Light", 0, 500, "🛵"),
                new("Medium", 500, 2500, "🚙"),
                new("Heavy", 2500, null, "🚚")
            };

        [Theory]
        [InlineData("0.01", "Light")]
        [InlineData("499.99", "Light")]
        [InlineData("500.00", "Medium")]
        [InlineData("2499.99", "Medium")]
        [InlineData("2500.00", "Heavy")]
        [InlineData("90000", "Heavy")]
        public void Category_uses_inclusive_minimum_and_exclusive_maximum(string weightText, string expected)
        {
            var categories = Defaults().Select((x, index) => new VehicleCategory
            {
                Id = index + 1,
                Name = x.Name,
                MinimumWeightKg = x.MinimumWeightKg,
                MaximumWeightKg = x.MaximumWeightKg,
                Icon = x.Icon
            });
            Assert.Equal(expected, CategoryConfigurationService.FindCategory(categories, decimal.Parse(weightText, System.Globalization.CultureInfo.InvariantCulture))?.Name);
        }

        [Fact]
        public void Valid_contiguous_configuration_is_accepted_independent_of_input_order()
        {
            var reversed = Defaults().Reverse<CategoryInput>().ToList();
            Assert.Empty(CategoryConfigurationService.Validate(reversed));
        }

        [Fact]
        public void Gap_is_rejected()
        {
            var ranges = Defaults();
            ranges[0] = ranges[0] with { MaximumWeightKg = 499 };
            Assert.Contains(CategoryConfigurationService.Validate(ranges), x => x.Contains("gap or overlap"));
        }

        [Fact]
        public void Overlap_is_rejected()
        {
            var ranges = Defaults();
            ranges[0] = ranges[0] with { MaximumWeightKg = 501 };
            Assert.Contains(CategoryConfigurationService.Validate(ranges), x => x.Contains("gap or overlap"));
        }

        [Fact]
        public void Missing_unbounded_final_category_is_rejected()
        {
            var ranges = Defaults();
            ranges[^1] = ranges[^1] with { MaximumWeightKg = 5000 };
            Assert.Contains(CategoryConfigurationService.Validate(ranges), x => x.Contains("final category"));
        }

        [Fact]
        public void Reconfigured_ranges_reclassify_existing_weight_without_stored_category()
        {
            const decimal existingVehicleWeight = 2200;
            var changed = new List<VehicleCategory>
            {
                new() { Name = "Light", MinimumWeightKg = 0, MaximumWeightKg = 500 },
                new() { Name = "Medium", MinimumWeightKg = 500, MaximumWeightKg = 2000 },
                new() { Name = "Heavy", MinimumWeightKg = 2000, MaximumWeightKg = null }
            };
            Assert.Equal("Heavy", CategoryConfigurationService.FindCategory(changed, existingVehicleWeight)?.Name);
        }

        [Fact]
        public void Empty_ranges_are_rejected()
        {
            Assert.Contains(CategoryConfigurationService.Validate(new List<CategoryInput>()), x => x.Contains("At least one category is required."));
        }
    }

}
