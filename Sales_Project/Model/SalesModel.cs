﻿using System.ComponentModel.DataAnnotations;

namespace SalesProject.Model
{
    public class SalesModel
    {
        [Key]
        [Display(Name ="Invoice ID")]
        public string? invoice_id { get; set; }


        public string? location_id { get; set; }
        [Display(Name = "Branch")]
        public string? branch { get; set; }

        public string? product_id { get; set; }
        [Display(Name = "Product Category")]
        public string? product_category { get; set; } 

        public string? customer_id { get; set; }
        [Display(Name = "Customer Type")]
        public string? customer_type { get; set; }
        [Display(Name = "Gender")]
        public string? gender { get; set; }

        public string? date_id { get; set; }
        [Display(Name = "Date")]
        public DateTime datetime_of_purchase { get; set; }

        [Display(Name = "Unit Price")]
        public double unit_price { get; set; }

        [Display(Name = "Quantity Sold")]
        public int quantity_sold { get; set; }

        
        public double total_price_of_goods_sold{ get; set; }
        public double tax_5_percent { get; set; }

        [Display(Name = "Total Sales After Taxes(5%)")]
        public double total_sales_after_taxes { get; set; }

        [Display(Name = "Payment Type")]
        public string? payment_type { get; set; }

    }

}

