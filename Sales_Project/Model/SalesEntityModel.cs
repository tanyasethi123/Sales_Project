using System.ComponentModel.DataAnnotations;

namespace SalesProject.Model
{
    public class SalesEntityModel
    {
        [Key]
        public string? invoice_id { get; set; }


        public string? location_id { get; set; }

        public string? product_id { get; set; }

        public string? customer_id { get; set; }

        public string? date_id { get; set; }

        public double unit_price { get; set; }

        public int quantity_sold { get; set; }


        public double total_price_of_goods_sold { get; set; }
        public double tax_5_percent { get; set; }

        public double total_sales_after_taxes { get; set; }

        public string? payment_type { get; set; }

        public SalesViewModel ViewModel()
        {
            return new SalesViewModel
            {

                invoice_id = this.invoice_id,
                customer_id = this.customer_id,
                product_id = this.product_id,
                location_id = this.location_id,
                unit_price = this.unit_price,
                quantity_sold = this.quantity_sold,
                total_sales_after_taxes = this.total_sales_after_taxes,
                payment_type = this.payment_type

            };

        }
    }
}

