using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Exceptions.UserExceptions
{
    public class UserRefreshNotFoundExceptions(string UserId) : NotfoundException($"User With User {UserId} is Not Found!!")
    {

    }
}
