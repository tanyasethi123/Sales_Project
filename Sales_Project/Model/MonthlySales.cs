using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SalesProject.Model
{
	public class MonthlySales
	{
        [Key]
        [Display(Name = "Month - Year")]
        public string? month_year { get; set; }

        [Display(Name = "Total Sales")]
        public double total_sales { get; set; }
        public MonthlySales()
		{
		}
	}
}

