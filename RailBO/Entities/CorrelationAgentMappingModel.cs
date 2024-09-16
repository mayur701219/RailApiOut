using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO.Entities
{
    [Table("Eurail_agent_correlation_mapping", Schema = "Rail")]
    public class CorrelationAgentMappingModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string? CorrelationId { get; set; }
        public long AgentId { get; set; }
        public long RiyaUserId { get; set; }
    }
}
