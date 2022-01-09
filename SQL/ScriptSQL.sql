CREATE DATABASE `dbcontext` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;

USE dbcontext;

CREATE TABLE `adresses` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Street` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Neighborhood` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Number` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `City` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `State` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `product` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Quantity` int NOT NULL,
  `Price` decimal(10,2) NOT NULL,
  `StockMinimum` int NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `users` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Login` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Password` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CPF` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Email` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreationDate` datetime(6) NOT NULL,
  `Birthday` datetime(6) NOT NULL,
  `Role` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `AddressId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Users_AddressId` (`AddressId`),
  CONSTRAINT `FK_Users_Adresses_AddressId` FOREIGN KEY (`AddressId`) REFERENCES `adresses` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `orders` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `TotalValue` decimal(10,2) NOT NULL,
  `CreationDate` datetime(6) NOT NULL,
  `CancelDate` datetime(6) DEFAULT NULL,
  `Status` int NOT NULL,
  `FinishedDate` datetime(6) DEFAULT NULL,
  `Payment` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Orders_UserId` (`UserId`),
  CONSTRAINT `FK_Orders_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `item` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `OrderId` int NOT NULL,
  `ProductId` int NOT NULL,
  `Quantity` int NOT NULL,
  `Price` decimal(10,2) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Item_OrderId` (`OrderId`),
  CONSTRAINT `FK_Item_Orders_OrderId` FOREIGN KEY (`OrderId`) REFERENCES `orders` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

SELECT * FROM adresses;
SELECT * FROM product;
SELECT * FROM users;
SELECT * FROM orders;
SELECT * FROM item;





