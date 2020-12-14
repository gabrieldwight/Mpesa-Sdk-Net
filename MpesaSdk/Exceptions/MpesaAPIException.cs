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

        public MpesaAPIException(Exception ex, HttpStatusCode statusCode) : base (ex.Message)
        {
            StatusCode = statusCode;
        }
    }
}
