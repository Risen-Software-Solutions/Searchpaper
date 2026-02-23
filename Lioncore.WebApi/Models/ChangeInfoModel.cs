// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace Lioncore.WebApi.Models;

public class ChangeInfoModel
{
    [Required]
    public string FullName { get; set; } = string.Empty;
}
