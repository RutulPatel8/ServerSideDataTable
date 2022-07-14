using Microsoft.AspNetCore.Mvc;
using ServerSideDataTable.Models;
using System.Diagnostics;

namespace ServerSideDataTable.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TestDBContext context;

        public HomeController(ILogger<HomeController> logger, TestDBContext context)
        {
            this.context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult GetCustomers()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var customerData = (from tempcustomer in context.MockData select tempcustomer);
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    customerData = customerData.OrderBy(x => x.Id);
                }
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    customerData = customerData.Where(m => m.FirstName.Contains(searchValue)
                //                                || m.LastName.Contains(searchValue)
                //                                || m.Contact.Contains(searchValue)
                //                                || m.Email.Contains(searchValue));
                //}
                recordsTotal = customerData.Count();
                var data = customerData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[HttpPost]
        //public ActionResult LoadData()
        //{

        //    var draw = Request.Form.GetValues("draw").FirstOrDefault();
        //    var start = Request.Form.GetValues("start").FirstOrDefault();
        //    var length = Request.Form.GetValues("length").FirstOrDefault();
        //    //Find Order Column
        //    var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
        //    var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
        //    var searchText = Request.Form.GetValues("search[value]").FirstOrDefault();

        //    int pageSize = length != null ? Convert.ToInt32(length) : 0;
        //    int skip = start != null ? Convert.ToInt32(start) : 0;
        //    int recordsTotal = 0;
        //    //using (PMAgroDbContext dc = new PMAgroDbContext())
        //    //{

        //    PMAgroDbContext tempDataContext = orderViewModel.MasterDataContext();
        //    var v = from a in tempDataContext.MasterSales select a;


        //    //SORT
        //    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
        //    {
        //        v = v.OrderByDescending(x => x.Id);
        //    }

        //    //FilterData

        //    var minDate = Request.Form.GetValues("minDate").FirstOrDefault();
        //    var maxDate = Request.Form.GetValues("maxDate").FirstOrDefault();
        //    var userId = Request.Form.GetValues("userId").FirstOrDefault();
        //    var statusId = Request.Form.GetValues("statusId").FirstOrDefault();
        //    var districtId = Request.Form.GetValues("districtId").FirstOrDefault();
        //    var talukaId = Request.Form.GetValues("talukaId").FirstOrDefault();

        //    if (minDate != "" && maxDate != "")
        //    {
        //        //v.Where(p => p.CreatedDate > Convert.ToDateTime(minDate) && p.CreatedDate < Convert.ToDateTime(maxDate[0]));
        //        DateTime min = Convert.ToDateTime(minDate);
        //        DateTime max = Convert.ToDateTime(maxDate);
        //        v = from a in v where a.CreatedDate > min && a.CreatedDate < max select a;
        //    }

        //    if (userId != "")
        //    {
        //        int userIDINT = Convert.ToInt32(userId);
        //        v = from a in v where a.UserId == userIDINT select a;
        //    }

        //    if (statusId != "")
        //    {
        //        int statusIdINT = Convert.ToInt32(statusId);
        //        v = from a in v where a.OrderStatusId.Value == statusIdINT select a;
        //    }
        //    if (districtId != "")
        //    {
        //        int districtIdINT = Convert.ToInt32(districtId);
        //        v = from a in v where a.MasterCustomer.MasterDistrictId.Value == districtIdINT select a;
        //    }
        //    if (talukaId != "" && Convert.ToInt32(talukaId) != 0)
        //    {
        //        int talukaIdINT = Convert.ToInt32(talukaId);
        //        v = from a in v where a.MasterCustomer.MasterTalukaId.Value == talukaIdINT select a;
        //    }

        //    recordsTotal = v.Count();
        //    var data = v.Skip(skip).Take(pageSize).ToList().Select(x => new { Id = x.Id, OrderCode = x.OrderCode, Date = x.GetCreatedDate, CustomerName = x.MasterCustomer.FullName, District = x.MasterCustomer.MasterDistrict.DistrictName, Taluka = x.GetCustomerTaluka, TotalAmountOfOrder = x.TotalAmountOfOrder, UserName = x.GetUserName, status = x.GetEnumStatus, checkbox = false });
        //    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
        //    //}
        //}
    }
}