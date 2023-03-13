using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Author;
using AutoMapper;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
	[Authorize]
	public class AuthorsController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
		private readonly IMapper mapper;
		private readonly ILogger<AuthorsController> logger;

		public AuthorsController(BookStoreDbContext context, IMapper mapper, ILogger<AuthorsController> logger)
        {
            _context = context;
			this.mapper = mapper;
			this.logger = logger;
		}

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorSelectDTO>>> GetAuthors()
        {
            if (_context.Authors == null)
            {
                return NotFound();
            }

            try
            {
				var authors = mapper.Map<IEnumerable<AuthorSelectDTO>>(await _context.Authors.ToListAsync());
				return Ok(authors);
			}
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error performing GET in {nameof(GetAuthors)} (api/Authors)");
                return StatusCode(500, Messages.Error500Message);
            }

		}

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorSelectDTO>> GetAuthor(int id)
        {
            if (_context.Authors == null)
            {
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<AuthorSelectDTO>(author));
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDTO authorDTO)
        {
            // IDs must match:
            if (id != authorDTO.Id)
            {
                return BadRequest();
            }

            // Fetch the requested author by ID. If no such author, return not found:
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            // Apply differences in authorDTO to the fetched author using mapper:
           mapper.Map(authorDTO, author);

            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
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

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<AuthorCreateDTO>> PostAuthor(AuthorCreateDTO authorDto)
        {
            if (_context.Authors == null)
            {
                return Problem("Entity set 'BookStoreDbContext.Authors'  is null.");
            }

            // Convert DTO to DB author:
            var author = mapper.Map<Author>(authorDto);

            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteAuthor(int id)
        {
            if (_context.Authors == null)
            {
                return NotFound();
            }
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            return (_context.Authors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
