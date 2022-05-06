namespace API.Entities
{
    public class UserLike
    {
        public AppUser SourceUser { get; set; }
        public int SourceUserId { get; set; }
        public AppUser Liked { get; set; }
        public int LikeUserId { get; set; }
    }
}