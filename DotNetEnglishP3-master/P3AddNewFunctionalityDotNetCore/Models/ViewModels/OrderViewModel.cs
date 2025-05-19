using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using P3.Resources.Models;
using P3AddNewFunctionalityDotNetCore.Resources.Models;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class OrderViewModel
    {
        [BindNever]
        public int OrderId { get; set; }

        [BindNever]
        public ICollection<CartLine> Lines { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(Order),
            ErrorMessageResourceName = nameof(Order.ErrorMissingName))]
        public string Name { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(Order),
            ErrorMessageResourceName = nameof(Order.ErrorMissingAddress))]
        public string Address { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(Order),
            ErrorMessageResourceName = nameof(Order.ErrorMissingCity))]
        public string City { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(Order),
            ErrorMessageResourceName = nameof(Order.ErrorMissingZipCode))]
        public string Zip { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(Order),
            ErrorMessageResourceName = nameof(Order.ErrorMissingCountry))]
        public string Country { get; set; }

        [BindNever]
        public DateTime Date { get; set; }
    }
}
