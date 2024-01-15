using System;
using System.ComponentModel.DataAnnotations;

namespace SalesProject.Model
{
	public class DateModel
	{
		[Key]
		public string? date_id { get; set; }

		public DateTime datetime_of_purchase { get; set; }


	}
}

