ALTER DATABASE [$(DatabaseName)]
    ADD LOG FILE (NAME = [DB_RollCall_log], FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\DB_RollCall_log.LDF', SIZE = 576 KB, MAXSIZE = 2097152 MB, FILEGROWTH = 10 %);



