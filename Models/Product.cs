using System.ComponentModel.DataAnnotations;

namespace SampleEndPoint.Models
{
    public class Product
    {
        [Key]
		public long Id { get; set; }

		[Required(ErrorMessage = "Field is required")]
		[MaxLength(50, ErrorMessage = "Max size are 50 characters")]
		[MinLength(1, ErrorMessage = "Min size is 1 characters")]
		public string Name { get; set; }

		public long CategoryId { get; set; }

		public Category Category { get; set; }
    }
}