using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using SendMeetingsDemo.Data.Repositories;
using SendReceiptsDemo.Data.Repositories;
using SendReceiptsDemo.Data.Services;
using SendReceiptsDemo.Models;
using SendReceiptsDemo.Utilities;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using TrezSmsSampleCore3.Content;
using static TrezSmsSampleCore3.Content.SendSms;
using SmsSender;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace SendReceiptsDemo.Controllers
{
    public class HomeController : Controller
    {
        private ICustomerRep _customerRep;
        private IBankAccountRep _accountRep;
        private IMeetingRep _meetingRep;
        private INotificationRep _notificationRep;
        private IHttpClientFactory _clientFactory;
        private IContentRep _contentRep;
        private ITokenRep _tokenRep;
        public HomeController(ICustomerRep customerRep, IBankAccountRep accountRep,IMeetingRep meetingRep, INotificationRep notificationRep, IHttpClientFactory clientFactory, IContentRep contentRep, ITokenRep tokenRep)
        {
            _customerRep = customerRep;
            _accountRep = accountRep;
            _meetingRep = meetingRep;
            _notificationRep = notificationRep;
            _clientFactory = clientFactory;
            _contentRep = contentRep;
            _tokenRep = tokenRep;
        }


        #region Shared On Meetings And Receipts

        private void SetReadOnlyValues(bool meet = false)
        {
            if (ViewBag.FullName == null)
                ViewBag.FullName = _customerRep.GetCustomerById(int.Parse(GetCookie("SecondCustomerId"))).FullName;
            if (ViewBag.FatherName == null)
                ViewBag.FatherName = _customerRep.GetCustomerById(int.Parse(GetCookie("SecondCustomerId"))).FatherName;
            if (!meet)
            {
                if (ViewBag.NationalCode == null)
                    ViewBag.NationalCode = _customerRep.GetCustomerById(int.Parse(GetCookie("SecondCustomerId"))).NationalCode;
            }
            else
            {
                if (ViewBag.AccountNumber == null)
                    ViewBag.AccountNumber = _accountRep.GetBankAccountById(int.Parse(GetCookie("AccountId"))).AccountNumber;
            }

        }

        public IActionResult Enter()
        {
            SetCookie("FirstCustomerId", 1.ToString());

            SetCookie("Mobile", 091.ToString());

            return View();
        }

        [HttpPost] //Post Action For Send Data
        public IActionResult Enter(EnterVM enter) //Input page model (RegisterViewModel) for refresh the page to show errors
        {
            if (!ModelState.IsValid) //If Input Data in form is invalid
            {
                return View(enter); //Show the page with Input RegisterViewModel to show errors
            }

            var customer = _customerRep.GetCustomerByMobileNumber(enter.MobileNumber);

            SetCookie("FirstCustomerId", customer.CustomerId.ToString());

            SetCookie("Mobile", customer.MobileNumber);

            /*از کامنت در بیاد*/
            //ToolBox.SendCode(enter.MobileNumber);
            //return RedirectToAction("VerifyCode");
            /*بره تو کامنت*/
            return RedirectToAction("MeetingTasks");
        }

        public IActionResult VerifyCode()
        {
            return View();
        }

        [HttpPost] //Post Action For Send Data
        public IActionResult VerifyCode(VerifyCodeVM verify) //Input page model (RegisterViewModel) for refresh the page to show errors
        {
            if (!ModelState.IsValid) //If Input Data in form is invalid
            {
                return View(verify); //Show the page with Input RegisterViewModel to show errors
            }
            if (ManualVerifySendedCode(verify.VerifyCode).Result != true.ToString()) //If Input Data in form is invalid
            {
                ModelState.AddModelError("VerifyCode", ManualVerifySendedCode(verify.VerifyCode).Result);
                return View(verify); //Show the page with Input RegisterViewModel to show errors
            }

            //_firstcustomerId = _customerRep.GetCustomerByMobileNumber(_mobile).CustomerId;
            return RedirectToAction("MeetingTasks");
        }


        [HttpPost]
        public void ReSendCode()
        {
            ToolBox.SendCode(GetCookie("Mobile"));
        }

        [HttpPost]
        public bool ExistEnableNotification()
        {
           return _notificationRep.ExistEnableNotification();
        }


        /*اضافه کردن متن به اس ام اس*/
        public async Task<bool> SendMessage(string mobilenumber, string text)
        {
            List<string> str = new List<string>();
            str.Add(mobilenumber);
            bool send = false;
            try
            {
                object input = new
                {
                    PhoneNumber = "50002892164953", // شماره اختصاصی
                    Message = text, // متن پیام ارساالی
                    UserGroupID = Guid.NewGuid(), // شماره پیگیری
                    Mobiles = str, // لیست شماره موبایل ها
                    SendDateInTimeStamp = new SendSms().GetTimeStamp(DateTime.Now, DateTimeKind.Local) // تاریخ ارسال به صورت timestamp
                };

                string inputJson = (new JavaScriptSerializer()).Serialize(input);//تبدیل ورودی به json
                string res = await PostData("SendMessage", inputJson, "mohammadh1", "shayan1999smsrj");//فراخوانی تابع post data
                var output = (new JavaScriptSerializer()).Deserialize<SendMessageResult>(res);//دریافت نتیجه و تبدیل به رشته
                if (output.Message.Contains("موفقیت")) send = true;
                else send = false;
            }
            catch (Exception)
            {
                send = false;
            }
            return send;
        }

        public async Task<string> PostData(string method, string inputJson, string UserName, string Password)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "http://smspanel.trez.ir/api/smsAPI/SendMessage/");
            //request.Headers.Add("Content-type", "application/json");
            string token = new SendSms().Base64Encode(UserName + ":" + Password);
            request.Headers.Add("Authorization", string.Format("Basic {0}", token));
            request.Content = new StringContent(inputJson, Encoding.UTF8, "application/json");
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        private decimal RetreivePrice(string finalPrice)
        {
            for (int i = 0; i < finalPrice.Length; i++)
            {
                if (finalPrice[i] == ',') finalPrice.Remove(i, 1);
            }
            return decimal.Parse(finalPrice);
        }

        public IActionResult VerifyMobileNumber(string MobileNumber) // an Action that remoted for check the field value validation (no need to post page)
        {
            if (_customerRep.ExistMobileNumber(MobileNumber.ToLower()))
            {
                return Json(true); // send true value
            }
            else return Json($"شماره موبایل {MobileNumber} در سیستم وجود ندارد"); //send error text
        }

        public async Task<IActionResult> VerifySendedCode(string VerifyCode) // an Action that remoted for check the field value validation (no need to post page)
        {
            if (VerifyCode.Length == 6 && (await ToolBox.CheckCode(GetCookie("Mobile"), VerifyCode)))
            {
                return Json(true); // send true value
            }
            else return Json("کد وارد شده صحیح نیست"); //send error text
        }

        public async Task<string> ManualVerifySendedCode(string VerifyCode) // an Action that remoted for check the field value validation (no need to post page)
        {
            if (VerifyCode.Length == 6 && (await ToolBox.CheckCode(GetCookie("Mobile"), VerifyCode)))
            {
                return true.ToString(); // send true value
            }
            else return "کد وارد شده صحیح نیست"; //send error text
        }



        [HttpPost]
        public async Task<string> GetCustomerNameByAccountNumber(string accountNumber)
        {
            try
            {
                if (accountNumber == null) return null;
                var acc = _accountRep.GetAccountByAccountNumber(accountNumber);
                if (acc == null) return null;
                if (_customerRep.GetCustomerByAccountId(acc.AcountId) == null) return null;
                return $"متعلق به {_customerRep.GetCustomerByAccountId(_accountRep.GetAccountByAccountNumber(accountNumber).AcountId).FullName}";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";

        }


        public IActionResult Error()
        {
            return View();
        }

        #endregion



        #region Meetings

        public IActionResult MeetingTasks()
        {
            return View();
        }

        public IActionResult GetMyAccounts() // an Action that remoted for check the field value validation (no need to post page)
        {
            if (int.Parse(GetCookie("FirstCustomerId")) != 0)
            {
                //ViewBag.CustomerName = _customerRep.GetCustomerById(_firstcustomerId).FullName;
                ViewBag.Accounts = _accountRep.GetBankAccountsOfCustomer(int.Parse(GetCookie("FirstCustomerId")), 2);
            }
            return View();
        }
        [HttpPost]

        public IActionResult GetMyAccounts(SelectAccountVM selectAccountVM) // an Action that remoted for check the field value validation (no need to post page)
        {
            //ViewBag.CustomerName = _customerRep.GetCustomerById(_firstcustomerId).FullName;
            ViewBag.Accounts = _accountRep.GetBankAccountsOfCustomer(int.Parse(GetCookie("FirstCustomerId")), 2);
            if (!ModelState.IsValid) //If Input Data in form is invalid
            {
                return View(selectAccountVM); //Show the page with Input RegisterViewModel to show errors
            }
            SetCookie("AccountId", ((int)selectAccountVM.BankAcountId).ToString());
            SetCookie("SecondCustomerId", _accountRep.GetCustomerIdByAccountId((int)selectAccountVM.BankAcountId).ToString());
            return RedirectToAction("GetMeeting");
        }

        public IActionResult GetMeeting()
        {
            SetReadOnlyValues(true);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetMeeting(GetMeetingVM meeting)
        {
            SetReadOnlyValues(true);
            if (string.IsNullOrWhiteSpace(meeting.ScorerAccountNumber)) meeting.Loan = false;
            if (ModelState.IsValid)
            {
                Meeting theMeeting = new Meeting()
                {
                    Amount = meeting.Amount,
                    BankAcountId = int.Parse(GetCookie("AccountId")),
                    CustomerId = int.Parse(GetCookie("SecondCustomerId")),
                    AccountNumber = ViewBag.AccountNumber,
                    Count = meeting.Count,
                    Address = meeting.Address,
                    Job = meeting.Job,
                    Reagent = meeting.Reagent ?? "",
                    Description = meeting.Description ?? "",
                    Response = "",
                    PassedStatus = "0",
                    Status = "در حال بررسی",
                    BlockerConditions = "",
                    BlockPassedStatus = "0",
                    GuarantorsConditions = "",
                    RepaymentPeriod = 0,
                    ScorerAccountNumber = "",
                    ReqScore = "0",
                    ScorerName = "",
                    ScorerMobileNumber = "",
                    BlockScorerAccountNumber = "",
                    BlockReqScore = "0",
                    BlockScorerName = "",
                    BlockScorerMobileNumber = "",
                    MeetingDate = DateTime.Now.ToString("yyyy/MM/dd - HH:mm").ToShamsi()
                };
                if (meeting.Loan)
                {
                    theMeeting.ScorerAccountNumber = meeting.ScorerAccountNumber;
                    theMeeting.ReqScore = meeting.ReqScore;
                    theMeeting.ScorerName = _customerRep.GetCustomerByAccountId(_accountRep.GetAccountByAccountNumber(meeting.ScorerAccountNumber).AcountId).FullName;
                    theMeeting.ScorerMobileNumber = _customerRep.GetCustomerByAccountId(_accountRep.GetAccountByAccountNumber(meeting.ScorerAccountNumber).AcountId).MobileNumber;
                }
                if (meeting.BlockRequest)
                {
                    theMeeting.BlockScorerAccountNumber = meeting.BlockScorerAccountNumber;
                    theMeeting.BlockReqScore = meeting.BlockReqScore;
                    theMeeting.BlockScorerName = _customerRep.GetCustomerByAccountId(_accountRep.GetAccountByAccountNumber(meeting.BlockScorerAccountNumber).AcountId).FullName;
                    theMeeting.BlockScorerMobileNumber = _customerRep.GetCustomerByAccountId(_accountRep.GetAccountByAccountNumber(meeting.BlockScorerAccountNumber).AcountId).MobileNumber;
                }
                _meetingRep.AddMeeting(theMeeting);
                string receiptMessage = MakeMeetingMessage(theMeeting);
                /*از کامنت دربیاد*/
                //bool sentState = await SendMessage(_customerRep.GetCustomerById((int)theMeeting.CustomerId).MobileNumber, receiptMessage);
                ViewBag.CustomerName = _customerRep.GetCustomerById((int)theMeeting.CustomerId).FullName;
                return View("MeetTrackingCode", theMeeting);
            }
            return View(meeting);
        }

        public IActionResult TrackReqs()
        {
            return View();
        }

        public IActionResult EditScore(int id)
        {
            var req = _meetingRep.GetAllMeetings().Where(m => m.MeetingId == id).Select(sc => new ScoreReqVM()
            {
                MeetId = sc.MeetingId,
                ReqAccountNumber = sc.AccountNumber,
                ScorerAccountNumber = sc.ScorerAccountNumber,
                ReqName = sc.Customer.FullName,
                ReqScore = sc.ReqScore
            }).FirstOrDefault();
            return View(req);
        }

        public IActionResult EditBlockReq(int id)
        {
            var req = _meetingRep.GetAllMeetings().Where(m => m.MeetingId == id).Select(sc => new BlockReqVM()
            {
                MeetId = sc.MeetingId,
                ReqAccountNumber = sc.AccountNumber,
                ScorerAccountNumber = sc.BlockScorerAccountNumber,
                ReqName = sc.Customer.FullName,
                BlockReqScore = sc.BlockReqScore,
                BlockerConditions = sc.BlockerConditions,
                GuarantorsConditions = sc.GuarantorsConditions
            }).FirstOrDefault();
            if (_contentRep.GetContentById(1)?.ContentText?.IndexOf('؛') >=0)
                ViewBag.DefaultGuarantorsConditions = _contentRep.GetContentById(1)?.ContentText?.Split("؛\r\n");
            if (_contentRep.GetContentById(2)?.ContentText?.IndexOf('؛') >= 0)
                ViewBag.DefaultBlockerConditions = _contentRep.GetContentById(2)?.ContentText?.Split("؛\r\n");
            return View(req);
        }

        public IActionResult ScoreReqs()
        {
            var reqs = _meetingRep.GetScoreReqsForCustomer(GetCookie("Mobile"));
            return View(reqs);
        }

        public IActionResult BlockReqs()
        {
            var reqs = _meetingRep.GetBlockReqsForCustomer(GetCookie("Mobile"));
            return View(reqs);
        }

        private string MakeMeetingMessage(Meeting theMeeting)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{_customerRep.GetCustomerById((int)theMeeting.CustomerId).FullName} عزیز!");
            sb.AppendLine($"درخواست  وام شما");
            sb.AppendLine($"به مبلغ {theMeeting.Amount} ریال");
            sb.AppendLine($"با تعداد اقساط  {theMeeting.Count}");
            //sb.AppendLine($"با شماره حساب {theMeeting.AccountNumber }");
            if (!string.IsNullOrEmpty(theMeeting.ScorerAccountNumber) && !string.IsNullOrEmpty(theMeeting.ScorerName))
            {
                sb.AppendLine($"با درخواست امتیاز از  {theMeeting.ScorerName}");
                //sb.AppendLine($"به نام {theMeeting.ScorerName}");
            }
            sb.AppendLine($"در تاریخ  {theMeeting.MeetingDate}");
            sb.AppendLine($"با کد پیگیری {theMeeting.MeetingId} ثبت شده و پس از بررسی نتیجه آن به اطلاع شما خواهد رسید.");
            sb.AppendLine("از حسن توجه شما متشکریم.");
            return sb.ToString();
        }

        public IActionResult CheckAmount(string Amount) // an Action that remoted for check the field value validation (no need to post page)
        {
            if (!string.IsNullOrWhiteSpace(Amount))
            {
                if (RetreivePrice(Amount) > 0)
                {
                    return Json(true); // send true value
                }
            }
            return Json($"مبلغ وارد شده نامعتبر است"); //send error text
        }

        public IActionResult CheckReqAccount(string ScorerAccountNumber, bool Loan) // an Action that remoted for check the field value validation (no need to post page)
        {
            if (!Loan)
            {
                return Json(true); // send true value
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(ScorerAccountNumber))
                {
                    if (_accountRep.ExistBankAccountNumber(ScorerAccountNumber))
                    {
                        if (ScorerAccountNumber != ViewBag.AccountNumber)
                        {
                            return Json(true); // send true value
                        }
                        else return Json($"مقدار شماره حساب مبدا و مقصد یکسان است"); //send error text
                    }
                    else return Json($"شماره حساب وارد شده نامعتبر است"); //send error text
                }
                else return Json($"شماره حساب وارد شده نامعتبر است"); //send error text
            }
        }

        public IActionResult CheckBlockAccount(string BlockScorerAccountNumber, bool BlockRequest) // an Action that remoted for check the field value validation (no need to post page)
        {
            if (!BlockRequest)
            {
                return Json(true); // send true value
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(BlockScorerAccountNumber))
                {
                    if (_accountRep.ExistBankAccountNumber(BlockScorerAccountNumber))
                    {
                        if (BlockScorerAccountNumber != ViewBag.AccountNumber)
                        {
                            return Json(true); // send true value
                        }
                        else return Json($"مقدار شماره حساب مبدا و مقصد یکسان است"); //send error text
                    }
                    else return Json($"شماره حساب وارد شده نامعتبر است"); //send error text
                }
                else return Json($"شماره حساب وارد شده نامعتبر است"); //send error text
            }
        }

        public IActionResult CheckReqScore(string ReqScore, bool Loan, string Amount) // an Action that remoted for check the field value validation (no need to post page)
        {
            if (!Loan)
            {
                return Json(true); // send true value
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(ReqScore))
                {
                    if (RetreivePrice(ReqScore) > 0)
                    {
                        if (RetreivePrice(ReqScore) <= RetreivePrice(Amount))
                        {
                            return Json(true); // send true value
                        }
                        else return Json($"امتیاز وارد شده بیش از حد مورد نیاز است"); //send error text
                    }
                    else return Json($"امتیاز وارد شده نامعتبر است"); //send error text
                }
                else return Json($"امتیاز وارد شده نامعتبر است"); //send error text
            }
        }

        public IActionResult CheckBlockScore(string BlockReqScore, bool BlockRequest, string Amount) // an Action that remoted for check the field value validation (no need to post page)
        {
            if (!BlockRequest)
            {
                return Json(true); // send true value
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(BlockReqScore))
                {
                    if (RetreivePrice(BlockReqScore) > 0)
                    {
                        if (RetreivePrice(BlockReqScore) <= RetreivePrice(Amount))
                        {
                            return Json(true); // send true value
                        }
                        else return Json($"امتیاز وارد شده بیش از حد مورد نیاز است"); //send error text
                    }
                    else return Json($"امتیاز وارد شده نامعتبر است"); //send error text
                }
                else return Json($"امتیاز وارد شده نامعتبر است"); //send error text
            }
        }

        public IActionResult ChangeReqScore(string ReqScore, int MeetId) // an Action that remoted for check the field value validation (no need to post page)
        {
            if (!string.IsNullOrWhiteSpace(ReqScore))
            {
                if (RetreivePrice(ReqScore) > 0)
                {
                    if (RetreivePrice(ReqScore) <= RetreivePrice(_meetingRep.GetMeetingById(MeetId).ReqScore))
                    {
                        return Json(true); // send true value
                    }
                    else return Json($"امتیاز وارد شده بیش از حد مورد نیاز است"); //send error text
                }
                else return Json($"امتیاز وارد شده نامعتبر است"); //send error text
            }
            else return Json($"امتیاز وارد شده نامعتبر است"); //send error text
        }

        public IActionResult ChangeBlockReqScore(string BlockReqScore, int MeetId) // an Action that remoted for check the field value validation (no need to post page)
        {
            if (!string.IsNullOrWhiteSpace(BlockReqScore))
            {
                if (RetreivePrice(BlockReqScore) > 0)
                {
                    if (RetreivePrice(BlockReqScore) <= RetreivePrice(_meetingRep.GetMeetingById(MeetId).BlockReqScore))
                    {
                        return Json(true); // send true value
                    }
                    else return Json($"امتیاز وارد شده بیش از حد مورد نیاز است"); //send error text
                }
                else return Json($"امتیاز وارد شده نامعتبر است"); //send error text
            }
            else return Json($"امتیاز وارد شده نامعتبر است"); //send error text
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptScore(ScoreReqVM scoreReq)
        {
            if (ModelState.IsValid)
            {
                var req = _meetingRep.GetMeetingById(scoreReq.MeetId);
                req.PassedStatus = scoreReq.ReqScore + " امتیاز تایید شده";
                _meetingRep.EditMeeting(req);
                return Redirect("/Home/ScoreReqs");
            }
            return View(scoreReq);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptBlockReq(BlockReqVM blockReq, List<string> selectedGuarantor, List<string> selectedBlocker)
        {
            if (ModelState.IsValid)
            {
                var req = _meetingRep.GetMeetingById(blockReq.MeetId);
                req.BlockPassedStatus = blockReq.BlockReqScore + " امتیاز تایید شده";
                req.RepaymentPeriod = blockReq.RepaymentPeriod;
                req.BlockerConditions =string.Join('\n',selectedBlocker) + '\n' + blockReq.BlockerConditions;
                req.GuarantorsConditions = string.Join('\n', selectedGuarantor) + '\n' + blockReq.GuarantorsConditions;
                _meetingRep.EditMeeting(req);
                return Redirect("/Home/BlockReqs");
            }
            return View(blockReq);
        }

        public async Task<IActionResult> RejectScore(int id)
        {
            var req = _meetingRep.GetMeetingById(id);
            req.PassedStatus = "رد شده";
            _meetingRep.EditMeeting(req);
            return Redirect("/Home/ScoreReqs");
        }

        public async Task<IActionResult> RejectBlockScore(int id)
        {
            var req = _meetingRep.GetMeetingById(id);
            req.BlockPassedStatus = "رد شده";
            _meetingRep.EditMeeting(req);
            return Redirect("/Home/BlockReqs");
        }

        [HttpPost]
        public async Task<TrackReqVM> TrackCode(int code)
        {
            TrackReqVM track = new TrackReqVM();
            var req = _meetingRep.GetMeetingById(code);
            if (req != null && req.Customer.MobileNumber == GetCookie("Mobile"))
            {
                SetCookie("MeetId", code.ToString());
                track.Status = req.Status;
                track.Response = req.Response;
            }
            else
            {
                SetCookie("MeetId", "0");
                track.Status = "نامعتبر";
                track.Response = "";
            }
            return track;
        }

        [HttpPost]
        public async Task<IActionResult> SaveBills(SaveBillsVM bill)
        {
            int i = 1;
            foreach (IFormFile pic in bill.BillPics)
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot",
                "pics",
                "meetings",
                "meet" + GetCookie("MeetId") + "-" + i.ToString() + "-" + DateTime.Now.ToString("yyyy/MM/dd - HH:mm").ToShamsi().Replace('/', '-').Replace(':', '_') + Path.GetExtension(pic.FileName));
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await pic.CopyToAsync(stream);
                }
                i++;
            }
            ViewBag.Message = "Y";
            return View("TrackReqs");
        }

        [HttpPost]
        public IActionResult SendRivision(SendRivisionVM rivision)
        {
            var meet = _meetingRep.GetMeetingById(int.Parse(GetCookie("MeetId")));
            meet.Response = "";
            meet.Status = "درخواست تجدید نظر - در حال بررسی";
            meet.Count = rivision.Count;
            meet.Amount = rivision.Amount;
            _meetingRep.EditMeeting(meet);
            ViewBag.Message = "Y2";
            return View("TrackReqs");
        }


        #endregion


        #region ManageCookies

        //public void SetCookie(string key, string value, bool isPersistant = false)
        //{
        //    CookieOptions options = new CookieOptions() { IsEssential = true, Path = "/", HttpOnly = true };
        //    if (isPersistant) options.Expires = DateTime.Now.AddMinutes(20);
        //    Response.Cookies.Delete(key);
        //    Response.Cookies.Append(key, value, options);
        //}

        //public string GetCookie(string key)
        //{
        //    return Request.Cookies[key].ToString();
        //}

        public void SetCookie(string key, string value, bool isPersistant = false)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var keyBytes = Encoding.UTF8.GetBytes(_tokenRep.GetCookieToken());
            var signingKey = new SymmetricSecurityKey(keyBytes);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(key, value)
                }),
                Expires = isPersistant ? DateTime.UtcNow.AddMinutes(20) : DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var cookieOptions = new CookieOptions
            {
                IsEssential = true,
                HttpOnly = true,
                Secure = true, // If your site uses HTTPS
                SameSite = SameSiteMode.None // May need to adjust this based on your site's requirements
            };

            Response.Cookies.Delete(key);
            Response.Cookies.Append(key, tokenString, cookieOptions);
        }

        public string GetCookie(string key)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var keyBytes = Encoding.UTF8.GetBytes(_tokenRep.GetCookieToken());
            var signingKey = new SymmetricSecurityKey(keyBytes);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = false, // Set these values based on your JWT configuration
                ValidateAudience = false // Set these values based on your JWT configuration
            };

            try
            {
                var token = Request.Cookies[key];
                var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
                var claim = claimsPrincipal.FindFirst(key);

                if (claim != null)
                {
                    return claim.Value;
                }
            }
            catch (SecurityTokenException)
            {
                // Handle token validation error (e.g., expired or invalid token)
            }

            return null; // If the cookie is not found or validation fails
        }

        #endregion
    }
}