namespace AlVer.Models
{
    public class Favorite
    {
        public int FavoriteId { get; set; }
        public string? UserId { get; set; }
        public int? ProductId { get; set; }
        public virtual AppUser? User { get; set; }
        public virtual Product? Product { get; set; }
    }
}
