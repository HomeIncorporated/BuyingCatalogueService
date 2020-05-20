﻿CREATE PROCEDURE test.DropUserAndLogin AS
    SET NOCOUNT ON;

    ALTER DATABASE [$(DB_NAME)] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;

    DROP USER NHSD;
    DROP LOGIN NHSD;
GO
