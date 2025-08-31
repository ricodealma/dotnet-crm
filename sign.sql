CREATE DATABASE IF NOT EXISTS sign CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE sign;

CREATE TABLE ContractStatus (
    Id INT NOT NULL PRIMARY KEY,
    Description VARCHAR(100) NOT NULL
);

CREATE TABLE Contract (
    Id VARCHAR(64) NOT NULL PRIMARY KEY,
    ProposalId VARCHAR(64) NOT NULL,
    Content VARCHAR(2000) NOT NULL,
    StatusId INT NOT NULL,
    CONSTRAINT FK_Contract_Status FOREIGN KEY (StatusId) REFERENCES ContractStatus(Id)
);

INSERT INTO ContractStatus (Id, Description) VALUES
(1, 'AwaitingSignature'),
(2, 'Signed');
