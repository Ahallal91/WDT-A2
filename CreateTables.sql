if not exists (select * from Information_Schema.Tables where Table_Name = 'Customer')
begin
create table Customer
(
    CustomerID int not null,
    CustomerName nvarchar(50) not null,
    TFN int not null;
    Address nvarchar(50) not null,
    City nvarchar(40) not null,
    State nvarchar(3) not null,
    PostCode nvarchar(4) not null,
    Phone nvarchar(15) not null,
    constraint PK_Customer primary key (CustomerID)
);
end

if not exists (select * from Information_Schema.Tables where Table_Name = 'Logins')
begin
create table Logins
(
    LoginID nchar(8) not null,
    CustomerID int not null,
    PasswordHash nchar(64) not null,
    ModifyDate nvarchar(15) not null,
    constraint PK_Login primary key (LoginID),
    constraint FK_Login_Customer foreign key (CustomerID) references Customer (CustomerID),
    constraint CH_Login_LoginID check (len(LoginID) = 8),
    constraint CH_Login_PasswordHash check (len(PasswordHash) = 64)
)
end;

if not exists (select * from Information_Schema.Tables where Table_Name = 'Account')
begin
create table Account
(
    AccountNumber int not null,
    AccountType char not null,
    CustomerID int not null,
    Balance money not null,
    ModifyDate nvarchar(20) not null, 
    constraint PK_Account primary key (AccountNumber),
    constraint FK_Account_Customer foreign key (CustomerID) references Customer (CustomerID),
    constraint CH_Account_AccountType check (AccountType in ('C', 'S')),
    constraint CH_Account_Balance check (Balance >= 0)
)
end;