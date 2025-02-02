using MainProgram.AllRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MainProgram.Interfaces;

namespace MainProgram.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostCervice _postService;

        public PostController(IPostCervice postService)
        {
            _postService = postService;
        }

        [Authorize(Roles = "Author")]
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest postRequest)
        {
            var authorId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var newPost = await _postService.CreatePost(authorId, postRequest);
            return CreatedAtAction(nameof(GetPostById), new { id = newPost.PostId }, newPost);
        }

        [Authorize(Roles = "Author")]
        [HttpPost("{postId}/images")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddImagesToPost([FromRoute] string postId, [FromForm] List<IFormFile> images)
        {
            var authorId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

            if (images == null || !images.Any())
            {
                return BadRequest(new { Message = "No images provided." });
            }

            var uploadedImages = await _postService.AddImage(Guid.Parse(postId), authorId, images);
            return Created("", new { UploadedImages = uploadedImages });
        }

        [Authorize(Roles = "Author")]
        [HttpPost("{postId}")]
        public async Task<IActionResult> EditPost([FromRoute] string postId, [FromBody] UpdatePost updatePost)
        {
            var authorId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var updatedPost = await _postService.UpdatePost(Guid.Parse(postId), authorId, updatePost);
            return Ok(new { Message = "Post successfully updated.", UpdatedPost = updatedPost });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return Unauthorized(new { Message = "User identifier claim is missing." });
            }

            var userId = claim.Value;

            if (userRole == "Author")
            {
                var posts = await _postService.GetPostsByAuthorId(Guid.Parse(userId));
                return Ok(new { Message = "List of posts for the author.", Posts = posts });
            }

            if (userRole == "Reader")
            {
                var posts = await _postService.GetPublishedPosts();
                return Ok(new { Message = "List of published posts.", Posts = posts });
            }

            return Forbid();
        }

        [Authorize(Roles = "Author")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById([FromRoute] string id)
        {
            var post = await _postService.GetPostById(Guid.Parse(id));

            if (post == null)
                return NotFound(new { Message = "Post not found." });

            return Ok(post);
        }

        [Authorize(Roles = "Author")]
        [HttpPatch("{postId}/status")]
        public async Task<IActionResult> PublishPost([FromRoute] string postId, [FromBody] PublishPostRequest request)
        {
            var authorId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var updatedPost = await _postService.PublishPost(Guid.Parse(postId), authorId, request);
            return Ok(new { Message = "Post successfully published.", UpdatedPost = updatedPost });
        }

        [Authorize(Roles = "Author")]
        [HttpDelete("{postId}/images/{imageId}")]
        public async Task<IActionResult> DeleteImages([FromRoute] string postId, [FromRoute] string imageId)
        {
            var authorId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var success = await _postService.DeleteImage(Guid.Parse(postId), Guid.Parse(imageId), authorId);

            if (!success)
                return NotFound(new { Message = "Image or post not found, or access denied." });

            return Ok(new { Message = "Image successfully deleted." });
        }

        [Authorize(Roles = "Author")]
        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeletePost([FromRoute] string postId)
        {
            var authorId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var success = await _postService.DeletePost(Guid.Parse(postId), authorId);

            if (!success)
                return NotFound(new { Message = "Post not found or access denied." });

            return Ok(new { Message = "Post successfully deleted." });
        }
    }
}
