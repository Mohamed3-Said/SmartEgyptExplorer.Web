using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Exceptions.FormException
{
    public class DataNotfoundException(int submissionId) : NotfoundException($"Submission with ID {submissionId} not found.")
    {
    }
}
