using System;
using System.Collections.Generic;

namespace Frontend_InkSketch.ApplicationData;

public partial class Image
{
    public int ImageId { get; set; }

    public string SourceImage { get; set; } = null!;

    public int UserId { get; set; }

    public virtual ICollection<FavoritesImage> FavoritesImages { get; } = new List<FavoritesImage>();

    public virtual ICollection<ImagesCategory> ImagesCategories { get; } = new List<ImagesCategory>();

    public virtual ICollection<ImagesTag> ImagesTags { get; } = new List<ImagesTag>();

    public virtual User User { get; set; } = null!;
}
