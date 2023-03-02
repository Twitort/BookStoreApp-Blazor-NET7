using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Author;
using BookStoreApp.API.Models.Book;

namespace BookStoreApp.API.Configurations
{
	public class MapperConfig : Profile
	{
		public MapperConfig()
		{
			// Create a mapping (using AutoMapper) to and from AuthorCreateDTO and Author.
			// ReverseMap is what give use the bidirectional mapping support:
			CreateMap<AuthorCreateDTO, Author>().ReverseMap();
			CreateMap<AuthorSelectDTO, Author>().ReverseMap();
			CreateMap<AuthorUpdateDTO, Author>().ReverseMap();

			// Book mapping:
			CreateMap<BookCreateDTO, Book>().ReverseMap();
			CreateMap<Book, BookSelectDTO>()
				.ForMember(q => q.AuthorName, d => d.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"))
				.ReverseMap();
			CreateMap<Book, BookDetailsDTO>()
				.ForMember(q => q.AuthorName, d => d.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"))
				.ReverseMap();
			CreateMap<BookUpdateDTO, Book>().ReverseMap();
		}
	}
}
