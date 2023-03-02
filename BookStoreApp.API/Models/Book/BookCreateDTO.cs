using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Models.Book
{
	public class BookCreateDTO
	{
		[Required]
		[StringLength(150)]
		[DefaultValue("Enter a book title")]
		public string Title { get; set; }

		[Required]
		[Range(1800,3000)]
		[DefaultValue(2020)]
		public int Year { get; set; }

		[Required]
		[StringLength(20)]
		[DefaultValue("Enter book ISBN")]
		public string Isbn { get; set; }

		[Required]
		[StringLength(250, MinimumLength = 10)]
		[DefaultValue("Enter a brief summary of book")]
		public string Summary { get; set; }

		[DefaultValue("-no image-")]
		public string Image { get; set; }

		[Required]
		[Range(0, int.MaxValue)]
		[DefaultValue(0)]
		public decimal Price { get; set; }
	}
}
