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

 

    [HttpPost] 

    public IActionResult AddQuote([FromBody] Quote quote)

    {

        try

        {

            if (quote == null)

            {

                return BadRequest("Quote is null");

            }

            if (string.IsNullOrWhiteSpace(quote.Text))

            {

                return BadRequest("Text is empty");

            }

            _context.Quotes.Add(quote);

            _context.SaveChanges();

            return Ok(quote);

        }

        catch (Exception ex)

        {

            return StatusCode(500, new

            {

                Message = ex.Message,

                Inner = ex.InnerException?.Message,

                Full = ex.ToString()

            });

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