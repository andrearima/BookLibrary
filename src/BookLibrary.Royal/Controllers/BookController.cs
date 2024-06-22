using BookLibrary.Royal.App;
using BookLibrary.Royal.Domain.Filter;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Royal.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookApp _bookApp;
    private readonly ILogger<BookController> _logger;

    public BookController(ILogger<BookController> logger, IBookApp bookApp)
    {
        _logger = logger;
        _bookApp = bookApp;
    }

    [HttpGet]
    public async Task<IActionResult> GetBooks([FromQuery] BookFilter filters)
    {
        return Ok(await _bookApp.GetBooks(filters));
    }
}
