using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Part_1.Model
{
    public class Bill
    {
        public int Id { get; set; }

        [Display(Name ="Customer")]
        [Required(ErrorMessage = "required")]
        public int CustomerId { get; set; }

        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Display(Name = "Bill Created Date")]
        public DateTime BillDate { get; set; }

        [Display(Name = "Bill Amount")]
        [Required(ErrorMessage = "required")]
        public float BillAmount { get; set; }

        [Display(Name = "Paid Amount")]
        public float PaidAmount { get; set; }

        [Display(Name = "Bill Paid Date")]
        public DateTime? PaidDate { get; set; }
        public float Pay { get; set; }
    }
}
