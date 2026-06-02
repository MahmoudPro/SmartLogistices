using SmartLogistics.Domain.Common;
using SmartLogistics.Domain.Enums;
using SmartLogistics.Domain.Events;

namespace SmartLogistics.Domain.Entities
{
    public sealed class Driver: AuditableEntity
    {
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public string FullName => $"{FirstName} {LastName}".Trim();

        public string? LicenseNumber { get; private set; }
        public DateTime LicenseExpirationDate { get; private set; }
        public DriverStatus Status { get; private set; } = DriverStatus.Active;
        public bool IsAvailable { get; private set; } = true;
        public string? Nationality { get; private set; }
        public string? NationalID { get; private set; }       // nullable ✓
        public string? PassportNumber { get; private set; }   // nullable ✓
        public int RewardPoints { get; private set; }
        public string? SuspensionReason { get; private set; }

        private readonly List<Shipment> _shipments = new List<Shipment>();
        public IReadOnlyCollection<Shipment> Shipments => _shipments.AsReadOnly();

        private Driver() { }

        public static Driver Create(
            string firstName, string lastName,
            string licenseNumber, DateTime licenseExpiry,
            string? nationality = null, string? nationalID = null, string? passportNumber = null)
        {
            var driver = new Driver
            {
                FirstName = firstName,
                LastName = lastName,
                LicenseNumber = licenseNumber,
                LicenseExpirationDate = licenseExpiry,
                Nationality = nationality,
                NationalID = nationalID,
                PassportNumber = passportNumber
            };
            return driver;
        }

        public bool IsLicenseValid() => LicenseExpirationDate > DateTime.UtcNow;

        public void MakeAvailable()
        {
            if (Status == DriverStatus.Suspended)
                throw new InvalidOperationException("Suspended drivers cannot be made available.");
            IsAvailable = true;
        }

        public void MakeUnavailable() => IsAvailable = false;
        
        public void Suspend(string reason)
        {
            if (Status == DriverStatus.Suspended)
                throw new InvalidOperationException("Driver is already suspended.");
            Status = DriverStatus.Suspended;
            SuspensionReason = reason;
            AddDomainEvent(new DriverSuspendedEvent(Id, reason));

        }

        public void AddRewardPoints(int points)
        {
                if (points <= 0)
                    throw new ArgumentException("Points must be positive.");
                RewardPoints += points;
        }
    }
}