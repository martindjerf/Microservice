using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Service.Models
{
    public class Member
    {
        public Guid ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Member()
        {

        }

        public Member(Guid id) : this ()
        {
            ID = id;
        }

        public Member(string firstname, string lastname, Guid id) : this(id)
        {
            FirstName = firstname;
            LastName = lastname;
        }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
}
