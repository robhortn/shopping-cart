using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart
{
  public class ShoppingCartService
  {
    private readonly List<CartItem> _cartItems;
    private const decimal SalesTaxPercentage = 0.125M;

    public ShoppingCartService()
    {
      _cartItems = new List<CartItem>();
    }

    public void AddItem(CartItem cartItem)
    {
      if (cartItem == null) throw new ArgumentNullException();
      if (cartItem.Item == null) throw new ArgumentNullException("Product must be given as part of Cart Item");

      var existingCartItem = _cartItems.FirstOrDefault(x => x.Item.Name == cartItem.Item.Name);
      if (existingCartItem != null)
      {
        existingCartItem.Quantity += cartItem.Quantity;
        return;
      }

      _cartItems.Add(cartItem);
    }

    public decimal GetTotalItemsCost()
    {
      return _cartItems.Sum(cartItem => cartItem.LineItemTotal);
    }

    public decimal GetTotalSalesTaxCost()
    {
      return decimal.Round(GetTotalItemsCost() * SalesTaxPercentage, MidpointRounding.AwayFromZero);
    }

    public decimal GetGrandTotalCartCost()
    {
      return GetTotalItemsCost() + GetTotalSalesTaxCost();
    }

    public List<CartItem> GetCartItems()
    {
      return _cartItems;
    }
  }
}