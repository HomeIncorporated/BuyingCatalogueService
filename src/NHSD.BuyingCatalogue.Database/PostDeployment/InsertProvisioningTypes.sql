﻿
IF NOT EXISTS (SELECT * FROM dbo.ProvisioningType)
    INSERT INTO dbo.ProvisioningType(ProvisioningTypeId, [Name])
    VALUES
    (1, 'Patient Numbers'),
    (2, 'Declarative'),
    (3, 'On Demand');
GO