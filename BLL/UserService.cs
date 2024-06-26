﻿//using DAL;
//using System.Net;
//using System.Net.Mail;

//namespace BLL
//{
//    public class UserService
//    {
//        private readonly QuizDBContext _dbContext;
//        private readonly Dictionary<string, (string otp, DateTime expiration)> otpStorage = new Dictionary<string, (string otp, DateTime expiration)>();
//        private readonly Random random = new Random();

//        public UserService(QuizDBContext dbContext)
//        {
//            _dbContext = dbContext;
//        }

//        public void SendOTP(string email)
//        {
//            int otp = random.Next(100000, 999999);

//            // Save the OTP and its expiration time in the otpStorage dictionary
//            otpStorage[email] = (otp.ToString(), DateTime.Now.AddMinutes(10));

//            SendEmail(email, $"Your OTP for password reset: {otp}");
//        }

//        public bool VerifyOTP(string email, string otp)
//        {
//            // Retrieve OTP and its expiration time from the otpStorage dictionary
//            if (otpStorage.TryGetValue(email, out var storedOTP))
//            {
//                if (storedOTP.otp == otp && storedOTP.expiration > DateTime.Now)
//                {
//                    // OTP verified successfully, remove it from otpStorage dictionary
//                    otpStorage.Remove(email);
//                    return true;
//                }
//            }
//            return false;
//        }

//        private void SendEmail(string to, string body)
//        {
//            try
//            {
//                // Configure SMTP client
//                SmtpClient client = new SmtpClient("smtp-mail.outlook.com");
//                client.Port = 587;
//                client.EnableSsl = true;
//                client.UseDefaultCredentials = false;
//                client.Credentials = new NetworkCredential("Gaurav.Tripathi@triconinfotech.com", "Secure@15$%");

//                // Create and send email
//                MailMessage mailMessage = new MailMessage("Gaurav.Tripathi@triconinfotech.com", to, "Password Reset OTP", body);
//                mailMessage.IsBodyHtml = true;
//                client.Send(mailMessage);
//            }
//            catch (Exception ex)
//            {
//                // Handle email sending failure
//                Console.WriteLine($"Failed to send email: {ex.Message}");
//            }
//        }

//        public void UpdatePassword(string email, string newPassword)
//        {
//            var user = _dbContext.Users.FirstOrDefault(u => u.EmailId == email);

//            if (user != null)
//            {
//                user.Password = newPassword; // Update the password
//                _dbContext.SaveChanges();
//            }
//            else
//            {
//                throw new Exception("Email not found");
//            }
//        }

//    }
//}


using DAL;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Net.Mail;

namespace BLL
{
    public class UserService
    {
        private readonly QuizDBContext _dbContext;
        private readonly IMemoryCache _cache;
        private readonly Random random = new Random();

        public UserService(QuizDBContext dbContext, IMemoryCache cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }

        public void SendOTP(string email)
        {
            int otp = random.Next(100000, 999999);

            // Save the OTP and its expiration time in the cache
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10));
            _cache.Set(email, otp.ToString(), cacheEntryOptions);

            SendEmail(email, $"Your OTP for password reset: {otp}");
        }

        public bool VerifyOTP(string email, string otp)
        {
            // Retrieve OTP from the cache
            if (_cache.TryGetValue(email, out string storedOTP))
            {
                if (storedOTP == otp)
                {
                    // OTP verified successfully, remove it from the cache
                    _cache.Remove(email);
                    return true;
                }
            }
            return false;
        }

        private void SendEmail(string to, string body)
        {
            try
            {
                // Configure SMTP client
                SmtpClient client = new SmtpClient("smtp-mail.outlook.com");
                client.Port = 587;
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("Gaurav.Tripathi@triconinfotech.com", "Secure@15$%");

                // Create and send email
                MailMessage mailMessage = new MailMessage("Gaurav.Tripathi@triconinfotech.com", to, "Password Reset OTP", body);
                mailMessage.IsBodyHtml = true;
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                // Handle email sending failure
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }

        public void UpdatePassword(string email, string newPassword)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.EmailId == email);

            if (user != null)
            {
                user.Password = newPassword; // Update the password
                _dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("Email not found");
            }
        }
    }
}
