using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO
{
    public class SearchPassesFormPost
    {
        public Place? place { get; set; }
        public int Adult { get; set; }
        public int Senior { get; set; }
        public int Youth { get; set; }
        public List<int>? YouthAge { get; set; }
        public string? validityStartDate { get; set; }
    }
}
