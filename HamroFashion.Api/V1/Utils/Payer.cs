using Azure.Core;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace HamroFashion.Api.V1.Utils
{
    public class Payer
    {

        /// <summary>
        /// We load these from configuration on app start
        /// </summary>
        private static string? merchantId = null;

        /// <summary>
        /// We load this from configuration on app start
        /// </summary>
        private static string? secretKey = null;

        /// <summary>
        /// Reusable client, contains all the information needed by smtpserver(ms365) to send mail to client (user)
        /// </summary>
        public static string? callbackUrl = null;

        private static string? transactionId = null;

        public static void Configure(IConfiguration config)
        {
            var merchantId = config["eSewa:MerchantId"];
            var secretKey = config["eSewa:SecretKey"];
            var callbackUrl = config["eSewa:CallbackUrl"];

            var transactionId = Guid.NewGuid().ToString();
            var paymentUrl = "https://esewa.com.np/payment/main/";
        }

        /*private string GenerateHmacSha256Hash(string data, string key)
        {
            string hashString = $"{merchantId}|{transactionId}|{request.Amount}|{callbackUrl}";
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }*/
    }
}
