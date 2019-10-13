using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Models.GroupModels
{
    public class GroupKeyUpdate
    {
        [Required]
        public int GroupId { get; set; }

        [StringLength(16, MinimumLength = 8)]
        public string CustomKey { get; set; }
    }
}
