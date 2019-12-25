using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DashboardApi.Entities
{
	public class LessonItem
	{
		public int LessonId { get; set; }
		public Lesson Lesson { get; set; }

		public int ItemId { get; set; }
		public Item Item { get; set; }

	}
}