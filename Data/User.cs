using System;
using System.Collections.Generic;

namespace Frontend_InkSketch.ApplicationData;

public partial class User
{
    public int UserId { get; set; }

    public string? Name { get; set; }

    public string Phone { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? CoverImage { get; set; }

    public DateTime RegistrationDate { get; set; }

    public virtual ICollection<FavoritesImage> FavoritesImages { get; } = new List<FavoritesImage>();

    public virtual ICollection<Image> Images { get; } = new List<Image>();

    public virtual ICollection<UsersCategory> UsersCategories { get; } = new List<UsersCategory>();

    public virtual ICollection<UsersTag> UsersTags { get; } = new List<UsersTag>();
}
