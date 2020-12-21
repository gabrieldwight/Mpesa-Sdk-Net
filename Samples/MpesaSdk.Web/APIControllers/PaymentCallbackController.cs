using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MpesaSdk.Callbacks;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MpesaSdk.Web.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentCallbackController : ControllerBase
    {
        private readonly ILogger<PaymentCallbackController> _logger;
        private readonly IWebHostEnvironment _environment;

        public PaymentCallbackController(ILogger<PaymentCallbackController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        /// <summary>
        /// Receives callback for payments done through stk push
        /// </summary>
        /// <param name="requestId">Randomly generated request Id</param>
        /// <param name="lipaNaMpesaCallback">Push payment payload</param>
        /// <returns></returns>
        [HttpPost("{requestId}/StkPushCallback")]
        public async Task<IActionResult> LipaNaMpesaCallback(string requestId, [FromBody] LipaNaMpesaCallback lipaNaMpesaCallback)
        {
            // Handle callback data for processing. Either store data in a database or do further processing.

            if (lipaNaMpesaCallback is null)
            {
                return Ok(new 
                {
                    ResultCode = 1,
                    ResultDesc = "Transaction Rejected"
                });
            }

            var filename = $"{requestId}.json";

            // Get root path directory
            var rootPath = Path.Combine(_environment.WebRootPath, "Application_Files\\MpesaResults\\");
            // To check if directory exists. If the directory does not exists we create a new directory
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            // Get the path of filename
            var filePath = Path.Combine(_environment.WebRootPath, "Application_Files\\MpesaResults\\", filename);

            await System.IO.File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(lipaNaMpesaCallback, Formatting.Indented));

            _logger.LogInformation(JsonConvert.SerializeObject(lipaNaMpesaCallback, Formatting.Indented));
            return Ok(new
            {
                ResultCode = "00000000",
                ResultDesc = "Success"
            });
        }

        /// <summary>
        /// C2B Payment validation
        /// </summary>
        /// <param name="customerToBusinessValidationCallback"></param>
        /// <returns></returns>
        [HttpPost("c2b/validation")]
        public async Task<IActionResult> C2BValidationCallback([FromBody]CustomerToBusinessValidationCallback customerToBusinessValidationCallback)
        {
            if (customerToBusinessValidationCallback is null)
            {
                return Ok(new
                {
                    ResultCode = 1,
                    ResultDesc = "Rejecting the transaction"
                });
            }

            var filename = $"{Guid.NewGuid()}.json";

            // Get root path directory
            var rootPath = Path.Combine(_environment.WebRootPath, "Application_Files\\C2BValidationResults\\");
            // To check if directory exists. If the directory does not exists we create a new directory
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            // Get the path of filename
            var filePath = Path.Combine(_environment.WebRootPath, "Application_Files\\C2BValidationResults\\", filename);

            await System.IO.File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(customerToBusinessValidationCallback, Formatting.Indented));

            _logger.LogInformation(JsonConvert.SerializeObject(customerToBusinessValidationCallback, Formatting.Indented));

            return Ok(new
            {
                ResultCode = "0",
                ResponseDesc = "success"
            });
        }

        /// <summary>
        /// C2B Payment and confirmation can be processed here
        /// </summary>
        /// <param name="response">Response Payload</param>
        /// <returns></returns>
        [HttpPost("c2b/confirmation")]
        public async Task<IActionResult> C2BPaymentCallback([FromBody]CustomerToBusinessCallback response)
        {
            if (response is null)
            {
                return Ok(new 
                { 
                    ResultCode = 1, 
                    ResultDesc = "Rejecting the transaction" 
                });
            }

            var filename = $"{Guid.NewGuid()}.json";

            // Get root path directory
            var rootPath = Path.Combine(_environment.WebRootPath, "Application_Files\\C2BConfirmationResults\\");
            // To check if directory exists. If the directory does not exists we create a new directory
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            // Get the path of filename
            var filePath = Path.Combine(_environment.WebRootPath, "Application_Files\\C2BConfirmationResults\\", filename);

            await System.IO.File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(response, Formatting.Indented));

            _logger.LogInformation(JsonConvert.SerializeObject(response, Formatting.Indented));

            return Ok(new 
            { 
                ResultCode = "0", 
                ResponseDesc = "success" 
            });
        }

        [HttpPost("b2c/confirmation")]
        public async Task<IActionResult> B2CPaymentCallback([FromBody] BusinessToCustomerCallback response)
        {
            if (response is null)
            {
                return Ok(new
                {
                    ResultCode = 1,
                    ResultDesc = "Rejecting the transaction"
                });
            }

            var filename = $"{Guid.NewGuid()}.json";

            // Get root path directory
            var rootPath = Path.Combine(_environment.WebRootPath, "Application_Files\\B2CConfirmationResults\\");
            // To check if directory exists. If the directory does not exists we create a new directory
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            // Get the path of filename
            var filePath = Path.Combine(_environment.WebRootPath, "Application_Files\\B2CConfirmationResults\\", filename);

            await System.IO.File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(response, Formatting.Indented));

            _logger.LogInformation(JsonConvert.SerializeObject(response, Formatting.Indented));

            return Ok(new
            {
                ResultCode = "00000000",
                ResponseDesc = "success"
            });
        }

        [HttpPost("b2c/timeout")]
        public async Task<IActionResult> B2CTimeoutCallback([FromBody] BusinessToCustomerCallback response)
        {
            if (response is null)
            {
                return Ok(new
                {
                    ResultCode = 1,
                    ResultDesc = "Rejecting the transaction"
                });
            }

            var filename = $"{Guid.NewGuid()}.json";

            // Get root path directory
            var rootPath = Path.Combine(_environment.WebRootPath, "Application_Files\\B2CTimeoutResults\\");
            // To check if directory exists. If the directory does not exists we create a new directory
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            // Get the path of filename
            var filePath = Path.Combine(_environment.WebRootPath, "Application_Files\\B2CTimeoutResults\\", filename);

            await System.IO.File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(response, Formatting.Indented));

            _logger.LogInformation(JsonConvert.SerializeObject(response, Formatting.Indented));

            return Ok(new
            {
                ResultCode = "00000000",
                ResponseDesc = "success"
            });
        }

        [HttpPost("b2b/confirmation")]
        public async Task<IActionResult> B2BPaymentCallback([FromBody] BusinessToBusinessCallback response)
        {
            if (response is null)
            {
                return Ok(new
                {
                    ResultCode = 1,
                    ResultDesc = "Rejecting the transaction"
                });
            }

            var filename = $"{Guid.NewGuid()}.json";

            // Get root path directory
            var rootPath = Path.Combine(_environment.WebRootPath, "Application_Files\\B2BConfirmationResults\\");
            // To check if directory exists. If the directory does not exists we create a new directory
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            // Get the path of filename
            var filePath = Path.Combine(_environment.WebRootPath, "Application_Files\\B2BConfirmationResults\\", filename);

            await System.IO.File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(response, Formatting.Indented));

            _logger.LogInformation(JsonConvert.SerializeObject(response, Formatting.Indented));

            return Ok(new
            {
                ResultCode = "00000000",
                ResponseDesc = "success"
            });
        }

        [HttpPost("b2b/timeout")]
        public async Task<IActionResult> B2BTimeoutCallback([FromBody] BusinessToBusinessCallback response)
        {
            if (response is null)
            {
                return Ok(new
                {
                    ResultCode = 1,
                    ResultDesc = "Rejecting the transaction"
                });
            }

            var filename = $"{Guid.NewGuid()}.json";

            // Get root path directory
            var rootPath = Path.Combine(_environment.WebRootPath, "Application_Files\\B2BTimeoutResults\\");
            // To check if directory exists. If the directory does not exists we create a new directory
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            // Get the path of filename
            var filePath = Path.Combine(_environment.WebRootPath, "Application_Files\\B2BTimeoutResults\\", filename);

            await System.IO.File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(response, Formatting.Indented));

            _logger.LogInformation(JsonConvert.SerializeObject(response, Formatting.Indented));

            return Ok(new
            {
                ResultCode = "00000000",
                ResponseDesc = "success"
            });
        }

        [HttpPost("reversal/confirmation")]
        public async Task<IActionResult> MpesaReversalCallback([FromBody] ReversalCallback response)
        {
            if (response is null)
            {
                return Ok(new
                {
                    ResultCode = 1,
                    ResultDesc = "Rejecting the transaction"
                });
            }

            var filename = $"{Guid.NewGuid()}.json";

            // Get root path directory
            var rootPath = Path.Combine(_environment.WebRootPath, "Application_Files\\MpesaReversalResults\\");
            // To check if directory exists. If the directory does not exists we create a new directory
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            // Get the path of filename
            var filePath = Path.Combine(_environment.WebRootPath, "Application_Files\\MpesaReversalResults\\", filename);

            await System.IO.File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(response, Formatting.Indented));

            _logger.LogInformation(JsonConvert.SerializeObject(response, Formatting.Indented));

            return Ok(new
            {
                ResultCode = "00000000",
                ResponseDesc = "success"
            });
        }

        [HttpPost("reversal/timeout")]
        public async Task<IActionResult> MpesaReversalTimeoutCallback([FromBody] ReversalCallback response)
        {
            if (response is null)
            {
                return Ok(new
                {
                    ResultCode = 1,
                    ResultDesc = "Rejecting the transaction"
                });
            }

            var filename = $"{Guid.NewGuid()}.json";

            // Get root path directory
            var rootPath = Path.Combine(_environment.WebRootPath, "Application_Files\\MpesaReversalTimeoutResults\\");
            // To check if directory exists. If the directory does not exists we create a new directory
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            // Get the path of filename
            var filePath = Path.Combine(_environment.WebRootPath, "Application_Files\\MpesaReversalTimeoutResults\\", filename);

            await System.IO.File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(response, Formatting.Indented));

            _logger.LogInformation(JsonConvert.SerializeObject(response, Formatting.Indented));

            return Ok(new
            {
                ResultCode = "00000000",
                ResponseDesc = "success"
            });
        }

        [HttpPost("transactionStatus")]
        public async Task<IActionResult> MpesaTransactionStatusResultCallback([FromBody] TransactionStatusCallback response)
        {
            if (response is null)
            {
                return Ok(new
                {
                    ResultCode = 1,
                    ResultDesc = "Rejecting the transaction"
                });
            }

            var filename = $"{Guid.NewGuid()}.json";

            // Get root path directory
            var rootPath = Path.Combine(_environment.WebRootPath, "Application_Files\\MpesaTransactionStatusResults\\");
            // To check if directory exists. If the directory does not exists we create a new directory
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            // Get the path of filename
            var filePath = Path.Combine(_environment.WebRootPath, "Application_Files\\MpesaTransactionStatusResults\\", filename);

            await System.IO.File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(response, Formatting.Indented));

            _logger.LogInformation(JsonConvert.SerializeObject(response, Formatting.Indented));

            return Ok(new
            {
                ResultCode = "00000000",
                ResponseDesc = "success"
            });
        }

        [HttpPost("transactionStatus/timeout")]
        public async Task<IActionResult> MpesaTransactionStatusTimeoutCallback([FromBody] TransactionStatusCallback response)
        {
            if (response is null)
            {
                return Ok(new
                {
                    ResultCode = 1,
                    ResultDesc = "Rejecting the transaction"
                });
            }

            var filename = $"{Guid.NewGuid()}.json";

            // Get root path directory
            var rootPath = Path.Combine(_environment.WebRootPath, "Application_Files\\MpesaTransactionStatusTimeout\\");
            // To check if directory exists. If the directory does not exists we create a new directory
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            // Get the path of filename
            var filePath = Path.Combine(_environment.WebRootPath, "Application_Files\\MpesaTransactionStatusTimeout\\", filename);

            await System.IO.File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(response, Formatting.Indented));

            _logger.LogInformation(JsonConvert.SerializeObject(response, Formatting.Indented));

            return Ok(new
            {
                ResultCode = "00000000",
                ResponseDesc = "success"
            });
        }

        [HttpPost("AccountBalance")]
        public async Task<IActionResult> MpesaAccountBalanceResultCallback([FromBody] AccountBalanceCallback response)
        {
            if (response is null)
            {
                return Ok(new
                {
                    ResultCode = 1,
                    ResultDesc = "Rejecting the transaction"
                });
            }

            var filename = $"{Guid.NewGuid()}.json";

            // Get root path directory
            var rootPath = Path.Combine(_environment.WebRootPath, "Application_Files\\MpesaAccountBalanceResults\\");
            // To check if directory exists. If the directory does not exists we create a new directory
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            // Get the path of filename
            var filePath = Path.Combine(_environment.WebRootPath, "Application_Files\\MpesaAccountBalanceResults\\", filename);

            await System.IO.File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(response, Formatting.Indented));

            _logger.LogInformation(JsonConvert.SerializeObject(response, Formatting.Indented));

            return Ok(new
            {
                ResultCode = "00000000",
                ResponseDesc = "success"
            });
        }

        [HttpPost("AccountBalance/timeout")]
        public async Task<IActionResult> MpesaAccountBalanceTimeoutCallback([FromBody] AccountBalanceCallback response)
        {
            if (response is null)
            {
                return Ok(new
                {
                    ResultCode = 1,
                    ResultDesc = "Rejecting the transaction"
                });
            }

            var filename = $"{Guid.NewGuid()}.json";

            // Get root path directory
            var rootPath = Path.Combine(_environment.WebRootPath, "Application_Files\\MpesaAccountBalanceTimeout\\");
            // To check if directory exists. If the directory does not exists we create a new directory
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            // Get the path of filename
            var filePath = Path.Combine(_environment.WebRootPath, "Application_Files\\MpesaAccountBalanceTimeout\\", filename);

            await System.IO.File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(response, Formatting.Indented));

            _logger.LogInformation(JsonConvert.SerializeObject(response, Formatting.Indented));

            return Ok(new
            {
                ResultCode = "00000000",
                ResponseDesc = "success"
            });
        }
    }
}
