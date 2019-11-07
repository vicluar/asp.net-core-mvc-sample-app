using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public int PageSize { get; set; } = 4;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public ViewResult List(int productPage = 1)
        {
            var productsListViewModel = new ProductsListViewModel
            {
                Products = GetProducts(productPage),
                PagingInfo = GetPagingInfo(productPage)
            };

            return View(productsListViewModel);
        }

        private IQueryable<Product> GetProducts(int productPage)
        {
            return _productRepository.Products
                .OrderBy(p => p.ProductID)
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize);
        }

        private PagingInfo GetPagingInfo(int productPage)
        {
            return new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = _productRepository.Products.Count()
            };
        }
    }
}
