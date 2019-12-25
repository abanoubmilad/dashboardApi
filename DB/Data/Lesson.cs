using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DashboardApi.Entities
{
	public class Lesson
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int LessonId { get; set; }
		public string LessonName { get; set; }

		// relations
		public List<LessonItem> LessonItems { get; set; }
		public List<LessonCategory> LessonCategories { get; set; }

	}
}