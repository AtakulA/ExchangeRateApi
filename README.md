# MeDirectProject
Web Api project for exchange rates

=============================================

1 - Please change the connection string named "DefaultConnection" in appsettings.json file.
(User in the connection string MUST have the authorization for Creating/Deleting DBs/Tables)


2 - After changing connection string open "Package Manager Console", select "ExchangeService.Data" as Default Project and run "update-database" command.

(Migration files have already been created)

(I have added auto migration settings in Program.cs but because of the logger it doesn't work)

3- I used "https://exchangeratesapi.io/" api for exchange rates. Please enter a valid api-key into appsettings.json file.

=============================================

Features

1- Gets exchange rate for 2 given currencies, also checks if given rate whether older than 30 mins or not. 
If it is older, then project gets the current rate from an api. (From-To currencies. Example: From=USD To=EUR)

2- With sending userId you can trade from one currency to another. Every user have 10 trade chance per hour.

3- You can list all trades or user specific trades or get a specific trade detail.

4- You can check when a user can trade.(In minutes)

