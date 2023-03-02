using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Models.Book
{
	public class BookUpdateDTO : BookBaseDTO
	{
		[Required]
		[StringLength(150)]
		[DefaultValue("Enter updated book title")]
		public string Title { get; set; }

		[Required]
		[Range(1800, 3000)]
		public int Year { get; set; }

		[Required]
		[StringLength(20)]
		public string Isbn { get; set; }

		[Required]
		[StringLength(250, MinimumLength = 10)]
		public string Summary { get; set; }

		public string? Image { get; set; }

		[Required]
		[Range(0, int.MaxValue)]
		public decimal Price { get; set; }
	}
}
