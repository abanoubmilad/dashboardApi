using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DashboardApi.Entities
{
	public class Tag
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int TagId { get; set; }
		public string TagName { get; set; }
		public TagType TagType { get; set; }

		// relations
		public List<ItemTag> ItemTags { get; set; }
	}
}