using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Linq;

namespace P2FixAnAppDotNetCode.Models
{
    /// <summary>
    /// The Cart class
    /// </summary>
    public class Cart : ICart
    {
        private List<CartLine> cartLine = new List<CartLine>();
        /// <summary>
        /// Read-only property for display only
        /// </summary>
        public IEnumerable<CartLine> Lines => GetCartLineList();

        /// <summary>
        /// Return the actual cartline list
        /// </summary>
        /// <returns></returns>
        private List<CartLine> GetCartLineList()
        {
            return cartLine;
        }

        /// <summary>
        /// Adds a product in the cart or increment its quantity in the cart if already added
        /// </summary>//
        public void AddItem(Product product, int quantity)
        {
            // Selection de l'index de la ligne du panier en fonction du produit
            int index = GetCartLineList().FindIndex(l => l.Product.Id == product.Id);

            // Si le produit existe déja dans les lignes de Cart
            if (FindProductInCartLines(product.Id) is Product)
            {
                GetCartLineByIndex(index).Quantity = GetCartLineByIndex(index).Quantity + 1;
            }
            else
            {
                GetCartLineList().Add(new CartLine() { Product = product, Quantity = quantity });
            }
        }

        /// <summary>
        /// Removes a product form the cart
        /// </summary>
        public void RemoveLine(Product product) =>
            GetCartLineList().RemoveAll(l => l.Product.Id == product.Id);

        /// <summary>
        /// Get total value of a cart
        /// </summary>
        public double GetTotalValue()
        {
            double somme = 0;
            foreach (CartLine Line in Lines)
            {
                somme = somme + (Line.Product.Price * Line.Quantity);
            }
            return somme;
        }

        /// <summary>
        /// Get average value of a cart
        /// </summary>
        public double GetAverageValue()
        {
            int nombreArticlesTotal = 0;
            foreach (CartLine Line in Lines)
            {
                nombreArticlesTotal = nombreArticlesTotal + Line.Quantity;
            }
            return (GetTotalValue() / nombreArticlesTotal);
        }

        /// <summary>
        /// Looks after a given product in the cart and returns if it finds it
        /// </summary>
        public Product FindProductInCartLines(int productId)
        {
            foreach (CartLine Line in Lines)
            {
                if (Line.Product.Id == productId)
                {
                    return Line.Product;
                }
            }
            return null;
        }

        /// <summary>
        /// Get a specific cartline by its index
        /// </summary>
        public CartLine GetCartLineByIndex(int index)
        {
            return Lines.ToArray()[index];
        }

        /// <summary>
        /// Clears a the cart of all added products
        /// </summary>
        public void Clear()
        {
            List<CartLine> cartLines = GetCartLineList();
            cartLines.Clear();
        }
    }

    public class CartLine
    {
        public int OrderLineId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
