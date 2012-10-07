using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Resty.Net.Tests
{
    public class Person
    {
        public int Id { get; set; }
        public Guid UID { get; set; }
        public string Email { get; set; }
        public int NoOfSiblings { get; set; }
        public DateTime DOB { get; set; }
        public bool IsActive { get; set; }
        public decimal Salary { get; set; }
    }
}
