using System;
using System.Collections.Generic;

namespace ServerSideDataTable.Models
{
    public partial class MockDatum
    {
        public int? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public string? IpAddress { get; set; }
    }
}
