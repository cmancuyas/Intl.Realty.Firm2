-- Migration: 20261001143000_AddPreferredPartnerDocsAndReferredAgentPhone
-- Safe to run more than once: every step checks whether it was already applied.

BEGIN TRANSACTION;

-- 1. New column used by the Refer an Agent form
IF COL_LENGTH('AgentReferrals', 'ReferredAgentPhone') IS NULL
BEGIN
    ALTER TABLE AgentReferrals ADD ReferredAgentPhone nvarchar(max) NULL;
END;

-- 2. Preferred Partner document types and their assignments
DECLARE @emptyGuid UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000';
DECLARE @now DATETIME2 = GETUTCDATE();
DECLARE @photographerId INT;
DECLARE @homeInspectorId INT;
DECLARE @mortgageAgentId INT;

IF NOT EXISTS (SELECT 1 FROM DocumentTypes WHERE Description = N'Photographer (IRF Preferred Partner or other)' AND IsDeleted = 0)
    INSERT INTO DocumentTypes (Code, Description, IsRequired, IsActive, IsDeleted, CreatedBy, CreatedAt)
    VALUES (N'PHOTO', N'Photographer (IRF Preferred Partner or other)', 0, 1, 0, @emptyGuid, @now);

IF NOT EXISTS (SELECT 1 FROM DocumentTypes WHERE Description = N'Home Inspector (IRF Preferred Partner or other)' AND IsDeleted = 0)
    INSERT INTO DocumentTypes (Code, Description, IsRequired, IsActive, IsDeleted, CreatedBy, CreatedAt)
    VALUES (N'INSP', N'Home Inspector (IRF Preferred Partner or other)', 0, 1, 0, @emptyGuid, @now);

IF NOT EXISTS (SELECT 1 FROM DocumentTypes WHERE Description = N'Mortgage Agent/Bank (IRF Preferred Partner or other)' AND IsDeleted = 0)
    INSERT INTO DocumentTypes (Code, Description, IsRequired, IsActive, IsDeleted, CreatedBy, CreatedAt)
    VALUES (N'MORT', N'Mortgage Agent/Bank (IRF Preferred Partner or other)', 0, 1, 0, @emptyGuid, @now);

SELECT @photographerId  = Id FROM DocumentTypes WHERE Description = N'Photographer (IRF Preferred Partner or other)' AND IsDeleted = 0;
SELECT @homeInspectorId = Id FROM DocumentTypes WHERE Description = N'Home Inspector (IRF Preferred Partner or other)' AND IsDeleted = 0;
SELECT @mortgageAgentId = Id FROM DocumentTypes WHERE Description = N'Mortgage Agent/Bank (IRF Preferred Partner or other)' AND IsDeleted = 0;

-- Sale Listing (TransactionTypeId = 1): Photographer
IF @photographerId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM DocumentTypeAssignments WHERE DocumentTypeId = @photographerId AND TransactionTypeId = 1 AND IsDeleted = 0)
    INSERT INTO DocumentTypeAssignments (DocumentTypeId, TransactionTypeId, IsRequired, IsActive, IsDeleted, CreatedBy, CreatedAt)
    VALUES (@photographerId, 1, 0, 1, 0, @emptyGuid, @now);

-- Lease Listing (TransactionTypeId = 3): Photographer
IF @photographerId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM DocumentTypeAssignments WHERE DocumentTypeId = @photographerId AND TransactionTypeId = 3 AND IsDeleted = 0)
    INSERT INTO DocumentTypeAssignments (DocumentTypeId, TransactionTypeId, IsRequired, IsActive, IsDeleted, CreatedBy, CreatedAt)
    VALUES (@photographerId, 3, 0, 1, 0, @emptyGuid, @now);

-- Sale Co-op (TransactionTypeId = 2): Home Inspector
IF @homeInspectorId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM DocumentTypeAssignments WHERE DocumentTypeId = @homeInspectorId AND TransactionTypeId = 2 AND IsDeleted = 0)
    INSERT INTO DocumentTypeAssignments (DocumentTypeId, TransactionTypeId, IsRequired, IsActive, IsDeleted, CreatedBy, CreatedAt)
    VALUES (@homeInspectorId, 2, 0, 1, 0, @emptyGuid, @now);

-- Sale Co-op (TransactionTypeId = 2): Mortgage Agent/Bank
IF @mortgageAgentId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM DocumentTypeAssignments WHERE DocumentTypeId = @mortgageAgentId AND TransactionTypeId = 2 AND IsDeleted = 0)
    INSERT INTO DocumentTypeAssignments (DocumentTypeId, TransactionTypeId, IsRequired, IsActive, IsDeleted, CreatedBy, CreatedAt)
    VALUES (@mortgageAgentId, 2, 0, 1, 0, @emptyGuid, @now);

-- 3. Record the migration so EF Core does not try to apply it again
IF NOT EXISTS (SELECT 1 FROM __EFMigrationsHistory WHERE MigrationId = N'20261001143000_AddPreferredPartnerDocsAndReferredAgentPhone')
    INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion)
    VALUES (N'20261001143000_AddPreferredPartnerDocsAndReferredAgentPhone', N'9.0.0');

COMMIT;
