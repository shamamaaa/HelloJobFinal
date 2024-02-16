using System.ComponentModel.DataAnnotations;

public record RegisterVM
{
    [Required(ErrorMessage = "İstifadəçi adı daxil edilməlidir.")]
    [StringLength(25, MinimumLength = 2, ErrorMessage = "İstifadəçi adı 2 ilə 25 simvol aralığında olmalıdır")]
    [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "İstifadəçi adı yalnız hərflər, rəqəmlər və boşluqlardan ibarət ola bilər")]
    public string UserName { get; init; }

    [Required(ErrorMessage = "Ad vacibdir")]
    [StringLength(25, MinimumLength = 2, ErrorMessage = "Ad 2 ilə 25 simvol aralığında olmalıdır")]
    [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Ad yalnız hərflər, rəqəmlər və boşluqlardan ibarət ola bilər")]
    public string Name { get; init; }

    [Required(ErrorMessage = "Soyad vacibdir")]
    [StringLength(25, MinimumLength = 2, ErrorMessage = "Soyad 2 ilə 25 simvol aralığında olmalıdır")]
    [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Soyad yalnız hərflər, rəqəmlər və boşluqlardan ibarət ola bilər")]
    public string Surname { get; init; }

    [Required(ErrorMessage = "E-poçt vacibdir")]
    [StringLength(255, MinimumLength = 10, ErrorMessage = "E-poçt ünvanı 10 ilə 255 simvol aralığında olmalıdır")]
    [DataType(DataType.EmailAddress)]
    [EmailAddress(ErrorMessage = "Yanlış e-poçt ünvanı")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Yanlış e-poçt formatı")]
    public string Email { get; init; }

    [Required(ErrorMessage = "Şifrə vacibdir")]
    [StringLength(25, MinimumLength = 8, ErrorMessage = "Şifrə 8 ilə 25 simvol aralığında olmalıdır")]
    [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Şifrə yalnız hərflər, rəqəmlər və boşluqlardan ibarət ola bilər")]
    [DataType(DataType.Password)]
    public string Password { get; init; }

    [Required(ErrorMessage = "Şifrənin təsdiqi vacibdir")]
    [StringLength(25, MinimumLength = 8, ErrorMessage = "Şifrə 8 ilə 25 simvol aralığında olmalıdır")]
    [Compare(nameof(Password), ErrorMessage = "Şifrə eyni olmalıdır")]
    [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Şifrə yalnız hərflər, rəqəmlər və boşluqlardan ibarət ola bilər")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; init; }

    public string Role { get; init; }

    [Range(typeof(bool), "true", "true", ErrorMessage = "Şərtləri qəbul etməlisiniz.")]
    public bool AllowTerms { get; init; }
}
