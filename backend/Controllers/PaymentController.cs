using Microsoft.AspNetCore.Mvc;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;
using AuthorizeNetApp.Models;

namespace AuthorizeNetApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        [HttpPost("process")]
        public IActionResult ProcessPayment([FromBody] PaymentRequest request)
        {
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = request.ApiLoginId,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = request.TransactionKey
            };

            var creditCard = new creditCardType
            {
                cardNumber = request.CardNumber,
                expirationDate = request.ExpirationDate,
                cardCode = request.CardCode
            };

            var paymentType = new paymentType { Item = creditCard };

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),
                amount = request.Amount,
                payment = paymentType
            };

            var requestCreateTransaction = new createTransactionRequest { transactionRequest = transactionRequest };
            var controller = new createTransactionController(requestCreateTransaction);
            controller.Execute();

            var response = controller.GetApiResponse();

            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                return Ok(new { message = "Transaction Success", transactionId = response.transactionResponse.transId });
            }
            else
            {
                return BadRequest(response?.messages?.message[0]?.text ?? "Transaction Failed");
            }
        }
    }
}
