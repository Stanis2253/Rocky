using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocky_Models.ViewModels
{
    public class InquiryVM
    {
        public InquiryHeader inquiryHeader { get; set; }

        public IEnumerable<InquiryDetail> inquiryDetail { get; set; }
    }
}
