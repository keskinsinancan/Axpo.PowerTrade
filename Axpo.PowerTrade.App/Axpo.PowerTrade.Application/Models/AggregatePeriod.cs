using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axpo.PowerTrading.Application.Models
{
    public class AggregatePeriod
    {
        public int Period { get; set; }
        public DateTime DateTimePeriod { get; set; }
        public double Volume { get; set; }
    }
}
