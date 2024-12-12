using System.Security.Cryptography;
using System.Text;

namespace HamroFashion.Api.V1.Extensions
{
    /// <summary>
    /// Helper extensions to work with passwords
    /// </summary>
    public static class PasswordExtensions
    {
        /// <summary>
        /// Static setting for the number of iterations to hash with. Increasing this may improve secrity
        /// </summary>
        public static int Iterations { get; set; } = 250000;

        /// <summary>
        /// Static setting for size of the salt key to hash with.  Increasing this may improve security
        /// </summary>
        public static int KeySize { get; set; } = 64;

        /// <summary>
        /// Statically set the PasswordComplexityOptions object you want to use with these extensions
        /// </summary>
        public static PasswordComplexityOptions ComplexityOptions { get; set; } = new PasswordComplexityOptions();

        /// <summary>
        /// Check if the password is correct by hashing it and comparing it to the provided
        /// passwordHash
        /// </summary>
        /// <param name="password">The string we are calling this method on</param>
        /// <param name="passwordHash">The result of a previous call to ToPasswordHash</param>
        /// <returns>True if this password hashes to passwordHash</returns>
        public static bool EqualsPasswordHash(this string password, byte[] passwordHash)
        {
            var saltBytes = passwordHash.Skip(KeySize).Take(KeySize).ToArray();
            var passwordBytes = password.ToPasswordHash(saltBytes);

            return passwordBytes.SequenceEqual(passwordHash);
        }

        /// <summary>
        /// A helper that hashes a string password. Uses Pbkdf2 with KeySize salt length
        /// and Iterations number of hashes.
        /// </summary>
        /// <param name="password">The string we are calling this method on</param>
        /// <param name="salt">The hex string of the salt to hash this password with. Leave null to randomly generate a new salt</param>
        /// <returns>{salt as hex string}.{hashed password as hex string}</returns>
        public static byte[] ToPasswordHash(this string password, byte[]? saltBytes = null)
        {
            if (saltBytes == null)
                saltBytes = RandomNumberGenerator.GetBytes(KeySize);

            var hashBytes = Rfc2898DeriveBytes.Pbkdf2
            (
                Encoding.UTF8.GetBytes(password),
                saltBytes,
                Iterations,
                HashAlgorithmName.SHA512,
                KeySize
            );

            return hashBytes
                .Concat(saltBytes)
                .ToArray();
        }
    }

    /// <summary>
    /// Defines options for how string.IsStrongPassword evaluates the strength of the
    /// string as a password.
    /// </summary>
    public class PasswordComplexityOptions
    {
        /// <summary>
        /// Require minimum password length
        /// </summary>
        public int RequireLength { get; set; } = 8;

        /// <summary>
        /// Require at least one letter character
        /// </summary>
        public bool RequireLetter { get; set; } = true;

        /// <summary>
        /// Require at least one lower case character
        /// </summary>
        public bool RequireLower { get; set; } = false;

        /// <summary>
        /// Require at least one numeric character
        /// </summary>
        public bool RequireNumber { get; set; } = true;

        /// <summary>
        /// Require at least one puctuation / symbol
        /// </summary>
        public bool RequireSymbol { get; set; } = false;

        /// <summary>
        /// Require at least one uppercase character
        /// </summary>
        public bool RequireUpper { get; set; } = false;
    }
}
