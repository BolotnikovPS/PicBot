using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PicBot.Infrastructure.Contexts.BotPlatform.Migrations;

[DbContext(typeof(BotPlatformDbContext))]
[Migration("0001")]
public class M0001 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        const string sql = @"

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Users` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `TgUserId` bigint NOT NULL,
    `ChatId` bigint NOT NULL,
    `UserName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
    `FirstName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
    `LastName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
    `Role` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT 'User',
    `BlockType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT 'None',
    `RegisterDate` datetime(6) NULL,
    CONSTRAINT `PK_Users` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_general_ci;


CREATE TABLE `Verifications` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` int NOT NULL,
    `EventType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT 'None',
    CONSTRAINT `PK_Verifications` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_general_ci;


CREATE INDEX `IX_Users_TgUserId` ON `Users` (`TgUserId`);


CREATE INDEX `IX_Verifications_UserId` ON `Verifications` (`UserId`);

CREATE TABLE IF NOT EXISTS `FilesBox` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` int NOT NULL,
    `FileId` longtext NOT NULL,
    `Create` datetime(6) NOT NULL,
    CONSTRAINT `PK_FilesBox` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE INDEX IF NOT EXISTS `IX_FilesBox_UserId` ON `FilesBox` (`UserId`);

ALTER TABLE `FilesBox` ADD CONSTRAINT `FK_FilesBox_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE;
ALTER TABLE `Verifications` ADD CONSTRAINT `FK_Verifications_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE;
";

        migrationBuilder.Sql(sql);
    }
}