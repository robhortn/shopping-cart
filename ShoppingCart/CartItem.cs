namespace ShoppingCart
{
  public class CartItem
  {
    public CartItem()
    {
      Item = new Product();
    }

    public Product Item { get; set; }
    public int Quantity { get; set; }
    public decimal LineItemTotal => (Quantity * Item.Price);
  }
}