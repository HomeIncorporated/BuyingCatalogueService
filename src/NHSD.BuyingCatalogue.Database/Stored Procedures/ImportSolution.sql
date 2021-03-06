﻿CREATE PROCEDURE import.ImportSolution
     @SolutionId varchar(14),
     @SolutionName varchar(255),
     @IsFoundation bit,
     @Capabilities import.SolutionCapability READONLY
AS
    SET NOCOUNT ON;

    BEGIN TRANSACTION;

    BEGIN TRY
        DECLARE @emptyGuid AS uniqueidentifier = CAST(0x0 AS uniqueidentifier);
        DECLARE @frameworkId AS varchar(10) = 'NHSDGP001';
        DECLARE @now AS datetime = GETUTCDATE();
        DECLARE @supplierId AS varchar(6) = SUBSTRING(@SolutionId, 1, CHARINDEX('-', @SolutionId) - 1);

        IF NOT EXISTS (SELECT * FROM dbo.Supplier WHERE Id = @supplierId)
            THROW 51000, 'Supplier record does not exist.', 1;

        DECLARE @draftPublicationStatus AS int = (SELECT Id FROM dbo.PublicationStatus WHERE [Name] = 'Draft');
        DECLARE @solutionCatalogueItemType AS int = (SELECT CatalogueItemTypeId FROM dbo.CatalogueItemType WHERE [Name] = 'Solution');

        IF NOT EXISTS (SELECT * FROM dbo.CatalogueItem WHERE CatalogueItemId = @SolutionId)
            INSERT INTO dbo.CatalogueItem(CatalogueItemId, [Name], Created,
                        CatalogueItemTypeId, SupplierId, PublishedStatusId)
                 VALUES (@SolutionId, @SolutionName, @now,
                        @solutionCatalogueItemType, @supplierId, @draftPublicationStatus);

        IF NOT EXISTS (SELECT * FROM dbo.Solution WHERE Id = @SolutionId)
            INSERT INTO dbo.Solution(Id, LastUpdated, LastUpdatedBy)
                 VALUES (@SolutionId, @now, @emptyGuid);

        IF NOT EXISTS (SELECT * FROM dbo.FrameworkSolutions WHERE SolutionId = @SolutionId AND FrameworkId = @frameworkId)
            INSERT INTO dbo.FrameworkSolutions(FrameworkId, SolutionId, IsFoundation, LastUpdated, LastUpdatedBy)
                 VALUES (@frameworkId, @SolutionId, 0, @now, @emptyGuid);

        UPDATE dbo.FrameworkSolutions
           SET IsFoundation = @IsFoundation
         WHERE SolutionId = @SolutionId
           AND FrameworkId = @frameworkId;

        DELETE FROM dbo.SolutionCapability
              WHERE SolutionId = @SolutionId;

        DECLARE @passedFull AS int = (SELECT Id FROM dbo.SolutionCapabilityStatus WHERE [Name] = 'Passed – Full');

        INSERT INTO dbo.SolutionCapability(SolutionId, CapabilityId, StatusId, LastUpdated, LastUpdatedBy)
             SELECT @SolutionId, c.Id, @passedFull, @now, @emptyGuid
               FROM @Capabilities AS cap
                    INNER JOIN dbo.Capability AS c
                    ON c.CapabilityRef = cap.CapabilityRef;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
