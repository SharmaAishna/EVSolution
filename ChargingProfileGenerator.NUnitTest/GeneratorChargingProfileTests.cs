
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
        public void TestGenerateChargingProfile_BatteryLow_CheapestTariff()
        {

            // Arrange
            GeneratorChargingProfile generator = new GeneratorChargingProfile();
            DateTime startingTime = new DateTime(2024, 04, 17, 18, 12, 00);
            UserSettings userSettings = new UserSettings
            {
                DesiredStateOfCharge = 80,
                LeavingTime = new TimeSpan(7, 00, 00), 
                DirectChargingPercentage = 20,
                Tariffs = new List<Tariff>
                {
                new Tariff { StartTime = new DateTime(2024, 04, 16, 18, 00, 00), EndTime = new DateTime(2024, 04, 16, 20, 17, 00), EnergyPrice = 0.15m },
                new Tariff { StartTime = new DateTime(2024, 04, 16, 20, 17, 00), EndTime = new DateTime(2024, 04, 16, 23, 33, 00), EnergyPrice = 0.20m },
                new Tariff { StartTime = new DateTime(2024, 04, 16, 23, 33, 00), EndTime = new DateTime(2024, 04, 16, 05, 22, 00), EnergyPrice = 0.10m },
                new Tariff { StartTime = new DateTime(2024, 04, 16, 05, 22, 00), EndTime = new DateTime(2024, 04, 16, 07, 00, 00), EnergyPrice = 0.25m },
               
                }
            };
            CarData carData = new CarData
            {
                ChargePower = 9.5m,
                BatteryCapacity = 100m,
                CurrentBatteryLevel = 15m, 
                               
            };
            
            // Act
            List<ChargingSchedule> chargingSchedule = generator.GenerateChargingProfile(startingTime, userSettings, carData);

            // Configure JsonSerializerOptions
            var options = new JsonSerializerOptions();
            options.Converters.Add(new DecimalConverter());
            options.Converters.Add(new DateTimeConverter());
            options.Converters.Add(new TimeSpanConverter());
            options.WriteIndented = true;

            foreach (ChargingSchedule schedule in chargingSchedule)
            {

                // Assert
                Assert.That(schedule.StartTime, Is.EqualTo(new DateTime(2024, 04, 16, 23, 33, 00)));
                Assert.That(schedule.EndTime, Is.EqualTo(new DateTime(2024, 04, 16, 05, 22, 00)));
                Assert.That(schedule.IsCharging, Is.True);
            }

            Assert.IsNotNull(chargingSchedule);
}

        [Test]
        public void TestGenerateChargingProfile_BatteryNotLow_CheapestTariff()
        {
            // Arrange
            DateTime startingTime = new DateTime(2024, 04, 17, 08, 00, 00);
            UserSettings userSettings = new UserSettings
            {
                DesiredStateOfCharge = 80,
                LeavingTime = new TimeSpan(18, 00, 00),
                DirectChargingPercentage = 20,
                Tariffs = new List<Tariff>
                {
                new Tariff { StartTime = new DateTime(2024, 04, 16, 18, 00, 00), EndTime = new DateTime(2024, 04, 16, 20, 17, 00), EnergyPrice = 0.15m },
                new Tariff { StartTime = new DateTime(2024, 04, 16, 20, 17, 00), EndTime = new DateTime(2024, 04, 16, 23, 33, 00), EnergyPrice = 0.20m },
                new Tariff { StartTime = new DateTime(2024, 04, 16, 23, 33, 00), EndTime = new DateTime(2024, 04, 16, 05, 22, 00), EnergyPrice = 0.10m },
                new Tariff { StartTime = new DateTime(2024, 04, 16, 05, 22, 00), EndTime = new DateTime(2024, 04, 16, 07, 00, 00), EnergyPrice = 0.25m }
                }
            };
            CarData carData = new CarData
            {
                CurrentBatteryLevel = 9.5m,
                BatteryCapacity = 100m,
                ChargePower = 100
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
            foreach (ChargingSchedule schedule in chargingSchedule)
            {

                Assert.That(schedule.StartTime, Is.EqualTo(new DateTime(2024, 04, 16, 23, 33, 00)));
                Assert.That(schedule.EndTime, Is.EqualTo(new DateTime(2024, 04, 16, 05, 22, 00)));
                Assert.That(schedule.IsCharging, Is.False);
            }
        }
    }
}

