using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KetenstandaardModule.Models
{
    public class Message
    {
        public string businessRule { get; set; }
        public string xPath { get; set; }
        public int lineNumber { get; set; }
        public int linePosition { get; set; }
        public string message { get; set; }
        public string severity { get; set; }
    }
}
