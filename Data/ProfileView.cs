using System;
using System.Collections.Generic;

namespace Frontend_InkSketch.ApplicationData;

public partial class ProfileView
{
    public string Tagname { get; set; }

    public string? Username { get; set; }

    public int UserId { get; set; }

    public int TagId { get; set; }

    public decimal ViewPercent { get; set; }
}
