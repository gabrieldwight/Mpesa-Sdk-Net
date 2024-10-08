# Mpesa-Sdk-Net
This is an M-Pesa SDK that allows you to integrate Safaricom's M-Pesa API in Net Framework, NetCore, NET5, and Net Standard projects. 
[![Build status](https://gabrieldwight.visualstudio.com/MpesaSdk/_apis/build/status/MpesaSdk-CI)](https://gabrieldwight.visualstudio.com/MpesaSdk/_build/latest?definitionId=6)
[![NuGet version (MpesaSdk)](https://img.shields.io/nuget/v/MpesaSdk.svg?style=flat-square)](https://www.nuget.org/packages/MpesaSdk/)
 
A .NET Standard M-PESA API Helper Library for .NET Developers.
- [End User License](https://github.com/gabrieldwight/Mpesa-Sdk-Net/blob/master/LICENSE)
- [NuGet Package](https://www.nuget.org/packages/MpesaSdk/)
- [Mpesa Daraja Portal](https://developer.safaricom.co.ke/)
- [Pull Transaction API](https://documenter.getpostman.com/view/1724456/SVtTy8sd#intro)

## Required API Products Need To be enabled by api support to your mpesa applications
- Pull Transaction API
- Dynamic Mpesa QR
- Mpesa Ratiba

## Supported Platforms

|   *Platform*   | .Net 6.0 | .Net 5.0 | .NET Core | .NET Framework | Mono | Xamarin.iOS | Xamarin.Android | Xamarin.Mac |     UWP    |
|:--------------:|---------:|---------:|:---------:|:--------------:|:----:|:-----------:|:---------------:|:-----------:|:----------:|
| *Min. Version* |    6     |    5     |    2.0    |      4.6.1     |  5.4 |    10.14    |       8.0       |     3.8     | 10.0.16299 |

## Installation
- PackageManager: ```PM> Install-Package MpesaSdk```
- DotNetCLI: ```> dotnet add package MpesaSdk```

## Setting yourself up for successful Mpesa integration
Before proceeding, kindly acquaint yourself with Mpesa Apis by going through the Docs in Safaricom's developer portal or Daraja if you like.

1.  Obtain consumerKey, consumerSecret and Passkey (for Lipa Na Mpesa Online APIs) from daraja portal.

2.  Ensure your project is running on the minimum supported versions of .Net 

3.  MpesaSdk is dependency injection (DI) friendly and can be readily injected into your classes. You can read more on DI in Asp.Net core [**here**](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-5.0). If you can't use DI you can always manually create a new instance of MpesaClient and pass in an httpClient instance in its constructor. eg.

```c#
// When Dependency Injection is not possible...

//create httpclient instance
var httpClient = new HttpClient();

httpClient.BaseAddress = MpesaRequestEndPoint.SandboxBaseAddress; //Use MpesaRequestEndPoint.LiveBaseAddress in production
	
//create Mpesa API client instance
var mpesaClient = new MpesaClient(httpClient); //make sure to pass httpclient intance as an argument
	
```
I would recommend creating MpesaClient using Dependency Injection. [Optional] You can use any IOC container or Microsoft DI container in your legacy projects.
```c#
// Adding Dependency Injection into legacy projects

public static IServiceProvider ServiceProvider;


// To be used in the main application startup method
void Application_Start(object sender, EventArgs e)
{
  var hostBuilder = new HostBuilder();
  hostBuilder.ConfigureServices(ConfigureServices);
  var host = hostBuilder.Build();

  ServiceProvider = host.Services;
}

void ConfigureServices(IServiceCollection services)
{
   services.AddHttpClient<IMpesaClient, MpesaClient>(options => options.BaseAddress = MpesaRequestEndPoint.SandboxBaseAddress);
   //inject services here
}
	
```

## Registering MpesaClient & Set the BaseAddress -Dependency Injection Method in ASPNETCORE
* Install MpesaSdk Project via Nuget Package Manager Console or Nuget Package Manager GUI.

## For ASPNETCORE projects
* In **Startup.cs** add the namespace...

```c#    
using MpesaSdk;
```

* Inside ConfigureServices method add the following

```c#
services.Configure<MpesaApiConfiguration>(options =>
            {
                Configuration.GetSection("MpesaApiConfiguration").Bind(options);
            });
services.AddHttpClient<IMpesaClient, MpesaClient>(options => options.BaseAddress = MpesaRequestEndPoint.SandboxBaseAddress);
```

## OR using MpesaSDK Extension
```c#    
using MpesaSdk.Extensions;
```

* Inside ConfigureServices method add the following

```c#
services.Configure<MpesaApiConfiguration>(options =>
            {
                Configuration.GetSection("MpesaApiConfiguration").Bind(options);
            });
services.AddMpesaService(Enums.Environment.Sandbox);
```


Use ```MpesaRequestEndPoint.LiveBaseAddress``` as base address/base url in production. You can do an environment check using the IHostingEnvironment property in asp.net core.

* Once the MpesaClient is registered, you can pass it and use it in your classes to make API calls to Mpesa Server as follows;
```c#
using MpesaSdk; //Add MpesaSdk namespace
public class PaymentsController
{
	private readonly IMpesaClient _mpesaClient;
	public PaymentsController(IMpesaCleint mpesaClient)
	{
		_mpesaClient = mpesaClient;
	}
	....
	//code omitted for brevity
}
```

## Requesting for the Accesstoken
Mpesa APIs require authorization to use the APIs. The accesstoken (auth token) has to be used with each api call. The accesstoken expire after an hour so it is recommended that you use a caching strategy to refresh the token after every hour or less depending on how  much traffic your site has.

* To get an accesstoken, invoke the ``` _mpesaClient.GetAuthTokenAsync(*args); ``` method. You have to await the Async call. use Non-Async method call provided if you cannot leverage async.

```c# 
//Async Method
var accesstoken = await _mpesaClient.GetAuthTokenAsync(ConsumerKey, ConsumerSecret);

```

Note that you have to pass in a consusmerKey, ConsumerSecret provided by Mpesa.


## C2B Register Urls Request
```c#
var RegisterC2BUrlObject = new CustomerToBusinessRegisterUrl
(
  ShortCode: "ShortCode",
  ResponseType: "ResponseType",
  ConfirmationURL: "ConfirmationURL",
  ValidationURL: "ValidationURL"
);

var c2bRegisterUrlrequest = await _mpesaClient.RegisterC2BUrlAsync(RegisterC2BUrlObject, accesstoken);
```

## C2B Payment Request (For Sandbox Environment Only)
```c#
//C2B Object
Var CustomerToBusinessSimulateObject = new CustomerToBusinessSimulate
(
  ShortCode: "ShortCode",
  CommandID: "CommandID",
  Amount: "Amount",
  Msisdn: "Msisdn",
  BillRefNumber: "BillRefNumber"
);

var c2brequest = await _mpesaClient.MakeC2BPaymentAsync(CustomerToBusinessSimulateObject, accesstoken);
```

## LipaNaMpesaOnline/MpesaExpress (STK Push) Payment Request

```c#
// initialize object with data
var MpesaExpressObject = new LipaNaMpesaOnline
(
    businessShortCode: "BusinessShortCode",
    timeStamp: "TimeStamp",
    transactionType: "TransactionType",
    amount: "Amount",
    partyA: "PartyA",
    partyB: "PartyB",
    phoneNumber: "PhoneNumber",
    callBackUrl: "CallBackUrl",
    accountReference: "AccountReference",
    transactionDescription: "TransactionDescription",
    passkey: "PassKey"
);

//Make payment request 
var paymentrequest = await _mpesaClient.MakeLipaNaMpesaOnlinePaymentAsync(MpesaExpressObject, accesstoken));

```

## LipaNaMpesaOnline/MpesaExpress Transaction Query Request
```c#
var QueryLipaNaMpesaTransactionObject = new LipaNaMpesaQuery
(
	 businessShortCode: "LipaNaMpesaOnlineShortCode",
	 checkoutRequestId: "CheckoutRequestID",
	 passKey: "passKey",
	 timestamp: "Timestamp"
);

var stkpushquery = await _mpesaClient.QueryLipaNaMpesaTransactionAsync(QueryLipaNaMpesaTransactionObject, accesstoken);
```

## B2C Payment Request
```c#
//B2C Object
var BusinessToCustomerObject = new BusinessToCustomer
(
    InitiatorName: "InitiatorName", // Test data for initiator like safaricom.x or api_xxx
    SecurityCredential: "SecurityCredential", // Password credential used in mpesa portal (Use MpesaSdk.Extensions.MpesaCredentials)
    CommandID: "CommandID", // Please use the correct command -usage depends on what is enabled for your shortcode. More info 
    Amount: "Amount",
    PartyA: "PartyA", // Test for Party A 603047
    PartyB: "PartyB", // Receipient Phone Number (07XXXX123) 
    Remarks: "Remarks",
    QueueTimeOutURL: "QueueTimeOutURL", // URL to send the B2C timeout results
    ResultURL: "ResultURL" // URL to send the B2C callback results
    Occasion: "Occasion"
);

var b2crequest = await _mpesaClient.MakeB2CPaymentAsync(BusinessToCustomerObject, accesstoken);

```

## B2B Payment Request

```c#
var BusinessToBusinessObject = new BusinessToBusinessDto
( 
  InitiatorName: "InitiatorName", // Test data for initiator like safaricom.x or api_xxx
  SecurityCredential: "SecurityCredential", // Password credential used in mpesa portal (Use MpesaSdk.Extensions.MpesaCredentials)
  CommandID: "CommandID", // Please use the correct command -usage depends on what is enabled for your shortcode. More info
  SenderIdentifierType: "SenderIdentifierType", // Test for paybill identifiertype 4
  RecieverIdentifierType: "RecieverIdentifierType", // Test for paybill identifiertype 4. Read on receiver identifier types from daraja
  Amount: "Amount",
  PartyA: "PartyA", // Test for Party A 603047
  PartyB: "PartyB", // Test for Party B 600000
  AccountReference: "AccountReference"
  Remarks: "Remarks",
  QueueTimeOutURL: "QueueTimeOutURL", // URL to send the B2B timeout results
  ResultURL: "ResultURL" // URL to send the B2B callback results
);

var b2brequest = await _mpesaClient.MakeB2BPaymentAsync(BusinessToBusinessObject, accesstoken);

```

## Transaction Status Request
```c#
var TransactionStatusObject = new MpesaTransactionStatus
(  
  Initiator: "Initiator", // Test data for initiator like safaricom.x or api_xxx
  SecurityCredential: "SecurityCredential", // Password credential used in mpesa portal (Use MpesaSdk.Extensions.MpesaCredentials)
  CommandID: "CommandID", // Command set to "TransactionStatusQuery"
  TransactionID: "TransactionID", // TransactionID from the Mpesa reference that is tied to the short code that performed either a B2C, C2B or B2B
  PartyA: "PartyA", // Test for Party A 603047 OR 07XXXX123. Organization/MSISDN receiving the transaction
  IdentifierType: "IdentifierType"
  Remarks: "Remarks",
  QueueTimeOutURL: "QueueTimeOutURL", // URL to send the TransactionStatus timeout results
  ResultURL: "ResultURL", // URL to send the TransactionStatus callback results
  Occassion: "Occasion"
);

var transactionrequest = await _mpesaClient.QueryMpesaTransactionStatusAsync(TransactionStatusObject, accesstoken);
```
## Pull Transaction Register Url Request
```C#
var pullTransactionRegisterObject = new PullTransactionRegisterUrl
(
  ShortCode: "ShortCode",
  RequestType: "RequestType",
  NominatedNumber: "NominatedNumber",
  CallBackURL: "CallBackUrl"
);

var pullTransactionRegisterRequest = await _mpesaClient.RegisterPullTransactionAsync(pullTransactionRegisterObject, accesstoken);
```

## Pull Transaction Query Request
```C#
var pullTransactionQueryObject = new PullTransactionQuery
(
  ShortCode: "ShortCode",
  StartDate: "StartDate",
  EndDate: "EndDate",
  OffSetValue: "OffSetValue"
);

var pullTransactionRequest = await _mpesaClient.QueryPullTransactionAsync(pullTransactionQueryObject, accesstoken);
```

## Account Balance Query Request
```c#
var AccountBalanceObject = new AccountBalance
(	
  Initiator: "Initiator", // Test data for initiator like safaricom.x or api_xxx
  SecurityCredential: "SecurityCredential", // Password credential used in mpesa portal (Use MpesaSdk.Extensions.MpesaCredentials)
  CommandID: "CommandID", // Command set to "AccountBalance"
  PartyA: "PartyA", // Test for Party A 603047. Organization/MSISDN receiving the transaction
  IdentifierType: "IdentifierType"
  Remarks: "Remarks",
  QueueTimeOutURL: "QueueTimeOutURL", // URL to send the AccountBalance timeout results
  ResultURL: "ResultURL", // URL to send the AccountBalance callback results
);

var accountbalancerequest = await _mpesaClient.QueryAccountBalanceAsync(AccountBalanceObject, accesstoken); //async method

```

## Transaction Reversal Request. Must be done using the short code that receives a credit amount
```c#
var TransactionReversalObject = new Reversal
(
  Initiator: "Initiator", // Test data for initiator like safaricom.x or api_xxx
  SecurityCredential: "SecurityCredential", // Password credential used in mpesa portal (Use MpesaSdk.Extensions.MpesaCredentials)
  CommandID: "CommandID", // Command set to "TransactionReversal"
  TransactionID: "TransactionID", // TransactionID from the Mpesa reference that is tied to the short code that performed either a C2B or B2B.
  ReceiverParty: "ReceiverParty", // Test for Party A 603047. Organization/MSISDN receiving the transaction
  RecieverIdentifierType: "RecieverIdentifierType",
  Remarks: "Remarks",
  QueueTimeOutURL: "QueueTimeOutURL", // URL to send the TransactionReversal timeout results
  ResultURL: "ResultURL", // URL to send the TransactionReversal callback results
  Occasion: "Occasion"
);

var reversalrequest = await _mpesaClient.ReverseMpesaTransactionAsync(TransactionReversalObject, accesstoken);

```

## Getting Security Credential for B2B, B2C, Reversal, Transaction Status and Account Balance APIs
The Security Credential helper class is in MpesaSdk.Extensions namespace.

This class helps you generate the required credential to be used to authorize the above mentioned APIs.

```c#
using MpesaSdk.Extensions; // add this to your class or namespace

//get path to Mpesa public certificate. There are different certs for development and for production, ensure to use the correct one)
#if DEBUG
  string certificate = @"..\sandbox.cer";
#else
  string certificate = @"..\prod.cer";
#endif
 
 //generate security credential as follows... Initiator password from daraja

var SecutityCredential = Credentials.EncryptPassword(certificate, "Initiator Password");

```

## Dynamic Mpesa QR Request
```c#
DynamicMpesaQR dynamicMpesaQR = new DynamicMpesaQR(qrVersion: "01",
                qrFormat: 1, // 1, 2, 3 or 4
                qrType: "D", // D or S
                merchantName: dynamicQR.MerchantName,
                refNo: dynamicQR.Reference,
                amount: dynamicQR.Amount,
                trxCode: "PB", // BG, WA, PB, SM or SB
                cpi: dynamicQR.CPI);
		
DynamicMpesaQRResponse dynamicMpesaQRResponse = await _mpesaClient.GenerateDynamicMpesaQRAsync(dynamicMpesaQR, accessToken);
```

## Error handling
MpesaClient Throws ```MpesaApiException``` whenever A 200 status code is not returned. It is your role as the developer to catch
the exception and continue processing in your aplication. Snippet below shows how you can catch the MpesaApiException.

```c#
using MpesaSdk.Exceptions; // add this to you class or namespace


try
{	
	return await _mpesaClient.MakeLipaNaMpesaOnlinePaymentAsync(MpesaPayment, accesstoken);
}
catch (MpesaApiException e)
{
	_logger.LogError(ex, ex.Message);
}
			
```
