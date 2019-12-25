using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DashboardApi.Entities
{
	public class Item
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ItemId { get; set; }
		public string ItemName { get; set; }
		public string ItemExternalLink { get; set; }
		public string ItemInternalLink { get; set; }
		public string ItemImageUrl { get; set; }

		// relations
		public List<LessonItem> LessonItems { get; set; }
		public List<ItemTag> ItemTags { get; set; }
	}
}