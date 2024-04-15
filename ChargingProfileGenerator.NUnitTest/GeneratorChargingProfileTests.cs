using ChargingProfileGenerator.App;
using ChargingProfileGenerator.Domain;
using ChargingProfileGenerator.Domain.OutputViewModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;

[TestFixture]
public class GeneratorChargingProfileTests
{
    [Test]
    public void TestGenerateChargingProfile()
    {
        // Arrange
        DateTime startingTime = new DateTime(2020, 07, 01, 17, 00, 00); // 01-07-2020 17:00:00
        UserSettings userSettings = new UserSettings
        {
            LeavingTime = new TimeSpan(18, 00, 00), // 18:00:00
            DesiredStateOfCharge = 80 // 80%
        };
        CarData carData = new CarData
        {
            CurrentBatteryLevel = 40, // 40%
            BatteryCapacity = 100, // 100 kWh
            ChargePower = 10 // 10 kW
        };
        List<Tariff> tariffs = new List<Tariff>
        {
            new Tariff { StartTime = new DateTime(2024, 07, 01, 18, 00, 00), EndTime = new DateTime(2024, 07, 01, 20, 17, 00), EnergyPrice = 0.15m },
            new Tariff { StartTime = new DateTime(2024, 07, 01, 20, 17, 00), EndTime = new DateTime(2024, 07, 01, 23, 33, 00), EnergyPrice = 0.20m },
            new Tariff { StartTime = new DateTime(2024, 07, 01, 23, 33, 00), EndTime = new DateTime(2024, 07, 02, 05, 22, 00), EnergyPrice = 0.10m },
            new Tariff { StartTime = new DateTime(2024, 07, 02, 05, 22, 00), EndTime = new DateTime(2024, 07, 02, 07, 00, 00), EnergyPrice = 0.25m }
        };
        GeneratorChargingProfile generator = new GeneratorChargingProfile();

        // Act
        List<ChargingSchedule> chargingSchedule = generator.GenerateChargingProfile(startingTime, userSettings, carData);

        // Assert
        Assert.That(chargingSchedule.Count, Is.EqualTo(4));

        Assert.Multiple(() =>
        {
            Assert.That(chargingSchedule[0].StartTime, Is.EqualTo(new DateTime(2024, 07, 01, 18, 00, 00)));
            Assert.That(chargingSchedule[0].EndTime, Is.EqualTo(new DateTime(2024, 07, 01, 20, 17, 00)));
        });
        Assert.That(chargingSchedule[0].IsCharging, Is.True);

        Assert.Multiple(() =>
        {
            Assert.That(chargingSchedule[1].StartTime, Is.EqualTo(new DateTime(2024, 07, 01, 20, 17, 00)));
            Assert.That(chargingSchedule[1].EndTime, Is.EqualTo(new DateTime(2024, 07, 01, 23, 33, 00)));
        });
        Assert.That(chargingSchedule[1].IsCharging, Is.False);

        Assert.Multiple(() =>
        {
            Assert.That(chargingSchedule[2].StartTime, Is.EqualTo(new DateTime(2024, 07, 01, 23, 33, 00)));
            Assert.That(chargingSchedule[2].EndTime, Is.EqualTo(new DateTime(2024, 07, 02, 05, 22, 00)));
        });
        Assert.That(chargingSchedule[2].IsCharging, Is.True);

        Assert.Multiple(() =>
        {
            Assert.That(chargingSchedule[3].StartTime, Is.EqualTo(new DateTime(2024, 07, 02, 05, 22, 00)));
            Assert.That(chargingSchedule[3].EndTime, Is.EqualTo(new DateTime(2024, 07, 02, 07, 00, 00)));
        });
        Assert.That(chargingSchedule[3].IsCharging, Is.False);
    }
}
