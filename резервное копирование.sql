-- MySQL dump 10.13  Distrib 8.0.36, for Win64 (x86_64)
--
-- Host: localhost    Database: demo
-- ------------------------------------------------------
-- Server version	8.3.0

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
-- Table structure for table `catalog_orders`
--

DROP TABLE IF EXISTS `catalog_orders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `catalog_orders` (
  `id_catalog_orders` int NOT NULL AUTO_INCREMENT,
  `id_orders` int DEFAULT NULL,
  `id_employee` int DEFAULT NULL,
  PRIMARY KEY (`id_catalog_orders`),
  KEY `FK_id_employee_idx` (`id_employee`),
  KEY `FK_id_orders_idx` (`id_orders`),
  CONSTRAINT `FK_id_employee` FOREIGN KEY (`id_employee`) REFERENCES `human` (`id_user`),
  CONSTRAINT `FK_id_orders` FOREIGN KEY (`id_orders`) REFERENCES `order` (`id_application`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `catalog_orders`
--

LOCK TABLES `catalog_orders` WRITE;
/*!40000 ALTER TABLE `catalog_orders` DISABLE KEYS */;
INSERT INTO `catalog_orders` VALUES (1,1,3),(2,2,4),(3,3,5),(4,4,6),(5,5,7),(6,6,8),(7,7,9),(8,8,10),(9,9,3),(10,10,4),(11,4,3),(12,5,3),(13,6,6),(14,4,7);
/*!40000 ALTER TABLE `catalog_orders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `human`
--

DROP TABLE IF EXISTS `human`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `human` (
  `id_user` int NOT NULL AUTO_INCREMENT,
  `login` varchar(20) DEFAULT NULL,
  `password_user` varchar(64) DEFAULT NULL,
  `fullname_user` varchar(100) DEFAULT NULL,
  `role_user` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id_user`)
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `human`
--

LOCK TABLES `human` WRITE;
/*!40000 ALTER TABLE `human` DISABLE KEYS */;
INSERT INTO `human` VALUES (1,'login1@gmail.com','password1','Иванов Иван Иванович','Диспетчер'),(2,'login2@gmail.com','password2','Сидеренко Михаил Владимирович','Диспетчер'),(3,'login3@gmail.com','password3','Салоха Петр Ильич','Исполнитель'),(4,'login4@gmail.com','password4','Гниденко Анна Викторовна','Исполнитель'),(5,'login5@gmail.com','password5','Альметова Владлена Серьгеевна ','Исполнитель'),(6,'login6@gmail.com','password6','Ханбаев Артур Анварович','Исполнитель'),(7,'login7@gmail.com','password7','Туренков Анастия Рифовна','Исполнитель'),(8,'login8@gmail.com','password8','Тувошов Владимир Алексеевич ','Исполнитель'),(9,'login9@gmail.com','password9','Голоша Андрей Егорович','Исполнитель'),(10,'login10@gmail.com','password10','Хамданов Магомед Махинкамалович','Исполнитель'),(11,'login11@gmail.com','password11','Кулиев Шамиль Назарович','Исполнитель'),(13,'Walkerbe','1234567890','Мустафаев Аким Ринатович','Исполнитель'),(14,'Nagibator3000','Nagibator3000','Новиков Илья Павлович','Исполнитель'),(15,'Walkerbe1','BlItrRkU','Мустафаев Аким Ринатович','Менеджер'),(19,'Wapud','Wapud','Сарваров Камаль Альбертович','Исполнитель');
/*!40000 ALTER TABLE `human` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `order`
--

DROP TABLE IF EXISTS `order`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `order` (
  `id_application` int NOT NULL AUTO_INCREMENT,
  `date_addition` date DEFAULT NULL,
  `name_equipment` varchar(45) DEFAULT NULL,
  `id_type_problem` int DEFAULT NULL,
  `comment_application` text,
  `status` varchar(45) DEFAULT NULL,
  `name_client` varchar(45) DEFAULT NULL,
  `cost` decimal(10,2) DEFAULT NULL,
  `date_end` date DEFAULT NULL,
  `time_work` time DEFAULT NULL,
  `work_status` varchar(45) DEFAULT NULL,
  `period_execution` date DEFAULT NULL COMMENT 'предварительная дата завершения',
  `id_type_equipment` int DEFAULT NULL,
  `serial_number` int DEFAULT NULL COMMENT 'серийный номер',
  `description_application` text,
  PRIMARY KEY (`id_application`),
  KEY `Fk_IdStageWork_idx` (`work_status`),
  KEY `Fk_id_type_problem_idx` (`id_type_problem`),
  KEY `Fk_id_type_equipment_idx` (`id_type_equipment`),
  CONSTRAINT `Fk_id_type_equipment` FOREIGN KEY (`id_type_equipment`) REFERENCES `type_equipment` (`id_type_equipment`),
  CONSTRAINT `Fk_id_type_problem` FOREIGN KEY (`id_type_problem`) REFERENCES `type_problem` (`id_type_problem`)
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `order`
--

LOCK TABLES `order` WRITE;
/*!40000 ALTER TABLE `order` DISABLE KEYS */;
INSERT INTO `order` VALUES (1,'2024-02-05','Asus ноутбук',1,'Куплена матрица клавиатуры. Причина: износ и не бережное обращение','Выполнено','Клиент1',5200.00,'2024-02-10','10:00:00','Выполнено','2024-02-10',1,843828484,'Залипла кнопка ESC'),(2,'2024-02-06','Samsung холодильник',2,'Куплена ручка холодильника, крепление ручки. Причина: износ','Выполнено','Клиент2',1400.00,'2024-02-11','02:00:00','Выполнено','2024-02-11',2,93765,'Оторвана ручка'),(3,'2024-02-07','Apple телефон',2,'Куплено матрица камеры, стекло камеры, клей. Причина: сильный удар.','Выполнено','Клиент3',3700.00,'2024-02-12','04:00:00','Выполнено','2024-02-12',3,82545,'Разбито стекло камеры'),(4,'2024-02-07','J200 утюг',2,NULL,'В работе','Клиент4',0.00,NULL,NULL,'Не выполнено','2024-02-15',4,98263,'Поврежден кабель питания'),(5,'2024-02-09','LG стиральная машина',3,NULL,'В работе','Клиент5',0.00,NULL,NULL,'В работе','2024-02-14',5,48576,'Не включается'),(6,'2024-02-10','LG телевизор',3,NULL,'В работе','Клиент6',0.00,NULL,NULL,'В работе','2024-02-15',6,95683,'Горит белый экран'),(7,'2024-02-12','Minitech микроволновка',5,'','В ожидании','Клиент7',0.00,NULL,NULL,'Не выполнено','2024-02-17',7,57902,'Периодически не греет'),(8,'2024-02-13','Sony приставка',5,NULL,'В ожидании','Клиент8',0.00,NULL,'18:15:00','Не выполнено','2024-02-18',8,34759,'Периодически выключается'),(9,'2024-02-14','Dayson фен',5,'1234','В ожидании','Клиент9',1.00,'2024-03-23','21:15:00','В работе','2024-02-19',9,59237,'Периодически не греет'),(10,'2024-02-15','Apple плашнет',4,NULL,'В ожидании','Клиент10',0.00,NULL,NULL,'Не выполнено','2024-02-20',10,48507,'Не включается');
/*!40000 ALTER TABLE `order` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `type_equipment`
--

DROP TABLE IF EXISTS `type_equipment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `type_equipment` (
  `id_type_equipment` int NOT NULL,
  `name_type_equipment` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id_type_equipment`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `type_equipment`
--

LOCK TABLES `type_equipment` WRITE;
/*!40000 ALTER TABLE `type_equipment` DISABLE KEYS */;
INSERT INTO `type_equipment` VALUES (1,'Ноутбук'),(2,'Холодильник'),(3,'Телефон'),(4,'Утюг'),(5,'Стиральная машина'),(6,'Телевизор'),(7,'Микроволновка '),(8,'Приставка'),(9,'Фен'),(10,'Планшет');
/*!40000 ALTER TABLE `type_equipment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `type_problem`
--

DROP TABLE IF EXISTS `type_problem`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `type_problem` (
  `id_type_problem` int NOT NULL AUTO_INCREMENT COMMENT 'тип неисправностей',
  `name_type_problem` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id_type_problem`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `type_problem`
--

LOCK TABLES `type_problem` WRITE;
/*!40000 ALTER TABLE `type_problem` DISABLE KEYS */;
INSERT INTO `type_problem` VALUES (1,'Небольшое повреждение'),(2,'Повреждение'),(3,'Нарушение функционирования'),(4,'Отказ'),(5,'Периодические сбои');
/*!40000 ALTER TABLE `type_problem` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-04-04 16:29:16
