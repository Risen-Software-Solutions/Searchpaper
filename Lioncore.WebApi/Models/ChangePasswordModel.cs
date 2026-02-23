// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace Lioncore.WebApi.Models;

public class ChangePasswordModel
{
    [Required]
    public string OldPassword { get; set; } = string.Empty;

    [Required]
    [StringLength(
        100,
        ErrorMessage = "The {0} must be at least {2} characters long.",
        MinimumLength = 6
    )]
    public string NewPassword { get; set; } = string.Empty;
}
