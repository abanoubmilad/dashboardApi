using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DashboardApi.Entities
{
	public class ItemTag
	{
		public int ItemId { get; set; }
		public Item Item { get; set; }

		public int TagId { get; set; }
		public Tag Tag { get; set; }

	}
}