using System.Numerics;
using System.Security.AccessControl;
using System.Reflection.Metadata;
using System.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuotesApi.Data;
using QuotesApi.Models;

namespace QuotesApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class QuotesController : ControllerBase
  {
    //Database communication
    private QuotesDbContext _quotesDbContext;
    //Constructor
    public QuotesController(QuotesDbContext quotesDbContext)
    {
        _quotesDbContext = quotesDbContext;
    }

    // Get api/quotes -> If setting caching to client it will only be stored on each particular client but not on the proxy server. If stored on the proxy server it will be updated all the time. 
    [HttpGet]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
    public IActionResult Get(string sort)
    {
      IQueryable<Quote> quotes;
      switch(sort)
      {
        case "desc":
            quotes = _quotesDbContext.Quotes.OrderByDescending(q => q.CreatedAt);
            break;
        case "asc":
            quotes = _quotesDbContext.Quotes.OrderBy(q => q.CreatedAt);
            break;
        default: 
            quotes = _quotesDbContext.Quotes;
            break;

      }
      
      return Ok(quotes);
    }
    
    //Get api/quotes/{id}
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
      var quote = _quotesDbContext.Quotes.Find(id);
      if (quote == null)
      {
        return NotFound("Not record found against this id");
      }

      return Ok(quote);
    }
    

    //We have some default params here incase we forget to push it in. 
    [HttpGet("[action]")]
    public IActionResult PagingQuote(int? pageNumber, int? pageSize)
    {
      var quotes = _quotesDbContext.Quotes;
      var currentPageNumber = pageNumber ?? 1;
      var currentPageSize = pageSize ?? 5;
      return Ok(quotes.Skip((currentPageNumber -1) * currentPageSize).Take(currentPageSize));
    }

    [HttpGet("[action]")]
    public IActionResult SearchQuote(string type)
    {
      var quotes = _quotesDbContext.Quotes.Where(q=> q.Type.StartsWith(type));

      return Ok(quotes);
    }

    // api/quotes/test/1
    [HttpGet("[action]/{id}")]
    public int Test(int id)
    {
      return id;
    }

    //Post api/quotes
    [HttpPost]
    public IActionResult Post([FromBody]Quote quote)
    {
      if(!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      _quotesDbContext.Quotes.Add(quote);
      _quotesDbContext.SaveChanges();
      return StatusCode(StatusCodes.Status201Created);
    }

    //Put api/quotes/{id}
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody]Quote quote)
    {
      if(!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var entity = _quotesDbContext.Quotes.Find(id);

      if (entity == null)
      {
        return NotFound("No record found against this id");
      }
      else
      {
        entity.Title = quote.Title;
        entity.Author = quote.Author;
        entity.Description = quote.Description;
        entity.Type = quote.Type;
        entity.CreatedAt = quote.CreatedAt;
        _quotesDbContext.SaveChanges();
        return Ok("Record Updated Successfully");
      }

      
    }

    //Delete api/quotes/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      var quote = _quotesDbContext.Quotes.Find(id);
      if(quote==null)
      {
        return NotFound("No record found against this Id");
      }
      else 
      {
        _quotesDbContext.Quotes.Remove(quote);
        _quotesDbContext.SaveChanges();
        return Ok("Quote Deleted");
      }
      
    }
  }
}