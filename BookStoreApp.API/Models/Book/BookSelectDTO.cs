﻿namespace BookStoreApp.API.Models.Book
{
	public class BookSelectDTO : BookBaseDTO
	{
		public string Title { get; set; }

		public int? Year { get; set; }

		public string? Image { get; set; }

		public decimal? Price { get; set; }

		public int? AuthorId { get; set; }

		public string AuthorName { get; set; }
	}
}
