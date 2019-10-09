﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Data.Entities
{
    public class GroupEntity
    {
        [Key]
        public int GroupId { get; set; }

        [Required]
        public string GroupName { get; set; }

        [Required]
        public Guid OwnerId { get; set; }

        [Required]
        public string GroupInviteCode { get; set; }

        public DateTimeOffset DateFounded { get; set; }

        public virtual ICollection<GroupMemberEntity> GroupMembers { get; set; }
    }
}
