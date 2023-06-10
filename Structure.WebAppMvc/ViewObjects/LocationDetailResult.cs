using Structure.WebAppMvc.Models;

namespace Structure.WebAppMvc.ViewObjects;

public class LocationDetailResult
{
    public string Name { get; set; } = string.Empty!;
    public string Acronym { get; set; } = string.Empty!;

    public LocationResult Parent { get; set; } = new();
    public IEnumerable<LocationResult> Childrens { get; set; } = new List<LocationResult>();

    public static implicit operator LocationDetailResult(Location location)
    {
        return new LocationDetailResult
        {
            Acronym = location.Acronym,
            Name = location.Name,
            Parent = (LocationResult)location.Parent,
            Childrens = location.Children.Select(lc => (LocationResult)lc)
        };
    }
}