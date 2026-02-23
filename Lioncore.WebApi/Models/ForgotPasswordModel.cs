// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace Lioncore.WebApi.Models;

public class ForgotPasswordViewModel
{
    [Required]
    public string Email { get; set; } = string.Empty;
}
