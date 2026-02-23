using System.ComponentModel.DataAnnotations;

namespace Lioncore.WebApi.Entities;

public class System
{
    [Required]
    public string Name { get; set; } = "Lioncore";
}
