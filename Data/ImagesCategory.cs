using System;
using System.Collections.Generic;

namespace Frontend_InkSketch.ApplicationData;

public partial class ImagesCategory
{
    public int ImageCategoryId { get; set; }

    public int ImageId { get; set; }

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Image Image { get; set; } = null!;
}
