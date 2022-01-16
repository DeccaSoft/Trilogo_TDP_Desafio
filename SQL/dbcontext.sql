-- MySQL dump 10.13  Distrib 8.0.22, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: dbcontext
-- ------------------------------------------------------
-- Server version	8.0.22

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `adresses`
--

DROP TABLE IF EXISTS `adresses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `adresses` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Street` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Neighborhood` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Number` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `City` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `State` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `adresses`
--

LOCK TABLES `adresses` WRITE;
/*!40000 ALTER TABLE `adresses` DISABLE KEYS */;
INSERT INTO `adresses` VALUES (1,'Rua Tianguá','Parreão','13 - Apto. 106','Fortaleza','CE'),(2,'Rua Bonfim Sobrinho','Fátima','316 - Apto. 501','Fortaleza','CE'),(3,'Av. Luciano Carneiro','Vila União','2500 - Apto. 24 / D','Fortaleza','CE'),(4,'Rua Solon Pinheiro','Fátima','1525','Fortaleza','CE'),(5,'Rua CArlos Ribeiro','Fátima','531 - Apto. 104','Fortaleza','CE'),(6,'Rua 101','Conjunto Ceará','101','Fortaleza','CE');
/*!40000 ALTER TABLE `adresses` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `item`
--

DROP TABLE IF EXISTS `item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `item` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `OrderId` int NOT NULL,
  `ProductId` int NOT NULL,
  `Quantity` int NOT NULL,
  `Price` decimal(10,2) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Item_OrderId` (`OrderId`),
  CONSTRAINT `FK_Item_Orders_OrderId` FOREIGN KEY (`OrderId`) REFERENCES `orders` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `item`
--

LOCK TABLES `item` WRITE;
/*!40000 ALTER TABLE `item` DISABLE KEYS */;
INSERT INTO `item` VALUES (1,1,1,2,2.00),(2,1,2,4,6.00),(3,1,3,6,6.00),(4,2,4,1,0.50),(5,2,5,3,3.00),(6,3,1,1,1.00),(7,3,2,12,18.00),(8,3,3,2,2.00),(9,4,2,2,3.00);
/*!40000 ALTER TABLE `item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `orders`
--

DROP TABLE IF EXISTS `orders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
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
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orders`
--

LOCK TABLES `orders` WRITE;
/*!40000 ALTER TABLE `orders` DISABLE KEYS */;
INSERT INTO `orders` VALUES (1,1,14.00,'2022-01-06 21:06:10.965039',NULL,1,NULL,1),(2,2,3.50,'2022-01-06 21:06:19.511736','2022-01-15 17:33:16.455515',2,NULL,1),(3,3,21.00,'2022-01-06 21:06:26.166698',NULL,3,'2022-01-15 17:33:37.629560',1),(4,4,3.00,'2022-01-06 21:06:31.995612','2022-01-15 17:33:23.072569',2,NULL,1);
/*!40000 ALTER TABLE `orders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `product`
--

DROP TABLE IF EXISTS `product`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `product` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Quantity` int NOT NULL,
  `Price` decimal(10,2) NOT NULL,
  `StockMinimum` int NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `product`
--

LOCK TABLES `product` WRITE;
/*!40000 ALTER TABLE `product` DISABLE KEYS */;
INSERT INTO `product` VALUES (1,'Caneta','Azul',55,1.00,10),(2,'Caneta','Preta',67,1.50,10),(3,'Caneta','Vermelha',18,1.00,5),(4,'Lápis','Grafite',31,0.50,5),(5,'Lápis','Cor',43,1.00,15);
/*!40000 ALTER TABLE `product` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
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
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'andre','andre','$2a$11$lUPYE98OSgvvFFW85E1rdekdUyXA5yoFiLma4P7/XvwgRuYe4/zge','123','a@a.com','2022-01-06 23:58:30.512000','1975-08-18 23:58:30.512000','Admin',NULL),(2,'dani','dani','$2a$11$mBlwwqkUjGJuYx0t40ckAO5uiDvf3xT4PL8fRE0F1zy/6Jm6tftSu','456','d@d.com','2022-01-06 23:58:30.512000','1977-12-08 23:58:30.512000','Gerente',NULL),(3,'mateus','mateus','$2a$11$H7iLie3SAHbhIOzfNyjhEud5JiB4LX5eJNOXGXfwgc4S3k/aE9XnS','789','m@m.com','2022-01-06 23:58:30.512000','2005-07-18 23:58:30.512000','Funcionario',NULL),(4,'davi','davi','$2a$11$OKX/kIqAsUAcH..BdSlble26b6mMovWK6c2vtv3bVzKyNKDknsrQS','000','d2@d2.com','2022-01-06 23:58:30.512000','2007-11-22 23:58:30.512000',NULL,NULL),(5,'caio','caio','$2a$11$wACjaymsAEVnWdvFBAOkPukjj3HsAC54r5q1nMXyySOAmP9IDXV3S','999','c@c.com','2022-01-06 23:58:30.512000','2018-04-10 23:58:30.512000','Client',NULL),(6,'teste','teste','$2a$11$iBBGng8s.f3xGq07FA7bTuIB0Dbt7vSsPCsg4oD8AUCEPHVfRrcv6','555','t@t.com','2022-01-06 23:58:30.512000','2022-01-06 23:58:30.512000','',NULL),(7,'sete','sete','$2a$11$/oKGKvNoa415ZLuibzXl8emFp/5yhN2Fzx9MAXWfZbMVo0jA/IbyS','777','7@7.com','2007-07-07 19:18:27.775000','2022-01-15 19:18:27.775000','func',NULL);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'dbcontext'
--

--
-- Dumping routines for database 'dbcontext'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-01-16 18:33:22
