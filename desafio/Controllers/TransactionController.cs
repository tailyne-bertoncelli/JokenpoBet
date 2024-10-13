using desafio.DataTransferObject.Generics;
using desafio.DataTransferObject.Transactions;
using desafio.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace desafio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionService _transactionService;
        private readonly IHttpContextAccessor _httpContext;

        public TransactionController(TransactionService transactionService, IHttpContextAccessor httpContext)
        {
            _transactionService = transactionService;
            _httpContext = httpContext;
        }

        [HttpPost]
        [Authorize(Roles = "PLAYER")]
        public async Task<IActionResult> CreateAposta([FromBody] CreateApostaDto apostaDto)
        {
            try
            {
                var request = await _transactionService.CreateAposta(apostaDto);
                return Ok(request);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(null, new List<string> { ex.Message }, 400));
            }
        }

        [HttpPut]
        [Route("canceled/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CanceledAposta([FromRoute]long id)
        {
            try
            {
                var request = await _transactionService.CanceledAposta(id);
                return Ok(request);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(null, new List<string> { ex.Message }, 400));
            }
        }

        [HttpGet]
        [Route("allTransactions")]
        [Authorize(Roles = "ADMIN,PLAYER")]
        public async Task<IActionResult> ListTransactions([FromBody] RequestTransactions pages)
        {
            try
            {
                var request = await _transactionService.ListTransactions(pages);
                return Ok(request);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(null, new List<string> { ex.Message }, 400));
            }
        }
    }
}
