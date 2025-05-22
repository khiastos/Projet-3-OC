using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using P3AddNewFunctionalityDotNetCore.Resources.Models.Services;
using ProductService = P3AddNewFunctionalityDotNetCore.Models.Services.ProductService;
using Xunit;
using Microsoft.Extensions.Configuration;

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

    #region SaveNewProductToDB
    [Fact]
    public void SaveNewProductToDB()
    {
        // Crée un produit de test
        var productSave = new ProductViewModel
        {
            Name = "TestSave",
            Price = "30.00",
            Stock = "45"
        };

        // Ajoute le produit à la BDD via la fonction SaveProduct() de ProductService
        _productService.SaveProduct(productSave);

        // Créer un produit pour après le comparé au produit créé précédemment
        Product produitAjoute = null;

        foreach (var p in _context.Product)
        {
            if (p.Name == "TestSave")
            {
                produitAjoute = p;
                break;
            }
        }

        // Vérifie que le produit a bien été ajouté
        Assert.NotNull(produitAjoute);
        Assert.Equal("TestSave", produitAjoute.Name);
        Assert.Equal(30.00, produitAjoute.Price);
        Assert.Equal(45, produitAjoute.Quantity);

        // Nettoie la base
        _productService.DeleteProduct(produitAjoute.Id);
    }
#endregion

    #region DeleteProductFromDB
    [Fact]
    public void DeleteProductFromDB()
    {
        //Arrange
        ProductViewModel productDelete = new ProductViewModel
        {
            Name = "TestDelete",
            Price = "20.99",
            Stock = "10",
        };

        //Act
        // Ajoute le produit à la BDD via la fonction SaveProduct() de ProductService, et le stock dans produitASupprimer
        _productService.SaveProduct(productDelete);
        Product produitASupprimer = null;

        foreach (var p in _context.Product)
        {
            if (p.Name == "TestDelete")
            {
                produitASupprimer = p;
                break;
            }
        }

        // Vérifie que les deux produits sont bien les mêmes, sinon échoue le test
        Assert.NotNull(produitASupprimer);

        // Supprime le produit de la BDD via la fonction DeleteProduct() de ProductService
        _productService.DeleteProduct(produitASupprimer.Id);

        //Assert
        // Vérifie que le produit a bien été supprimé grâce à son ID

        Product TestDeleteVerification = null;

        foreach (var p in _context.Product)
        {
            if (p.Id == produitASupprimer.Id)
            {
                TestDeleteVerification = p;
                break;
            }
        }
        Assert.Null(TestDeleteVerification);
    }
    #endregion

    #region UpdateQuantitiesInDB
    [Fact]
    public void UpdatedQuantitiesInDB()
    {
        //Arrange
        // Crée un produit de test avec un stock de 10
        ProductViewModel productUpdate = new ProductViewModel
        {
            Name = "Product with updated quantities",
            Price = "20",
            Stock = "10",
        };

        // On ajoute le produit à la BDD via la fonction SaveProduct() de ProductService
        _productService.SaveProduct(productUpdate);

        Product productToUpdate = null;

        foreach (var p in _context.Product)
        {
            if (p.Name == "Product with updated quantities")
            {
                productToUpdate = p;
                break;
            }
        }

        // Vérifie que le produit a bien été ajouté
        Assert.NotNull(productToUpdate);

        // On ajoute le produit au panier avec une quantité de 3
        _cart.AddItem(new Product { Id = productToUpdate.Id, Name = productToUpdate.Name }, 3);

        // Act
        // On appelle la méthode UpdateProductQuantities() de ProductService pour mettre à jour les quantités
        _productService.UpdateProductQuantities();

        // Assert
        Product updatedProduct = null;
        foreach (var p in _context.Product)
        {
            if (p.Id == productToUpdate.Id)
            {
                updatedProduct = p;
                break;
            }
        }
        Assert.NotNull(updatedProduct);
        Assert.Equal(7, updatedProduct.Quantity); // 10 - 3 = 7
        _productService.DeleteProduct(updatedProduct.Id);
    }
    #endregion
}