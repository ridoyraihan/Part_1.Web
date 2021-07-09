using Part_1.Model;
using Part_1.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Part_1.Web.Controllers
{
    public class BillController : Controller
    {
        private BillRepo _billRepo;

        public BillController()
        {            
            _billRepo = new BillRepo();
        }
        // GET: Bill
        public ActionResult Index()
        {
            List<Customer> customers = _billRepo.GetCustomers();
            ViewBag.Customers = customers;            
            return View();
        }

        [HttpPost]
        public ActionResult Index(Bill bill)
        {
            List<Customer> customers = new List<Customer>()
            {
                new Customer() { Id = 1, Name = "Raihan Riody" },
                new Customer() { Id = 2, Name = "Mukhlaser Rahman"}
            };
            ViewBag.Customers = customers;
            long id = _billRepo.SaveBill(bill);
            if(id > 0)
            {
                return RedirectToAction("OutstandingBill");
            }
            return View();
        }
        public ActionResult OutstandingBill()
        {
            List<Bill> bills = _billRepo.GetOutstandingBillListByAll();
            ViewBag.Bills = bills;
            return View(bills);
        }
        public ActionResult MarkAsPaid(int id)
        {
            long billId = _billRepo.MarkBillAsPaid(id);
            return RedirectToAction("OutstandingBill");
        }

        public ActionResult AllBill()
        {
            List<Bill> bills = _billRepo.GetBillListByAll();
            return View(bills);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Message = "";
            Bill bill = _billRepo.GetBillById(id);
            return View(bill);
        }

        [HttpPost]
        public ActionResult Edit(Bill bill)
        {
            if(bill.BillAmount < (bill.PaidAmount + bill.Pay))
            {
                ViewBag.Message = "Paid amount is more then bill amount.";
                return View(bill);
            }
            else
            {
                ViewBag.Message = "";
                bill.PaidAmount = bill.Pay;
                long id = _billRepo.SaveBill(bill);
                if (id > 0)
                {
                    return RedirectToAction("OutstandingBill");
                }
                return View();
            }            
        }

        public ActionResult Search()
        {
            ViewBag.Message = "";
            return View();
        }

        [HttpPost]
        public ActionResult Search(float amountToMatch)
        {
            List<Customer> customers = _billRepo.GetCustomersWithOutstandingAmount(amountToMatch);            
            if (customers.Count > 0)
            {
                ViewBag.Customers = customers;
                return View();
            }
            else
            {
                ViewBag.Message = "No customer found having outstanding bill: " + amountToMatch;
            }
            return View();
        }
    }
}