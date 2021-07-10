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
        public BillRepo()
        {

        }

        public List<Bill> GetBillListByAll()
        {
            List<Bill> bList = new List<Bill>();

            billList = new BillList();
            billList.Load(BillList.LoadOption.LoadAll);

            foreach (BillItem item in billList)
            {
                Bill bill = this.MapBillItemToBill(item);
                bList.Add(bill);
            }

            return bList;
        }

        public List<Customer> GetCustomers()
        {
            List<Customer> customers = new List<Customer>()
            {
                new Customer() { Id = 1, Name = "Raihan Riody" },
                new Customer() { Id = 2, Name = "Mukhlaser Rahman"},
                new Customer() { Id = 3, Name = "Mr. Bakri Badawi"},
                new Customer() { Id = 4, Name = "Laxus Chin"},
                new Customer() { Id = 5, Name = "Forest Interactive"}
            };

            return customers;
        }

        private Bill MapBillItemToBill(BillItem item)
        {
            Bill bill = new Bill();

            bill.BillAmount = item.BillAmount;
            bill.BillDate = item.BillDate;
            bill.CustomerId = (int)item.CustomerId;
            bill.Id = (int)item.Id;
            bill.PaidAmount = item.PaidAmount;
            bill.PaidDate = item.PaidDate;

            Customer customer = this.GetCustomers().FirstOrDefault(c => c.Id == bill.CustomerId);

            bill.CustomerName = customer.Name;

            return bill;
        }

        public Bill GetBillById(int billId)
        {
            Bill bill = new Bill();

            billItem = new BillItem();
            billItem.Id = billId;
            billItem.Load(BillItem.LoadOption.LoadById);

            bill = this.MapBillItemToBill(billItem);

            return bill;
        }

        public List<Bill> GetOutstandingBillListByAll()
        {
            List<Bill> bList = new List<Bill>();

            billList = new BillList();
            billList.Load(BillList.LoadOption.LoadByOutstandingBill);

            foreach (BillItem item in billList)
            {
                Bill bill = this.MapBillItemToBill(item);
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

        public List<Customer> GetCustomersWithOutstandingAmount(float amountToMatch)
        {
            List<Customer> possiblePayors = new List<Customer>();

            List<Bill> outstandingBills = this.GetOutstandingBillListByAll();
            IEnumerable<int> possiblePayorsId = this.GetPossibleCustomerIdsForOutstandingAmount(outstandingBills, amountToMatch);            

            if (possiblePayorsId.ToList().Count > 0)
            {
                List<Customer> customers = this.GetCustomers();
                foreach (int customerId in possiblePayorsId)
                {
                    Customer customer = new Customer();
                    customer = customers.Find(c => c.Id == customerId);
                    possiblePayors.Add(customer);
                }
            }

            return possiblePayors;
        }

        private IEnumerable<int> GetPossibleCustomerIdsForOutstandingAmount(List<Bill> outstandingBills, float amountToMatch)
        {
            outstandingBills = outstandingBills.FindAll(bill => bill.BillAmount - bill.PaidAmount == amountToMatch);
            IEnumerable<int> possiblePayorsId = outstandingBills.Select(bill => bill.CustomerId).Distinct();

            return possiblePayorsId;
        }
    }
}
