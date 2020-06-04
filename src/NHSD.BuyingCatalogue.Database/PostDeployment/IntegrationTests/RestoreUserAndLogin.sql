﻿CREATE PROCEDURE test.RestoreUserAndLogin AS
    SET NOCOUNT ON;

    CREATE LOGIN NHSD
    WITH PASSWORD = '$(NHSD_PASSWORD)';

    CREATE USER NHSD
    FOR LOGIN NHSD
    WITH DEFAULT_SCHEMA = dbo;

    GRANT CONNECT TO NHSD;

    ALTER ROLE Api
    ADD MEMBER NHSD;

    ALTER DATABASE [$(DB_NAME)] SET MULTI_USER;
GO