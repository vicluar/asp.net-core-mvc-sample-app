using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SportsStore.Tests
{
    public class CartTests
    {
        [Fact]
        public void Can_Add_New_Lines()
        {
            Product firstProduct = new Product { ProductID = 1, Name = "P1" };
            Product secondProduct = new Product { ProductID = 2, Name = "P2" };

            Cart target = new Cart();

            target.AddItem(firstProduct, 1);
            target.AddItem(secondProduct, 1);
            CartLine[] results = target.Lines.ToArray();

            Assert.Equal(2, results.Length);
            Assert.Equal(firstProduct, results[0].Product);
            Assert.Equal(secondProduct, results[1].Product);
        }

        [Fact]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            Product firstProduct = new Product { ProductID = 1, Name = "P1" };
            Product secondProduct = new Product { ProductID = 2, Name = "P2" };

            Cart target = new Cart();

            target.AddItem(firstProduct, 1);
            target.AddItem(secondProduct, 1);
            target.AddItem(firstProduct, 10);
            CartLine[] results = target.Lines
            .OrderBy(c => c.Product.ProductID).ToArray();

            Assert.Equal(2, results.Length);
            Assert.Equal(11, results[0].Quantity);
            Assert.Equal(1, results[1].Quantity);
        }

        [Fact]
        public void Can_Remove_Line()
        {
            Product firstProduct = new Product { ProductID = 1, Name = "P1" };
            Product secondProduct = new Product { ProductID = 2, Name = "P2" };
            Product thirdProduct = new Product { ProductID = 3, Name = "P3" };

            Cart target = new Cart();

            target.AddItem(firstProduct, 1);
            target.AddItem(secondProduct, 3);
            target.AddItem(thirdProduct, 5);
            target.AddItem(secondProduct, 1);

            target.RemoveLine(secondProduct);

            Assert.Empty(target.Lines.Where(c => c.Product == secondProduct));
            Assert.Equal(2, target.Lines.Count());
        }

        [Fact]
        public void Calculate_Cart_Total()
        {
            Product firstProduct = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product secondProduct = new Product { ProductID = 2, Name = "P2", Price = 50M };

            Cart target = new Cart();

            target.AddItem(firstProduct, 1);
            target.AddItem(secondProduct, 1);
            target.AddItem(firstProduct, 3);
            decimal result = target.ComputeTotalValue();

            Assert.Equal(450M, result);
        }

        [Fact]
        public void Can_Clear_Contents()
        {
            Product firstProduct = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product secondProduct = new Product { ProductID = 2, Name = "P2", Price = 50M };

            Cart target = new Cart();

            target.AddItem(firstProduct, 1);
            target.AddItem(secondProduct, 1);

            target.Clear();

            Assert.Empty(target.Lines);
        }
    }
}
