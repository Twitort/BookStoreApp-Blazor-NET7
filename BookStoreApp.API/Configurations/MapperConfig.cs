using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Author;

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
		}
	}
}
