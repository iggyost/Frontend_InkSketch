using System;
using System.Collections.Generic;

namespace Frontend_InkSketch.ApplicationData;

public partial class ImagesTag
{
    public int ImageTagId { get; set; }

    public int ImageId { get; set; }

    public int TagId { get; set; }

    public virtual Image Image { get; set; }

    public virtual Tag Tag { get; set; }
}
