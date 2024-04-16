
using ChargingProfileGenerator.App;
using ChargingProfileGenerator.Domain;
using ChargingProfileGenerator.Domain.Helper;
using ChargingProfileGenerator.Domain.OutputViewModel;
using System.Text.Json;

class Program
{
    static void Main(string[] args)
    {
        // Read input from JSON file
        string Inputfile = @"C:\Users\aishn\source\repos\EVSolution\Input.json";

        using (StreamReader reader = new StreamReader(Inputfile))
        {
            string json = reader.ReadToEnd();
            var input = JsonSerializer.Deserialize<ChargingProfile>(json);
            if (input != null)
            {
                // Create charging profile generator
                GeneratorChargingProfile generator = new GeneratorChargingProfile();

                // Generate charging Schedule
                List<ChargingSchedule> chargingSchedule = generator.GenerateChargingProfile(input.StartingTime, input.UserSettings, input.CarData);

                // Configure JsonSerializerOptions
                var options = new JsonSerializerOptions();
                options.Converters.Add(new DecimalConverter());
                options.Converters.Add(new DateTimeConverter());
                options.Converters.Add(new TimeSpanConverter());
                options.WriteIndented = true;
                // Output charging profile in JSON format
                string jsonOutput = JsonSerializer.Serialize(chargingSchedule,options);

                Console.WriteLine(jsonOutput);

                //Write to Output Json File
                string Outputfile = @"C:\Users\aishn\source\repos\EVSolution\Output.json";
                File.WriteAllText(Outputfile, jsonOutput);
                //Write to Console
                Console.WriteLine(Outputfile);
            }
        }
    }
}