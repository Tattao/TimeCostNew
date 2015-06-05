using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeCost
{
    public class Voucher
    {
        public double Money { get; set; }
        public short Type { get; set; }
        public string Desc { get; set; }
        public DateTime DT { get; set; }
        public Guid ID { get; set; }
        public byte[] Picture { get; set; }
        public int PictureHeight { get; set; }
        public int PictureWidth { get; set; }
        public string Category { get; set; }
    }
}
