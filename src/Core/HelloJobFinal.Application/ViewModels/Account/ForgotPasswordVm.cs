using System.ComponentModel.DataAnnotations;

public record ForgotPasswordVm
{
    [Required(ErrorMessage = "Köhnə parol daxil edilməlidir.")]
    [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Parol rəqəm və hərflərdən ibarət olmalıdır.")]
    [StringLength(25, MinimumLength = 8, ErrorMessage = "Parol 8-25 aralığında simvoldan ibarət olmalıdır.")]
    public string Password { get; init; }

    [Required(ErrorMessage = "Yeni parol daxil edilməlidir.")]
    [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Yeni parol yalnız hərflər, rəqəmlər və boşluqlardan ibarət ola bilər")]
    [StringLength(25, MinimumLength = 8, ErrorMessage = "Yeni parol 8 ilə 25 simvol aralığında olmalıdır")]
    [DataType(DataType.Password)]
    public string NewPassword { get; init; }

    [Required(ErrorMessage = "Yeni parol daxil edilməlidir.")]
    [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Yeni parol yalnız hərflər, rəqəmlər və boşluqlardan ibarət ola bilər")]
    [StringLength(25, MinimumLength = 8, ErrorMessage = "Yeni parol 8 ilə 25 simvol aralığında olmalıdır")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Şifrə eyni olmalıdır")]
    public string NewConfirmPassword { get; init; }
}
