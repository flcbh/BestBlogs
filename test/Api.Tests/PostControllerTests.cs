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
    public class PostControllerTests
    {
        private Mock<ILogger<PostController>> _logger;
        private PostController _controller;
        private Guid Id;
        public PostControllerTests()
        {
            var options = new DbContextOptionsBuilder<BlogContext>()
                    .UseInMemoryDatabase(databaseName: "Database")
                    .Options;

            var context = new BlogContext(options);

            foreach (var item in context.Posts.ToList())
            {
                context.Posts.Remove(item);
            }

            context.Posts.Add(new Post { Id = Guid.NewGuid() });
            context.Posts.Add(new Post { Id = Guid.NewGuid() });
            context.Posts.Add(new Post { Id = Guid.NewGuid() });
            context.SaveChanges();

            Id = context.Posts.FirstAsync().Result.Id;

            _logger = new Mock<ILogger<PostController>>();

            _controller = new PostController(_logger.Object, context);

        }

        [Fact]
        public void GetAll()
        {
            var posts = _controller.GetAll();

            Assert.Equal(3, posts.Value.Count());
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
            var post = Mock.Of<Post>();
            // Act
            var result = _controller.Post(post);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Put()
        {
            var post = _controller.Get(Id).Value;
            post.CreationDate = DateTime.Now.Date;

            _controller.Put(Id, post);

            Assert.Equal(DateTime.Now.Date, post.CreationDate.Date);
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

