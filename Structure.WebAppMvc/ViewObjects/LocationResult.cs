using Structure.WebAppMvc.Models;

namespace Structure.WebAppMvc.ViewObjects;

public class LocationResult
{
    public string Name { get; set; } = string.Empty!;
    public string Acronym { get; set; } = string.Empty!;

    public static implicit operator LocationResult(Location? location)
    {
        if (location == null)
            return new();

        return new()
        {
            Name = location.Name,
            Acronym = location.Acronym
        };
    }
}
