using ChargingProfileGenerator.Domain;
using ChargingProfileGenerator.Domain.OutputViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargingProfileGenerator.App
{
    public class GeneratorChargingProfile

    {
        public List<ChargingSchedule> GenerateChargingProfile(DateTime startingTime, UserSettings userSettings, CarData carData)
        {
            //not checking DateTime startingTime as it will be always false.
            if (userSettings == null || carData == null)
                throw new ArgumentNullException("Input parameters cannot be null.");

            List<ChargingSchedule> chargingSchedule = new List<ChargingSchedule>();
            //DateTime plugInCarTime = startingTime.Date.Add(userSettings.LeavingTime);
            DateTime currentTime = startingTime;
         

            //checking currentTime is less than car leaving time.
            while (currentTime < startingTime.Date.Add(userSettings.LeavingTime))
            { 

                //Finding the available Tariff for the current Time.
           
                Tariff tariff = IsBatteryLow(carData)? 
                    CalculateCurrentTariff(currentTime, userSettings.Tariffs) : CalculateCheapestTariff(currentTime, userSettings.Tariffs);

                // Calculate the duration until the next change in tariff
                TimeSpan timeUntilNextChange = FindTimeUntilNextTariffChange(tariff.StartTime, tariff.EndTime);

                // Calculate the energy needed to reach the desired state of charge
                decimal energyNeeded = CalculateEnergyNeeded(userSettings, carData);

                // Calculate the charging duration needed
                decimal chargingDuration = (energyNeeded / (carData.ChargePower/100));

                bool isCharging = IsCharging(userSettings, carData, tariff, chargingDuration);

                // Add the charging schedule to the profile
                chargingSchedule.Add(new ChargingSchedule
                {
                    StartTime = currentTime,
                    EndTime = currentTime.Add(timeUntilNextChange),
                    IsCharging = isCharging
                });
                // Move to the next tariff period
                currentTime = currentTime.Date.Add(timeUntilNextChange);

            }


            return chargingSchedule;

        }

        /// <summary>
        /// Calculating The cheapest available tariff among the List
        /// </summary>
        /// <param name="currentTime"> provides currentTime on the basics of lowest Energy Price</param>
        /// <param name="Tariffs"> List of tariffs </param>
        /// <returns> returns cheapest tariff </returns>

        private Tariff CalculateCheapestTariff(DateTime plugInCarTime, List<Tariff> tariffs)
        {
          // Find the tariff with the cheapest energy price that is applicable for the plugInCar time
            Tariff cheapestTariff = tariffs[0];
            foreach (var tariff in tariffs)
            { // Check if the tariff is valid for the plugInCar time
                if (tariff.StartTime <= plugInCarTime && tariff.EndTime > plugInCarTime)
                {   // Check if the energy price of the current tariff is lower than the energy price of the cheapestTariff

                    if (tariff.EnergyPrice < cheapestTariff.EnergyPrice)
                    {
                        cheapestTariff = tariff;

                    }
                }
            }

            return cheapestTariff;
        }

        /// <summary>
        /// Calculate the available tariff among the List
        /// </summary>
        /// <param name="currentTime"> provides currentTime according to avalability</param>
        /// <param name="Tariffs"> List of tariffs </param>
        /// <returns> returns current available tariff </returns>
        private Tariff CalculateCurrentTariff(DateTime plugInCarTime, List<Tariff> tariffs)
        {
            foreach (var tariff in tariffs)
            {
                if (tariff.StartTime <= plugInCarTime && plugInCarTime < tariff.EndTime)
                {
                    return tariff;
                }
            }
            return null;
        }

        /// <summary>
        /// Calculate the available tariff among the List
        /// </summary>
        /// <param name="tariffStartTime"> start time of Tariff </param>
        /// <param name="tariffEndTime"> end time of Tariff </param>
        /// <returns> returns TimeSpan </returns>
        private TimeSpan FindTimeUntilNextTariffChange(DateTime tariffStartTime, DateTime tariffEndTime)
        {
            // Check if the end time is greater than the start time in terms of time of day
            if (tariffEndTime.TimeOfDay > tariffStartTime.TimeOfDay)
            {
                // If end time is greater, simply subtract start time from end time
                return tariffEndTime - tariffStartTime;
            }
            else
            {
                // If end time is not greater, it means the tariff period crosses midnight
                // Subtract start time from midnight, add end time, and subtract one day to get the correct time span
                TimeSpan timeUntilMidnight = DateTime.Today.AddDays(1) - tariffStartTime;
                TimeSpan timeFromMidnight = tariffEndTime.TimeOfDay;

                return timeUntilMidnight + timeFromMidnight;
            }
        }


        /// <summary>
        /// Calculate energy needed on the basics of userSettings and cardata
        /// </summary>
        /// <param name="userSettings"> provides userSettings Model</param>
        /// <param name="carData">provides carData Model </param>
        /// <returns> decimal value of energyNeeded </returns>
        private decimal CalculateEnergyNeeded(UserSettings userSettings, CarData carData)
        {
            decimal desiredCharge =  userSettings.DesiredStateOfCharge ;
            decimal currentCharge = carData.CurrentBatteryLevel / 100;
            decimal energyNeeded;
            //As mentioned car can either charging with the full Charge Power or not charging at all (e.g. charging at e.g. half speed is not possible).
            //Therefore charging completedly.
            if (currentCharge < desiredCharge)
            {
                energyNeeded = ((carData.BatteryCapacity/100) * desiredCharge / 100) - currentCharge;

                return energyNeeded;
            }
                

            return 0;
        }

        /// <summary>
        /// Calculate Is car charging 
        /// </summary>
        /// <param name="userSettings"> provides userSettings Model</param>
        /// <param name="carData"> Provide carData Model </param>
        /// <param name="currentTariff">Provides current Tariff based on the above method </param>
        /// <param name="chargingDuration"> how much hours needed to charge the car to reach desired charging </param>
        /// <returns> returns bool value </returns>
        private bool IsCharging(UserSettings userSettings, CarData carData, Tariff tariff, decimal chargingDuration)
        {
            bool charging;
            
            decimal energyNeeded = CalculateEnergyNeeded(userSettings, carData);
            decimal totalCost = energyNeeded * (tariff.EnergyPrice / 100);
           decimal chargingCost = chargingDuration * (carData.ChargePower/100) * (tariff.EnergyPrice/100);

            if (chargingCost < totalCost)
            { charging = true; }
            else { charging = false; }


            return charging;

        }


        /// <summary>
        /// Calculate Is car battery low 
        /// </summary>
        /// <param name="carData"> Provide carData Model </param>
        /// <returns> returns bool value </returns>
        private bool IsBatteryLow(CarData carData)
        {
            bool charging = false;
            decimal batteryPercentage = (carData.CurrentBatteryLevel / carData.BatteryCapacity) * 100;
            if (batteryPercentage < 20)
            {
                charging = true;
            }
            return charging;
        }
    }
}
