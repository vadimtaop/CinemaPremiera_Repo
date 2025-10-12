
GO
ALTER DATABASE [CinemaPremieraDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CinemaPremieraDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CinemaPremieraDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CinemaPremieraDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CinemaPremieraDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [CinemaPremieraDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CinemaPremieraDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CinemaPremieraDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CinemaPremieraDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CinemaPremieraDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CinemaPremieraDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CinemaPremieraDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CinemaPremieraDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CinemaPremieraDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CinemaPremieraDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CinemaPremieraDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CinemaPremieraDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CinemaPremieraDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CinemaPremieraDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CinemaPremieraDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CinemaPremieraDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CinemaPremieraDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CinemaPremieraDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [CinemaPremieraDB] SET  MULTI_USER 
GO
ALTER DATABASE [CinemaPremieraDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CinemaPremieraDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CinemaPremieraDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CinemaPremieraDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
USE [CinemaPremieraDB]
GO
/****** Object:  Table [dbo].[Auth]    Script Date: 04.10.2025 17:42:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Auth](
	[Auth_ID] [int] IDENTITY(1,1) NOT NULL,
	[Pincode] [int] NOT NULL,
	[Role_ID] [int] NOT NULL,
 CONSTRAINT [PK_Authorization] PRIMARY KEY CLUSTERED 
(
	[Auth_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Film]    Script Date: 04.10.2025 17:42:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Film](
	[Film_ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[AgeLimit] [int] NOT NULL,
	[DurationInMinutes] [decimal](10, 0) NOT NULL,
	[Genre] [nvarchar](max) NULL,
 CONSTRAINT [PK_Film] PRIMARY KEY CLUSTERED 
(
	[Film_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 04.10.2025 17:42:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[Order_ID] [int] IDENTITY(1,1) NOT NULL,
	[DateBuy] [date] NOT NULL,
	[Film_ID] [int] NOT NULL,
	[DateSession] [date] NOT NULL,
	[PriceList_ID] [int] NOT NULL,
	[Count] [int] NOT NULL,
	[CheckSum] [decimal](8, 2) NOT NULL,
	[PaymentType_ID] [int] NOT NULL,
	[Note] [nvarchar](max) NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[Order_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentType]    Script Date: 04.10.2025 17:42:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentType](
	[PaymentType_ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_PaymentType] PRIMARY KEY CLUSTERED 
(
	[PaymentType_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PriceList]    Script Date: 04.10.2025 17:42:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PriceList](
	[PriceList_ID] [int] IDENTITY(1,1) NOT NULL,
	[Price] [decimal](8, 2) NOT NULL,
 CONSTRAINT [PK_PriceList] PRIMARY KEY CLUSTERED 
(
	[PriceList_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 04.10.2025 17:42:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[Role_ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[Role_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Auth] ON 

INSERT [dbo].[Auth] ([Auth_ID], [Pincode], [Role_ID]) VALUES (1, 1111, 1)
INSERT [dbo].[Auth] ([Auth_ID], [Pincode], [Role_ID]) VALUES (2, 2222, 2)
SET IDENTITY_INSERT [dbo].[Auth] OFF
GO
SET IDENTITY_INSERT [dbo].[Film] ON 

INSERT [dbo].[Film] ([Film_ID], [Title], [AgeLimit], [DurationInMinutes], [Genre]) VALUES (1, N'Домовенок Кузя', 6, CAST(96 AS Decimal(10, 0)), N'Комедия, Семейное кино, Фэнтези')
INSERT [dbo].[Film] ([Film_ID], [Title], [AgeLimit], [DurationInMinutes], [Genre]) VALUES (2, N'Финист. Первый богатырь', 6, CAST(129 AS Decimal(10, 0)), N'Фэнтези, Приключения, Сказка')
INSERT [dbo].[Film] ([Film_ID], [Title], [AgeLimit], [DurationInMinutes], [Genre]) VALUES (3, N'Волшебник Изумрудного города', 6, CAST(112 AS Decimal(10, 0)), N'Фэнтези, Приключения, Семейный фильм, Сказка')
INSERT [dbo].[Film] ([Film_ID], [Title], [AgeLimit], [DurationInMinutes], [Genre]) VALUES (4, N'Елки 11', 6, CAST(104 AS Decimal(10, 0)), N'Комедия')
INSERT [dbo].[Film] ([Film_ID], [Title], [AgeLimit], [DurationInMinutes], [Genre]) VALUES (5, N'Иван Царевич 6', 6, CAST(92 AS Decimal(10, 0)), N'Комедия, Приключения, Семейное кино, Фэнтези')
SET IDENTITY_INSERT [dbo].[Film] OFF
GO
SET IDENTITY_INSERT [dbo].[Order] ON 

INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (1, CAST(N'2025-01-02' AS Date), 1, CAST(N'2025-01-02' AS Date), 4, 5, CAST(1250.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (2, CAST(N'2025-01-02' AS Date), 1, CAST(N'2025-01-02' AS Date), 4, 2, CAST(500.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (3, CAST(N'2025-01-02' AS Date), 1, CAST(N'2025-01-02' AS Date), 4, 1, CAST(250.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (4, CAST(N'2025-01-02' AS Date), 1, CAST(N'2025-01-02' AS Date), 4, 5, CAST(1250.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (5, CAST(N'2025-01-02' AS Date), 1, CAST(N'2025-01-02' AS Date), 4, 3, CAST(750.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (6, CAST(N'2025-01-02' AS Date), 1, CAST(N'2025-01-02' AS Date), 4, 3, CAST(750.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (7, CAST(N'2025-01-02' AS Date), 1, CAST(N'2025-01-02' AS Date), 4, 2, CAST(500.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (8, CAST(N'2025-01-02' AS Date), 1, CAST(N'2025-01-02' AS Date), 4, 3, CAST(750.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (9, CAST(N'2025-01-02' AS Date), 1, CAST(N'2025-01-02' AS Date), 4, 2, CAST(500.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (10, CAST(N'2025-01-02' AS Date), 1, CAST(N'2025-01-02' AS Date), 4, 3, CAST(750.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (11, CAST(N'2025-01-02' AS Date), 1, CAST(N'2025-01-02' AS Date), 4, 3, CAST(750.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (12, CAST(N'2025-01-02' AS Date), 1, CAST(N'2025-01-02' AS Date), 4, 3, CAST(750.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (13, CAST(N'2025-01-02' AS Date), 1, CAST(N'2025-01-02' AS Date), 4, 2, CAST(500.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (14, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-02' AS Date), 4, 5, CAST(1250.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (15, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-02' AS Date), 4, 4, CAST(1000.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (16, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-02' AS Date), 4, 4, CAST(1000.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (17, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-02' AS Date), 4, 2, CAST(500.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (18, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-02' AS Date), 4, 2, CAST(500.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (19, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-02' AS Date), 4, 4, CAST(1000.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (20, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-02' AS Date), 4, 2, CAST(500.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (21, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-02' AS Date), 4, 2, CAST(500.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (22, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-02' AS Date), 4, 2, CAST(500.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (23, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-02' AS Date), 4, 3, CAST(750.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (24, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-02' AS Date), 4, 2, CAST(500.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (25, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (26, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (27, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 4, CAST(1080.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (28, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 1, CAST(270.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (29, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-03' AS Date), 5, 4, CAST(1080.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (30, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 1, CAST(270.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (31, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 1, CAST(270.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (32, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 8, CAST(2160.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (33, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (34, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 4, CAST(1080.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (35, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (36, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 1, CAST(270.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (37, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (38, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 1, CAST(270.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (39, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 4, CAST(1080.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (40, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (41, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 8, CAST(2160.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (42, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (43, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 1, CAST(270.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (44, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 6, CAST(1620.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (45, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 6, CAST(1620.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (46, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 6, CAST(1620.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (47, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (48, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (49, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 1, CAST(270.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (50, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 3, CAST(810.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (51, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (52, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 5, CAST(1350.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (53, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (54, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (55, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 1, CAST(270.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (56, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (57, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 1, CAST(270.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (58, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (59, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (60, CAST(N'2025-01-02' AS Date), 1, CAST(N'2025-01-02' AS Date), 4, 5, CAST(1250.00 AS Decimal(8, 2)), 2, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (61, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-02' AS Date), 4, 4, CAST(1000.00 AS Decimal(8, 2)), 2, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (62, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-02' AS Date), 4, 3, CAST(750.00 AS Decimal(8, 2)), 2, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (63, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-02' AS Date), 4, 1, CAST(250.00 AS Decimal(8, 2)), 2, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (64, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-02' AS Date), 4, 2, CAST(500.00 AS Decimal(8, 2)), 2, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (65, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-02' AS Date), 4, 1, CAST(250.00 AS Decimal(8, 2)), 2, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (66, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 1, CAST(270.00 AS Decimal(8, 2)), 2, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (67, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 2, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (68, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 2, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (69, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 5, CAST(1350.00 AS Decimal(8, 2)), 2, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (70, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 4, CAST(1080.00 AS Decimal(8, 2)), 2, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (71, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 2, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (72, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 3, CAST(810.00 AS Decimal(8, 2)), 2, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (73, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 5, CAST(1350.00 AS Decimal(8, 2)), 2, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (74, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 2, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (75, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 1, CAST(270.00 AS Decimal(8, 2)), 2, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (76, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 6, CAST(1620.00 AS Decimal(8, 2)), 2, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (77, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 1, CAST(270.00 AS Decimal(8, 2)), 2, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (78, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 1, CAST(270.00 AS Decimal(8, 2)), 2, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (79, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-03' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 2, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (80, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-03' AS Date), 5, 3, CAST(810.00 AS Decimal(8, 2)), 2, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (81, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-03' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 2, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (82, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-03' AS Date), 5, 1, CAST(270.00 AS Decimal(8, 2)), 2, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (83, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-03' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 2, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (84, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-03' AS Date), 5, 4, CAST(1080.00 AS Decimal(8, 2)), 2, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (85, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-03' AS Date), 5, 3, CAST(810.00 AS Decimal(8, 2)), 2, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (86, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-03' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 2, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (87, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-02' AS Date), 4, 3, CAST(750.00 AS Decimal(8, 2)), 3, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (88, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 8, CAST(2160.00 AS Decimal(8, 2)), 3, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (89, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 26, CAST(7020.00 AS Decimal(8, 2)), 3, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (90, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-03' AS Date), 5, 1, CAST(270.00 AS Decimal(8, 2)), 3, N'')
INSERT [dbo].[Order] ([Order_ID], [DateBuy], [Film_ID], [DateSession], [PriceList_ID], [Count], [CheckSum], [PaymentType_ID], [Note]) VALUES (91, CAST(N'2025-01-02' AS Date), 5, CAST(N'2025-01-03' AS Date), 4, 1, CAST(250.00 AS Decimal(8, 2)), 3, N'')
SET IDENTITY_INSERT [dbo].[Order] OFF
GO
SET IDENTITY_INSERT [dbo].[PaymentType] ON 

INSERT [dbo].[PaymentType] ([PaymentType_ID], [Title]) VALUES (1, N'Банковская карта')
INSERT [dbo].[PaymentType] ([PaymentType_ID], [Title]) VALUES (2, N'Внешняя оплата')
INSERT [dbo].[PaymentType] ([PaymentType_ID], [Title]) VALUES (3, N'Пушкинская карта')
SET IDENTITY_INSERT [dbo].[PaymentType] OFF
GO
SET IDENTITY_INSERT [dbo].[PriceList] ON 

INSERT [dbo].[PriceList] ([PriceList_ID], [Price]) VALUES (1, CAST(170.00 AS Decimal(8, 2)))
INSERT [dbo].[PriceList] ([PriceList_ID], [Price]) VALUES (2, CAST(200.00 AS Decimal(8, 2)))
INSERT [dbo].[PriceList] ([PriceList_ID], [Price]) VALUES (3, CAST(230.00 AS Decimal(8, 2)))
INSERT [dbo].[PriceList] ([PriceList_ID], [Price]) VALUES (4, CAST(250.00 AS Decimal(8, 2)))
INSERT [dbo].[PriceList] ([PriceList_ID], [Price]) VALUES (5, CAST(270.00 AS Decimal(8, 2)))
SET IDENTITY_INSERT [dbo].[PriceList] OFF
GO
SET IDENTITY_INSERT [dbo].[Role] ON 

INSERT [dbo].[Role] ([Role_ID], [Title]) VALUES (1, N'Администратор')
INSERT [dbo].[Role] ([Role_ID], [Title]) VALUES (2, N'Кассир')
SET IDENTITY_INSERT [dbo].[Role] OFF
GO
ALTER TABLE [dbo].[Auth]  WITH CHECK ADD  CONSTRAINT [FK_Auth_Role] FOREIGN KEY([Role_ID])
REFERENCES [dbo].[Role] ([Role_ID])
GO
ALTER TABLE [dbo].[Auth] CHECK CONSTRAINT [FK_Auth_Role]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Film] FOREIGN KEY([Film_ID])
REFERENCES [dbo].[Film] ([Film_ID])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Orders_Film]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Orders_PaymentType] FOREIGN KEY([PaymentType_ID])
REFERENCES [dbo].[PaymentType] ([PaymentType_ID])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Orders_PaymentType]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Orders_PriceList] FOREIGN KEY([PriceList_ID])
REFERENCES [dbo].[PriceList] ([PriceList_ID])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Orders_PriceList]
GO
USE [master]
GO
ALTER DATABASE [CinemaPremieraDB] SET  READ_WRITE 
GO
