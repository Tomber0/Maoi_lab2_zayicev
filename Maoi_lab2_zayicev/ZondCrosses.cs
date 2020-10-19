using System;
using System.Collections.Generic;
using System.Text;

namespace Maoi_lab2_zayicev
{
    public class ZondCrosses
    {
        public int VerticalCrosses { get; set; }
        public int HorisontalCrosses { get; set; }
        public double Energy { get; set; }
        public double Homogen { get; set; }
        public double Correlation { get; set; }
        public string Letter { get; set; }
        public ZondCrosses(int x, int y)
        {
            VerticalCrosses = x;
            HorisontalCrosses = y;
        }
    }
}
