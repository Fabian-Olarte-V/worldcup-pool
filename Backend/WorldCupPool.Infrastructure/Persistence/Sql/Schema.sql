CREATE TABLE
    `SoccerTeams` (
        `Id` CHAR(36) COLLATE ascii_general_ci NOT NULL,
        `Name` VARCHAR(150) NOT NULL,
        `Code` VARCHAR(10) NOT NULL,
        `GroupName` VARCHAR(10) NOT NULL,
        `FlagUrl` VARCHAR(500) NULL,
        `CreatedAtUtc` DATETIME (6) NOT NULL,
        CONSTRAINT `PK_SoccerTeams` PRIMARY KEY (`Id`)
    ) CHARACTER
SET
    utf8mb4;

CREATE TABLE
    `Users` (
        `Id` CHAR(36) COLLATE ascii_general_ci NOT NULL,
        `FirstName` VARCHAR(100) NOT NULL,
        `LastName` VARCHAR(100) NOT NULL,
        `Email` VARCHAR(256) NOT NULL,
        `UserName` VARCHAR(100) NOT NULL,
        `PasswordHash` VARCHAR(500) NOT NULL,
        `RefreshToken` VARCHAR(200) NULL,
        `Role` VARCHAR(20) NOT NULL,
        `CreatedAtUtc` DATETIME (6) NOT NULL,
        CONSTRAINT `PK_Users` PRIMARY KEY (`Id`)
    ) CHARACTER
SET
    utf8mb4;

CREATE TABLE
    `Matches` (
        `Id` CHAR(36) COLLATE ascii_general_ci NOT NULL,
        `GroupName` VARCHAR(10) NOT NULL,
        `HomeTeamId` CHAR(36) COLLATE ascii_general_ci NOT NULL,
        `AwayTeamId` CHAR(36) COLLATE ascii_general_ci NOT NULL,
        `Status` VARCHAR(20) NOT NULL,
        `StartTimeUtc` DATETIME (6) NOT NULL,
        `CreatedAtUtc` DATETIME (6) NOT NULL,
        CONSTRAINT `PK_Matches` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Matches_SoccerTeams_HomeTeamId` FOREIGN KEY (`HomeTeamId`) REFERENCES `SoccerTeams` (`Id`) ON DELETE RESTRICT,
        CONSTRAINT `FK_Matches_SoccerTeams_AwayTeamId` FOREIGN KEY (`AwayTeamId`) REFERENCES `SoccerTeams` (`Id`) ON DELETE RESTRICT
    ) CHARACTER
SET
    utf8mb4;

CREATE TABLE
    `MatchResults` (
        `Id` CHAR(36) COLLATE ascii_general_ci NOT NULL,
        `MatchId` CHAR(36) COLLATE ascii_general_ci NOT NULL,
        `HomeGoals` INT NOT NULL,
        `AwayGoals` INT NOT NULL,
        `CreatedAtUtc` DATETIME (6) NOT NULL,
        `UpdatedAtUtc` DATETIME (6) NULL,
        CONSTRAINT `PK_MatchResults` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_MatchResults_Matches_MatchId` FOREIGN KEY (`MatchId`) REFERENCES `Matches` (`Id`) ON DELETE CASCADE
    ) CHARACTER
SET
    utf8mb4;

CREATE TABLE
    `Predictions` (
        `Id` CHAR(36) COLLATE ascii_general_ci NOT NULL,
        `UserId` CHAR(36) COLLATE ascii_general_ci NOT NULL,
        `MatchId` CHAR(36) COLLATE ascii_general_ci NOT NULL,
        `HomeGoals` INT NOT NULL,
        `AwayGoals` INT NOT NULL,
        `Points` INT NULL,
        `Status` INT NOT NULL,
        `CreatedAtUtc` DATETIME (6) NOT NULL,
        `UpdatedAtUtc` DATETIME (6) NULL,
        CONSTRAINT `PK_Predictions` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Predictions_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_Predictions_Matches_MatchId` FOREIGN KEY (`MatchId`) REFERENCES `Matches` (`Id`) ON DELETE CASCADE
    ) CHARACTER
SET
    utf8mb4;