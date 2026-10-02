using Intl.Realty.Firm.DataAccess;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intl.Realty.Firm.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20261001143000_AddPreferredPartnerDocsAndReferredAgentPhone")]
    public class AddPreferredPartnerDocsAndReferredAgentPhone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReferredAgentPhone",
                table: "AgentReferrals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.Sql(@"
DECLARE @emptyGuid UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000';
DECLARE @now DATETIME2 = GETUTCDATE();
DECLARE @photographerId INT;
DECLARE @homeInspectorId INT;
DECLARE @mortgageAgentId INT;

IF NOT EXISTS (SELECT 1 FROM DocumentTypes WHERE Description = N'Photographer (IRF Preferred Partner or other)' AND IsDeleted = 0)
BEGIN
    INSERT INTO DocumentTypes (Code, Description, IsRequired, IsActive, IsDeleted, CreatedBy, CreatedAt)
    VALUES (N'PHOTO', N'Photographer (IRF Preferred Partner or other)', 0, 1, 0, @emptyGuid, @now);
END

IF NOT EXISTS (SELECT 1 FROM DocumentTypes WHERE Description = N'Home Inspector (IRF Preferred Partner or other)' AND IsDeleted = 0)
BEGIN
    INSERT INTO DocumentTypes (Code, Description, IsRequired, IsActive, IsDeleted, CreatedBy, CreatedAt)
    VALUES (N'INSP', N'Home Inspector (IRF Preferred Partner or other)', 0, 1, 0, @emptyGuid, @now);
END

IF NOT EXISTS (SELECT 1 FROM DocumentTypes WHERE Description = N'Mortgage Agent/Bank (IRF Preferred Partner or other)' AND IsDeleted = 0)
BEGIN
    INSERT INTO DocumentTypes (Code, Description, IsRequired, IsActive, IsDeleted, CreatedBy, CreatedAt)
    VALUES (N'MORT', N'Mortgage Agent/Bank (IRF Preferred Partner or other)', 0, 1, 0, @emptyGuid, @now);
END

SELECT @photographerId = Id FROM DocumentTypes WHERE Description = N'Photographer (IRF Preferred Partner or other)' AND IsDeleted = 0;
SELECT @homeInspectorId = Id FROM DocumentTypes WHERE Description = N'Home Inspector (IRF Preferred Partner or other)' AND IsDeleted = 0;
SELECT @mortgageAgentId = Id FROM DocumentTypes WHERE Description = N'Mortgage Agent/Bank (IRF Preferred Partner or other)' AND IsDeleted = 0;

-- Sale Listing (TransactionTypeId = 1): Photographer
IF @photographerId IS NOT NULL AND NOT EXISTS (
    SELECT 1 FROM DocumentTypeAssignments
    WHERE DocumentTypeId = @photographerId AND TransactionTypeId = 1 AND IsDeleted = 0)
BEGIN
    INSERT INTO DocumentTypeAssignments (DocumentTypeId, TransactionTypeId, IsRequired, IsActive, IsDeleted, CreatedBy, CreatedAt)
    VALUES (@photographerId, 1, 0, 1, 0, @emptyGuid, @now);
END

-- Lease Listing (TransactionTypeId = 3): Photographer
IF @photographerId IS NOT NULL AND NOT EXISTS (
    SELECT 1 FROM DocumentTypeAssignments
    WHERE DocumentTypeId = @photographerId AND TransactionTypeId = 3 AND IsDeleted = 0)
BEGIN
    INSERT INTO DocumentTypeAssignments (DocumentTypeId, TransactionTypeId, IsRequired, IsActive, IsDeleted, CreatedBy, CreatedAt)
    VALUES (@photographerId, 3, 0, 1, 0, @emptyGuid, @now);
END

-- Sale Co-op (TransactionTypeId = 2): Home Inspector
IF @homeInspectorId IS NOT NULL AND NOT EXISTS (
    SELECT 1 FROM DocumentTypeAssignments
    WHERE DocumentTypeId = @homeInspectorId AND TransactionTypeId = 2 AND IsDeleted = 0)
BEGIN
    INSERT INTO DocumentTypeAssignments (DocumentTypeId, TransactionTypeId, IsRequired, IsActive, IsDeleted, CreatedBy, CreatedAt)
    VALUES (@homeInspectorId, 2, 0, 1, 0, @emptyGuid, @now);
END

-- Sale Co-op (TransactionTypeId = 2): Mortgage Agent/Bank
IF @mortgageAgentId IS NOT NULL AND NOT EXISTS (
    SELECT 1 FROM DocumentTypeAssignments
    WHERE DocumentTypeId = @mortgageAgentId AND TransactionTypeId = 2 AND IsDeleted = 0)
BEGIN
    INSERT INTO DocumentTypeAssignments (DocumentTypeId, TransactionTypeId, IsRequired, IsActive, IsDeleted, CreatedBy, CreatedAt)
    VALUES (@mortgageAgentId, 2, 0, 1, 0, @emptyGuid, @now);
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DECLARE @photographerId INT;
DECLARE @homeInspectorId INT;
DECLARE @mortgageAgentId INT;

SELECT @photographerId = Id FROM DocumentTypes WHERE Description = N'Photographer (IRF Preferred Partner or other)';
SELECT @homeInspectorId = Id FROM DocumentTypes WHERE Description = N'Home Inspector (IRF Preferred Partner or other)';
SELECT @mortgageAgentId = Id FROM DocumentTypes WHERE Description = N'Mortgage Agent/Bank (IRF Preferred Partner or other)';

IF @photographerId IS NOT NULL
BEGIN
    DELETE FROM DocumentTypeAssignments WHERE DocumentTypeId = @photographerId AND TransactionTypeId IN (1, 3);
    DELETE FROM DocumentTypes WHERE Id = @photographerId;
END

IF @homeInspectorId IS NOT NULL
BEGIN
    DELETE FROM DocumentTypeAssignments WHERE DocumentTypeId = @homeInspectorId AND TransactionTypeId = 2;
    DELETE FROM DocumentTypes WHERE Id = @homeInspectorId;
END

IF @mortgageAgentId IS NOT NULL
BEGIN
    DELETE FROM DocumentTypeAssignments WHERE DocumentTypeId = @mortgageAgentId AND TransactionTypeId = 2;
    DELETE FROM DocumentTypes WHERE Id = @mortgageAgentId;
END
");

            migrationBuilder.DropColumn(
                name: "ReferredAgentPhone",
                table: "AgentReferrals");
        }
    }
}
