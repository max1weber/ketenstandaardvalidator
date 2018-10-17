using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nl.ketenstandaard.api.models
{


    public class Message
    {
        /// <summary>
        /// The businessrule violated of warned
        /// </summary>
        public string businessRule { get; set; }

        /// <summary>
        /// Xpath Epression to find the element in the supplies payload 
        /// </summary>
        public string xPath { get; set; }
        /// <summary>
        /// Linenumber where error/warning has been detected
        /// </summary>
        public int lineNumber { get; set; }

        /// <summary>
        /// Position within the line the error/warning has been detected
        /// </summary>
        public int linePosition { get; set; }

        /// <summary>
        /// Description/message of the error
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// Severitiy of the Error/Warning
        /// </summary>
        public string severity { get; set; }
    }
}
