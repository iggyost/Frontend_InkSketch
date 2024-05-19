using System;
using System.Collections.Generic;

namespace Frontend_InkSketch.ApplicationData;

public partial class UsersCategory
{
    public int UserCategoryId { get; set; }

    public int UserId { get; set; }

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
