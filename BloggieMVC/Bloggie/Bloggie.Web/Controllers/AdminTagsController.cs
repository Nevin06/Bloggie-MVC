using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository TagRepository;

        //private readonly BloggieDbContext _bloggieDbContext;
        //constructor injection //23
        public AdminTagsController(ITagRepository tagRepository/*BloggieDbContext bloggieDbContext*/)
        {
            TagRepository = tagRepository;
            //_bloggieDbContext = bloggieDbContext;
        }

        //Add functionality / Create functionality //24
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        //[ActionName("Add")]
        //public IActionResult SubmitTag()
        public async Task<IActionResult> Add(AddTagRequest addTagRequest) //22

        {
            //var name = Request.Form["name"];
            //var displayName = Request.Form["displayName"];
            //var name = addTagRequest.Name;
            //var displayName = addTagRequest.DisplayName; //22

            //Mapping AddTagRequest to Tag domain model //23
            var tag = new Tag()
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };
            //await _bloggieDbContext.Tags.AddAsync(tag); //29
            //await _bloggieDbContext.SaveChangesAsync(); //29
            await TagRepository.AddAsync(tag);

            //return View("Add");
            return RedirectToAction("List"); //24
        }

        //Read functionality
        [HttpGet]
        public async Task<IActionResult> List() 
        {
            var tags = await TagRepository.GetAllAsync();/*await _bloggieDbContext.Tags.ToListAsync();*/
            return View(tags);
        }

        //25
        //Edit/Update functionality
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            // 1st method
            //var tag = _bloggieDbContext.Tags.Find(id);
            // 2nd method SingleOrDefault -> Unique identifier used
            //var tag = await _bloggieDbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);
            var tag = await TagRepository.GetAsync(id);
            if (tag != null)
            {
                var editTagRequest = new EditTagRequest()
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName,
                };

                return View(editTagRequest);
            }
            else
            {
                return View(null);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag()
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };

            //var existingTag = await _bloggieDbContext.Tags.FindAsync(tag.Id);
            //if (existingTag != null)
            //{
            //    existingTag.Name = tag.Name;
            //    existingTag.DisplayName = tag.DisplayName;
            //    await _bloggieDbContext.SaveChangesAsync();
            //    //return RedirectToAction("List");
            //    // show success notification
            var updatedTag = await TagRepository.UpdateAsync(tag);
            if (updatedTag != null)
            {
                //show success notification
                return RedirectToAction("Edit", new { Id = editTagRequest.Id });
            }
            //}
            else
            {
                // show error notification
                return RedirectToAction("Edit", new {Id = editTagRequest.Id});
            }
        }

        //26
        //Delete functionality
        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            //var tag = _bloggieDbContext.Tags.FindAsync(editTagRequest.Id);
            //if (tag != null)
            //{
            //    _bloggieDbContext.Tags.Remove(tag);
            //    await _bloggieDbContext.SaveChangesAsync();

            //    //Show a success notification
            //    return RedirectToAction("List");
            //}
            var deletedTag = await TagRepository.DeleteAsync(editTagRequest.Id);
            if (deletedTag != null)
            {
                //Show a success notification
                return RedirectToAction("List");
            }
            else
            {
                //Show an error notification
                return RedirectToAction("Edit", new { Id = editTagRequest.Id });
            }
        }
    }
}
