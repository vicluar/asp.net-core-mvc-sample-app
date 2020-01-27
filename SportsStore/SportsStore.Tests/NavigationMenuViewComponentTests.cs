using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using SportsStore.Components;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SportsStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void Can_Select_Categories()
        {
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(m => m.Products).Returns((
                new Product[]
                {
                    new Product { ProductID = 1, Name = "Product1", Category = "Apples" },
                    new Product { ProductID = 1, Name = "Product1", Category = "Apples" },
                    new Product { ProductID = 1, Name = "Product1", Category = "Apples" },
                    new Product { ProductID = 1, Name = "Product1", Category = "Apples" }
                }).AsQueryable());

            var sut = new NavigationMenuViewComponent(mockRepository.Object);

            var result = ((IEnumerable<string>)(sut.Invoke() as ViewViewComponentResult).ViewData.Model).ToArray();

            Assert.True(Enumerable.SequenceEqual(new string[] { "Apples", "Oranges", "Plums" }, result));
        }
    }
}
