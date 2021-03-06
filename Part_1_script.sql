USE [master]
GO
/****** Object:  Database [Part_1_DB]    Script Date: 7/10/2021 9:12:55 PM ******/
CREATE DATABASE [Part_1_DB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Part_1_DB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Part_1_DB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Part_1_DB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Part_1_DB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Part_1_DB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Part_1_DB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Part_1_DB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Part_1_DB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Part_1_DB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Part_1_DB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Part_1_DB] SET ARITHABORT OFF 
GO
ALTER DATABASE [Part_1_DB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Part_1_DB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Part_1_DB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Part_1_DB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Part_1_DB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Part_1_DB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Part_1_DB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Part_1_DB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Part_1_DB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Part_1_DB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Part_1_DB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Part_1_DB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Part_1_DB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Part_1_DB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Part_1_DB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Part_1_DB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Part_1_DB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Part_1_DB] SET RECOVERY FULL 
GO
ALTER DATABASE [Part_1_DB] SET  MULTI_USER 
GO
ALTER DATABASE [Part_1_DB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Part_1_DB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Part_1_DB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Part_1_DB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Part_1_DB] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Part_1_DB', N'ON'
GO
ALTER DATABASE [Part_1_DB] SET QUERY_STORE = OFF
GO
USE [Part_1_DB]
GO
/****** Object:  Table [dbo].[Bills]    Script Date: 7/10/2021 9:12:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bills](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[BillDate] [datetime] NULL,
	[BillAmount] [float] NULL,
	[PaidAmount] [float] NULL,
	[PaidDate] [datetime] NULL,
 CONSTRAINT [PK_Bills] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 7/10/2021 9:12:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[Id] [int] NOT NULL,
	[Name] [varchar](500) NULL
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[spGetbill]    Script Date: 7/10/2021 9:12:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spGetbill] (
@QryOption AS Int = -1,
@Id AS Int = -1,
@CustomerId AS Int = -1,
@BillDate as DateTime = NULL,
@BillAmount AS Float = NULL,
@PaidAmount AS Float = NULL,
@PaidDate as DateTime = NULL

--DeclareInputParamNext
)

AS

DECLARE @BY_ALL AS Int
DECLARE @BY_BILL_ID AS Int
DECLARE @BY_OUTSTANDING_BILL AS Int

--DeclareQryOptionParamNext

SET @BY_ALL = 1
SET @BY_BILL_ID = 2
SET @BY_OUTSTANDING_BILL = 3


--SetQryOptionParamNext

IF @QryOption = @BY_ALL
BEGIN
	SELECT Id, CustomerId, BillAmount, BillDate, PaidAmount, PaidDate
	FROM Bills

END

ELSE IF @QryOption = @BY_BILL_ID
BEGIN
	SELECT Id, CustomerId, BillAmount, BillDate, PaidAmount, PaidDate
	FROM Bills
	WHERE Id = @Id
END

ELSE IF @QryOption = @BY_OUTSTANDING_BILL
BEGIN
	SELECT Id, CustomerId, BillAmount, BillDate, PaidAmount, PaidDate
	FROM Bills
	WHERE PaidDate IS NULL
END
--QryOptionNext

GO
/****** Object:  StoredProcedure [dbo].[spGetCustomer]    Script Date: 7/10/2021 9:12:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spGetCustomer] (
@QryOption AS Int = -1,
@Id AS Int = -1

--DeclareInputParamNext
)

AS

DECLARE @BY_ALL AS Int
DECLARE @BY_CUSTOMER_ID AS Int

--DeclareQryOptionParamNext

SET @BY_ALL = 1
SET @BY_CUSTOMER_ID = 2


--SetQryOptionParamNext

IF @QryOption = @BY_ALL
BEGIN
	SELECT Id, Name
	FROM Customers
	ORDER BY Name
END

ELSE IF @QryOption = @BY_CUSTOMER_ID
BEGIN
	SELECT Id, Name
	FROM Customers
	WHERE Id = @Id
END
--QryOptionNext

GO
/****** Object:  StoredProcedure [dbo].[spSetBill]    Script Date: 7/10/2021 9:12:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spSetBill] (
@SaveOption AS Int = -1,
@Id AS Int = -1,
@CustomerId AS int = null,
@BillDate as DateTime = null,
@BillAmount AS Float = NULL,
@PaidAmount AS Float = NULL,
@PaidDate as DateTime = null,
--DeclareInputParamNext

@IdentityValue AS Int = - 1 OUTPUT
)

AS

--set default value
SET @BillDate = GetDate()
--SET @PaidDate = GetDate()

DECLARE @SAVE_ROW AS Int
DECLARE @MARK_AS_PAID AS Int

--DeclareQryOptionParamNext

SET @SAVE_ROW = 1
SET @MARK_AS_PAID = 2

--SetQryOptionParamNext

IF @Id = -1
BEGIN
	INSERT INTO Bills(CustomerId, BillDate, BillAmount, PaidAmount, PaidDate)
	VALUES(@CustomerId, @BillDate, @BillAmount, @PaidAmount, @PaidDate)
	SET @IdentityValue = @@IDENTITY
END
ELSE
BEGIN
	IF @SaveOption = @SAVE_ROW
	BEGIN
		UPDATE Bills SET
			PaidAmount = PaidAmount + @PaidAmount,
			PaidDate = CASE	WHEN BillAmount <= PaidAmount + @PaidAmount	THEN GETDATE() ELSE	NULL END FROM Bills
		WHERE Id = @Id
		SET @IdentityValue = @Id
	END
	ELSE IF @SaveOption = @MARK_AS_PAID
	BEGIN	
		SET @PaidDate = GetDate()
		UPDATE Bills SET
			PaidAmount = BillAmount - PaidAmount,
			PaidDate = @PaidDate
		WHERE Id = @Id
		SET @IdentityValue = @Id
	END

	--QryOptionNext
END
GO
USE [master]
GO
ALTER DATABASE [Part_1_DB] SET  READ_WRITE 
GO
