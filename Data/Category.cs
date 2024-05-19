using System;
using System.Collections.Generic;

namespace Frontend_InkSketch.ApplicationData;

public partial class Category
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ImagesCategory> ImagesCategories { get; } = new List<ImagesCategory>();

    public virtual ICollection<UsersCategory> UsersCategories { get; } = new List<UsersCategory>();
}
