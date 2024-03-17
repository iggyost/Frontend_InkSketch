using System;
using System.Collections.Generic;

namespace Frontend_InkSketch.ApplicationData;

public partial class ImagesView
{
    public string SourceImage { get; set; }

    public string Name { get; set; }

    public string HexColor { get; set; }

    public int ImageId { get; set; }

    public int TagId { get; set; }
}
