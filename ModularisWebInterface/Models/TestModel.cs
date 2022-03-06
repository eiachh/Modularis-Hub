using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ModularisWebInterface.Models
{
    public class TestModel
    {
        [Key]
        public int Id { get; set; }
        public string BotName { get; set; } = "random ass name?";
        public IEnumerable<string> BotList { get; set; } = new List<string> { "bot1", "bot2" };
    }
}
