using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
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
                    new Product { ProductID = 2, Name = "Product2", Category = "Oranges" },
                    new Product { ProductID = 3, Name = "Product3", Category = "Apples" },
                    new Product { ProductID = 4, Name = "Product4", Category = "Plums" }
                }).AsQueryable());

            var sut = new NavigationMenuViewComponent(mockRepository.Object);

            var result = ((IEnumerable<string>)(sut.Invoke() as ViewViewComponentResult).ViewData.Model).ToArray();

            Assert.True(Enumerable.SequenceEqual(new string[] { "Apples", "Oranges", "Plums" }, result));
        }

        [Fact]
        public void Indicates_Selected_Category()
        {
            var categoryToSelect = "Apples";

            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(m => m.Products).Returns((
                new Product[]
                {
                    new Product { ProductID = 1, Name = "Product1", Category = "Apples" },
                    new Product { ProductID = 4, Name = "Product4", Category = "Oranges" }
                }
                ).AsQueryable());

            var sut = new NavigationMenuViewComponent(mockRepository.Object);

            sut.ViewComponentContext = new ViewComponentContext
            {
                ViewContext = new ViewContext
                {
                    RouteData = new RouteData()
                }
            };
            sut.RouteData.Values["category"] = categoryToSelect;

            var result = (sut.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"].ToString();

            Assert.Equal(categoryToSelect, result);
        }
    }
}
