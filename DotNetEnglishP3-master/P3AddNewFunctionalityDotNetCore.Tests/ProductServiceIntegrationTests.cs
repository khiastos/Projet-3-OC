using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using ProductService = P3AddNewFunctionalityDotNetCore.Models.Services.ProductService;
using Xunit;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Xml.Linq;

namespace P3AddNewFunctionalityDotNetCore.Tests.Integration;

public class ProductServiceIntegrationTests
{
    private DbContextOptions<P3Referential> _options;
    private P3Referential _context;
    private ProductService _productService;
    private Cart _cart;

    public ProductServiceIntegrationTests()
    {
        // Configuration InMemory sur la base de ma BDD existante P3Referential, le .Options renvoie les infos que je viens de donner à la BDD
        _options = new DbContextOptionsBuilder<P3Referential>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        // Création du contexte (pont entre mon code et la BDD), qui est la BDD InMemory avec une config vide car exigé par le constructeur
        _context = new P3Referential(_options, new ConfigurationBuilder().Build());

        // Création des dépendances nécessaires
        var productRepository = new ProductRepository(_context);
        var orderRepository = new OrderRepository(_context);
        var localizer = new Mock<IStringLocalizer<ProductService>>(); // Créer un mock pour les traductions des messages d'erreurs, car pas besoin ici
        _cart = new Cart();

        // Instanciation du vrai productService avec les mock et la BDD InMemory
        _productService = new ProductService(_cart, productRepository, orderRepository, localizer.Object);
    }

    public ProductViewModel CreateModel()
    {
        var model = new ProductViewModel
        {
            Name = "Test",
            Price = "30.00",
            Stock = "10"
        };
        _productService.SaveProduct(model);

        return model;
    }

    public Product GetProductFromDB(ProductViewModel productTest)
    {
        Product productFromDB = null;

        foreach (var p in _context.Product)
        {
            if (p.Name == productTest.Name)
            {
                productFromDB = p;
                break;
            }
        }
        return productFromDB;
    }

    #region SaveNewProductToDB
    [Fact]
    public void SaveNewProductToDB()
    {
        // Arrange
        var productTestSave = CreateModel();

        // Act
        var productFromDB = GetProductFromDB(productTestSave);

        // Assert
        Assert.NotNull(productFromDB);
        Assert.Equal(productTestSave.Name, productFromDB.Name);
        Assert.Equal(double.Parse(productTestSave.Price,CultureInfo.InvariantCulture), productFromDB.Price);
        Assert.Equal(int.Parse(productTestSave.Stock), productFromDB.Quantity);

        // Cleanup
        _productService.DeleteProduct(productFromDB.Id);
    }
#endregion

    #region DeleteProductFromDB
    [Fact]
    public void DeleteProductFromDB()
    {
        //Arrange
        var productTestDelete = CreateModel();

        //Act
        var productFromDB = GetProductFromDB(productTestDelete);

        //Assert
        Assert.NotNull(productFromDB);
        _productService.DeleteProduct(productFromDB.Id);

        var testDeleteVerification = GetProductFromDB(productTestDelete);
        Assert.Null(testDeleteVerification);
    }
    #endregion

    #region UpdateQuantitiesInDB
    [Fact]
    public void UpdatedQuantitiesInDB()
    {
        //Arrange
        var productTestUpdate = CreateModel();
        int quantityOrder = 3;

        // Act
        var productFromDB = GetProductFromDB(productTestUpdate);

        // Assert
        Assert.NotNull(productFromDB);

        _cart.AddItem(productFromDB, quantityOrder);
        _productService.UpdateProductQuantities();

        Assert.Equal(int.Parse(productTestUpdate.Stock) - quantityOrder, productFromDB.Quantity);

        // Cleanup
        _productService.DeleteProduct(productFromDB.Id);
    }
    #endregion
}