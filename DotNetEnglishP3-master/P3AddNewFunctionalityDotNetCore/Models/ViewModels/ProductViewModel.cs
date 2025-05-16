using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using P3AddNewFunctionalityDotNetCore.Resources.Models.Services;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class ProductViewModel
    {
        [BindNever]
        public int Id { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(ProductService),
            ErrorMessageResourceName = nameof(ProductService.MissingName))]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Details { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(ProductService),
            ErrorMessageResourceName = nameof(ProductService.MissingStock))]
        [RegularExpression(@"^\d+$", 
            ErrorMessageResourceType = typeof(ProductService),
            ErrorMessageResourceName = nameof(ProductService.StockNotAnInteger))]
        [Range(1, int.MaxValue,
            ErrorMessageResourceType = typeof(ProductService),
            ErrorMessageResourceName = nameof(ProductService.StockNotGreaterThanZero))]
        public string Stock { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(ProductService),
            ErrorMessageResourceName = nameof(ProductService.MissingPrice))]
        [RegularExpression(@"^\d+(\.\d{1,2})?$",
            ErrorMessageResourceType = typeof(ProductService),
            ErrorMessageResourceName = nameof(ProductService.PriceNotANumber))]
        [Range(1, int.MaxValue,
            ErrorMessageResourceType = typeof(ProductService),
            ErrorMessageResourceName = nameof(ProductService.PriceNotGreaterThanZero))]
        public string Price { get; set; }
    }
}
