using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Adjustments.Domain
{
    public class TimesheetView
    {
        public string EEId { get; set; }

        public string Fullname { get; set; } = "";

        public string CutoffId { get; set; }
        public string RawPCV { get; set; }
    }
}
