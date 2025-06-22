using CSharpSalesBE.Models.Entities;
using System.Linq;

namespace CSharpSalesBE.Services
{
    public class CartService
    {
        public void AddItem(Cart cart, Item item)
        {
            cart.Items.Add(item);
            Calculate(cart);
        }

        public void EditItem(Cart cart, int index, Item updatedItem)
        {
            if (index >= 0 && index < cart.Items.Count)
            {
                cart.Items[index] = updatedItem;
                Calculate(cart);
            }
        }

        public void RemoveItem(Cart cart, int index)
        {
            if (index >= 0 && index < cart.Items.Count)
            {
                cart.Items.RemoveAt(index);
                Calculate(cart);
            }
        }

        public void Calculate(Cart cart)
        {
            cart.Total = cart.Items.Sum(i => i.TaxedPrice);
            cart.Tax = cart.Items.Sum(i => i.Taxes);
        }
    }
}