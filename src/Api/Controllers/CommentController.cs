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
    [Route("comments")]
    public class CommentController : Controller
    {
        private readonly ILogger<CommentController> _logger;
        private BlogContext _context;

        public CommentController(ILogger<CommentController> logger,
            BlogContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Comment>> GetAll()
        {
            return _context.Comments.ToList();
        }

        [HttpGet("{id:guid}")]
        public ActionResult<Comment> Get([FromRoute] Guid id)
        {
            return _context.Comments.Where(c => c.Id == id).SingleOrDefault();
        }

        [HttpPost]
        public ActionResult<Comment> Post([FromBody] Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Add(comment);
            _context.SaveChanges();

            return Ok(comment);
        }

        [HttpPut("{id:guid}")]
        public IActionResult Put([FromRoute] Guid id, [FromBody] Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (comment == null)
            {
                return NotFound();
            }

            comment.Id = id;

            _context.Comments.Update(comment);
            _context.SaveChanges();

            return Ok(comment);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            if(id == Guid.Empty)
            {
                return NotFound();
            }
            var comment = _context.Comments.Where(c => c.Id == id).SingleOrDefault();

            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            _context.SaveChanges();

            return Ok();
        }
    }
}