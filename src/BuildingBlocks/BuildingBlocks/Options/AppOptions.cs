using System.ComponentModel.DataAnnotations;

namespace BuildingBlocks.Options
{
    public class AppOptions : IOptionsRoot
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; } = "FSH.WebAPI";
    }
}
// Add from fsh