using System;
using System.Collections.Generic;

namespace Frontend_InkSketch.ApplicationData;

public partial class TagsView
{
    public int UserTagId { get; set; }

    public int UserId { get; set; }

    public int TagId { get; set; }

    public string Name { get; set; }

    public string HexColor { get; set; }
}
