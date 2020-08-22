using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace QuotesApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ValuesController : ControllerBase
  {
    // Get api/values
    [HttpGet]
    public ActionResult<IEnumerable<string>> Get()
    {
      return new string[] {"value 1", "value 2"};
    }

    [HttpGet("{id}")]
    public ActionResult<string> GetAction(int id)
    {
      return "value";
    }
  }
}