create database Petzilla;


use Petzilla;

create table USER_REGISTRATION(
USER_ID int primary key identity(1,1),
FIRST_NAME varchar(255)NOT NULL,
LAST_NAME varchar(255)NOT NULL,
USER_NAME varchar(255)NOT NULL,
USER_EMAIL varchar(255)NOT NULL,
USER_PASSWORD varchar(255)NOT NULL,
USER_ROLE varchar(255),
);


select * from USER_REGISTRATION
 
insert into USER_REGISTRATION VALUES ('admin','user','admin','admin@gmail.com','admin786!','ADMIN');

----dynamic dropdown work----
CREATE TABLE PetCategory (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    ActionName NVARCHAR(100) NOT NULL
);

select * from PetCategory
 
INSERT INTO PetCategory (Name, ActionName) 
VALUES 
('Home Pets', 'PetDetails'),
('Stray Pets', 'StrayPetDetails');


CREATE TABLE Pets (
    PetId INT PRIMARY KEY IDENTITY(1,1),
    PetName NVARCHAR(100) NOT NULL,
    Category NVARCHAR(100) NOT NULL,
    Age INT NULL,
    Description NVARCHAR(500) NULL,
    ImageUrl NVARCHAR(255) NULL,
    IsAvailable BIT NULL DEFAULT 1,
    ImagePath NVARCHAR(255) NULL
);

CREATE TABLE AdoptionRequestsHome (
    RequestId INT PRIMARY KEY IDENTITY,
    PetId INT,
    RequesterName NVARCHAR(100) NOT NULL,
    RequesterEmail NVARCHAR(150) NOT NULL,
    RequesterPhone NVARCHAR(50) NOT NULL,
    RequesterAddress NVARCHAR(255),
    Reason NVARCHAR(MAX),
    Status NVARCHAR(20) DEFAULT 'pending',
    RequestedOn DATETIME DEFAULT GETDATE(),
	--PetType NVARCHAR(100)
    FOREIGN KEY (PetId) REFERENCES Pets(PetId)
);

select * from AdoptionRequestsHome;
drop table AdoptionRequestsHome


CREATE TABLE AdoptionRequestsStray (
    RequestId INT PRIMARY KEY IDENTITY,
    PetId INT,
    RequesterName NVARCHAR(100) NOT NULL,
    RequesterEmail NVARCHAR(150) NOT NULL,
    RequesterPhone NVARCHAR(50) NOT NULL,
    RequesterAddress NVARCHAR(255),
    Reason NVARCHAR(MAX),
    Status NVARCHAR(20) DEFAULT 'pending',
    RequestedOn DATETIME DEFAULT GETDATE(),
	--PetType NVARCHAR(100)
    FOREIGN KEY (PetId) REFERENCES PetsStray(PetId)
);

select * from AdoptionRequestsStray;
drop table AdoptionRequestsStray

Select*From Pets


CREATE TABLE PetsStray (
    PetId INT PRIMARY KEY IDENTITY(1,1),
    Category NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500) NULL,
    ImageUrl NVARCHAR(255) NULL,
    IsAvailable BIT NULL DEFAULT 1,
    ImagePath NVARCHAR(255) NULL
);

Select*From PetsStray


SELECT * FROM Pets;

select * from AdoptionRequests;


alter TABLE PetsStray 

DROP COLUMN PetType;

--contact us---

CREATE TABLE ContactMessage(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Mobile NVARCHAR(20),
    Subject NVARCHAR(150),
    Message NVARCHAR(MAX),
    SubmittedAt DATETIME DEFAULT GETDATE()
);
select * from ContactMessage
 

--About us Work---

CREATE TABLE AboutSections (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(200),
    Description NVARCHAR(MAX),
    ImagePath NVARCHAR(300),
    IconClass NVARCHAR(100),
    ServiceTitle NVARCHAR(200),
    ServiceText NVARCHAR(MAX)
);
select * from AboutSections
 
 

----Reviews work----
 CREATE TABLE Reviews (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Text NVARCHAR(MAX),
    Name NVARCHAR(100),
    Position NVARCHAR(100),
    ImageUrl NVARCHAR(255)
); 
Select * from Reviews;
 
 

---FAQs------
CREATE TABLE Faqs (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Question NVARCHAR(500) NOT NULL,
    Answer NVARCHAR(MAX) NOT NULL
);
Select * from Faqs;
 
---NGOs----
CREATE TABLE Ngos (
    NgoId INT IDENTITY(1,1) PRIMARY KEY,
    NgoName NVARCHAR(200) NOT NULL,
    Email NVARCHAR(200) NOT NULL UNIQUE,
    PhoneNumber NVARCHAR(50) NOT NULL,
    Address NVARCHAR(300) NOT NULL,
    Branches NVARCHAR(300) NULL,
    AvailabilityStatus BIT NOT NULL DEFAULT 1,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending',
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NULL
);

 

select * from Ngos
drop table Ngos
 
 

CREATE TABLE PetCareGuidelines (
    GuidelineId INT PRIMARY KEY IDENTITY(1,1),
    PetId INT FOREIGN KEY REFERENCES Pets(PetId),
    Food NVARCHAR(255),
    Behavior NVARCHAR(500),
    IsKidFriendly BIT,
    Precautions NVARCHAR(500)
);

INSERT INTO PetCareGuidelines (Food, Behavior, IsKidFriendly, Precautions)
VALUES 
('Dry kibble, twice a day', 'Very friendly and playful', 1, 'Avoid chocolate, small toys'),
('Wet food in morning, dry food at night', 'Anxious with strangers', 0, 'Keep away from loud noises');



select * from AdoptionRequests
