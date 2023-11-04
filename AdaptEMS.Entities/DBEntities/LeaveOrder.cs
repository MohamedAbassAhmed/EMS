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
    public class LeaveOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public Employee? Employee { get; set; }
        public DateTime RequestedLeaveDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        [MaxLength(256)]
        public string Comment { get; set; }
        [MaxLength(10)]
        public string Status { get; set; } = Consts.NewLeaveOrder;
    }
}
