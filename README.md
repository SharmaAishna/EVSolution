# EVSolution
The purpose of the application is to generate the optimal schedule for the user from a given
moment (the StartingTime) until the Leaving Time. Optimize on the lowest energy bill for the user, 
considering all mentioned User Settings, Car Data and Price Tariff. This application reads the Charging Profile as a JSON
and generates the optimal ChargingSchedule as a JSON.

# To Start the application

- Download the latest version of Visual Studio 2022.
- In JSON file change the startingDate to the next day.

# Explation of application
The project contains Four folders 

- ChargingProfileGenerator.Domain 
This folder contains classes and Helper classes. The helper classes help in the conversion of DataType to JSON recognisable.

- ChargingProfileGenerator.JsonFiles
  - This folder contains Input JSON and output JSON files for the application.
  - Input File - application reads for Deserializing data to the application
  - Output File - application reads for Serializing data back.

- ChargingProfileGenerator.app
  - It contains the Console application.
  - In the program.cs, at lines 13 and 39. Please adjust the paths of JSON files according to the folder in your application
  - GeneratorChargingProfile has all the flow and logic of the application.

- ChargingProfileGenerator.NUnitTest
  - Contains NUnit Test for the application.
