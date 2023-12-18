-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Dec 18, 2023 at 07:29 AM
-- Server version: 10.4.28-MariaDB
-- PHP Version: 8.0.28

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `venue_ms`
--

-- --------------------------------------------------------

--
-- Table structure for table `re_venue`
--

CREATE TABLE `re_venue` (
  `id` int(50) NOT NULL,
  `fname` varchar(50) NOT NULL,
  `lname` varchar(50) NOT NULL,
  `act_event` varchar(50) NOT NULL,
  `nature_event` varchar(50) NOT NULL,
  `venue` varchar(50) NOT NULL,
  `department` varchar(50) NOT NULL,
  `contact` bigint(50) NOT NULL,
  `start_date` varchar(50) NOT NULL,
  `end_date` varchar(50) NOT NULL,
  `start_time` varchar(50) NOT NULL,
  `end_time` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `re_venue`
--

INSERT INTO `re_venue` (`id`, `fname`, `lname`, `act_event`, `nature_event`, `venue`, `department`, `contact`, `start_date`, `end_date`, `start_time`, `end_time`) VALUES
(19, 'dasd', 'safasdsad', 'sadfasd', 'asdasdasd', 'basketball', 'ffgfdf', 246324, '29/11/2023', '27/11/2023', '10:00:00 am', '5:00:00 pm'),
(21, 'asdasd', 'asdasdasd', 'asdasdas', 'asdsad', 'Inner court', 'asdasd', 80769869, '29/11/2023', '27/11/2023', '6:00:00 pm', '10:30:00 pm'),
(22, 'asdasd', 'sdasdsd', 'dfgasdf', 'sdasd', 'Inner court', 'asdasd', 63453425, '29/11/2023', '27/11/2023', '11:00:00 pm', '12:00:00 am');

-- --------------------------------------------------------

--
-- Table structure for table `venue_table`
--

CREATE TABLE `venue_table` (
  `id` int(50) NOT NULL,
  `venue` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `venue_table`
--

INSERT INTO `venue_table` (`id`, `venue`) VALUES
(1, 'downtown'),
(3, 'pangaon'),
(4, 'Inner court'),
(5, 'basketball');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `re_venue`
--
ALTER TABLE `re_venue`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `venue_table`
--
ALTER TABLE `venue_table`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `re_venue`
--
ALTER TABLE `re_venue`
  MODIFY `id` int(50) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=24;

--
-- AUTO_INCREMENT for table `venue_table`
--
ALTER TABLE `venue_table`
  MODIFY `id` int(50) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
