using System;
using System.ComponentModel.DataAnnotations;

namespace SalesProject.Model
{
	public class ProductModel
	{
		[Key]
		public string? product_id { get; set; }

		public string? product_category { get; set; }
	}
}

