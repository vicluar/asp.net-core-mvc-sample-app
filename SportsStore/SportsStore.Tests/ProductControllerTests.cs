using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SportsStore.Tests
{
    public class ProductControllerTests
    {
        private Mock<IProductRepository> _mockProductRepository;

        public ProductControllerTests() => Initialize();

        private void Initialize()
        {
            _mockProductRepository = new Mock<IProductRepository>();

            _mockProductRepository.Setup(mr => mr.Products).Returns((new Product[]
            {
                new Product { ProductID = 1, Name = "P1"},
                new Product { ProductID = 2, Name = "P2"},
                new Product { ProductID = 3, Name = "P3"},
                new Product { ProductID = 4, Name = "P4"},
                new Product { ProductID = 5, Name = "P5"}
            }).AsQueryable());
        }

        [Fact]
        public void Can_Paginate()
        {
            var sut = new ProductController(_mockProductRepository.Object)
            {
                PageSize = 3
            };

            var result = sut.List(null, 2).ViewData.Model as ProductsListViewModel;

            var products = result.Products.ToArray();
            Assert.True(products.Length == 2);
            Assert.Equal("P4", products[0].Name);
            Assert.Equal("P5", products[1].Name);
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {            
            // Arrange
            var sut = new ProductController(_mockProductRepository.Object) { PageSize = 3 };
            
            // Act
            var result = sut.List(null, 2).ViewData.Model as ProductsListViewModel;
            
            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);
        }
    }
}
