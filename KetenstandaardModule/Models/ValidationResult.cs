using System.Collections.Generic;

namespace KetenstandaardModule.Models
{
    public class ValidationResult
    {
        public string documentType { get; set; }
        public int totalErrorsFound { get; set; }
        public int totalWarningsFound { get; set; }
        public List<Message> messages { get; set; }
    }
}
