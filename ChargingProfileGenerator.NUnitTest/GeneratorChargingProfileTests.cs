using ChargingProfileGenerator.App;
using ChargingProfileGenerator.Domain.OutputViewModel;
using ChargingProfileGenerator.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;

[TestFixture]
public class GeneratorChargingProfileTests
{
    private GeneratorChargingProfile generator;

    [SetUp]
    public void Setup()
    {
        generator = new GeneratorChargingProfile();
    }

    [Test]
    public void GenerateChargingProfile_CarNeedsToCharge_ReturnsChargingSchedule()
    {
        // Arrange
        DateTime startingTime = new DateTime(2024, 04, 24, 18, 12, 0);
        UserSettings userSettings = new UserSettings
        {
            DesiredStateOfCharge = 80, // Desired state of charge is 80%
            LeavingTime = new TimeSpan(18, 0, 0), // User leaves at 7:00 AM
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
        // Act
        List<ChargingSchedule> chargingSchedule = generator.GenerateChargingProfile(startingTime, userSettings, carData);

        // Assert
        Assert.That(chargingSchedule.Count, Is.EqualTo(1));
        Assert.That(chargingSchedule[0].IsCharging, Is.True);
        Assert.That(chargingSchedule[0].StartTime, Is.EqualTo(new DateTime(2024, 04, 24, 00, 00, 00)));
       
    }

   
}
