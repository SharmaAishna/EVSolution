
using ChargingProfileGenerator.App;
using ChargingProfileGenerator.Domain;
using ChargingProfileGenerator.Domain.Helper;
using ChargingProfileGenerator.Domain.OutputViewModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ChargingProfileGenerator.Tests
{
    [TestFixture]
    public class GeneratorChargingProfileTests
    {
        [Test]
        public void GenerateChargingProfile_SpecificTimeIntervals_ReturnsExpectedChargingSchedule()
        {
            // Arrange
            DateTime startingTime = new DateTime(2020, 7, 1, 0, 0, 0); // Assuming starting time is midnight on July 1, 2020
            UserSettings userSettings = new UserSettings
            {
                DesiredStateOfCharge = 80, // Desired state of charge is 80%
                LeavingTime = new TimeSpan(7, 0, 0), // User leaves at 7:00 AM
                DirectChargingPercentage = 20, // Direct charging threshold is 20%
                Tariffs = new List<Tariff>
        {
            new Tariff { StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(18, 11, 59), EnergyPrice = 0.22m }, // Low tariff until 6:11:59 PM
            new Tariff { StartTime = new TimeSpan(18, 12, 0), EndTime = new TimeSpan(20, 16, 59), EnergyPrice = 0.25m }, // High tariff until 8:16:59 PM
            new Tariff { StartTime = new TimeSpan(20, 17, 0), EndTime = new TimeSpan(23, 32, 59), EnergyPrice = 0.22m }, // Low tariff until 11:32:59 PM
            new Tariff { StartTime = new TimeSpan(23, 33, 0), EndTime = new TimeSpan(23, 59, 59), EnergyPrice = 0.22m }, // Low tariff until midnight
            new Tariff { StartTime = new TimeSpan(0, 0, 0), EndTime = new TimeSpan(5, 21, 59), EnergyPrice = 0.22m } // Low tariff until 5:21:59 AM
        }
            };
            CarData carData = new CarData
            {
                ChargePower = 9.6m, // Charge power is 9.6 kW
                BatteryCapacity = 55, // Battery capacity is 55 kWh
                CurrentBatteryLevel = 30 // Current battery level is 30%
            };
            GeneratorChargingProfile generator = new GeneratorChargingProfile();

            // Act
            List<ChargingSchedule> chargingSchedule = generator.GenerateChargingProfile(startingTime, userSettings, carData);

            // Configure JsonSerializerOptions
            var options = new JsonSerializerOptions();
            options.Converters.Add(new DecimalConverter());
            options.Converters.Add(new DateTimeConverter());
            options.Converters.Add(new TimeSpanConverter());
            options.WriteIndented = true;

            // Assert
            Assert.IsNotNull(chargingSchedule);
            Assert.AreEqual(4, chargingSchedule.Count); // Assuming each iteration charges for one hour

            // Asserting the specific charging schedule entries
            Assert.IsTrue(chargingSchedule[0].IsCharging); // 07-01-2020 18:12:00 to 07-01-2020 20:17:00 should be charging
            Assert.IsFalse(chargingSchedule[1].IsCharging); // 07-01-2020 20:17:00 to 07-01-2020 23:33:00 should not be charging
            Assert.IsTrue(chargingSchedule[2].IsCharging); // 07-01-2020 23:33:00 to 08-01-2020 05:22:00 should be charging
            Assert.IsFalse(chargingSchedule[3].IsCharging); // 08-01-2020 05:22:00 to 08-01-2020 07:00:00 should not be charging
        }

    }
}

