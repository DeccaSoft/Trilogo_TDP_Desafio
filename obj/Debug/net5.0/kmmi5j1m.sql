ALTER DATABASE CHARACTER SET utf8mb4;
CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Adresses` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Street` longtext CHARACTER SET utf8mb4 NULL,
    `Neighborhood` longtext CHARACTER SET utf8mb4 NULL,
    `Number` longtext CHARACTER SET utf8mb4 NULL,
    `City` longtext CHARACTER SET utf8mb4 NULL,
    `State` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_Adresses` PRIMARY KEY (`Id`)
) CHARACTER SET utf8mb4;

CREATE TABLE `Product` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` longtext CHARACTER SET utf8mb4 NULL,
    `Description` longtext CHARACTER SET utf8mb4 NULL,
    `Quantity` int NOT NULL,
    `decimal(10, 2)` decimal(65,30) NOT NULL,
    `StockMinimum` int NOT NULL,
    CONSTRAINT `PK_Product` PRIMARY KEY (`Id`)
) CHARACTER SET utf8mb4;

CREATE TABLE `Users` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Login` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Password` longtext CHARACTER SET utf8mb4 NOT NULL,
    `CPF` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Email` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Birthday` datetime(6) NOT NULL,
    `AddressId` int NULL,
    CONSTRAINT `PK_Users` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Users_Adresses_AddressId` FOREIGN KEY (`AddressId`) REFERENCES `Adresses` (`Id`) ON DELETE RESTRICT
) CHARACTER SET utf8mb4;

CREATE TABLE `Orders` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` int NOT NULL,
    `decimal(10, 2)` decimal(65,30) NOT NULL,
    `CreationDate` datetime(6) NOT NULL,
    `CancelDate` datetime(6) NULL,
    `Status` int NOT NULL,
    `FinishedDate` datetime(6) NULL,
    `Payment` int NOT NULL,
    CONSTRAINT `PK_Orders` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Orders_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) CHARACTER SET utf8mb4;

CREATE TABLE `Item` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `OrderId` int NOT NULL,
    `ProductId` int NOT NULL,
    `Quantity` int NOT NULL,
    `decimal(10, 2)` decimal(65,30) NOT NULL,
    CONSTRAINT `PK_Item` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Item_Orders_OrderId` FOREIGN KEY (`OrderId`) REFERENCES `Orders` (`Id`) ON DELETE CASCADE
) CHARACTER SET utf8mb4;

CREATE INDEX `IX_Item_OrderId` ON `Item` (`OrderId`);

CREATE INDEX `IX_Orders_UserId` ON `Orders` (`UserId`);

CREATE INDEX `IX_Users_AddressId` ON `Users` (`AddressId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20211227221304_PrimeiraMigracao', '5.0.5');

COMMIT;

START TRANSACTION;

ALTER TABLE `Product` RENAME COLUMN `decimal(10, 2)` TO `Price`;

ALTER TABLE `Orders` RENAME COLUMN `decimal(10, 2)` TO `TotalValue`;

ALTER TABLE `Item` RENAME COLUMN `decimal(10, 2)` TO `Price`;

ALTER TABLE `Users` ADD `CreationDate` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00';

ALTER TABLE `Users` ADD `Role` longtext CHARACTER SET utf8mb4 NULL;

ALTER TABLE `Product` MODIFY COLUMN `Price` decimal(10,2) NOT NULL;

ALTER TABLE `Orders` MODIFY COLUMN `TotalValue` decimal(10,2) NOT NULL;

ALTER TABLE `Item` MODIFY COLUMN `Price` decimal(10,2) NOT NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20220106231325_ScriptMigration', '5.0.5');

COMMIT;

