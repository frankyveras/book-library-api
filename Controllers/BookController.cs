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

    // Search books based on Author, ISBN, or Type (own/love/want-to-read)
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

        var results = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        if (!results.Any())
        {
            return NotFound("No books found.");
        }

        return Ok(results);
    }

    // Retrieve all books
    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await _context.Books.ToListAsync();
        return Ok(books);
    }

    // Add a new book
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

    // Update book details
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

    // Delete a book
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
