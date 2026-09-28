namespace CreditWorksVehicleWeight.ViewModels
{
    public sealed record VehicleListItemViewModel(int Id, string OwnerName, string Manufacturer,
     int YearOfManufacture, decimal WeightKg, string CategoryName, string CategoryIcon);

    public sealed class VehicleListViewModel
    {
        public IReadOnlyList<VehicleListItemViewModel> Vehicles { get; init; } = [];
        public string SortBy { get; init; } = "owner";
        public string Direction { get; init; } = "asc";
        public string NextDirection(string column) => SortBy == column && Direction == "asc" ? "desc" : "asc";
    }
}
