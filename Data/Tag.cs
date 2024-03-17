using System;
using System.Collections.Generic;

namespace Frontend_InkSketch.ApplicationData;

public partial class Tag
{
    public int TagId { get; set; }

    public string Name { get; set; }

    public string HexColor { get; set; }

    public virtual ICollection<ImagesTag> ImagesTags { get; } = new List<ImagesTag>();

    public virtual ICollection<UsersTag> UsersTags { get; } = new List<UsersTag>();
}
