using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Maoi_lab2_zayicev.Models
{
    class EuclidLetterModel
    {

        [Key]
        public int Id { get; set; }
        public char Letter { get; set; }

        public int Horizontal { get; set; }
        public int Vertical { get; set; }
        public double R { get; set; }

    }
}
