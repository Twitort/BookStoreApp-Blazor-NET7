using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using AutoMapper;
using BookStoreApp.API.Models.Book;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
	[Authorize]
	public class BooksController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
		private readonly IMapper mapper;

		public BooksController(BookStoreDbContext context, IMapper mapper)
        {
            _context = context;
			this.mapper = mapper;
		}

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookSelectDTO>>> GetBooks()
        {
            if (_context.Books == null)
            {
                return NotFound();
            }

            var books = await _context.Books.Include(q => q.Author)
                .ProjectTo<BookSelectDTO>(mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(books);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailsDTO>> GetBook(int id)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(q => q.Author)
                .ProjectTo<BookDetailsDTO>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> PutBook(int id, BookUpdateDTO bookDTO)
        {
            if (id != bookDTO.Id)
            {
                return BadRequest();
            }

            // Try to get book by ID:
            var book = await _context.Books.FindAsync(id);

            // If no such book ID, return not found:
            if (book == null)
            {
                return NotFound();
            }

            // Have a book, apply the differences from update DTO to the retrieved book:
            mapper.Map(bookDTO, book);
            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<BookCreateDTO>> PostBook(BookCreateDTO bookDTO)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'BookStoreDbContext.Books'  is null.");
            }

            var book = mapper.Map<Book>(bookDTO);
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteBook(int id)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> BookExists(int id)
        {
            return await _context.Books.AnyAsync(e => e.Id == id);
        }
    }
}
