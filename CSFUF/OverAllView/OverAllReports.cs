using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CSFUF.Models;
using System.Collections;

namespace CSFUF.OverAllView
{
    public class OverAllReports
    {
        public IEnumerable<CustomerReg> Registration { get; set; }
        public IEnumerable<ConReg> Contribution { get; set; }
        public IEnumerable<Decision> Decision { get; set; }
        public IEnumerable<DecisionExpertsTask> DecisionTasks { get; set; }
        public IEnumerable<ApproverTask> Approver { get; set; }
        public IEnumerable<Payment> Payment { get; set; }
       

    }
}