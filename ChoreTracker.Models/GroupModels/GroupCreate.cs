using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Models.GroupModels
{
    public class GroupCreate
    {
        [Required]
        [Display(Name = "Group Name")]
        public string GroupName { get; set; }
    }
}
