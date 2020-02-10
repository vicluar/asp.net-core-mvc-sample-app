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

        public ViewResult List(string category, int productPage = 1)
        {
            var productsListViewModel = new ProductsListViewModel
            {
                Products = GetProducts(productPage, category),
                PagingInfo = GetPagingInfo(productPage, category),
                CurrentCategory = category
            };

            return View(productsListViewModel);
        }

        private IQueryable<Product> GetProducts(int productPage, string category)
        {
            return _productRepository.Products
                .Where(p => category == null || p.Category == category)
                .OrderBy(p => p.ProductID)
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize);
        }

        private PagingInfo GetPagingInfo(int productPage, string category)
        {
            return new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = category == null ? 
                    _productRepository.Products.Count() :
                    _productRepository.Products.Where(_ => _.Category == category).Count()
            };
        }
    }
}
