using Microsoft.AspNetCore.Mvc;
using BooksAndQuotesApplication.Data;
[ApiController]
[Route("api/[controller]")]

public class QuotesController : ControllerBase
{
    private readonly AppDbContext _context;
    public QuotesController(AppDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    public IActionResult GetQuotes()
    {
        return Ok(_context.Quotes.ToList());
    }

    [HttpGet("{id}")]
    public IActionResult GetQuote(int id)
    {
        var quote = _context.Quotes.Find(id);

        if (quote == null)
            return NotFound();

        return Ok(quote);
    }

    [HttpPost]
    public IActionResult AddQuote([FromBody] Quote quote)
    {
        try
        {
            _context.Quotes.Add(quote);
            _context.SaveChanges();
            return Ok(quote);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.ToString());
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateQuote(int id, Quote updated)
    {
        var quote = _context.Quotes.Find(id);

        if (quote == null)
            return NotFound();

        quote.Text = updated.Text;
        

        _context.SaveChanges();

        return Ok(quote);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteQuote(int id)
    {
        var quote = _context.Quotes.Find(id);

        if (quote == null)
            return NotFound();

        _context.Quotes.Remove(quote);

        _context.SaveChanges();

        return Ok();
    }
}