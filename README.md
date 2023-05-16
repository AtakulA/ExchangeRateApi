# MeDirectProject
Web Api project for exchange rates

=============================================

1 - Please change the connection string named "DefaultConnection" in appsettings.json file.
(User in the connection string MUST have the authorization for Creating/Deleting DBs/Tables)
2 - After changing connection string open "Package Manager Console", select "ExchangeService.Data" as Default Project and run "update-database" command.
(Migration files have already been created)
(I have added auto migration settings in Program.cs but because of the logger it doesn't work)

=============================================

Missing Requirements
1- Caching

