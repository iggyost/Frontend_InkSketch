using System;
using System.Collections.Generic;

namespace Frontend_InkSketch.ApplicationData;

public partial class FavoritesView
{
    public int FavoriteImageId { get; set; }

    public int UserId { get; set; }

    public int ImageId { get; set; }

    public string SourceImage { get; set; }
}
