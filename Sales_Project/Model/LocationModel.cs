using System;
using System.ComponentModel.DataAnnotations;

namespace SalesProject.Model
{
	public class LocationModel
	{
		[Key]
		public string? location_id { get; set; }

		public string? branch { get; set; }

		public string? city { get; set; }



	}
}
