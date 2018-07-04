CREATE TABLE "Order" (
	OrderId serial,
	UserId integer,
	Amount numeric(26, 10),
	Status varchar(35),
	Entity varchar(10),
	OpenTimeStamp timestamp,
	ExecutedAmount numeric(26, 10),
	SettledAmount numeric(26, 10),
	Reference varchar(50),
	CONSTRAINT PK_Order PRIMARY KEY (OrderId)
);

CREATE TABLE UserMoneyTransfer (
	TransferId serial,
	UserId integer,
	UserAccount varchar(50),
	Status varchar(35),
	Amount numeric(26, 2),
	Currency char(3),
	OpenTimeStamp timestamp,
	Reference varchar(50),
	CONSTRAINT PK_UserMoneyTransfer PRIMARY KEY (TransferId)
);


ALTER TABLE UserMoneyTransfer ADD CONSTRAINT AK_Reference UNIQUE (REference);


CREATE TABLE MarketMoneyTransfer (
	TransferId serial,
	Market varchar(50),
	Status varchar(50),
	Amount numeric(26, 2),
	Currency char(3),
	OpenTimeStamp timestamp,
	InReference varchar(50),
	OutReference varchar(50),
	CONSTRAINT PK_MarketMoneyTransfer PRIMARY KEY (TransferId)
);

CREATE TABLE Withdrawal (
	WithdrawalId serial,
	UserId integer,
	Status varchar(35),
	ToName varchar(250),
	ToIban char(23),
	ToBic char(8),
	ToAddress varchar(250),
	ToPostalCode varchar(250),
	ToCity varchar(250),
	ToCountryCode char(2),
	Amount numeric(26, 2),
	Currency char(3),
	OpenTimeStamp timestamp,
	InReference varchar(50),
	OutReference varchar(50),
	CONSTRAINT PK_Withdrawal PRIMARY KEY (WithdrawalId)
);

CREATE TABLE UserMoneyTransferTask (
	TaskId serial,
	UserId integer,
	UserAccount varchar(50),
	Status varchar(35),
	Amount numeric(26, 2),
	Currency char(3),
	Inserted timestamp,
	Executed timestamp,
	Reference varchar(50),
    BankTransactionId varchar(50),
	CONSTRAINT PK_UserMoneyTransferTask PRIMARY KEY (TaskId)
);



CREATE TABLE UserMoneyTransferTT (
	TaskId int references UserMoneyTransferTask(TaskId),
	TransferId int references UserMoneyTransfer(TransferId),
	CONSTRAINT PK_UserMoneyTransferTT PRIMARY KEY (TaskId, TransferId)
);







CREATE OR REPLACE FUNCTION OrderInsert (
	UserId integer,
	Amount numeric(26, 10),
	Status varchar(35),
	Entity varchar(10),
	OpenTimeStamp timestamp,
	Reference varchar(50)
)
RETURNS integer 
AS $$
DECLARE InsertedId integer;
BEGIN
  INSERT INTO "Order" (
      UserId,
      Amount,
      Status,
      Entity,
      OpenTimeStamp,
      ExecutedAmount,
      SettledAmount,
      Reference
  ) VALUES (
      UserId,
      Amount,
      Status,
      Entity,
      OpenTimestamp,
      0,
      0,
      Reference
  ) RETURNING OrderId INTO InsertedId;

  RETURN InsertedId;

END $$
LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION UserMoneyTransferInsert (
	UserId integer,
	UserAccount varchar(50),
	Status varchar(35),
	Amount numeric(26, 2),
	Currency char(3),
	OpenTimeStamp timestamp,
	Reference varchar(50)
)
RETURNS integer 
AS $$
DECLARE InsertedId integer;
BEGIN
  INSERT INTO UserMoneyTransfer (
      UserId,
      UserAccount,
      Status,
      Amount,
      Currency,
      OpenTimeStamp,
      Reference
  ) VALUES (
      UserId,
      UserAccount,
      Status,
      Amount,
      Currency,
      OpenTimeStamp,
      Reference
  ) RETURNING TransferId INTO InsertedId;
  
  RETURN InsertedId;
END $$
LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION UserMoneyTransferTaskInsert (
	UserId integer,
	UserAccount varchar(50),
	Status varchar(35),
	Amount numeric(26, 2),
	Currency char(3),
	Reference varchar(50)
)
RETURNS integer 
AS $$
DECLARE InsertedId integer;
BEGIN
  INSERT INTO UserMoneyTransferTask (
      UserId,
      UserAccount,
      Status,
      Amount,
      Currency,
      Inserted,
      Reference
  ) VALUES (
      UserId,
      UserAccount,
      Status,
      Amount,
      Currency,
      now(),
      Reference
  ) RETURNING TaskId INTO InsertedId;
  
  RETURN InsertedId;
END $$
LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION UserMoneyTransferTTInsert (
    TaskId int,
    TransferId int
)
RETURNS void 
AS $$
BEGIN
  INSERT INTO UserMoneyTransferTT (
      TaskId,
      TransferId
  ) VALUES (
      TaskId,
      TransferId
  );
END $$
LANGUAGE plpgsql;



CREATE OR REPLACE FUNCTION MarketMoneyTransferInsert (
	Market varchar(50),
	Status varchar(50),
	Amount numeric(26, 2),
	Currency char(3),
	OpenTimeStamp timestamp,
	InReference varchar(50),
	OutReference varchar(50)
)
RETURNS integer 
AS $$
DECLARE InsertedId integer;
BEGIN
  INSERT INTO MarketMoneyTransfer (
      Market,
      Status,
      Amount,
      Currency,
      OpenTimeStamp,
      InReference,
      OutReference
  ) VALUES (
      Market,
      Status,
      Amount,
      Currency,
      OpenTimeStamp,
      InReference,
      OutReference
  ) RETURNING TransferId INTO InsertedId;
  
  RETURN InsertedId;
END $$
LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION WithdrawalInsert (
	UserId integer,
	Status varchar(35),
	ToName varchar(250),
	ToIban char(23),
	ToBic char(8),
	ToAddress varchar(250),
	ToPostalCode varchar(250),
	ToCity varchar(250),
	ToCountryCode char(2),
	Amount numeric(26, 2),
	Currency char(3),
	OpenTimeStamp timestamp,
	InReference varchar(50),
	OutReference varchar(50)
)
RETURNS integer 
AS $$
DECLARE InsertedId integer;
BEGIN
  INSERT INTO Withdrawal (
	UserId,
	Status,
	ToName,
	ToIban,
	ToBic,
	ToAddress,
	ToPostalCode,
	ToCity,
	ToCountryCode,
	Amount,
	Currency,
	OpenTimeStamp,
	InReference,
	OutReference
  ) VALUES (
	UserId,
	Status,
	ToName,
	ToIban,
	ToBic,
	ToAddress,
	ToPostalCode,
	ToCity,
	ToCountryCode,
	Amount,
	Currency,
	OpenTimeStamp,
	InReference,
	OutReference
  ) RETURNING WithdrawalId INTO InsertedId;
  
  RETURN InsertedId;
END $$
LANGUAGE plpgsql;


REVOKE ALL ON FUNCTION OrderInsert FROM PUBLIC;
GRANT EXECUTE ON FUNCTION OrderInsert TO "BisonUser";
REVOKE ALL ON FUNCTION UserMoneyTransferInsert FROM PUBLIC;
GRANT EXECUTE ON FUNCTION UserMoneyTransferInsert TO "BisonUser";
REVOKE ALL ON FUNCTION UserMoneyTransferTaskInsert FROM PUBLIC;
GRANT EXECUTE ON FUNCTION UserMoneyTransferTaskInsert TO "BisonUser";
REVOKE ALL ON FUNCTION UserMoneyTransferTTInsert FROM PUBLIC;
GRANT EXECUTE ON FUNCTION UserMoneyTransferTTInsert TO "BisonUser";
REVOKE ALL ON FUNCTION MarketMoneyTransferInsert FROM PUBLIC;
GRANT EXECUTE ON FUNCTION MarketMoneyTransferInsert TO "BisonUser";
REVOKE ALL ON FUNCTION WithdrawalInsert FROM PUBLIC;
GRANT EXECUTE ON FUNCTION WithdrawalInsert TO "BisonUser";
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO "BisonUser"