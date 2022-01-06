using System;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;

namespace ShoppingCart.UnitTests
{
  [TestFixture]
  public class Tests
  {
    private ShoppingCartService _shoppingCartService;

    [SetUp]
    public void Setup()
    {
      _shoppingCartService = new ShoppingCartService();
    }

    [Test]
    public void Add_WhenCalledWithSingleProduct_ShouldAddNewProduct()
    {
      const int expectedNumberOfCartItems = 1;
      const string itemName = "Dove Soap";
      const decimal itemPrice = 39.99M;
      var item = new CartItem { Item = new Product { Name = itemName, Price = itemPrice }, Quantity = 1 };

      _shoppingCartService.AddItem(item);

      Assert.That(_shoppingCartService.GetCartItems().Count == expectedNumberOfCartItems, $"Number of cart items did not equal {expectedNumberOfCartItems}");
      Assert.That(_shoppingCartService.GetCartItems()[0].Item.Name == itemName, $"Item name does not equal {itemName}");
      Assert.That(_shoppingCartService.GetTotalItemsCost() == itemPrice, $"Item price does not equal {itemPrice}");
    }

    [Test]
    public void Add_WhenCalledWithNullCartItem_ShouldThrowArgumentNullException()
    {
      try
      {
        _shoppingCartService.AddItem(null);
        Assert.Fail("Expected this to throw ArgumentNullException");
      }
      catch (Exception e)
      {
        Assert.That(e, Is.TypeOf<ArgumentNullException>());
      }
    }

    [Test]
    public void Add_WhenCalledWithNullCartItemProduct_ShouldThrowArgumentNullException()
    {
      try
      {
        _shoppingCartService.AddItem(new CartItem { Quantity = 1, Item = null });
        Assert.Fail("Expected this to throw ArgumentNullException");
      }
      catch (Exception e)
      {
        Assert.That(e, Is.TypeOf<ArgumentNullException>());
      }
    }

    [Test]
    public void Add_WhenCalledWithManyProducts_ShouldAddNewProducts()
    {
      const int expectedNumberOfCartItems = 1;
      const int expectedItemQuantity = 8;
      const string itemName = "Dove Soap";
      const decimal itemPrice = 39.99M;
      const decimal cartItemsTotalPrice = expectedItemQuantity * itemPrice;
      var item = new CartItem { Item = new Product { Name = itemName, Price = itemPrice }, Quantity = 5 };

      _shoppingCartService.AddItem(item);

      var additionalCartItem = new CartItem { Item = new Product { Name = itemName, Price = itemPrice }, Quantity = 3 };
      _shoppingCartService.AddItem(additionalCartItem);
      var totalCartItemsCount = _shoppingCartService.GetCartItems().Count;
      var cartTotal = _shoppingCartService.GetTotalItemsCost();

      Assert.That(totalCartItemsCount == expectedNumberOfCartItems, $"Number of cart items did not equal {expectedNumberOfCartItems}, got {totalCartItemsCount}");
      Assert.That(_shoppingCartService.GetCartItems()[0].Item.Name == itemName, $"Item name does not equal {itemName}, got {_shoppingCartService.GetCartItems()[0].Item.Name}");
      Assert.That(_shoppingCartService.GetCartItems()[0].Quantity == expectedItemQuantity, $"Item name does not equal {itemName}, got {_shoppingCartService.GetCartItems()[0].Quantity}");
      Assert.That(_shoppingCartService.GetCartItems()[0].Item.Price == itemPrice, $"Item price does not equal {itemPrice}, got {_shoppingCartService.GetCartItems()[0].Item.Price}");
      Assert.That(cartTotal == cartItemsTotalPrice, $"Total cart items price does not equal {cartItemsTotalPrice}, got {cartTotal}");
    }

    [Test]
    public void GetTotalSalesTaxCost_WhenCalled_ShouldCalculateSalesTax()
    {
      var lineItem1 = new CartItem { Quantity = 2, Item = new Product { Price = 39.99M, Name = "Dove Soap" } };
      var lineItem2 = new CartItem { Quantity = 2, Item = new Product { Price = 99.99M, Name = "Axe Deo" } };
      var expectedGrandTotalCartCost = 314.96M;

      _shoppingCartService.AddItem(lineItem1);
      _shoppingCartService.AddItem(lineItem2);

      Assert.That(_shoppingCartService.GetCartItems()[0].Item.Name == "Dove Soap", $"Item name does not equal 'Dove Soap', got {_shoppingCartService.GetCartItems()[0].Item.Name}");
      Assert.That(_shoppingCartService.GetCartItems()[0].Quantity == 2, $"Item name does not equal '2', got {_shoppingCartService.GetCartItems()[0].Quantity}");

      Assert.That(_shoppingCartService.GetCartItems()[1].Item.Name == "Axe Deo", $"Item name does not equal 'Axe Deo', got {_shoppingCartService.GetCartItems()[0].Item.Name}");
      Assert.That(_shoppingCartService.GetCartItems()[1].Quantity == 2, $"Item name does not equal '2', got {_shoppingCartService.GetCartItems()[0].Quantity}");

      Assert.That(_shoppingCartService.GetGrandTotalCartCost() == expectedGrandTotalCartCost, $"Expected cart grand total does not equal {expectedGrandTotalCartCost}, got {_shoppingCartService.GetGrandTotalCartCost()}");
    }

    [Test]
    public void GetTotalSalesTaxCost_WhenCalledOnEmptyCart_ShouldReturnZero()
    {
      Assert.That(_shoppingCartService.GetTotalSalesTaxCost() == 0, "Expected empty shopping cart tax total to be zero");
    }

    [Test]
    public void GetGrandTotalCartCost_WhenCalledOnEmptyCart_ShouldReturnZero()
    {
      Assert.That(_shoppingCartService.GetGrandTotalCartCost() == 0, "Expected empty shopping cart tax total to be zero");
    }
  }
}
