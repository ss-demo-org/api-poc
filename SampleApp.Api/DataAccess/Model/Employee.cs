using System;
using System.Collections.Generic;

namespace EF.Model;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? AddressLine { get; set; }

    public string? City { get; set; }
}
