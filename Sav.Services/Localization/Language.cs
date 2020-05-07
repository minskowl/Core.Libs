using System.ComponentModel.DataAnnotations;

namespace Savchin.Services.Localization
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum Language
    {
        [Display(Description = "en-GB", Order = 1)]
        English = 69,
        [Display(Description = "ar", Order = 2)]
        Arabic = 162,
        [Display(Description = "de-DE", Order = 3)]
        German = 7,
        [Display(Description = "zh-Hans", Order = 6)]
        Chinese = 161,
        [Display(Description = "hu-HU", Order = 4)]
        Hungarian = 13,
        [Display(Description = "pl-PL", Order = 5)]
        Polish = 20,
        [Display(Description = "ja-jp", Order = 7)]
        Japanese = 16
    }
}