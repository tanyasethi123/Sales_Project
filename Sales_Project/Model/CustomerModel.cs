using System;
using System.ComponentModel.DataAnnotations;

namespace SalesProject.Model
{
    public class CustomerModel
	{
		[Key]
		public string? customer_id { get; set; }

		public string? customer_type { get; set; }

		public string? gender { get; set; }

	}
}
