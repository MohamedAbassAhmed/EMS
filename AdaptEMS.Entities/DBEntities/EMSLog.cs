using AdaptEMS.Entities.SharedEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptEMS.Entities.DBEntities
{
    public class EMSLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
        public string? Message { get; set; }
        public string? InnerException { get; set; }
        public string? StackTrace { get; set; }
        public DateTime Time { get; set; }
        [MaxLength(10)]
        public string Category { get; set; } = Consts.DefaultLogCategory;
        public string Transaction { get; set; }

    }
}
