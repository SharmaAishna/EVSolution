using ChargingProfileGenerator.Domain;
using ChargingProfileGenerator.Domain.DTOs;
using ChargingProfileGenerator.Domain.OutputViewModel;

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

            DateTime currentTime = DateTime.Now;
            startingTime = startingTime.Date.Add(userSettings.LeavingTime);

            //checking currentTime is less than car leaving time.
            if (currentTime < startingTime)
            {

                //Finding the available Tariff for the current Time.

                TariffDTO tariff = IsBatteryLow(carData) ?
                    CalculateCurrentTariff(currentTime, userSettings.Tariffs) : CalculateCheapestTariff(currentTime, userSettings.Tariffs, startingTime);

                // Calculate the hours needed to charge
                CalculateTariffHours(tariff.StartTime.TimeOfDay, tariff.EndTime.TimeOfDay);

                // Calculate the energy needed to reach the desired state of charge
                decimal energyNeeded = CalculateEnergyNeeded(userSettings, carData);

                // Calculate the charging duration needed
                decimal chargingDuration = (energyNeeded / (carData.ChargePower / 100));
                bool isCharging = IsCharging(userSettings, carData, tariff, chargingDuration);

                // Add the charging schedule to the profile
                chargingSchedule.Add(new ChargingSchedule
                {
                    StartTime = tariff.StartTime,
                    EndTime = tariff.EndTime,
                    IsCharging = isCharging
                });


            }


            return chargingSchedule;

        }

        /// <summary>
        /// Calculating The cheapest available tariff among the List
        /// </summary>
        /// <param name="currentTime"> provides currentTime on the basics of lowest Energy Price</param>
        /// <param name="Tariffs"> List of tariffs </param>
        /// <returns> returns cheapest tariff </returns>

        private TariffDTO CalculateCheapestTariff(DateTime currentTime, List<Tariff> tariffs, DateTime startingTime)
        {
            TariffDTO tariffDTO = new TariffDTO();
            // Find the tariff with the cheapest energy price that is applicable for the current time
            Tariff cheapestEnergyPriceTariff = tariffs.OrderBy(tariff => tariff.EnergyPrice)
               .ThenBy(tariff => tariff.StartTime >= currentTime.TimeOfDay && tariff.EndTime > startingTime.TimeOfDay)
               .FirstOrDefault();
            if (cheapestEnergyPriceTariff != null)
            {
                tariffDTO.StartTime = startingTime.Date.Add(cheapestEnergyPriceTariff.StartTime);
                tariffDTO.EndTime = startingTime.Date.Add(cheapestEnergyPriceTariff.EndTime);
                tariffDTO.EnergyPrice = cheapestEnergyPriceTariff.EnergyPrice;

            }


            return tariffDTO;
        }

        /// <summary>
        /// Calculate the available tariff among the List
        /// </summary>
        /// <param name="currentTime"> provides currentTime according to avalability</param>
        /// <param name="Tariffs"> List of tariffs </param>
        /// <returns> returns current available tariff </returns>
        private TariffDTO CalculateCurrentTariff(DateTime currentTime, List<Tariff> tariffs)
        {
            TariffDTO tariffDTO = new TariffDTO();
            foreach (var tariff in tariffs)
            {
                tariffDTO.StartTime = currentTime.Date.Add(tariff.StartTime);
                tariffDTO.EndTime = currentTime.Date.Add(tariff.EndTime);
                tariffDTO.EnergyPrice = tariff.EnergyPrice;
                if (tariffDTO.StartTime <= currentTime && tariffDTO.EndTime > currentTime)
                {
                    return tariffDTO;
                }
            }
            return null;
        }

        /// <summary>
        /// Calculate tariff hours
        /// </summary>
        /// <param name="tariffStartTime"> start time of Tariff </param>
        /// <param name="tariffEndTime"> end time of Tariff </param>
        /// <returns> returns TimeSpan </returns>
        private TimeSpan CalculateTariffHours(TimeSpan tariffStartTime, TimeSpan tariffEndTime)
        {
            // Check if the end time is greater than the start time in terms of time of day
            if (tariffEndTime > tariffStartTime)
            {
                // If end time is greater, simply subtract start time from end time to get the hours.
                return tariffEndTime - tariffStartTime;
            }
            else
            {
                // If end time is not greater, it means the tariff period crosses midnight
                // Subtract start time from midnight, add end time, and subtract one day to get the correct time span
                TimeSpan timeUntilMidnight = DateTime.Today.AddDays(1).TimeOfDay - tariffStartTime;
                TimeSpan timeFromMidnight = tariffEndTime;

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
            decimal desiredCharge = userSettings.DesiredStateOfCharge;
            decimal currentCharge = carData.CurrentBatteryLevel / 100;
            decimal energyNeeded;
            //As mentioned car charging with the full Charge Power 
            if (currentCharge < desiredCharge)
            {
                energyNeeded = ((carData.BatteryCapacity / 100) * desiredCharge / 100) - currentCharge;

                return energyNeeded;
            }
            //Car doesn't need charging

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
        private bool IsCharging(UserSettings userSettings, CarData carData, TariffDTO tariff, decimal chargingDuration)
        {
            bool charging;

            decimal chargingCost = chargingDuration * (carData.ChargePower / 100) * (tariff.EnergyPrice / 100);

            if (chargingDuration > 0)
            {
                charging = true;
            }
            else
            {
                charging = false;
            }


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
