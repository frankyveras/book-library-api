using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

[ApiController]
[Route("api/books")]
public class BookController : ControllerBase
{
    private readonly BookContext _context;

    public BookController(BookContext context)
    {
        _context = context;
    }


    [HttpGet("search")]
    public IActionResult Search([FromQuery] string? author, [FromQuery] string? isbn, [FromQuery] string? type, [FromQuery] string? status, int page = 1, int pageSize = 10)
    {
        var query = _context.Books.AsQueryable();

        if (!string.IsNullOrEmpty(author))
        {
            string authorLower = author.ToLower();
            query = query.Where(b => (b.FirstName + " " + b.LastName).ToLower().Contains(authorLower));
        }

        if (!string.IsNullOrEmpty(isbn))
        {
            query = query.Where(b => b.ISBN == isbn);
        }

        if (!string.IsNullOrEmpty(type))
        {
            query = query.Where(b => b.Type.Contains(type));
        }

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(b => b.Status == status);
        }

        var totalCount = query.Count();
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        var results = query.OrderBy(b => b.BookId).Skip((page - 1) * pageSize).Take(pageSize).ToList();


        var result = new
        {
            totalItems = totalCount,
            totalPages = totalPages,
            page = page,
            items = results
        };

        return Ok(result);
    }


    [HttpGet]
    public async Task<IActionResult> GetAllBooks([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var query = _context.Books.AsQueryable();

        var totalBooks = await query.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalBooks / pageSize);

        if (page < 1 || page > totalPages)
        {
            return BadRequest("Invalid page number.");
        }

        var books = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return Ok(new
        {
            totalItems = totalBooks,
            totalPages = totalPages,
            page = page,
            items = books
        });
    }


    [HttpPost]
    public IActionResult AddBook([FromBody] Book book)
    {
        if (book == null)
        {
            return BadRequest("Invalid book data.");
        }

        _context.Books.Add(book);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetAllBooks), new { id = book.BookId }, book);
    }


    [HttpPut("{id}")]
    public IActionResult UpdateBook(int id, [FromBody] Book updatedBook)
    {
        var book = _context.Books.Find(id);
        if (book == null)
        {
            return NotFound("Book not found.");
        }

        book.Title = updatedBook.Title;
        book.FirstName = updatedBook.FirstName;
        book.LastName = updatedBook.LastName;
        book.TotalCopies = updatedBook.TotalCopies;
        book.CopiesInUse = updatedBook.CopiesInUse;
        book.Type = updatedBook.Type;
        book.ISBN = updatedBook.ISBN;
        book.Category = updatedBook.Category;

        _context.SaveChanges();

        return NoContent();
    }


    [HttpDelete("{id}")]
    public IActionResult DeleteBook(int id)
    {
        var book = _context.Books.Find(id);
        if (book == null)
        {
            return NotFound("Book not found.");
        }

        _context.Books.Remove(book);
        _context.SaveChanges();

        return NoContent();
    }
}
