using System.ComponentModel.DataAnnotations;
using BuildingBlocks.Options;

namespace BuildingBlocks.Auth.OpenId;
public class OpenIdOptions : IOptionsRoot
{
    [Required(AllowEmptyStrings = false)]
    public string? Authority { get; set; } = string.Empty;
    [Required(AllowEmptyStrings = false)]
    public string? Audience { get; set; } = string.Empty;
}
