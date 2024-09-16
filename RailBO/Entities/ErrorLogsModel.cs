using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.Entities
{
    [Table("tblErrorLogs", Schema = "Rail")]
    public class ErrorLogsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string? Request { get; set; }
        public string? Response { get; set; }
        public string? Error { get; set; }
        public string? StackTrace { get; set; }
        public string? URL { get; set; }
        public string? AgentId { get; set; }
        public string? AgentDevice { get; set; }
        public string? StatusCode { get; set; }
        public string? Remark { get; set; }
        //public DateTime? CreatedOn { get; set; }

    }
}
