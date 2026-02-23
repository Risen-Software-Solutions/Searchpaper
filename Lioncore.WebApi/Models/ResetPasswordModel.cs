// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace Lioncore.WebApi.Models;

public class ResetPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(
        100,
        ErrorMessage = "The {0} must be at least {2} characters long.",
        MinimumLength = 6
    )]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string Code { get; set; } = string.Empty;
}
