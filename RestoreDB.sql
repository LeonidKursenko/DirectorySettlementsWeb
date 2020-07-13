RESTORE DATABASE directorySettlementsDb
FROM DISK = 'D:\Робота\Тестові завдання\Інфоплюс\initial'

WITH MOVE 'directorySettlementsDb' TO 'C:\Users\Leonid\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\directorySettlementsDb.mdf',
MOVE 'directorySettlementsDb_log' TO 'C:\Users\Leonid\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\directorySettlementsDb.ldf',
REPLACE;