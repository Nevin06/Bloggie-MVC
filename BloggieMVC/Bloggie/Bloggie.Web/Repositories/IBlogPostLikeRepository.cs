using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories
{
    public interface IBlogPostLikeRepository
    {
        Task<int> GetTotalLikes(Guid blogPostId);
        Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike);
        //67
        Task<IEnumerable<BlogPostLike>> GetLikesForBlog/*ForUser*/(Guid blogPostId);
    }
}
