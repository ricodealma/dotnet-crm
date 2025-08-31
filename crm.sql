CREATE DATABASE IF NOT EXISTS crm CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE crm;

CREATE TABLE Client (
    Id VARCHAR(64) NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL,
    Email VARCHAR(200) NOT NULL,
    Company VARCHAR(200) NOT NULL
);

CREATE TABLE ProposalStatus (
    Id INT NOT NULL PRIMARY KEY,
    Description VARCHAR(100) NOT NULL
);

CREATE TABLE Proposal (
    Id VARCHAR(64) NOT NULL PRIMARY KEY,
    ClientId VARCHAR(64) NOT NULL,
    StatusId INT NOT NULL,
    CONSTRAINT FK_Proposal_Client FOREIGN KEY (ClientId) REFERENCES Client(Id),
    CONSTRAINT FK_Proposal_Status FOREIGN KEY (StatusId) REFERENCES ProposalStatus(Id)
);

CREATE TABLE Items (
    Id VARCHAR(64) NOT NULL PRIMARY KEY,
    ProposalId VARCHAR(64) NOT NULL,
    Name VARCHAR(200) NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice FLOAT NOT NULL,
    CONSTRAINT FK_Items_Proposal FOREIGN KEY (ProposalId) REFERENCES Proposal(Id)
);

INSERT INTO ProposalStatus (Id, Description) VALUES
(1, 'Draft'),
(2, 'SentForSignature'),
(3, 'Signed');
