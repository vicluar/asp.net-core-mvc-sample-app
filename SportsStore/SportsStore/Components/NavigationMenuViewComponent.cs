using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private IProductRepository _productRepository;

        public NavigationMenuViewComponent(IProductRepository productRepository)
            => _productRepository = productRepository;

        public IViewComponentResult Invoke()
        {
            var result = _productRepository.Products
                .Select(_ => _.Category)
                .Distinct()
                .OrderBy(_ => _);

            return View(result);
        }
    }
}
