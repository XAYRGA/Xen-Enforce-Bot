
-- Dumping database structure for xenf_test
CREATE DATABASE IF NOT EXISTS `xenf_test` /*!40100 DEFAULT CHARACTER SET utf8mb4 */;
USE `xenf_test`;

-- Dumping structure for table xenf_test.blacklist
CREATE TABLE IF NOT EXISTS `blacklist` (
  `uid` int(11) NOT NULL AUTO_INCREMENT,
  `user` bigint(20) NOT NULL DEFAULT '0',
  `why` text NOT NULL,
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.

-- Dumping structure for table xenf_test.cleanup
CREATE TABLE IF NOT EXISTS `cleanup` (
  `uid` bigint(20) NOT NULL AUTO_INCREMENT,
  `group` bigint(20) NOT NULL DEFAULT '0',
  `mid` bigint(20) NOT NULL DEFAULT '0',
  `when` int(11) NOT NULL,
  `life` int(11) NOT NULL,
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.

-- Dumping structure for table xenf_test.configs
CREATE TABLE IF NOT EXISTS `configs` (
  `groupid` bigint(20) NOT NULL,
  `language` text NOT NULL,
  `attackmode` tinyint(1) NOT NULL DEFAULT '0',
  `smartdetect` tinyint(1) NOT NULL DEFAULT '0',
  `phraseban` tinyint(1) NOT NULL DEFAULT '0',
  `kicknohandle` tinyint(1) NOT NULL DEFAULT '0',
  `kicknoicons` tinyint(1) NOT NULL DEFAULT '0',
  `kickblacklisted` tinyint(1) NOT NULL DEFAULT '0',
  `kickunverifiedmedia` tinyint(1) NOT NULL DEFAULT '0',
  `verifymode` int(11) NOT NULL DEFAULT '1',
  `verifyannounce` tinyint(1) NOT NULL DEFAULT '1',
  `verifymute` tinyint(1) NOT NULL DEFAULT '0',
  `verifydelay` int(11) NOT NULL DEFAULT '30',
  `verifyask` text NOT NULL,
  `verifymessage` text NOT NULL,
  `mediadelay` tinyint(1) DEFAULT '0',
  `mediadelaytime` int(11) DEFAULT '30',
  PRIMARY KEY (`groupid`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Data exporting was unselected.

-- Dumping structure for table xenf_test.removals
CREATE TABLE IF NOT EXISTS `removals` (
  `uid` int(11) NOT NULL AUTO_INCREMENT,
  `user` bigint(20) NOT NULL DEFAULT '0',
  `group` bigint(20) DEFAULT NULL,
  `cause` text,
  `when` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.

-- Dumping structure for table xenf_test.verify
CREATE TABLE IF NOT EXISTS `verify` (
  `uid` int(11) NOT NULL AUTO_INCREMENT,
  `user` bigint(20) DEFAULT NULL,
  `group` bigint(20) DEFAULT NULL,
  `texpire` int(11) DEFAULT NULL,
  `tcreated` bigint(20) DEFAULT NULL,
  `tverified` bigint(20) DEFAULT '0',
  `verified` tinyint(1) DEFAULT '0',
  `notified` tinyint(1) DEFAULT '0',
  `trusted` tinyint(1) DEFAULT '0',
  `challenge` mediumtext,
  `message` bigint(20) DEFAULT '0',
  `joinmessage` bigint(20) DEFAULT '0',
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.

-- Dumping structure for table xenf_test.verify_doubt
CREATE TABLE IF NOT EXISTS `verify_doubt` (
  `uid` int(11) NOT NULL AUTO_INCREMENT,
  `user` bigint(20) DEFAULT NULL,
  `group` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.

