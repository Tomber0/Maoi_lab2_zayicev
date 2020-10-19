using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Maoi_lab2_zayicev.Models
{
    class StandartModel
    {

        [Key]
        public int Id { get; set; }
        public char Letter { get; set; }
        public double Horizontal { get; set; }
        public double Vertical { get; set; }

        public double R { get; set; }

    }
}
