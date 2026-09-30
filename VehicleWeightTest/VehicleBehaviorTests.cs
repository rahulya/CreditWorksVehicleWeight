using CreditWorksVehicleWeight.Services;
using CreditWorksVehicleWeight.ViewModels;
using DataAccess.Models;
using DataAccess.Repository.Implementation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VehicleWeightTest
{
    public sealed class VehicleBehaviorTests
    {
        private static List<Vehicle> SampleVehicles() =>
        [
            new() { OwnerName = "Zoe", Manufacturer = new Manufacturer { Name = "Toyota" }, YearOfManufacture = 2020, WeightKg = 1700 },
            new() { OwnerName = "Alex", Manufacturer = new Manufacturer { Name = "Mazda" }, YearOfManufacture = 2018, WeightKg = 1300 },
            new() { OwnerName = "Mia", Manufacturer = new Manufacturer { Name = "Honda" }, YearOfManufacture = 2022, WeightKg = 1900 }
        ];

        [Fact]
        public void Sorting_supports_each_required_column()
        {
            var vehicles = SampleVehicles().AsQueryable();
            Assert.Equal(new[] { "Alex", "Mia", "Zoe" }, VehicleSorting.Apply(vehicles, "owner", "asc").Select(x => x.OwnerName).ToArray());
            Assert.Equal(new[] { "Mia", "Alex", "Zoe" }, VehicleSorting.Apply(vehicles, "manufacturer", "asc").Select(x => x.OwnerName).ToArray());
            Assert.Equal(new[] { "Alex", "Zoe", "Mia" }, VehicleSorting.Apply(vehicles, "year", "asc").Select(x => x.OwnerName).ToArray());
            Assert.Equal(new[] { "Alex", "Zoe", "Mia" }, VehicleSorting.Apply(vehicles, "weight", "asc").Select(x => x.OwnerName).ToArray());
            Assert.Equal(new[] { "Zoe", "Mia", "Alex" }, VehicleSorting.Apply(vehicles, "owner", "desc").Select(x => x.OwnerName).ToArray());
        }

        [Fact]
        public void Vehicle_form_requires_owner_manufacturer_and_positive_weight()
        {
            var model = new VehicleFormViewModel { OwnerName = "", ManufacturerId = null, YearOfManufacture = 2020, WeightKg = 0 };
            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(model, new ValidationContext(model), results, validateAllProperties: true);
            Assert.False(valid);
            Assert.Contains(results, x => x.MemberNames.Contains(nameof(model.OwnerName)));
            Assert.Contains(results, x => x.MemberNames.Contains(nameof(model.ManufacturerId)));
            Assert.Contains(results, x => x.MemberNames.Contains(nameof(model.WeightKg)));
        }

        [Theory]
        [InlineData(2020, true)]
        [InlineData(1800, false)]
        [InlineData(2101, false)]
        public void Vehicle_year_must_be_sensible(int year, bool expectedValid)
        {
            var model = new VehicleFormViewModel { OwnerName = "Taylor", ManufacturerId = 1, YearOfManufacture = year, WeightKg = 1200 };
            var results = new List<ValidationResult>();
            Assert.Equal(expectedValid, Validator.TryValidateObject(model, new ValidationContext(model), results, validateAllProperties: true));
        }
    }

}
