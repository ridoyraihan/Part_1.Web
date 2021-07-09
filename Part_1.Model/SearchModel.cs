using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Part_1.Model
{
    public class SearchModel
    {
        [Display(Name ="Amount to match")]
        public float AmountToMatch { get; set; }
    }
}
