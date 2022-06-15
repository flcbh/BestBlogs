using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model;
using Repository;

namespace Api.Controllers
{
    [ApiController]
    [Route("posts")]
    public class PostController : ControllerBase
    {
        private readonly ILogger<PostController> _logger;
        private BlogContext _context;

        public PostController(ILogger<PostController> logger, BlogContext context)
        {
            _logger = logger;
            _context = context; 
        }

        [HttpGet]
        public ActionResult<IEnumerable<Post>> GetAll()
        {
            return _context.Posts.ToList();
        }

        [HttpGet("{id:guid}")]
        public ActionResult<Post> Get([FromRoute] Guid id)
        {
            return _context.Posts.Where(c => c.Id == id).SingleOrDefault();
        }

        [HttpPost]
        public ActionResult<Post> Post([FromBody] Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Add(post);
            _context.SaveChanges();

            return Ok(post);
        }

        [HttpPut("{id:guid}")]
        public ActionResult<Post> Put([FromRoute] Guid id, [FromBody] Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (post == null)
            {
                return NotFound();
            }

            post.Id = id;

            _context.Posts.Update(post);
            _context.SaveChanges();

            return Ok(post);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }
            var post = _context.Posts.Where(c => c.Id == id).SingleOrDefault();

            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet("{id:guid}/comments")]
        public ActionResult<IEnumerable<Comment>> GetComments([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            return _context.Comments.Where(c => c.PostId == id).ToList();
        }
    }
}