using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DashboardApi.Entities
{
	public class LessonCategory
	{
		public int LessonId { get; set; }
		public Lesson Lesson { get; set; }

		public int CategoryId { get; set; }
		public Category Category { get; set; }

	}
}