
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
        string Inputfile = File.ReadAllText(@"C:\Users\aishn\source\repos\EVSolution\ChargingprofileGenerator.JsonFiles\Input.json");


        // Configure JsonSerializerOptions
        var options = new JsonSerializerOptions();
        options.Converters.Add(new DecimalConverter());
        options.Converters.Add(new DateTimeConverter());
        options.Converters.Add(new TimeSpanConverter());
        options.WriteIndented = true;

        //Parse JSON

        var input = JsonSerializer.Deserialize<ChargingProfile>(Inputfile, options);
        if (input != null)
        {
            // Create charging profile generator
            GeneratorChargingProfile generator = new GeneratorChargingProfile();

            // Generate charging Schedule
            List<ChargingSchedule> chargingSchedule = generator.GenerateChargingProfile(input.StartingTime, input.UserSettings, input.CarData);

            // Output charging profile in JSON format
            string jsonOutput = JsonSerializer.Serialize(chargingSchedule, options);

            Console.WriteLine(jsonOutput);

            //Write to Output Json File

            string Outputfile = @"C:\Users\aishn\source\repos\EVSolution\ChargingprofileGenerator.JsonFiles\Output.json";
            File.WriteAllText(Outputfile, jsonOutput);

            //Write to Console
            Console.WriteLine(Outputfile);
        }

    }
}