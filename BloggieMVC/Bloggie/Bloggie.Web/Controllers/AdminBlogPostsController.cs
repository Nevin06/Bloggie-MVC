using Azure;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository TagRepository;
        private readonly IBlogPostRepository BlogPostRepository;

        public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            TagRepository = tagRepository;
            BlogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            // get tags from repository
            var tags = await TagRepository.GetAllAsync();
            var model = new AddBlogPostRequest()
            {
                Tags = tags.Select(t => new SelectListItem { Text = t.DisplayName, Value = t.Id.ToString() })
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            // Mapping AddBlogPostRequest view model to BlogPost domain model //33
            var blogPost = new BlogPost()
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                UrlHandler = addBlogPostRequest.UrlHandler,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author = addBlogPostRequest.Author,
                Visible = addBlogPostRequest.Visible
                //Tags = addBlogPostRequest.Tags
            };

            // Map Tags from selected Tags
            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in addBlogPostRequest.SelectedTags)
            {
                var selectedTagIdasGuid = Guid.Parse(selectedTagId);
                var existingTag = await TagRepository.GetAsync(selectedTagIdasGuid);
                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }
            // Mapping tags back to domain model
            blogPost.Tags = selectedTags;
            await BlogPostRepository.AddAsync(blogPost);

            //return RedirectToAction("Add");
            return RedirectToAction("List");

        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var blogPosts = await BlogPostRepository.GetAllAsync();
            return View(blogPosts);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var blogPost = await BlogPostRepository.GetAsync(id);
            //35
            var tagsDomainModel = await TagRepository.GetAllAsync();

            if (blogPost != null)
            {
                var editBlogPostRequest = new EditBlogPostRequest()
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    ShortDescription = blogPost.ShortDescription,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandler = blogPost.UrlHandler,
                    PublishedDate = blogPost.PublishedDate,
                    Author = blogPost.Author,
                    Visible = blogPost.Visible,
                    //Tags = blogPost.Tags
                    Tags = tagsDomainModel.Select(t => new SelectListItem
                    {
                        Text = t.Name, Value = t.Id.ToString()
                    }),
                    SelectedTags = blogPost.Tags.Select(t => t.Id.ToString()).ToArray()
                };
                
                return View(editBlogPostRequest);
            }
            else
            {
                // pass data to view
                return View(null);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            // map view model back to domain model
            var blogPost = new BlogPost()
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                ShortDescription = editBlogPostRequest.ShortDescription,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                UrlHandler = editBlogPostRequest.UrlHandler,
                PublishedDate = editBlogPostRequest.PublishedDate,
                Author = editBlogPostRequest.Author,
                Visible = editBlogPostRequest.Visible,
                //Tags = editBlogPostRequest.Tags
            };

            // Map Tags from selected Tags
            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in editBlogPostRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedTagId, out var tag))
                {
                    var foundTag = await TagRepository.GetAsync(tag);
                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }
            // Mapping tags back to domain model
            blogPost.Tags = selectedTags;

            // Submit information to repository to update
            var updatedBlogPost = await BlogPostRepository.UpdateAsync(blogPost);
            if (updatedBlogPost != null)
            {
                // show success notification
                return RedirectToAction("Edit");
            }
            else
            {
                // show error notification
                return RedirectToAction("Edit");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            var deletedBlogPost = await BlogPostRepository.DeleteAsync(editBlogPostRequest.Id);
            if (deletedBlogPost != null)
            {
                //Show a success notification
                return RedirectToAction("List");
            }
            else
            {
                //Show an error notification
                return RedirectToAction("Edit", new { id = editBlogPostRequest.Id});
            }
        }
    }
}
