using System;
using System.Collections.Generic;

namespace nl.ketenstandaard.api.models
{
    /// <summary>
    /// Result of the XML Validation
    /// </summary>
    public class  ValidationResult
    {
        /// <summary>
        /// .ctor of the ValidationResult class
        /// </summary>
        public ValidationResult()
        {
            messages = new List<Message>();
        }

        /// <summary>
        /// DocumentType what has been found
        /// </summary>
        public string documentType { get; set; }


        /// <summary>
        /// Format for the Ketenstandaard
        /// </summary>
        public string format { get; set; }

        /// <summary>
        /// version of the ketenstandaard payload
        /// </summary>
        public string version { get; set; }

        /// <summary>
        /// messagetype of the payload 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// Total number of errors found within the supplied payload
        /// </summary>
        public int totalErrorsFound { get; set; }

        /// <summary>
        /// Total number of warnings found within the supplied payload
        /// </summary>
        public int totalWarningsFound { get; set; }

        /// <summary>
        /// List of Error & Warning messages
        /// </summary>
        ///  <seealso cref="nl.ketenstandaard.api.models.Message">
        /// Take a look at the Message Class
        /// </seealso>
        public List<Message> messages { get; set; }



        /// <summary>
        /// Format for the Ketenstandaard
        /// </summary>
        public string ExpectedFormat { get; set; }

        /// <summary>
        /// version of the ketenstandaard payload
        /// </summary>
        public string ExpectedVersion { get; set; }

        /// <summary>
        /// messagetype of the payload 
        /// </summary>
        public string ExpectedType { get; set; }


        /// <summary>
        /// Check input & output parameters
        /// </summary>
        public bool IsValid => ValidateResult();

        private bool ValidateResult()
        {
            bool isvalid = true;


            if (totalErrorsFound > 0)
            {
                isvalid = false;
                return isvalid;
            }

            
            bool formatvalid = format.Equals(ExpectedFormat, StringComparison.InvariantCultureIgnoreCase);
            bool typevalid = type.Equals(ExpectedType, StringComparison.InvariantCultureIgnoreCase);
            bool versionvalid = version.Equals(ExpectedVersion, StringComparison.InvariantCultureIgnoreCase);


            if (!formatvalid)
            {

                AddErrorMessage(string.Format("Format {0} is not the Expected Format {1}", format, ExpectedFormat));
                isvalid = false;
            }

            if (!typevalid)
            {

                AddErrorMessage(string.Format("Type {0} is not the Expected Type {1}", type, ExpectedType));
                isvalid = false;
            }

            if (!versionvalid)
            {

                AddErrorMessage(string.Format("Version {0} is not the Expected Version {1}", version, ExpectedVersion));
                isvalid = false;
            }
            if (totalErrorsFound > 0)
            {
                isvalid = false;
            }

            return isvalid;

        }

        private void AddErrorMessage(string msg)
        {
            messages.Add(new Message() { businessRule = "Message Service Input ", message = msg, severity = "Error" });
            totalErrorsFound += 1;
        }
    }
}
