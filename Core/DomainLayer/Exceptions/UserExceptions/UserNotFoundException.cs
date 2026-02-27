using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Exceptions.UserExceptions
{
    public class UserNotFoundException(string email) : NotfoundException($"User With Email {email} is Not Found!!")
    {
    }
}
