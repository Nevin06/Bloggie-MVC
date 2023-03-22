namespace Bloggie.Web.Models.Domain
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; } //solve 2 different purposes
        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}
