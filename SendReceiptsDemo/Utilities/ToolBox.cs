using System.Globalization;
using System.Text;
using Nancy.Json;
using SmsSender;
using TrezSmsSampleCore3.Content;
using SendReceiptsDemo.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Cryptography;

namespace SendReceiptsDemo.Utilities
{
    public static class ToolBox
    {
        public static string ToShamsi(this string value) // use this word for use the method for all DateTime variables in project
        {
            var date = value.Split(' ')[0].Split('/');
            DateTime dateTime = new DateTime(Convert.ToInt32(date[0]), Convert.ToInt32(date[1]), Convert.ToInt32(date[2]));
            PersianCalendar persianCalendar = new PersianCalendar();
            return persianCalendar.GetYear(dateTime) + "/" + persianCalendar.GetMonth(dateTime).ToString("00") + "/" + persianCalendar.GetDayOfMonth(dateTime).ToString("00") + " - " + DateTime.Now.ToString("HH:mm");
        }

        public static bool SendCode(string mobileNumber)
        {
            bool send = false;
            try
            {
                FastSendSoapClient client = new FastSendSoapClient(FastSendSoapClient.EndpointConfiguration.FastSendSoap);
                client.AutoSendCodeAsync("mohammadh1", "shayan1999smsrj", mobileNumber, "");
                send = true;
            }
            catch (Exception)
            {
                send = false;
            }
            return send;
        }

        public async static Task<bool> CheckCode(string mobileNumber, string code)
        {
            bool currect = false;
            try
            {
                FastSendSoapClient client = new FastSendSoapClient(FastSendSoapClient.EndpointConfiguration.FastSendSoap);
                CheckSendCodeResponse response = await client.CheckSendCodeAsync("mohammadh1", "mhsms1234", mobileNumber, code);
                currect = response.Body.CheckSendCodeResult;
            }
            catch (Exception)
            {
                currect = false;
            }
            return currect;
        }

        public static string GenerateToken()
        {
            byte[] randomBytes = new byte[10]; // اندازه توکن را می‌توانید تغییر دهید
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                rngCryptoServiceProvider.GetBytes(randomBytes);
            }
            return $"{Convert.ToBase64String(randomBytes)}{Convert.ToBase64String(Encoding.UTF8.GetBytes(DateTime.Now.ToString("yyyy/MM/dd - HH:mm").ToShamsi()))}";
        }

    }
}
