using MpesaSdk.Response;
using System;
using System.Net;

namespace MpesaSdk.Exceptions
{
    /// <summary>
    /// Mpesa Api Exception Custom class
    /// </summary>
    public class MpesaAPIException : Exception
    {
        /// <summary>
        /// Http Status code retrieved from the Mpesa API call
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        public MpesaErrorResponse _mpesaErrorResponse { get; set; }

        public MpesaAPIException()
        {

        }

        public MpesaAPIException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public MpesaAPIException(Exception ex, HttpStatusCode statusCode, MpesaErrorResponse mpesaErrorResponse) : base (ex.Message)
        {
            StatusCode = statusCode;
            _mpesaErrorResponse = mpesaErrorResponse;
        }
    }
}
