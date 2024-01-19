-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jan 19, 2024 at 06:35 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

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
-- Table structure for table `department`
--

CREATE TABLE `department` (
  `id` int(100) NOT NULL,
  `dep` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `department`
--

INSERT INTO `department` (`id`, `dep`) VALUES
(6, 'COLLEGE OF ENGINEERING'),
(7, 'COLLEGE OF ART AND SCIENCE'),
(8, 'COLLEGE OF EDUCATION'),
(9, 'COLLEGE OF TECHNOLOGY');

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
  `contact` varchar(50) DEFAULT NULL,
  `start_date` varchar(50) NOT NULL,
  `end_date` varchar(50) NOT NULL,
  `start_time` varchar(50) NOT NULL,
  `end_time` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `re_venue`
--

INSERT INTO `re_venue` (`id`, `fname`, `lname`, `act_event`, `nature_event`, `venue`, `department`, `contact`, `start_date`, `end_date`, `start_time`, `end_time`) VALUES
(57, 'Elisia', 'Abar', 'Dance', 'Sayaw ss', 'Inner Court', 'COE', '9972414803', '19-01-2024', '19-01-2024', '10:30:00 am', '11:00:00 am'),
(59, 'sam', 'falguera', 'valentine', 'social', 'GS Function Room', 'COLLEGE OF ENGINEERING', '9435464575', '14-01-2024', '14-01-2024', '9:00:00 am', '4:00:00 pm'),
(60, 'Jordan', 'Chan', 'Meeting', 'Social', 'ORDEX', 'COLLEGE OF ENGINEERING', '9124816249', '11-01-2024', '11-01-2024', '9:00:00 AM', '11:00:00 AM'),
(61, 'Jelly', 'Ace', 'Snack', 'Social', 'ORDEX', 'COLLEGE OF ART AND SCIENCE', '9213625612', '11-01-2024', '11-01-2024', '1:00:00 PM', '5:00:00 PM'),
(63, 'Gehlee', 'Acee', 'Basketball', 'Social', 'Inner Court', 'COLLEGE OF ENGINEERING', '9216448214', '14-02-2024', '14-02-2024', '9:00:00 AM', '11:00:00 AM'),
(64, 'Ambot', 'Ngahaw', 'Basketball', 'Social', 'Inner Court', 'COLLEGE OF ENGINEERING', '9128401628', '20-02-2024', '20-02-2024', '9:00:00 AM', '11:00:00 AM'),
(65, 'Jello', 'Asus', 'Graduation', 'Social', 'Inner Court', 'COLLEGE OF TECHNOLOGY', '9126416428', '20-02-2024', '20-02-2024', '1:00:00 PM', '8:00:00 PM'),
(66, 'Jordan', 'Esmale', 'Sayaw', 'Social', 'Auditorium', 'COLLEGE OF ART AND SCIENCE', '9128641262', '20-02-2024', '20-02-2024', '8:00:00 AM', '8:00:00 PM'),
(67, 'Joshua', 'Garcia', 'Meeting', 'Social', 'ORDEX', 'COLLEGE OF ENGINEERING', '9126428214', '17-01-2024', '17-01-2024', '8:00:00 AM', '11:00:00 AM'),
(68, 'JAJJA', 'KJBSFIU', 'AHLHS', 'asdasd', 'ORDEX', 'COLLEGE OF ART AND SCIENCE', '9210741826', '17-01-2024', '17-01-2024', '12:00:00 PM', '5:00:00 AM'),
(70, 'mncxbvkzjasd', 'asdzsfcas', 'KHADLHAd', 'asdasd', 'ORDEX', 'COLLEGE OF ART AND SCIENCE', '9218621424', '20-01-2024', '20-01-2024', '9:00:00 AM', '11:00:00 AM'),
(72, 'zvbrqwrqwa', 'xrevowrxb', 'hdsskasdhksad', 'asdasdasd', 'ORDEX', 'COLLEGE OF ART AND SCIENCE', '9149612424', '20-01-2024', '20-01-2024', '11:01:00 AM', '5:00:00 PM'),
(76, 'hhrer', 'cbrww', 'qwewqr', 'xzvfsa', 'ORDEX', 'COLLEGE OF ENGINEERING', '9235125326', '24-01-2024', '24-01-2024', '8:00:00 AM', '12:00:00 PM'),
(77, 'xcbvxcr', 'bnnter', 'zcvwer', 'xcvxcv', 'ORDEX', 'COLLEGE OF ART AND SCIENCE', '9231241251', '24-01-2024', '24-01-2024', '12:01:00 PM', '5:00:00 PM'),
(78, 'zxczxv', 'vcbsdgs', 'akndas', 'zxczxc', 'ORDEX', 'COLLEGE OF ENGINEERING', '9196422124', '25-01-2024', '25-01-2024', '8:00:00 AM', '11:00:00 AM'),
(79, 'xcvxdv', 'ghjg', 'Sdasdasd', 'dszfdf', 'ORDEX', 'COLLEGE OF ART AND SCIENCE', '9823122425', '25-01-2024', '25-01-2024', '1:00:00 PM', '5:00:00 PM'),
(80, 'aetasd', 'cvxzd', 'zxczxc', 'xcvzx', 'ORDEX', 'COLLEGE OF ENGINEERING', '9124234786', '26-01-2024', '26-01-2024', '8:00:00 AM', '11:00:00 AM'),
(82, 'ffdgter', 'cxbvset', 'afdqwr', 'wqrqwr', 'ORDEX', 'COLLEGE OF ART AND SCIENCE', '9123412512', '26-01-2024', '26-01-2024', '12:00:00 PM', '5:00:00 AM'),
(83, 'ter', 'zwrer', 'asdjkbad', 'zxcw', 'Inner Court', 'COLLEGE OF ENGINEERING', '9757657436', '01-02-2024', '01-02-2024', '8:00:00 AM', '11:00:00 AM'),
(84, 'juik', 'qwd', 'ye', 'cc', 'Inner Court', 'COLLEGE OF ART AND SCIENCE', '9124567322', '01-02-2024', '01-02-2024', '1:00:00 PM', '5:00:00 AM'),
(85, 'hehe', 'zxz', 'erewr', 'fgfdg', 'Inner Court', 'COLLEGE OF EDUCATION', '9235461112', '01-02-2024', '01-02-2024', '12:16:03 PM', '12:59:03 PM'),
(86, 'sfwawe', 'cvsedfs', 'asdasdasd', 'asdasd', 'ORDEX', 'COLLEGE OF TECHNOLOGY', '9235124124', '01-02-2024', '01-02-2024', '8:00:00 AM', '12:00:00 PM'),
(87, 'qty', 'geh', 'qqe', 'vwe', 'ORDEX', 'COLLEGE OF EDUCATION', '9234351712', '01-02-2024', '01-02-2024', '12:50:27 PM', '5:00:00 PM'),
(88, 'Ad', 'xc', 'asdasd', 'zxcwa', 'ORDEX', 'COLLEGE OF ART AND SCIENCE', '9235235235', '22-01-2024', '22-01-2024', '8:00:00 AM', '11:00:00 AM'),
(89, 'hree', 'etf', 'zxc', 'zvzx', 'ORDEX', 'COLLEGE OF ENGINEERING', '9475432312', '22-01-2024', '22-01-2024', '12:00:00 PM', '5:00:00 PM'),
(90, 'xzxc', 'qer', 'yfufhfdt', 'sdfsdf', 'ORDEX', 'COLLEGE OF ENGINEERING', '09232342341', '05-02-2024', '05-02-2024', '8:00:00 AM', '5:00:00 PM'),
(91, 'asd', 'xca', 'asdasd', 'zxcawd', 'ORDEX', 'COLLEGE OF ENGINEERING', '09164296425', '30-01-2024', '30-01-2024', '8:00:00 AM', '11:00:00 AM'),
(92, 'btbtbt', 'wewe', 'asdasdxz', 'cxc', 'ORDEX', 'COLLEGE OF ART AND SCIENCE', '09567456214', '30-01-2024', '30-01-2024', '11:00:00 AM', '1:00:00 PM');

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
(8, 'ORDEX'),
(9, 'GS Function Room'),
(10, 'Inner Court'),
(11, 'Auditorium');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `department`
--
ALTER TABLE `department`
  ADD PRIMARY KEY (`id`);

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
-- AUTO_INCREMENT for table `department`
--
ALTER TABLE `department`
  MODIFY `id` int(100) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT for table `re_venue`
--
ALTER TABLE `re_venue`
  MODIFY `id` int(50) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=93;

--
-- AUTO_INCREMENT for table `venue_table`
--
ALTER TABLE `venue_table`
  MODIFY `id` int(50) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
