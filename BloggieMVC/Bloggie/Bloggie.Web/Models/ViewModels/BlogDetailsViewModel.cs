﻿using Bloggie.Web.Models.Domain;

//65
namespace Bloggie.Web.Models.ViewModels
{
    public class BlogDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string UrlHandler { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool Visible { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public int TotalLikes { get; set; }
        //67
        public bool Liked { get; set; }
        //69
        public string CommentDescription { get; set; }
        //70
        public IEnumerable<BlogComment> Comments { get; set; }
    }
}
