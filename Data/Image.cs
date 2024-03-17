using System;
using System.Collections.Generic;

namespace Frontend_InkSketch.ApplicationData;

public partial class Image
{
    public int ImageId { get; set; }

    public string SourceImage { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<ImagesTag> ImagesTags { get; } = new List<ImagesTag>();

    public virtual User User { get; set; }
}
