CREATE TABLE `adresses` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Street` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Neighborhood` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Number` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `City` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `State` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `product` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Quantity` int NOT NULL,
  `Price` decimal(10,2) NOT NULL,
  `StockMinimum` int NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `users` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Login` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Password` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `CPF` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Email` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Birthday` datetime(6) NOT NULL,
  `AddressId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Users_AddressId` (`AddressId`),
  CONSTRAINT `FK_Users_Adresses_AddressId` FOREIGN KEY (`AddressId`) REFERENCES `adresses` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `orders` (
  `Id` int NOT NULL AUTO_INCREMENT,livro
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

use dbcontext;
select * from adresses;SELECT `products`.`Id`,
    `products`.`Name`,
    `products`.`Description`,
    `products`.`Price`
FROM `cleancontext`.`products`;

select * from product;
select * from users;
select * from orders;
select * from item;