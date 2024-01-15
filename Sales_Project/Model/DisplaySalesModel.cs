using System;
using System.ComponentModel.DataAnnotations;

namespace SalesProject.Model
{
	public class DisplaySalesModel
	{
        [Key]
        public string? invoice_id { get; set; }

       // public string? location_id { get; set; }
       public string branch { get; set; } = null!;

        //public string? product_id { get; set; }
       public string product_category { get; set; } = null!;

       // public string? customer_id { get; set; }
        public string customer_type { get; set; } = null!;

        //public string? date_id { get; set; }
       public DateTime date_of_purchase { get; set; }

        public double unit_price { get; set; }
        public int quantity_sold { get; set; }
        public double total_sales_after_taxes { get; set; }
        public string? payment_type { get; set; }
    }
}

