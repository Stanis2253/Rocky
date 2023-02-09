using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rocky_DataAccess.Repository.IRepository;
using Rocky_Models;
using Rocky_Models.ViewModels;
using Rocky_Utility;
using System.Data;

namespace Rocky.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class InquiryController : Controller
    {
        private readonly IInquiryHeaderRepository _inqHRepo;

        private readonly IInquiryDetailRepository _inqDRepo;

        [BindProperty]
        public InquiryVM InquiryVM { get; set; }

        public InquiryController(IInquiryHeaderRepository inqHRepo, IInquiryDetailRepository iqDRepo)
        {
            _inqHRepo = inqHRepo;
            _inqDRepo = iqDRepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetInquiryList() 
        {
            return Json(new { data = _inqHRepo.GetAll() });
        }
        #endregion
        public IActionResult Details(int Id)
        {
            InquiryVM = new InquiryVM()
            {
                inquiryHeader = _inqHRepo.FirstOrDefault(u=>u.Id == Id),   
                inquiryDetail= _inqDRepo.GetAll(u => u.InquiryHeaderId == Id, includeProperties: "Product"),
            };
            return View(InquiryVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details()
        {
            List<ShoppingCart> shoppingCartList= new List<ShoppingCart>();
            InquiryVM.inquiryDetail= _inqDRepo.GetAll(u=> u.InquiryHeaderId == InquiryVM.inquiryHeader.Id);

            foreach (var detail in InquiryVM.inquiryDetail)
            {
                ShoppingCart shoppingCart = new ShoppingCart()
                {
                    ProductId = detail.ProductId,
                };
                shoppingCartList.Add(shoppingCart);
            }
            HttpContext.Session.Clear();
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            HttpContext.Session.Set(WC.SessionInquiryId, InquiryVM.inquiryHeader.Id);


            return RedirectToAction("Index", "Cart");
        }
        [HttpPost]
        public IActionResult Delete()
        {
            InquiryHeader inquiryHeader = _inqHRepo.FirstOrDefault(u => u.Id == InquiryVM.inquiryHeader.Id);
            IEnumerable<InquiryDetail> inquiryDetails = _inqDRepo.GetAll(u => u.InquiryHeaderId == InquiryVM.inquiryHeader.Id);

            _inqDRepo.RemoveRange(inquiryDetails);
            _inqHRepo.Remove(inquiryHeader);
            _inqDRepo.Save();

            return RedirectToAction(nameof(Index));
        }

    }

}
