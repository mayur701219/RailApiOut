using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.ApiOutModels
{
    [Table("SearchHistory", Schema = "Rail")]
    public class SearchHistoryModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string? SearchId { get; set; }
        public string? CorrelationId { get; set; }
        public string? Type { get; set; }
        public string? Response { get; set; }
        public long AgentId { get; set; }
        //public DateTime? CreatedDate { get; set; }
    }
}
