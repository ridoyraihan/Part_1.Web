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
        private CustomerRepo _customerRepo;

        // GET: Bill
        public ActionResult Index()
        {
            _customerRepo = new CustomerRepo();
            List<Customer> customers = _customerRepo.GetCustomerListByAll();

            ViewBag.Customers = customers;

            return View();
        }

        [HttpPost]
        public ActionResult Index(Bill bill)
        {
            _customerRepo = new CustomerRepo();
            _billRepo = new BillRepo();

            ViewBag.Customers = _customerRepo.GetCustomerListByAll();
            long id = _billRepo.SaveBill(bill);

            if (id > 0) return RedirectToAction("OutstandingBill");

            return View(bill);
        }
        public ActionResult OutstandingBill()
        {
            _billRepo = new BillRepo();
            List<Bill> bills = _billRepo.GetOutstandingBillListByAll();

            return View(bills);
        }
        public ActionResult MarkAsPaid(int id)
        {
            _billRepo = new BillRepo();
            long billId = _billRepo.MarkBillAsPaid(id);

            return RedirectToAction("OutstandingBill");
        }

        public ActionResult AllBill()
        {
            _billRepo = new BillRepo();
            List<Bill> bills = _billRepo.GetBillListByAll();

            return View(bills);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Message = "";

            _billRepo = new BillRepo();
            Bill bill = _billRepo.GetBillById(id);

            return View(bill);
        }

        [HttpPost]
        public ActionResult Edit(Bill bill)
        {
            if (bill.BillAmount < (bill.PaidAmount + bill.Pay))
            {
                ViewBag.Message = "Paid amount is more than bill amount.";

                return View(bill);
            }
            else
            {
                ViewBag.Message = "";

                _billRepo = new BillRepo();

                float totalPaid = bill.PaidAmount + bill.Pay;
                bill.PaidAmount = bill.Pay;

                long id = _billRepo.SaveBill(bill);
                if (id > 0)
                {
                    if (totalPaid < bill.BillAmount) return RedirectToAction("OutstandingBill");
                    return RedirectToAction("AllBill");
                }

                return View(bill);
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
            _customerRepo = new CustomerRepo();
            List<Customer> customers = _customerRepo.GetCustomersWithOutstandingAmount(amountToMatch);
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