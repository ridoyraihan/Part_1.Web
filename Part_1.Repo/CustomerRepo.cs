using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Part_1.Model;
using Part_1.Dal;

namespace Part_1.Repo
{
    public class CustomerRepo
    {        
        CustomerList customerList;
        BillRepo _billRepo;

        public CustomerRepo()
        {

        }

        public List<Customer> GetCustomerListByAll()
        {
            List<Customer> cList = new List<Customer>();

            customerList = new CustomerList();
            customerList.Load(CustomerList.LoadOption.LoadAll);

            foreach (CustomerItem item in customerList)
            {
                Customer customer = this.MapCustomerItemToCustomer(item);
                cList.Add(customer);
            }

            return cList;
        }

        private Customer MapCustomerItemToCustomer(CustomerItem item)
        {
            Customer customer = new Customer();
            customer.Id = (int)item.Id;
            customer.Name = item.Name;

            return customer;
        }

        public List<Customer> GetHardCodedCustomers()
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

        public List<Customer> GetCustomersWithOutstandingAmount(float amountToMatch)
        {           
            List<Customer> possiblePayors = new List<Customer>();

            _billRepo = new BillRepo();
            List<Bill> outstandingBills = _billRepo.GetOutstandingBillListByAll();
            IEnumerable<int> possiblePayorsId = this.GetPossibleCustomerIdsForOutstandingAmount(outstandingBills, amountToMatch);

            if (possiblePayorsId.ToList().Count > 0)
            {
                List<Customer> customers = this.GetCustomerListByAll();
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
