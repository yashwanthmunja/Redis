using System;
using System.Collections.Generic;

namespace Redis.Models;

public partial class Customer
{
    public int Customerid { get; set; }

    public string? Customername { get; set; }

    public string? Lastname { get; set; }

    public string? Country { get; set; }

    public int? Age { get; set; }

    public int? Phone { get; set; }
}
