using Microsoft.AspNetCore.Mvc;
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
                new Product { ProductID = 1, Name = "P1", Category = "Cat1" },
                new Product { ProductID = 2, Name = "P2", Category = "Cat2" },
                new Product { ProductID = 3, Name = "P3", Category = "Cat1" },
                new Product { ProductID = 4, Name = "P4", Category = "Cat2" },
                new Product { ProductID = 5, Name = "P5", Category = "Cat3" }
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

        [Fact]
        public void Can_Filter_Products()
        {
            ProductController controller = new ProductController(_mockProductRepository.Object)
            {
                PageSize = 3
            };

            Product[] result =
            (controller.List("Cat2", 1).ViewData.Model as ProductsListViewModel)
            .Products.ToArray();

            Assert.Equal(2, result.Length);
            Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.True(result[1].Name == "P4" && result[1].Category == "Cat2");
        }

        [Fact]
        public void Generate_Category_Specific_Product_Count()
        {
            var sut = new ProductController(_mockProductRepository.Object)
            {
                PageSize = 3
            };

            Func<ViewResult, ProductsListViewModel> GetModel = result =>
                result?.ViewData?.Model as ProductsListViewModel;

            var totalItemsCat1 = GetModel(sut.List("Cat1"))?.PagingInfo.TotalItems;
            var totalItemsCat2 = GetModel(sut.List("Cat2"))?.PagingInfo.TotalItems;
            var totalItemsCat3 = GetModel(sut.List("Cat3"))?.PagingInfo.TotalItems;
            var totalItemsAll = GetModel(sut.List(null))?.PagingInfo.TotalItems;

            Assert.Equal(2, totalItemsCat1);
            Assert.Equal(2, totalItemsCat2);
            Assert.Equal(1, totalItemsCat3);
            Assert.Equal(5, totalItemsAll);
        }
    }
}
