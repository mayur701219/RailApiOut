using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rail.ApiOut.CommonFunctions
{
    [Table("ApiOutLogs",Schema ="Rail")]
    public class ApiOutLogs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }    
        public string Request { get; set; }
        public string Response { get; set; }
        public string Url { get; set; }
        public DateTime CreatedDate { get; set; }
        public string correlationId { get; set; }
        public string Stage { get; set; }
    }
}
