using Api.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using Moq;
using Repository;
using System;
using System.Linq;
using Xunit;

namespace Api.Tests
{
    public class CommentControllerTests
    {
        private Mock<ILogger<CommentController>> _logger;
        private CommentController _controller;
        private Guid Id;
        public CommentControllerTests()
        {
            var options = new DbContextOptionsBuilder<BlogContext>()
                    .UseInMemoryDatabase(databaseName: "Database")
                    .Options;

            var context = new BlogContext(options);

            foreach (var item in context.Comments.ToList())
            {
                context.Comments.Remove(item);
            }

            context.Comments.Add(new Comment { Id = Guid.NewGuid() });
            context.Comments.Add(new Comment { Id = Guid.NewGuid() });
            context.Comments.Add(new Comment { Id = Guid.NewGuid() });
            context.SaveChanges();

            Id = context.Comments.FirstAsync().Result.Id;

            _logger = new Mock<ILogger<CommentController>>();

            _controller = new CommentController(_logger.Object, context);

        }

        [Fact]
        public void GetAll()
        {
            var comments = _controller.GetAll();

            Assert.Equal(3, comments.Value.Count());
        }

        [Fact]
        public void Get()
        {
            // Act
            var result = _controller.Get(Id);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Post()
        {
            var comment = Mock.Of<Comment>();
            // Act
            var result = _controller.Post(comment);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Put()
        {
            var comment = _controller.Get(Id).Value;
            comment.Author = "Frank";

            _controller.Put(Id, comment);

            Assert.Equal("Frank", comment.Author);
        }

        [Fact]
        public void Delete()
        {
            // Act
            var result = _controller.Delete(Id);

            // Assert
            Assert.NotNull(result);
        }

    }

}