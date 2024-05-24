using System.ComponentModel.DataAnnotations;
namespace SalesProject.Model
{
	public class TotalSalesByProductCategory
	{
        [Key]
        [Display(Name = "Product Category")]
        public string? product_category { get; set; }

        [Display(Name = "Total Sales")]
        public double total_sales { get; set; }

        [Display(Name = "Average Rating")]
        public double average_rating { get; set; }

        public TotalSalesByProductCategory()
		{
		}
	}
}

