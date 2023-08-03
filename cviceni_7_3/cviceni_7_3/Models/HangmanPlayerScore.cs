using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace cviceni_7_3
{
    public class HangmanPlayerScore
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Player { get; set; }
        public int Misses { get; set; }

        public HangmanPlayerScore(string nick, int misses)
        {
            Player = nick;
            Misses = misses;
        }

        public HangmanPlayerScore() { }
    }
}
