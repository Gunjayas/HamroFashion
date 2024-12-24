using HamroFashion.Api.V1.Commands;
using HamroFashion.Api.V1.Data;
using System.Security.Cryptography;
using System.Text;

namespace HamroFashion.Api.V1.Services
{
    public class PaymentService
    {
        /// <summary>
        /// The HamroFashionContext we persist to
        /// </summary>
        public readonly HamroFashionContext db;

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="db">The HamroFashionContext to use</param>
        public PaymentService(HamroFashionContext db)
        {
            this.db = db;
        }

        /*public async Task InitiatePayment(PaymentRequest request)
        {

            string hashString = $"{merchantId}|{transactionId}|{request.Amount}|{callbackUrl}";
            string hash = GenerateHmacSha256Hash(hashString, secretKey);

            return Ok(new { paymentUrl = $"{paymentUrl}?amt={request.Amount}&pid={transactionId}&scd={merchantId}&su={callbackUrl}&fu={callbackUrl}" });
        }

        private string GenerateHmacSha256Hash(string data, string key)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }*/
    }
}
