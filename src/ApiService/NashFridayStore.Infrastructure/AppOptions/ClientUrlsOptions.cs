using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NashFridayStore.Infrastructure.AppOptions;

public sealed record ClientUrlsOption
{
    public const string ClientUrls = "ClientUrls";

    [Required]
    public string[] AdminSites { get; init; } = [];
}
