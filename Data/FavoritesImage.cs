using System;
using System.Collections.Generic;

namespace Frontend_InkSketch.ApplicationData;

public partial class FavoritesImage
{
    public int FavoriteImageId { get; set; }

    public int ImageId { get; set; }

    public int UserId { get; set; }

    public virtual Image Image { get; set; }

    public virtual User User { get; set; }
}
