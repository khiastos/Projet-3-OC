using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using P3AddNewFunctionalityDotNetCore.Resources.Models.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests.Unitaire;

public class ProductServiceTests
{
    private List<ValidationResult> ValidateModel(ProductViewModel model)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(model);
        Validator.TryValidateObject(model, context, results, true);
        return results;
    }
    #region NameTest
    [Fact]
    public void Product_MissingName_ShouldReturnMissingNameError()
    {
        // Arrange
        var model = new ProductViewModel
        {
            Name = null,
            Price = "9.99",
            Stock = "5"
        };

        // Act
        var results = ValidateModel(model);

        // Assert
        bool found = false;

        foreach (var r in results)
        {
            if (r.ErrorMessage == ProductService.MissingName)
            {
                found = true;
                break;
            }
        }

        Assert.True(found);
    }
    #endregion

    #region PriceTests
    [Fact]
    public void Product_MissingPrice_ShouldReturnMissingPriceError()
    {
        // Arrange
        var model = new ProductViewModel
        {
            Name = "Produit test",
            Price = null,
            Stock = "5"
        };

        // Act
        var results = ValidateModel(model);

        // Assert
        bool found = false;

        foreach (var r in results)
        {
            if (r.ErrorMessage == ProductService.MissingPrice)
            {
                found = true;
                break;
            }
        }

        Assert.True(found);
    }

    [Fact]
    public void Product_PriceNotANumber_ShouldReturnPriceNotANumberError()
    {
        // Arrange
        var model = new ProductViewModel
        {
            Name = "Produit test",
            Price = "abc",
            Stock = "5"
        };

        // Act
        var results = ValidateModel(model);

        // Assert
        bool found = false;

        foreach (var r in results)
        {
            if (r.ErrorMessage == ProductService.PriceNotANumber)
            {
                found = true;
                break;
            }
        }

        Assert.True(found);
    }

    [Fact]
    public void Product_PriceNotGreaterThanZero_ShouldReturnPriceNotGreaterThanZero()
    {
        // Arrange
        var model = new ProductViewModel
        {
            Name = "Produit test",
            Price = "-1",
            Stock = "5"
        };

        // Act
        var results = ValidateModel(model);

        // Assert
        bool found = false;

        foreach (var r in results)
        {
            if (r.ErrorMessage == ProductService.PriceNotGreaterThanZero)
            {
                found = true;
                break;
            }
        }

        Assert.True(found);
    }
    #endregion

    #region StockTests
    [Fact]
    public void Product_MissingStock_ShouldReturnMissingStockError()
    {
        // Arrange
        var model = new ProductViewModel
        {
            Name = "Produit test",
            Price = "9.99",
            Stock = null
        };

        // Act
        var results = ValidateModel(model);

        // Assert
        bool found = false;

        foreach (var r in results)
        {
            if (r.ErrorMessage == ProductService.MissingStock)
            {
                found = true;
                break;
            }
        }

        Assert.True(found);
    }

    [Fact]
    public void Product_StockNotAnInteger_ShouldReturnStockNotAnInteger()
    {
        // Arrange
        var model = new ProductViewModel
        {
            Name = "Produit test",
            Price = "9.99",
            Stock = "5.5"
        };

        // Act
        var results = ValidateModel(model);

        // Assert
        bool found = false;

        foreach (var r in results)
        {
            if (r.ErrorMessage == ProductService.StockNotAnInteger)
            {
                found = true;
                break;
            }
        }

        Assert.True(found);
    }

    [Fact]
    public void Product_StockNotGreaterThanZero_ShouldReturnStockNotGreaterThanZero()
    {
        // Arrange
        var model = new ProductViewModel
        {
            Name = "Produit test",
            Price = "9.99",
            Stock = "-1"
        };

        // Act
        var results = ValidateModel(model);

        // Assert
        bool found = false;

        foreach (var r in results)
        {
            if (r.ErrorMessage == ProductService.StockNotGreaterThanZero)
            {
                found = true;
                break;
            }
        }

        Assert.True(found);
    }
    #endregion
}