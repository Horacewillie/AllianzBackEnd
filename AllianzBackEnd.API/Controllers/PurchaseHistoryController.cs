using AllianzBackEnd.Core.Managers;
using AllianzBackEnd.Domain;
using AllianzBackEnd.Domain.Base.Entities.Users;
using AllianzBackEnd.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AllianzBackEnd.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [ProducesErrorResponseType(typeof(ApiResponse<>))]
    public class PurchaseHistoryController : BaseController
    {
        private readonly PurchaseHistoryManager _purchaseHistoryManager;
        public PurchaseHistoryController(PurchaseHistoryManager purchaseHistoryManager) 
            : base()
        {
            _purchaseHistoryManager = purchaseHistoryManager;
        }


        [HttpPost("MakePayment")]
        [Produces(typeof(ApiResponse<bool>))]
        public async Task<IActionResult> MakePayment([FromBody] PurchaseRequest request)
        {
            var response = await _purchaseHistoryManager.MakePayment(request);

            return Ok(response);
        }


        [HttpPost("GetPurchaseHistory")]
        [Produces(typeof(ApiResponse<PurchaseHistoryResponse>))]
        public async Task<IActionResult> GetPurchaseHistory([FromBody] GetPurchaseHistoryRequest request)
        {
            var response = await _purchaseHistoryManager.GetPurchaseHistory(request);

            return Ok(response);
        }


    }
}
