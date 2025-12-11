using Doan.Models;
using Doan.service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Doan.Controllers
{
    [Route("Home/Chat")]   // <--- Route phải khớp với fetch()
    [ApiController]
    public class QueryController : ControllerBase
    {
        private readonly RagPipeline _rag;

        public QueryController(RagPipeline rag)
        {
            _rag = rag;
        }

        [HttpPost]   // <--- Chỉ nhận POST
        public async Task<IActionResult> Post([FromBody] QueryRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.question))
            {
                return BadRequest("Question cannot be empty.");
            }

            var answer = await _rag.AskAsync(request.question, cancellationToken);
            return Ok(new { Answer = answer });
        }
    }
}
