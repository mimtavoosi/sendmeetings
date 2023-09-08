using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Nancy.Json;
using Nancy.TinyIoc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SendMeetingsDemo.Data.Repositories;
using SendReceiptsDemo.Data;
using SendReceiptsDemo.Data.Repositories;
using SendReceiptsDemo.Data.Services;
using SendReceiptsDemo.Models;
using SendReceiptsDemo.Utilities;
using SmsSender;
using TrezSmsSampleCore3.Content;
using static TrezSmsSampleCore3.Content.SendSms;

namespace SendReceiptsDemo.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private ICustomerRep _customerRep;
        private IBankAccountRep _accountRep;
        private IMeetingRep _meetingRep;
        private IRightRep _rightRep;
        private IAdminRep _adminRep;
        private IMessageRep _messageRep;
        private INotificationRep _notificationRep;
        private IHttpClientFactory _clientFactory;
        private IContentRep _contentRep;
        private ITokenRep _tokenRep;

        public AdminController(ICustomerRep customerRep, IBankAccountRep accountRep, IRightRep rightRep, IAdminRep adminRep, IMessageRep messageRep,IMeetingRep meetingRep, INotificationRep notificationRep, IHttpClientFactory clientFactory, IContentRep contentRep, ITokenRep tokenRep)
        {
            _customerRep = customerRep;
            _accountRep = accountRep;
            _rightRep = rightRep;
            _adminRep = adminRep;
            _messageRep = messageRep;
            _meetingRep = meetingRep;
            _notificationRep = notificationRep;
            _clientFactory = clientFactory;
            _contentRep = contentRep;
            _tokenRep = tokenRep;
        }


        #region Search

        public JsonResult SearchRecords(string searchtext, int searchtype)
        {
            JsonResult jsonResult;
            switch (searchtype)
            {
                case 1:
                default:
                    {
                        if (!string.IsNullOrEmpty(searchtext))
                        {
                            var Searchlist = _customerRep.GetAllCustomers().Where(c => (!string.IsNullOrEmpty(c.FullName.ToString()) && c.FullName.ToString().Contains(searchtext))
                            || (!string.IsNullOrEmpty(c.NationalCode.ToString()) && c.NationalCode.ToString().Contains(searchtext))
                            || (!string.IsNullOrEmpty(c.MobileNumber.ToString()) && c.MobileNumber.ToString().Contains(searchtext))
                            || (!string.IsNullOrEmpty(c.CustomerId.ToString()) && c.CustomerId.ToString().Contains(searchtext))
                            || (!string.IsNullOrEmpty(c.FatherName.ToString()) && c.FatherName.ToString().Contains(searchtext))
                            || (!string.IsNullOrEmpty(c.CustomerDescription.ToString()) && c.CustomerDescription.ToString().Contains(searchtext))
                            ).ToList();
                            jsonResult = Json(Searchlist);
                        }
                        else
                        {
                            int skip = (int.Parse(GetCookie("CustomersPageNumber")) - 1) * 20;
                            int Count = _customerRep.GetAllCustomers().Count();
                            ViewBag.CustomersPageID = GetCookie("CustomersPageNumber");
                            ViewBag.CustomersPageCount = Count / 20;
                            jsonResult = Json(_customerRep.GetCustomersForPages(skip));
                        } 
                    }
                    break;
                    case 2:
                    {
                        if (!string.IsNullOrEmpty(searchtext))
                        {
                            var Searchlist = _accountRep.GetAllBankAccounts().Where(b => (!string.IsNullOrEmpty(b.Customer.FullName.ToString()) && b.Customer.FullName.ToString().Contains(searchtext))
                            || (!string.IsNullOrEmpty(b.AccountNumber.ToString()) && b.AccountNumber.ToString().Contains(searchtext))
                            ).ToList();
                            jsonResult = Json(Searchlist);
                        }
                        else
                        {
                            int skip = (int.Parse(GetCookie("AccountsPageNumber")) - 1) * 20;
                            int Count = _accountRep.GetAllBankAccounts().Count();
                            ViewBag.AccountsPageID = GetCookie("AccountsPageNumber");
                            ViewBag.AccountsPageCount = Count / 20;
                            jsonResult = Json(_accountRep.GetAccountsForPages(skip));
                        }
                    }
                    break;
                case 4:
                    {
                        List<RightVM> rightsVM = _rightRep.GetAlRights().Select(vm => new RightVM()
                        {
                            RightId = vm.RightId,
                            RighterName = _customerRep.GetCustomerById(vm.RighterId).FullName,
                            RightOwnerName = _customerRep.GetCustomerByAccountId(vm.AcountId).FullName,
                            AccountNumber = _accountRep.GetBankAccountById(vm.AcountId).AccountNumber
                        }).ToList();
                        if (!string.IsNullOrEmpty(searchtext))
                        {
                            var Searchlist = rightsVM.Where(r => (!string.IsNullOrEmpty(r.RighterName.ToString()) && r.RighterName.ToString().Contains(searchtext))
                            || (!string.IsNullOrEmpty(r.RightOwnerName.ToString()) && r.RightOwnerName.ToString().Contains(searchtext))
                            || (!string.IsNullOrEmpty(r.AccountNumber.ToString()) && r.AccountNumber.ToString().Contains(searchtext))
                            ).ToList();
                            jsonResult = Json(Searchlist);
                        }
                        else
                        {
                            int skip = (int.Parse(GetCookie("RightsPageNumber")) - 1) * 20;
                            int Count = _rightRep.GetAlRights().Count();
                            ViewBag.RightsPageID = GetCookie("RightsPageNumber");
                            ViewBag.RightsPageCount = Count / 20;
                            List<RightVM> _rightsVM = _rightRep.GetRightsForPages(skip).Select(vm => new RightVM()
                            {
                                RightId = vm.RightId,
                                RighterName = _customerRep.GetCustomerById(vm.RighterId).FullName,
                                RightOwnerName = _customerRep.GetCustomerByAccountId(vm.AcountId).FullName,
                                AccountNumber = _accountRep.GetBankAccountById(vm.AcountId).AccountNumber
                            }).ToList();
                            jsonResult = Json(_rightsVM);
                        }
                    }
                    break;
                case 5:
                    {
                        if (!string.IsNullOrEmpty(searchtext))
                        {
                            var Searchlist = _adminRep.GetAllAdmins().Where(a => (!string.IsNullOrEmpty(a.UserName.ToString()) && a.UserName.ToString().Contains(searchtext))
                            || (!string.IsNullOrEmpty(a.AdminType.ToString()) && a.AdminType.ToString().Contains(searchtext))
                            ).ToList();
                            jsonResult = Json(Searchlist);
                        }
                        else
                        {
                            int skip = (int.Parse(GetCookie("AdminsPageNumber")) - 1) * 20;
                            int Count = _adminRep.GetAllAdmins().Count();
                            ViewBag.AdminsPageID = GetCookie("AdminsPageNumber");
                            ViewBag.AdminsPageCount = Count / 20;
                            jsonResult = Json(_adminRep.GetAdminsForPages(skip));
                        }
                    }
                    break;
                case 6:
                    {
                        if (!string.IsNullOrEmpty(searchtext))
                        {
                            var Searchlist = _messageRep.GetAllMessages().Where(m => (!string.IsNullOrEmpty(m.Customer.FullName.ToString()) && m.Customer.FullName.ToString().Contains(searchtext))
                            || (!string.IsNullOrEmpty(m.Customer.MobileNumber.ToString()) && m.Customer.MobileNumber.ToString().Contains(searchtext))
                            || (!string.IsNullOrEmpty(m.MessageText.ToString()) && m.MessageText.ToString().Contains(searchtext))
                            || (!string.IsNullOrEmpty(m.SentDate.ToString()) && m.SentDate.ToString().Contains(searchtext))
                            || (!string.IsNullOrEmpty(m.SentState.ToString()) && m.SentState.ToString().Contains(searchtext))
                            ).ToList();
                            jsonResult = Json(Searchlist);
                        }
                        else
                        {
                            int skip = (int.Parse(GetCookie("MessagesPageNumber")) - 1) * 20;
                            int Count = _messageRep.GetAllMessages().Count();
                            ViewBag.MessagesPageID = GetCookie("MessagesPageNumber");
                            ViewBag.MessagesPageCount = Count / 20;
                            jsonResult = Json(_messageRep.GetMessagesForPages(skip));
                        }
                    }
                    break;
                case 8:
                    {
                        if (!string.IsNullOrEmpty(searchtext))
                        {
                            var Searchlist = _meetingRep.GetNewMeetings().Where(r =>
                            (r.Customer != null && (!string.IsNullOrEmpty(r.Customer.FullName.ToString()) && r.Customer.FullName.ToString().Contains(searchtext)))
                            || (!string.IsNullOrEmpty(r.AccountNumber.ToString()) && r.AccountNumber.ToString().Contains(searchtext))
                             || (!string.IsNullOrEmpty(r.MeetingDate.ToString()) && r.MeetingDate.ToString().Contains(searchtext))
                            || (!string.IsNullOrEmpty(r.Amount.ToString()) && r.Amount.ToString().Contains(searchtext))
                            || (!string.IsNullOrEmpty(r.Count.ToString()) && r.Count.ToString().Contains(searchtext))
                            || (!string.IsNullOrEmpty(r.Description.ToString()) && r.Description.ToString().Contains(searchtext))
                            || (!string.IsNullOrEmpty(r.Status.ToString()) && r.Status.ToString().Contains(searchtext))
                            || (!string.IsNullOrEmpty(r.Response.ToString()) && r.Response.ToString().Contains(searchtext))
                            || (!string.IsNullOrEmpty(r.MeetingId.ToString()) && r.MeetingId.ToString().Contains(searchtext))
                            ).ToList();
                            jsonResult = Json(Searchlist);
                        }
                        else
                        {
                            int skip = (int.Parse(GetCookie("NewMeetingsPageNumber")) - 1) * 20;
                            int Count = _meetingRep.GetNewMeetings().Count();
                            ViewBag.NewMeetingsPageID = GetCookie("NewMeetingsPageNumber");
                            ViewBag.NewMeetingsPageCount = Count / 20;
                            jsonResult = Json(_meetingRep.GetNewMeetingsForPages(skip));
                        }
                    }
                    break;
                case 9:
                    {
                        if (!string.IsNullOrEmpty(searchtext))
                        {
                            var Searchlist = _meetingRep.GetOldMeetings().Where(r =>
                            (r.Customer != null && (!string.IsNullOrEmpty(r.Customer.FullName.ToString()) && r.Customer.FullName.ToString().Contains(searchtext)))
                            || (!string.IsNullOrEmpty(r.AccountNumber.ToString()) && r.AccountNumber.ToString().Contains(searchtext))
                             || (!string.IsNullOrEmpty(r.MeetingDate.ToString()) && r.MeetingDate.ToString().Contains(searchtext))
                            || (!string.IsNullOrEmpty(r.Amount.ToString()) && r.Amount.ToString().Contains(searchtext))
                            || (!string.IsNullOrEmpty(r.Count.ToString()) && r.Count.ToString().Contains(searchtext))
                            || (!string.IsNullOrEmpty(r.Description.ToString()) && r.Description.ToString().Contains(searchtext))
                            || (!string.IsNullOrEmpty(r.Status.ToString()) && r.Status.ToString().Contains(searchtext))
                            || (!string.IsNullOrEmpty(r.Response.ToString()) && r.Response.ToString().Contains(searchtext))
                            || (!string.IsNullOrEmpty(r.MeetingId.ToString()) && r.MeetingId.ToString().Contains(searchtext))
                            ).ToList();
                            jsonResult = Json(Searchlist);
                        }
                        else
                        {
                            int skip = (int.Parse(GetCookie("OldMeetingsPageNumber")) - 1) * 20;
                            int Count = _meetingRep.GetOldMeetings().Count();
                            ViewBag.OldMeetingsPageID = GetCookie("OldMeetingsPageNumber");
                            ViewBag.OldMeetingsPageCount = Count / 20;
                            jsonResult = Json(_meetingRep.GetOldMeetingsForPages(skip));
                        }
                    }
                    break;
                case 10:
                    {
                        if (!string.IsNullOrEmpty(searchtext))
                        {
                            var Searchlist = _notificationRep.GetAllNotifications().Where(a => (!string.IsNullOrEmpty(a.NotificationTitle.ToString()) && a.NotificationTitle.ToString().Contains(searchtext))
                            || (!string.IsNullOrEmpty(a.NotificationDescription.ToString()) && a.NotificationDescription.ToString().Contains(searchtext))
                            || (!string.IsNullOrEmpty(a.NotificationStatus.ToString()) && a.NotificationStatus.ToString().Contains(searchtext))
                            ).ToList();
                            jsonResult = Json(Searchlist);
                        }
                        else
                        {
                            int skip = (int.Parse(GetCookie("NotificationsPageNumber")) - 1) * 20;
                            int Count = _notificationRep.GetAllNotifications().Count();
                            ViewBag.NotificationsPageID = GetCookie("NotificationsPageNumber");
                            ViewBag.NotificationsPageCount = Count / 20;
                            jsonResult = Json(_notificationRep.GetNotificationsForPages(skip));
                        }
                    }
                    break;
            }

            return jsonResult;
        }
        #endregion

        #region Customers

        // GET: Admin

        [Route("Admin")]
        public async Task<IActionResult> ShowCustomers(int pageid=1)
        {
            SetDefaultPageNumbers(1,pageid);
            int skip = (pageid - 1) * 20;
            int Count = _customerRep.GetAllCustomers().Count();
            ViewBag.CustomersPageID = pageid;
            ViewBag.CustomersPageCount = Count / 20;
            return View(_customerRep.GetCustomersForPages(skip));
        }

        // GET: Admin/Create
        public IActionResult AddCustomer()
        {
            //if (User.GetClaimValue("AdminType") == null) return Redirect("Account/Login");
            ViewBag.CustomersPageID = GetCookie("CustomersPageNumber");
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCustomer(AddCustomerVM customer)
        {
            ViewBag.CustomersPageID = GetCookie("CustomersPageNumber");
            if (ModelState.IsValid)
            {
                Customer theCustomer = new Customer()
                {
                    FatherName = customer.FatherName,
                    FullName = customer.FullName,
                    CustomerDescription = customer.CustomerDescription ?? "",
                    MobileNumber= customer.MobileNumber,
                    NationalCode = customer.NationalCode,
                    VarizId= customer.VarizId?? ""
                };
                _customerRep.AddCustomer(theCustomer);
                BankAccount theAccount = new BankAccount()
                {
                    AccountNumber = customer.AccountNumber,
                    CustomerId = theCustomer.CustomerId,
                    Score= "0"
                };
                _accountRep.AddBankAccount(theAccount);
                return Redirect("/Admin?pageid=" + GetCookie("CustomersPageNumber"));
            }
            return View(customer);
        }

        public IActionResult AddCustomersGroup()
        {
            //if (User.GetClaimValue("AdminType") == null) return Redirect("Account/Login");
            ViewBag.CustomersPageID = GetCookie("CustomersPageNumber");
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       public async Task<IActionResult> AddCustomersGroup(AddCustomersGroup customersGroup)
        {
            ViewBag.CustomersPageID = GetCookie("CustomersPageNumber");
            if (ModelState.IsValid)
            {
                if (customersGroup.CustomersFile?.Length > 0)
                {
                    var stream = customersGroup.CustomersFile.OpenReadStream();
                    try
                    {
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.First();
                            var rowCount = worksheet.Dimension.Rows;
                            for (var row = rowCount; row >= 2; row--)
                            {
                                try
                                {
                                    var varizId = worksheet.Cells[row, 1].Value?.ToString()??"".Trim();
                                    var nationalCode = worksheet.Cells[row, 2].Value?.ToString()??"".Trim();
                                    var mobileNumber = worksheet.Cells[row, 3].Value?.ToString()??"".Trim();
                                    if (!mobileNumber.StartsWith("0")) mobileNumber = "0" + mobileNumber;
                                    var fatherName = worksheet.Cells[row, 4].Value?.ToString()??"".Trim();
                                    var unnormalName = worksheet.Cells[row, 5].Value?.ToString()?? "".Trim();
                                    if (unnormalName == "") unnormalName = " = ";
                                    var arrName = unnormalName.Split('=');
                                    var firstName = arrName[1].Trim();
                                    var lastName = arrName[0].Trim();
                                    var fullName = firstName + " " + lastName;
                                    var accountNumber = worksheet.Cells[row, 6].Value?.ToString()??"".Trim();

                                    Customer theCustomer = new Customer()
                                    {
                                        FatherName = fatherName ?? "",
                                        FullName = fullName ?? "",
                                        CustomerDescription = "",
                                        MobileNumber = mobileNumber ?? "",
                                        NationalCode = nationalCode ?? "",
                                        VarizId = varizId ?? ""
                                    };
                                    _customerRep.AddCustomer(theCustomer);

                                    if ((!string.IsNullOrEmpty(accountNumber) && _accountRep.ExistBankAccountNumber(accountNumber)))
                                    {
                                        var account = _accountRep.GetAccountByAccountNumber(accountNumber);
                                        var customer = _customerRep.GetCustomerById((int)account.CustomerId);
                                        if (customer.MobileNumber != theCustomer.MobileNumber)

                                        {
                                            Right right = new Right()
                                            {
                                                RighterId = theCustomer.CustomerId,
                                                AcountId = account.AcountId
                                            };
                                            if (!_rightRep.ExistRight(right.RighterId, right.AcountId))
                                            {
                                                _rightRep.AddRight(right);
                                            }
                                        }
                                        else _accountRep.EditBankAccount(account);
                                        continue;
                                    }

                                    BankAccount theAccount = new BankAccount()
                                    {
                                        AccountNumber = accountNumber ?? "",
                                        Score = "0"
                                    };
                                    theAccount.CustomerId = theCustomer.CustomerId;
                                    _accountRep.AddBankAccount(theAccount);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    return Redirect("/Admin?pageid=" + GetCookie("CustomersPageNumber"));
                }
            }
            return View(customersGroup);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> EditCustomer(int id)
        {
            //if (User.GetClaimValue("AdminType") == null) return Redirect("Account/Login");
            if (id == 0 ||_customerRep == null)
            {
                return NotFound();
            }

            var customer = _customerRep.GetCustomerById(id);
            if (customer == null)
            {
                return NotFound();
            }
            EditCustomerVM customerVM = new EditCustomerVM()
            {
                CustomerId = customer.CustomerId,
                CustomerDescription = customer.CustomerDescription,
                FatherName = customer.FatherName,
                VarizId = customer.VarizId,
                MobileNumber = customer.MobileNumber,
                FullName = customer.FullName,
                NationalCode = customer.NationalCode
            };
            ViewBag.CustomersPageID = GetCookie("CustomersPageNumber");
            return View(customerVM);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCustomer(int id, EditCustomerVM customer)
        {
            ViewBag.CustomersPageID = GetCookie("CustomersPageNumber");
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Customer theCustomer = new Customer()
                {
                    CustomerId = customer.CustomerId,
                    FatherName = customer.FatherName,
                    FullName = customer.FullName,
                    CustomerDescription = customer.CustomerDescription ?? "",
                    MobileNumber = customer.MobileNumber,
                    NationalCode = customer.NationalCode,
                    VarizId = customer.VarizId ?? ""
                };
                _customerRep.EditCustomer(theCustomer);
                return Redirect("/Admin?pageid=" +GetCookie("CustomersPageNumber"));
            }
            return View(customer);
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            //if (User.GetClaimValue("AdminType") == null) return Redirect("Account/Login");
            if (id == 0 || _customerRep == null)
            {
                return NotFound();
            }

            var customer = _customerRep.GetCustomerById(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewBag.CustomersPageID = GetCookie("CustomersPageNumber");
            return View(customer);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("DeleteCustomer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CustomerDeleteConfirmed(int id)
        {
            if (_customerRep == null)
            {
                return Problem("Entity set 'SendReceiptContext.Customers'  is null.");
            }
            _customerRep.RemoveCustomer(id);
            return Redirect("/Admin?pageid=" + GetCookie("CustomersPageNumber"));
        }

        private bool CustomerExists(int id)
        {
          return _customerRep.ExistCustomer(id);
        }
        public IActionResult isNewMobileNumber(string MobileNumber, int CustomerId = 0) // an Action that remoted for check the field value validation (no need to post page)
        {
            if (!_customerRep.ExistMobileNumber(MobileNumber.ToLower(),CustomerId))
            {
                return Json(true); // send true value
            }
            else return Json($"شماره موبایل {MobileNumber} تکراری است"); //send error text
        }

        public IActionResult isNewNationalCode(string NationalCode, int CustomerId=0) // an Action that remoted for check the field value validation (no need to post page)
        {
            if (!_customerRep.ExistNationalCode(NationalCode.ToLower(),CustomerId))
            {
                return Json(true); // send true value
            }
            else return Json($"کد ملی {NationalCode} تکراری است"); //send error text
        }

        public IActionResult isNewVarizId(string VarizId, int CustomerId = 0) // an Action that remoted for check the field value validation (no need to post page)
        {
            if (!_customerRep.ExistVarizId(VarizId.ToLower(), CustomerId))
            {
                return Json(true); // send true value
            }
            else return Json($"شناسه واریز {VarizId} تکراری است"); //send error text
        }

        #endregion

        #region Admins

        public async Task<IActionResult> ShowAdmins(int pageid=1)
        {
            SetDefaultPageNumbers(4, pageid);
            int skip = (pageid - 1) * 20;
            int Count = _adminRep.GetAllAdmins().Count();
            ViewBag.AdminsPageID = pageid;
            ViewBag.AdminsPageCount = Count / 20;
            return View(_adminRep.GetAdminsForPages(skip));
        }

        // GET: Admins/Create
        public IActionResult AddAdmin()
        {
            //if (User.GetClaimValue("AdminType") == null) return Redirect("Account/Login");

            ViewBag.AdminsPageID = GetCookie("AdminsPageNumber");
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAdmin(AdminVM admin)
        {
            ViewBag.AdminsPageID = GetCookie("AdminsPageNumber");
            if (ModelState.IsValid && CheckChief((admin.AdminType == "مدیر ارشد"), 1))
            {
                Admin theAdmin = new Admin()
                {
                    UserName = admin.UserName,
                    Password = admin.Password,
                    AdminType = admin.AdminType
                };
                _adminRep.AddAdmin(theAdmin);
                return Redirect("/Admin/ShowAdmins?pageid=" + GetCookie("AdminsPageNumber"));
            }
            if (!CheckChief((admin.AdminType == "مدیر ارشد"), 1)) ModelState.AddModelError("IsChief", "وجود حداقل یک مدیر ارشد الزامی است");
            return View(admin);
        }

        // GET: Admins/Edit/5
        public async Task<IActionResult> EditAdmin(int id)
        {
            //if (User.GetClaimValue("AdminType") == null) return Redirect("Account/Login");

            if (id == 0 || _adminRep == null)
            {
                return NotFound();
            }

            var admin = _adminRep.GetAdminById(id);
            if (admin == null)
            {
                return NotFound();
            }
            AdminVM adminVM = new AdminVM()
            {
                AdminId = admin.AdminId,
                Password = admin.Password,
                RePassword = admin.Password,
                UserName = admin.UserName,
                AdminType = admin.AdminType
            };
            ViewBag.AdminsPageID = GetCookie("AdminsPageNumber");
            return View(adminVM);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAdmin(int id, AdminVM admin)
        {
            ViewBag.AdminsPageID = GetCookie("AdminsPageNumber");
            if (id != admin.AdminId)
            {
                return NotFound();
            }

            if (ModelState.IsValid && CheckChief((admin.AdminType == "مدیر ارشد"), 1, id))
            {
                try
                {
                    Admin theAdmin = new Admin()
                    {
                        AdminId = admin.AdminId,
                        UserName = admin.UserName,
                        Password = admin.Password,
                        AdminType = admin.AdminType
                    };
                    _adminRep.EditAdmin(theAdmin);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminExists(admin.AdminId))
                    {
                        return NotFound();
                    }
                }
                return Redirect("/Admin/ShowAdmins?pageid=" + GetCookie("AdminsPageNumber"));
            }
            if (!CheckChief((admin.AdminType == "مدیر ارشد"), 1, admin.AdminId)) ModelState.AddModelError("IsChief", "وجود حداقل یک مدیر ارشد الزامی است");
            return View(admin);
        }

        // GET: Admins/Delete/5
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            //if (User.GetClaimValue("AdminType") == null) return Redirect("Account/Login");

            ViewBag.AdminsPageID = GetCookie("AdminsPageNumber");
            if (id == 0 || _adminRep == null)
            {
                return NotFound();
            }

            var admin = _adminRep.GetAdminById(id);
            if (admin == null)
            {
                return NotFound();
            }
            if (_adminRep.GetAllAdmins().Count <= 1) ViewBag.Alert = "alert";
            bool x = admin.AdminType == "مدیر ارشد" ? true : false;
            if (!CheckChief(x, 2, admin.AdminId)) ViewBag.Alert = "alert2";

            return View(admin);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("DeleteAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminDeleteConfirmed(int id)
        {
            if (_adminRep == null)
            {
                return Problem("Entity set 'SendReceiptContext.Admins'  is null.");
            }
            _adminRep.RemoveAdmin(id);
            return Redirect("/Admin/ShowAdmins?pageid=" + GetCookie("AdminsPageNumber"));
        }

        private bool AdminExists(int id)
        {
            return _adminRep.ExistAdmin(id);
        }

        public IActionResult isNewUserName(string UserName, int AdminId = 0) // an Action that remoted for check the field value validation (no need to post page)
        {
            if (!_adminRep.ExistUserName(UserName.ToLower(),AdminId))
            {
                return Json(true); // send true value
            }
            else return Json($"شماره حساب {UserName} تکراری است"); //send error text
        }

        public bool CheckChief(bool isChief, int actCase, int adminId = 0)
        {
            switch (actCase)
            {
                case 1:
                default:
                    if (isChief) return true;
                    else
                    {
                        if (adminId == 0) return _adminRep.GetAllAdmins().Any(a => a.AdminType == "مدیر ارشد");
                        else return _adminRep.GetAllAdmins().Any(a => a.AdminType == "مدیر ارشد" && a.AdminId != adminId);
                    }
                case 2:
                    if (!isChief) return true;
                    else return _adminRep.GetAllAdmins().Any(a => a.AdminType == "مدیر ارشد" && a.AdminId != adminId);
            }
        }

        [HttpPost]
        public async Task<string> GetAdminsCount()
        {
            return _adminRep.GetAllAdmins().Count().ToString();
        }

        #endregion

        #region Accounts

        // GET: BankAccounts
        public async Task<IActionResult> ShowAccounts(int pageid=1)
        {
            SetDefaultPageNumbers(2, pageid);
            int skip = (pageid - 1) * 20;
            int Count = _accountRep.GetAllBankAccounts().Count();
            ViewBag.AccountsPageID = pageid;
            ViewBag.AccountsPageCount = Count / 20;
            return View(_accountRep.GetAccountsForPages(skip));
        }

        // GET: BankAccounts/Create
        public IActionResult AddAccount()
        {
            //if (User.GetClaimValue("AdminType") == null) return Redirect("Account/Login");

            ViewBag.AccountsPageID = GetCookie("AccountsPageNumber");
            return View();
        }

        // POST: BankAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAccount(AddEditAccountVM accountVM)
        {
            ViewBag.AccountsPageID = GetCookie("AccountsPageNumber");
            if (ModelState.IsValid)
            {
                BankAccount bankAccount = new BankAccount()
                {
                    AccountNumber = accountVM.AccountNumber,
                    CustomerId=int.Parse(accountVM.CustomerId),
                    Score = accountVM.Score ?? "0"
                };
                _accountRep.AddBankAccount(bankAccount);
                return Redirect("/Admin/ShowAccounts?pageid=" + GetCookie("AccountsPageNumber"));
            }
            return View(accountVM);
        }

        // GET: BankAccounts/Edit/5
        public async Task<IActionResult> EditAccount(int id)
        {
            //if (User.GetClaimValue("AdminType") == null) return Redirect("Account/Login");

            if (id == 0 || _accountRep == null)
            {
                return NotFound();
            }

            var bankAccount = _accountRep.GetBankAccountById(id);
            if (bankAccount == null)
            {
                return NotFound();
            }
            AddEditAccountVM accountVM = new AddEditAccountVM()
            {
                AcountId = bankAccount.AcountId,
                AccountNumber=bankAccount.AccountNumber,
                CustomerId = bankAccount.CustomerId.ToString(),
                Score = bankAccount.Score ?? "0"
            };
            ViewBag.AccountsPageID = GetCookie("AccountsPageNumber");
            return View(accountVM);
        }

        // POST: BankAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAccount(AddEditAccountVM accountVM, int id)
        {
            ViewBag.AccountsPageID = GetCookie("AccountsPageNumber");
            if (id != accountVM.AcountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                BankAccount bankAccount = new BankAccount()
                {
                    AcountId = accountVM.AcountId,
                    AccountNumber = accountVM.AccountNumber,
                    CustomerId = int.Parse(accountVM.CustomerId),
                    Score= accountVM.Score ?? "0",
                };
                _accountRep.EditBankAccount(bankAccount);
                return Redirect("/Admin/ShowAccounts?pageid=" + GetCookie("AccountsPageNumber"));
            }
            return View(accountVM);
        }

        // GET: BankAccounts/Delete/5
        public async Task<IActionResult> DeleteAccount(int id)
        {
            //if (User.GetClaimValue("AdminType") == null) return Redirect("Account/Login");

            if (id == 0 || _accountRep == null)
            {
                return NotFound();
            }

            var bankAccount = _accountRep.GetBankAccountById(id);
            if (bankAccount == null)
            {
                return NotFound();
            }
            ViewBag.AccountsPageID = GetCookie("AccountsPageNumber");
            return View(bankAccount);
        }

        // POST: BankAccounts/Delete/5
        [HttpPost, ActionName("DeleteAccount")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AccountDeleteConfirmed(int id)
        {
            if (_accountRep == null)
            {
                return Problem("Entity set 'SendReceiptContext.BankAccounts'  is null.");
            }
            _accountRep.RemoveBankAccount(id);
            return Redirect("/Admin/ShowAccounts?pageid=" + GetCookie("AccountsPageNumber"));
        }

        private bool BankAccountExists(int id)
        {
            return _accountRep.ExistBankAccount(id);
        }

        public IActionResult isNewAccountNumber(string AccountNumber, int AcountId = 0) // an Action that remoted for check the field value validation (no need to post page)
        {
            if (!_accountRep.ExistBankAccountNumber(AccountNumber.ToLower(), AcountId))
            {
                return Json(true); // send true value
            }
            else return Json($"شماره حساب {AccountNumber} تکراری است"); //send error text
        }

        public IActionResult VerifyCustomerIdForAccount(string CustomerId) // an Action that remoted for check the field value validation (no need to post page)
        {
            if (_customerRep.ExistCustomer(int.Parse(CustomerId)))
            {
                return Json(true); // send true value
            }
            else return Json($"کد مشتری {CustomerId} در سیستم وجود ندارد"); //send error text
        }

        #endregion


        #region Meetings


        public IActionResult SetSiteDefaultData()
        {
            var infos = new DefaultDataVM()
            {
                DefaultGuarantorsConditions = _contentRep.GetContentById(1).ContentText,
                DefaultBlockerConditions = _contentRep.GetContentById(2).ContentText,
            };
            return View(infos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetSiteDefaultData(DefaultDataVM infos)
        {
            if (ModelState.IsValid)
            {
                Content blockerConditions = new Content()
                {
                    ContentId = 1,
                    ContentTitle = "شرایط پیش فرض ضامنین",
                    ContentType = "اطلاعات",
                    ContentText =infos.DefaultGuarantorsConditions??"",
                    HasImage = false
                };
                _contentRep.EditContent(blockerConditions);

                Content guarantorsConditions = new Content()
                {
                    ContentId = 2,
                    ContentTitle = "شرایط پیش فرض مسدود کننده",
                    ContentType = "اطلاعات",
                    ContentText = infos.DefaultBlockerConditions??"",
                    HasImage = false
                };
                _contentRep.EditContent(guarantorsConditions);
            }
          
            ViewBag.Success = true;
            return View();
        }

        // GET: Meetings
        public async Task<IActionResult> ShowNewMeetings(int pageid = 1)
        {
            SetDefaultPageNumbers(8, pageid);
            int skip = (pageid - 1) * 20;
            int Count = _meetingRep.GetNewMeetings().Count();
            ViewBag.NewMeetingsPageID = pageid;
            ViewBag.NewMeetingsPageCount = Count / 20;
            return View(_meetingRep.GetNewMeetingsForPages(skip));
        }

        public async Task<IActionResult> ShowOldMeetings(int pageid = 1)
        {
            SetDefaultPageNumbers(9, pageid);
            int skip = (pageid - 1) * 20;
            int Count = _meetingRep.GetOldMeetings().Count();
            ViewBag.OldMeetingsPageID = pageid;
            ViewBag.OldMeetingsPageCount = Count / 20;
            return View(_meetingRep.GetOldMeetingsForPages(skip));
        }
        // GET: Meetings/Delete/5
        public async Task<IActionResult> DeleteMeeting(int id)
        {
            //if (User.GetClaimValue("AdminType") == null) return Redirect("Account/Login");

            if (id == 0 || _meetingRep == null)
            {
                return NotFound();
            }

            var meeting = _meetingRep.GetMeetingById(id);
            if (meeting == null)
            {
                return NotFound();
            }
            var files = GetMeetingFilesUrls(id);
            ViewBag.files = GetMeetingUrlsForShow(files);
            ViewBag.filescount = GetMeetingUrlsForShow(files).Count;
            ViewBag.NewMeetingsPageID = GetCookie("NewMeetingsPageNumber");
            return View(meeting);
        }

        public async Task<IActionResult> CheckMeeting(int id)
        {
            //if (User.GetClaimValue("AdminType") == null) return Redirect("Account/Login");

            if (id == 0 || _meetingRep == null)
            {
                return NotFound();
            }

            var meeting = _meetingRep.GetMeetingById(id);
            if (meeting == null)
            {
                return NotFound();
            }
            var files = GetMeetingFilesUrls(id);
            SetCookie("MeetId", id.ToString());
            ViewBag.files = GetMeetingUrlsForShow(files);
            ViewBag.filescount = GetMeetingUrlsForShow(files).Count;
            ViewBag.NewMeetingsPageID = GetCookie("NewMeetingsPageNumber");
            return View(meeting);
        }

        private List<string> GetMeetingUrlsForShow(List<string> files)
        {
            List<string> urls = new List<string>();
            foreach (string file in files)
            {
                urls.Add(file.Remove(0, file.IndexOf("pics")).Replace('\\', '/').Insert(0, "/"));
            }
            return urls;
        }

        // POST: Receipts/Delete/5
        [HttpPost, ActionName("DeleteMeeting")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MeetingDeleteConfirmed(int id)
        {
            if (_meetingRep == null)
            {
                return Problem("Entity set 'SendReceiptContext.Receipts'  is null.");
            }
            _meetingRep.RemoveMeeting(id);
            await RemoveMeetingFiles(id);
            return Redirect("/Admin/ShowNewMeetings?pageid=" + GetCookie("NewMeetingsPageNumber"));
        }

        private async Task RemoveMeetingFiles(int id)
        {
            var files = GetMeetingFilesUrls(id);
            if (files.Count > 0)
            {
                foreach (string file in files)
                {
                    System.IO.File.Delete(file);
                }
            }
        }

        private List<string> GetMeetingFilesUrls(int id)
        {
            List<string> ResultUrls = new List<string>();
            string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                      "wwwroot",
                      "pics");
            var files = Directory.GetFiles(filePath, "*.*", SearchOption.AllDirectories);
            if (files.Length > 0)
            {
                foreach (string file in files)
                {
                    if (Path.GetFileNameWithoutExtension(file).Contains($"meet{id}-"))
                        ResultUrls.Add(file);
                }
            }
            return ResultUrls;
        }

        private bool MeetingExists(int id)
        {
            return _meetingRep.ExistMeeting(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResponseMeeting(CheckMeetingVM checkMeeting,int btn)
        {

            ViewBag.NewMeetingsPageID = GetCookie("NewMeetingsPageNumber");
            if (ModelState.IsValid)
            {
                var meet = _meetingRep.GetMeetingById(int.Parse(GetCookie("MeetId")));
                if (meet.Status.Contains("درخواست تجدید نظر"))
                {
                    meet.Status = (btn == 1) ? "درخواست تجدید نظر - تایید شده توسط " + User.Identity.Name : "درخواست تجدید نظر - رد شده توسط " + User.Identity.Name;
                }
                else
                {
                    meet.Status = (btn == 1) ? "تایید شده توسط " + User.Identity.Name : "رد شده توسط " + User.Identity.Name;
                }
                meet.Response = checkMeeting.Response;
                _meetingRep.EditMeeting(meet);
                string meetingResponseMessage = MakeMeetingResponseMessage(meet);
                //bool sentState = await SendMessage(_customerRep.GetCustomerById((int)meet.CustomerId).MobileNumber, meetingResponseMessage);
                return Redirect("/Admin/ShowNewMeetings?pageid=" + GetCookie("NewMeetingsPageNumber"));
            }
            return RedirectToAction("CheckMeeting", GetCookie("MeetId"));
        }

        private string MakeMeetingResponseMessage(Meeting meet)
        {
            string rivision = meet.Status.Contains("تجدید نظر") ? "تجدید نظر" : "";
            string staus = meet.Status.Contains("تایید شده") ? "تایید شده" : "رد شده";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{_customerRep.GetCustomerById((int)meet.CustomerId).FullName} عزیز!");
            sb.AppendLine($"درخواست {rivision} وام شما");
            sb.AppendLine($"به مبلغ {meet.Amount} ریال");
            sb.AppendLine($"با تعداد اقساط  {meet.Count}");
            //sb.AppendLine($"با شماره حساب {meet.AccountNumber}");
            if (!string.IsNullOrEmpty(meet.ScorerAccountNumber) && !string.IsNullOrEmpty(meet.ScorerName))
            {
                sb.AppendLine($"با درخواست امتیاز از   {meet.ScorerName}");
               // sb.AppendLine($"به نام {meet.ScorerName}");
            }
            sb.AppendLine($"با کد پیگیری {meet.MeetingId} {staus}");
            sb.AppendLine($"شرح پاسخ :");
            sb.AppendLine(meet.Response);
            sb.AppendLine("از حسن توجه شما متشکریم.");
            return sb.ToString();
        }

        #region Prints
        public IActionResult PrintRequest(int id)
        {
            //if (User.GetClaimValue("AdminType") == null) return Redirect("Account/Login");

            var meet = _meetingRep.GetMeetingById(id);
            return View(meet);
        }

        public IActionResult PrintScore(int id)
        {
            //if (User.GetClaimValue("AdminType") == null) return Redirect("Account/Login");

            var meet = _meetingRep.GetMeetingById(id);
            return View(meet);
        }

        public IActionResult PrintBlockScore(int id)
        {
            //if (User.GetClaimValue("AdminType") == null) return Redirect("Account/Login");

            var meet = _meetingRep.GetMeetingById(id);
            return View(meet);
        }
         public IActionResult PrintRivisionRequest(int id)
        {
            //if (User.GetClaimValue("AdminType") == null) return Redirect("Account/Login");

            var meet = _meetingRep.GetMeetingById(id);
            return View(meet);
        }

        #endregion

        #endregion

        #region Rights

        public async Task<IActionResult> ShowRights(int pageid=1)
        {
            SetDefaultPageNumbers(3, pageid);
            int skip = (pageid - 1) * 20;
            int Count = _rightRep.GetAlRights().Count();
            ViewBag.RightsPageID = pageid;
            ViewBag.RightsPageCount = Count / 20;
            List<RightVM> rightsVM = _rightRep.GetRightsForPages(skip).Select(vm => new RightVM()
            {
                RightId = vm.RightId,
                RighterName = _customerRep.GetCustomerById(vm.RighterId).FullName,
                RightOwnerName = _customerRep.GetCustomerByAccountId(vm.AcountId).FullName,
                AccountNumber = _accountRep.GetBankAccountById(vm.AcountId).AccountNumber
            }).ToList();
            return View(rightsVM);
        }

        // GET: Rights/Create
        public IActionResult AddRight()
        {
            //if (User.GetClaimValue("AdminType") == null) return Redirect("Account/Login");

            ViewBag.RightsPageID = GetCookie("RightsPageNumber");
            return View();
        }

        // POST: Rights/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRight(AddEditRightVM right)
        {
            ViewBag.RightsPageID = GetCookie("RightsPageNumber");
            if (ModelState.IsValid)
            {
                Right theRight = new Right()
                {
                    RighterId = int.Parse(right.RighterCustomerId),
                    AcountId = _accountRep.GetAccountByAccountNumber(right.AccountNumber).AcountId
                };
                _rightRep.AddRight(theRight);
                return Redirect("/Admin/ShowRights?pageid=" + GetCookie("RightsPageNumber"));
            }
            return View(right);
        }

        // GET: Rights/Delete/5
        public async Task<IActionResult> DeleteRight(int id)
        {
            //if (User.GetClaimValue("AdminType") == null) return Redirect("Account/Login");

            if (id == 0 || _rightRep == null)
            {
                return NotFound();
            }

            var right = _rightRep.GetRightById(id);
            if (right == null)
            {
                return NotFound();
            }
            RightVM theRight = new RightVM()
            {
                RightId = right.RightId,
                RighterName = _customerRep.GetCustomerById(right.RighterId).FullName,
                RightOwnerName = _customerRep.GetCustomerByAccountId(right.AcountId).FullName,
                AccountNumber = _accountRep.GetBankAccountById(right.AcountId).AccountNumber
            };
            ViewBag.RightsPageID = GetCookie("RightsPageNumber");
            return View(theRight);
        }

        // POST: Rights/Delete/5
        [HttpPost, ActionName("DeleteRight")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RightDeleteConfirmed(int id)
        {
            if (_rightRep == null)
            {
                return Problem("Entity set 'SendReceiptContext.Rights'  is null.");
            }
            _rightRep.RemoveRight(id);
            return Redirect("/Admin/ShowRights?pageid=" + GetCookie("RightsPageNumber"));
        }

        private bool RightExists(int id)
        {
            return _rightRep.ExistRight(id);
        }
        public IActionResult VerifyCustomerId1(string RighterCustomerId) // an Action that remoted for check the field value validation (no need to post page)
        {
            if (_customerRep.ExistCustomer(int.Parse(RighterCustomerId)))
            {
                return Json(true); // send true value
            }
            else return Json($"کد مشتری امضادار دوم {RighterCustomerId} در سیستم وجود ندارد"); //send error text
        }

        public IActionResult VerifyAccountForRight(string AccountNumber, string RighterCustomerId) // an Action that remoted for check the field value validation (no need to post page)
        {
            var accounts = _accountRep.GetBankAccountsOfCustomer(int.Parse(RighterCustomerId),1);
            if (_accountRep.ExistBankAccountNumber(AccountNumber.ToLower()) && !accounts.Any(a=> a.BankAcountId == _accountRep.GetAccountByAccountNumber(AccountNumber).AcountId))
            {
                return Json(true); // send true value
            }
            else if (!_accountRep.ExistBankAccountNumber(AccountNumber.ToLower())) return Json($"شماره حساب  {AccountNumber} در سیستم وجود ندارد"); //send error text
            else return Json($"مشتری {_customerRep.GetCustomerById(int.Parse(RighterCustomerId)).FullName} حق امضای حساب به شماره {AccountNumber} را دارد یا صاحب اصلی این حساب است"); //send error text
        }

        #endregion

        #region Notifications

        public async Task<IActionResult> ShowNotifications(int pageid=1)
        {
            SetDefaultPageNumbers(10, pageid);
            int skip = (pageid - 1) * 20;
            int Count = _notificationRep.GetAllNotifications().Count();
            ViewBag.NotificationsPageID = pageid;
            ViewBag.NotificationsPageCount = Count / 20;
            return View(_notificationRep.GetNotificationsForPages(skip));
        }

        // GET: Notifications/Create
        public IActionResult AddNotification()
        {

            ViewBag.NotificationsPageID = GetCookie("NotificationsPageNumber");
            return View();
        }

        // POST: Notifications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNotification(NotificationVM notification)
        {
            ViewBag.NotificationsPageID = GetCookie("NotificationsPageNumber");
            if (ModelState.IsValid)
            {
                Notification theNotification = new Notification()
                {
                  NotificationTitle = notification.NotificationTitle,
                  NotificationDescription = notification.NotificationDescription,
                  NotificationStatus = notification.NotificationStatus ? "فعال" : "غیرفعال"
                };
                _notificationRep.AddNotification(theNotification);
                return Redirect("/Admin/ShowNotifications?pageid=" + GetCookie("NotificationsPageNumber"));
            }
            return View(notification);
        }

          // GET: Admins/Edit/5
        public async Task<IActionResult> EditNotification(int id)
        {

            if (id == 0 || _notificationRep == null)
            {
                return NotFound();
            }

            var notification = _notificationRep.GetNotificationById(id);
            if (notification == null)
            {
                return NotFound();
            }
            NotificationVM notificationVM = new NotificationVM()
            {
                NotificationId = notification.NotificationId,
                NotificationTitle = notification.NotificationTitle,
                NotificationDescription = notification.NotificationDescription,
                NotificationStatus = notification.NotificationStatus=="فعال",
            };
            ViewBag.AdminsPageID = GetCookie("AdminsPageNumber");
            return View(notificationVM);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditNotification(int id, NotificationVM notification)
        {
            ViewBag.NotificationsPageID = GetCookie("NotificationsPageNumber");
            if (id != notification.NotificationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Notification theNotification = new Notification()
                    {
                        NotificationId = notification.NotificationId,
                        NotificationTitle = notification.NotificationTitle,
                        NotificationDescription = notification.NotificationDescription,
                        NotificationStatus = notification.NotificationStatus ? "فعال" : "غیرفعال"
                    };
                    _notificationRep.EditNotification(theNotification);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationExists(notification.NotificationId))
                    {
                        return NotFound();
                    }
                }
                return Redirect("/Admin/ShowNotifications?pageid=" + GetCookie("NotificationsPageNumber"));
            }
            return View(notification);
        }

        // GET: Notifications/Delete/5
        public async Task<IActionResult> DeleteNotification(int id)
        {
            if (id == 0 || _notificationRep == null)
            {
                return NotFound();
            }

            var notification = _notificationRep.GetNotificationById(id);
            if (notification == null)
            {
                return NotFound();
            }
            ViewBag.NotificationsPageID = GetCookie("NotificationsPageNumber");
            return View(notification);
        }

        // POST: Notifications/Delete/5
        [HttpPost, ActionName("DeleteNotification")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NotificationDeleteConfirmed(int id)
        {
            if (_notificationRep == null)
            {
                return Problem("Entity set 'SendReceiptContext.Notifications'  is null.");
            }
            _notificationRep.RemoveNotification(id);
            return Redirect("/Admin/ShowNotifications?pageid=" + GetCookie("NotificationsPageNumber"));
        }

        private bool NotificationExists(int id)
        {
            return _notificationRep.ExistNotification(id);
        }

        public IActionResult CheckNotificationStatus(bool NotificationStatus, int NotificationId) // an Action that remoted for check the field value validation (no need to post page)
        {
            if (_notificationRep.CheckNotificationStatus(NotificationStatus,NotificationId))
            {
                return Json(true); // send true value
            }
            else return Json($"اعلان فعال دیگری وجود دارد ابتدا آن را غیرفعال نمایید");
        }
        #endregion

        #region Messages

        public async Task<IActionResult> ShowMessages(int pageid = 1)
        {
            SetDefaultPageNumbers(5, pageid);
            int skip = (pageid - 1) * 20;
            int Count = _messageRep.GetAllMessages().Count();
            ViewBag.MessagesPageID = pageid;
            ViewBag.MessagesPageCount = Count / 20;
            return View(_messageRep.GetMessagesForPages(skip));
        }

        public IActionResult AddMessage(int id)
        {
            //if (User.GetClaimValue("AdminType") == null) return Redirect("Account/Login");

            if (id == 0)
            {
                return NotFound();
            }
            ViewBag.ParticipantMob=_customerRep.GetCustomerById(id).MobileNumber;
            ViewBag.ParticipantName = _customerRep.GetCustomerById(id).FullName;
            ViewBag.MessagePageID = GetCookie("MessagesPageNumber");
            return View();
        }
        public IActionResult MakeMessage()
        {
            //if (User.GetClaimValue("AdminType") == null) return Redirect("Account/Login");

            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMessage(SendMessageVM messageVM)
        {
            ViewBag.MessagePageID = GetCookie("MessagesPageNumber");
            if (ModelState.IsValid)
            {
                bool sentState = await SendMessage(messageVM.MobileNumber,messageVM.MessageText);
                Message theMessage = new Message()
                {
                    CustomerId = _customerRep.GetCustomerByMobileNumber(messageVM.MobileNumber).CustomerId,
                    MessageText = messageVM.MessageText,
                    SentState = (sentState) ? "موفق" : "ناموفق",
                    SentDate = DateTime.Now.ToString("yyyy/MM/dd - HH:mm").ToShamsi()
                };
                _messageRep.AddMessage(theMessage);
                return Redirect("/Admin/ShowMessages?pageid=" + GetCookie("MessagesPageNumber"));
            }
            return View(messageVM);
        }

        public async Task<IActionResult> DeleteMessage(int id)
        {
            //if (User.GetClaimValue("AdminType") == null) return Redirect("Account/Login");

            if (id == 0 || _messageRep == null)
            {
                return NotFound();
            }

            var message = _messageRep.GetMessageById(id);
            if (message == null)
            {
                return NotFound();
            }
            ViewBag.MessagePageID = GetCookie("MessagesPageNumber");
            return View(message);
        }

        [HttpPost, ActionName("DeleteMessage")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MessageDeleteConfirmed(int id)
        {
            if (_messageRep == null)
            {
                return Problem("Entity set 'SendReceiptContext.Receipts'  is null.");
            }
            _messageRep.RemoveMessage(id);
            return Redirect("/Admin/ShowMessages?pageid=" + GetCookie("MessagesPageNumber"));
        }

        public async Task<IActionResult> ReadMessage(int id)
        {
            //if (User.GetClaimValue("AdminType") == null) return Redirect("Account/Login");

            if (id == 0 || _messageRep == null)
            {
                return NotFound();
            }

            var message = _messageRep.GetMessageById(id);
            if (message == null)
            {
                return NotFound();
            }
            ViewBag.MessagesPageID = GetCookie("MessagesPageNumber");
            return View(message);
        }

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

        #endregion

        #region ExportExcel

        public IActionResult ExportToExcel(int mode)
        {
            var stream = new MemoryStream();
            string listtitle = "";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("Sheet1");
                var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
                namedStyle.Style.Font.UnderLine = true;
                namedStyle.Style.Font.Color.SetColor(Color.Blue);
                const int startRow = 2;
                var row = startRow;

                //Create Headers and format them
                worksheet.Cells["A1:O1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A1:O1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                worksheet.Cells["A1:O1"].Style.Font.Bold = true;

                row = 2;

                switch (mode)
                {
                    case 3:
                    default:
                        var meetings = _meetingRep.GetNewMeetings();
                        listtitle = "درخواست های وام جدید";

                        worksheet.Cells["A1"].Value = "کد پیگیری";
                        worksheet.Cells["B1"].Value = "نام مشتری";
                        worksheet.Cells["C1"].Value = "مبلغ";
                        worksheet.Cells["D1"].Value = "تعداد اقساط";
                        worksheet.Cells["E1"].Value = "شماره حساب";
                        worksheet.Cells["F1"].Value = "شغل";
                        worksheet.Cells["G1"].Value = "معرف";
                        worksheet.Cells["H1"].Value = "نشانی محل کار";
                        worksheet.Cells["I1"].Value = "توضیحات";
                        worksheet.Cells["J1"].Value = "شماره حساب امتیاز دهنده";
                        worksheet.Cells["K1"].Value = "نام امتیاز دهنده";
                        worksheet.Cells["L1"].Value = "امتیاز درخواستی";
                        worksheet.Cells["M1"].Value = "وضعیت قرض امتیاز";
                        worksheet.Cells["N1"].Value = "وضعیت";
                        worksheet.Cells["O1"].Value = "پاسخ";

                        foreach (var meeting in meetings)
                        {
                            worksheet.Cells[row, 1].Value = meeting.MeetingId;
                            worksheet.Cells[row, 2].Value = meeting.Customer.FullName;
                            worksheet.Cells[row, 3].Value = meeting.Amount;
                            worksheet.Cells[row, 4].Value = meeting.Count;
                            worksheet.Cells[row, 5].Value = meeting.AccountNumber;
                            worksheet.Cells[row, 6].Value = meeting.Job;
                            worksheet.Cells[row, 7].Value = meeting.Reagent;
                            worksheet.Cells[row, 8].Value = meeting.Address;
                            worksheet.Cells[row, 9].Value = meeting.Description;
                            worksheet.Cells[row, 10].Value = meeting.ScorerAccountNumber;
                            worksheet.Cells[row, 11].Value = meeting.ScorerName;
                            worksheet.Cells[row, 12].Value = meeting.ReqScore;
                            worksheet.Cells[row, 13].Value = meeting.PassedStatus;
                            worksheet.Cells[row, 14].Value = meeting.Status;
                            worksheet.Cells[row, 15].Value = meeting.Response;

                            row++;
                        }
                        break;
                    case 4:
                        var oldmeetings = _meetingRep.GetOldMeetings();
                        listtitle = "درخواست های وام بررسی شده";

                        worksheet.Cells["A1"].Value = "کد پیگیری";
                        worksheet.Cells["B1"].Value = "نام مشتری";
                        worksheet.Cells["C1"].Value = "مبلغ";
                        worksheet.Cells["D1"].Value = "تعداد اقساط";
                        worksheet.Cells["E1"].Value = "شماره حساب";
                        worksheet.Cells["F1"].Value = "شغل";
                        worksheet.Cells["G1"].Value = "معرف";
                        worksheet.Cells["H1"].Value = "نشانی محل کار";
                        worksheet.Cells["I1"].Value = "توضیحات";
                        worksheet.Cells["J1"].Value = "شماره حساب امتیاز دهنده";
                        worksheet.Cells["K1"].Value = "نام امتیاز دهنده";
                        worksheet.Cells["L1"].Value = "امتیاز درخواستی";
                        worksheet.Cells["M1"].Value = "وضعیت قرض امتیاز";
                        worksheet.Cells["N1"].Value = "وضعیت";
                        worksheet.Cells["O1"].Value = "پاسخ";

                        foreach (var meeting in oldmeetings)
                        {
                            worksheet.Cells[row, 1].Value = meeting.MeetingId;
                            worksheet.Cells[row, 2].Value = meeting.Customer.FullName;
                            worksheet.Cells[row, 3].Value = meeting.Amount;
                            worksheet.Cells[row, 4].Value = meeting.Count;
                            worksheet.Cells[row, 5].Value = meeting.AccountNumber;
                            worksheet.Cells[row, 6].Value = meeting.Job;
                            worksheet.Cells[row, 7].Value = meeting.Reagent;
                            worksheet.Cells[row, 8].Value = meeting.Address;
                            worksheet.Cells[row, 9].Value = meeting.Description;
                            worksheet.Cells[row, 10].Value = meeting.ScorerAccountNumber;
                            worksheet.Cells[row, 11].Value = meeting.ScorerName;
                            worksheet.Cells[row, 12].Value = meeting.ReqScore;
                            worksheet.Cells[row, 13].Value = meeting.PassedStatus;
                            worksheet.Cells[row, 14].Value = meeting.Status;
                            worksheet.Cells[row, 15].Value = meeting.Response;

                            row++;
                        }
                        break;
                }
                // set some core property values
                xlPackage.Workbook.Properties.Title = listtitle;
                xlPackage.Workbook.Properties.Author = User.Identity.Name;
                xlPackage.Workbook.Properties.Subject = listtitle;


               // save the new spreadsheet
                xlPackage.Save();
                // Response.Clear();
            }
           
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{listtitle + "-" + DateTime.Now.ToString("yyyy/MM/dd - HH:mm").ToShamsi().Replace('/', '-').Replace(':', '_')}.xlsx");
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

        private void SetDefaultPageNumbers(int field, int pageid)
        {
            switch (field)
            {
                case 1:
                default:
                    SetCookie("CustomersPageNumber", pageid.ToString());
                    SetCookie("AccountsPageNumber", "1");
                    SetCookie("RightsPageNumber", "1");
                    SetCookie("AdminsPageNumber", "1");
                    SetCookie("MessagesPageNumber", "1");
                    SetCookie("NewMeetingsPageNumber", "1");
                    SetCookie("OldMeetingsPageNumber", "1");
                    SetCookie("NotificationsPageNumber", "1");
                    break;
                case 2:
                    SetCookie("CustomersPageNumber", "1");
                    SetCookie("AccountsPageNumber", pageid.ToString());
                    SetCookie("RightsPageNumber", "1");
                    SetCookie("AdminsPageNumber", "1");
                    SetCookie("MessagesPageNumber", "1");
                    SetCookie("NewMeetingsPageNumber", "1");
                    SetCookie("OldMeetingsPageNumber", "1");
                    SetCookie("NotificationsPageNumber", "1");
                    break;
                case 3:
                    SetCookie("CustomersPageNumber", "1");
                    SetCookie("AccountsPageNumber", "1");
                    SetCookie("RightsPageNumber", pageid.ToString());
                    SetCookie("AdminsPageNumber", "1");
                    SetCookie("MessagesPageNumber", "1");
                    SetCookie("NewMeetingsPageNumber", "1");
                    SetCookie("OldMeetingsPageNumber", "1");
                    SetCookie("NotificationsPageNumber", "1");
                    break;
                case 4:
                    SetCookie("CustomersPageNumber", "1");
                    SetCookie("AccountsPageNumber", "1");
                    SetCookie("RightsPageNumber", "1");
                    SetCookie("AdminsPageNumber", pageid.ToString());
                    SetCookie("MessagesPageNumber", "1");
                    SetCookie("NewMeetingsPageNumber", "1");
                    SetCookie("OldMeetingsPageNumber", "1");
                    SetCookie("NotificationsPageNumber", "1");
                    break;
                case 5:
                    SetCookie("CustomersPageNumber", "1");
                    SetCookie("AccountsPageNumber", "1");
                    SetCookie("RightsPageNumber", "1");
                    SetCookie("AdminsPageNumber", "1");
                    SetCookie("MessagesPageNumber", pageid.ToString());
                    SetCookie("NewMeetingsPageNumber", "1");
                    SetCookie("OldMeetingsPageNumber", "1");
                    SetCookie("NotificationsPageNumber", "1");
                    break;
                case 8:
                    SetCookie("CustomersPageNumber", "1");
                    SetCookie("AccountsPageNumber", "1");
                    SetCookie("RightsPageNumber", "1");
                    SetCookie("AdminsPageNumber", "1");
                    SetCookie("MessagesPageNumber", "1");
                    SetCookie("NewReceiptsPageNumber", "1");
                    SetCookie("OldReceiptsPageNumber", "1");
                    SetCookie("NewMeetingsPageNumber", pageid.ToString());
                    SetCookie("OldMeetingsPageNumber", "1");
                    SetCookie("NotificationsPageNumber", "1");
                    break;
                case 9:
                    SetCookie("CustomersPageNumber", "1");
                    SetCookie("AccountsPageNumber", "1");
                    SetCookie("RightsPageNumber", "1");
                    SetCookie("AdminsPageNumber", "1");
                    SetCookie("MessagesPageNumber", "1");
                    SetCookie("NewMeetingsPageNumber", "1");
                    SetCookie("OldMeetingsPageNumber", pageid.ToString());
                    SetCookie("NotificationsPageNumber", "1");
                    break;
                case 10:
                    SetCookie("CustomersPageNumber", "1");
                    SetCookie("AccountsPageNumber", "1");
                    SetCookie("RightsPageNumber", "1");
                    SetCookie("AdminsPageNumber", "1");
                    SetCookie("MessagesPageNumber", "1");
                    SetCookie("NewMeetingsPageNumber", "1");
                    SetCookie("OldMeetingsPageNumber", "1");
                    SetCookie("NotificationsPageNumber", pageid.ToString());
                    break;
            }

        }


        #endregion

    }
}