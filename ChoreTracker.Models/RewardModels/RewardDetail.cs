using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Models.RewardModels
{
    public class RewardDetail
    {
        public int RewardId { get; set; }
        public string RewardName { get; set; }
        public int NumberAvailable { get; set; }
        public int GroupId { get; set; }
        public int Cost { get; set; }
    }
}
