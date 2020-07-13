# DirectorySettlementsWeb
### Information
System of work with the directory of settlements of Ukraine (KOATUU). This is my implementation of the test task.
To perform the test task were used: ASP.NET CORE, SQL Server Express LocalDB, Entity Framework Core.

The main functionality that needs to be implemented:
1. Adding an element to the tree of settlements.
2. Removal of an element from the tree of settlements.
3. Rename the element.
4. Filtering of tree by name and type of settlements.
5. Export of a tree (taking into account filtering) in PDF.

### Installation
1) The "backup" file is a database.
2) The file "RestoreDB.sql" contains an example of a script for deployment of a DB.
3) The file "appsettings.json" contains the term of connection to the database:
"DbConnection": "Server =. \\ SQLEXPRESS; Database = directorySettlementsDb; Trusted_Connection = True; MultipleActiveResultSets = true;"
4) After all settings, the project should start.
