using BooksAndQuotesApplication.Data;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly AppDbContext _context;

    public BooksController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetBooks()
    {
        return Ok(_context.Books.ToList());
    }

    [HttpPost]
    public IActionResult AddBook([FromBody] Book book)
    {
        _context.Books.Add(book);
        _context.SaveChanges();

        return Ok(book);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateBook(int id, Book updated)
    {
        var book = _context.Books.Find(id);

        if (book == null)
            return NotFound();

        book.Title = updated.Title;
        book.Author = updated.Author;
        book.PublicationDate = updated.PublicationDate;

        _context.SaveChanges();

        return Ok(book);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBook(int id)
    {
        var book = _context.Books.Find(id);

        if (book == null)
            return NotFound();

        _context.Books.Remove(book);
        _context.SaveChanges();

        return Ok();
    }
}