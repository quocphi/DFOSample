using Dfo.Sample.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfo.Sample.DummyData
{
    public static class UsersData
    {
        static UsersData()
        {
            UsersDataInMem = new List<UserDto>();
        }
        public static List<UserDto> UsersDataInMem { get; set; }
    }
}
