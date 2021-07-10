using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Part_1.Model;
using Part_1.Dal;

namespace Part_1.Repo
{
    public class BillRepo
    {
        BillItem billItem;
        BillList billList;
        CustomerRepo _customerRepo;
        public BillRepo()
        {
            _customerRepo = new CustomerRepo();
        }

        public List<Bill> GetBillListByAll()
        {
            List<Bill> bList = new List<Bill>();

            billList = new BillList();
            billList.Load(BillList.LoadOption.LoadAll);

            List<Customer> customers = _customerRepo.GetCustomerListByAll();

            foreach (BillItem item in billList)
            {
                Bill bill = this.MapBillItemToBill(item, customers);
                bList.Add(bill);
            }

            return bList;
        }

        private Bill MapBillItemToBill(BillItem item, List<Customer> customers)
        {
            Bill bill = new Bill();

            bill.BillAmount = item.BillAmount;
            bill.BillDate = item.BillDate;
            bill.CustomerId = (int)item.CustomerId;
            bill.Id = (int)item.Id;
            bill.PaidAmount = item.PaidAmount;
            bill.PaidDate = item.PaidDate;

            Customer customer = customers.FirstOrDefault(c => c.Id == bill.CustomerId);

            bill.CustomerName = customer.Name;

            return bill;
        }

        public Bill GetBillById(int billId)
        {
            Bill bill = new Bill();

            billItem = new BillItem();
            billItem.Id = billId;
            billItem.Load(BillItem.LoadOption.LoadById);

            List<Customer> customers = _customerRepo.GetCustomerListByAll();

            bill = this.MapBillItemToBill(billItem, customers);

            return bill;
        }

        public List<Bill> GetOutstandingBillListByAll()
        {
            List<Bill> bList = new List<Bill>();

            billList = new BillList();
            billList.Load(BillList.LoadOption.LoadByOutstandingBill);

            List<Customer> customers = _customerRepo.GetCustomerListByAll();

            foreach (BillItem item in billList)
            {
                Bill bill = this.MapBillItemToBill(item, customers);
                bList.Add(bill);
            }

            return bList;
        }

        public long MarkBillAsPaid(int id)
        {
            billItem = new BillItem();
            billItem.Id = id;
            billItem.Save(BillItem.SaveOption.MarkAsPaid);

            long billId = billItem.Id;

            return billId;
        }

        public long SaveBill(Bill bill)
        {
            billItem = new BillItem();
            billItem = this.MapBillToBillItem(bill);

            if (bill.Id == 0)
            {
                // When the id is -1 there will be an insert operation in Database
                // Otherwise it will execute update statements
                billItem.Id = -1;
            }

            billItem.Save(BillItem.SaveOption.SaveRow);
            long id = billItem.Id;

            return id;
        }

        private BillItem MapBillToBillItem(Bill bill)
        {
            BillItem billItem = new BillItem();

            billItem.BillAmount = bill.BillAmount;
            billItem.BillDate = bill.BillDate;
            billItem.CustomerId = bill.CustomerId;
            billItem.Id = bill.Id;
            billItem.PaidAmount = bill.PaidAmount;
            billItem.PaidDate = bill.PaidDate;

            return billItem;
        }
    }
}
