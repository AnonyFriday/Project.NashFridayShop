using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NashFridayStore.Domain.Entities;

internal class SubCategory
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;

    public ICollection<Product> Products { get; set; }
}
