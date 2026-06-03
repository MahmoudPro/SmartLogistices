using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLogistics.Application.Drivers.Queries
{
    public sealed record DriverDto(
        Guid Id,
        string FullName,
        string LicenseNumber,
        string PhoneNumber,
        int RewardPoints
    );
}
