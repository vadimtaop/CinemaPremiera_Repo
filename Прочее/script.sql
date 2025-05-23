USE [CinemaPremieraDB]
GO
/****** Object:  Table [dbo].[Authorization]    Script Date: 02.05.2025 11:36:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Authorization](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Pincode] [int] NOT NULL,
 CONSTRAINT [PK_Authorization] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Film]    Script Date: 02.05.2025 11:36:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Film](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Film] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 02.05.2025 11:36:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DateBuy] [date] NOT NULL,
	[ID_Film] [int] NOT NULL,
	[DateSession] [date] NOT NULL,
	[ID_PriceList] [int] NOT NULL,
	[Count] [int] NOT NULL,
	[CheckSum] [decimal](8, 2) NOT NULL,
	[ID_PaymentType] [int] NOT NULL,
	[Note] [nvarchar](max) NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentType]    Script Date: 02.05.2025 11:36:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_PaymentType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PriceList]    Script Date: 02.05.2025 11:36:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PriceList](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Price] [decimal](8, 2) NOT NULL,
 CONSTRAINT [PK_PriceList] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Authorization] ON 

INSERT [dbo].[Authorization] ([ID], [Pincode]) VALUES (1, 1111)
SET IDENTITY_INSERT [dbo].[Authorization] OFF
GO
SET IDENTITY_INSERT [dbo].[Film] ON 

INSERT [dbo].[Film] ([ID], [Title]) VALUES (1, N'Домовенок Кузя')
INSERT [dbo].[Film] ([ID], [Title]) VALUES (2, N'Финист. Первый богатырь')
INSERT [dbo].[Film] ([ID], [Title]) VALUES (3, N'Волшебник Изумрудного города')
INSERT [dbo].[Film] ([ID], [Title]) VALUES (4, N'Елки 11')
INSERT [dbo].[Film] ([ID], [Title]) VALUES (5, N'Иван Царевич 6')
SET IDENTITY_INSERT [dbo].[Film] OFF
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 

INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (1, CAST(N'2025-01-02' AS Date), 1, CAST(N'2025-01-02' AS Date), 4, 5, CAST(1250.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (2, CAST(N'2025-01-02' AS Date), 1, CAST(N'2025-01-02' AS Date), 4, 2, CAST(500.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (3, CAST(N'2025-01-02' AS Date), 1, CAST(N'2025-01-02' AS Date), 4, 1, CAST(250.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (4, CAST(N'2025-01-02' AS Date), 1, CAST(N'2025-01-02' AS Date), 4, 5, CAST(1250.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (5, CAST(N'2025-01-02' AS Date), 1, CAST(N'2025-01-02' AS Date), 4, 3, CAST(750.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (6, CAST(N'2025-01-02' AS Date), 1, CAST(N'2025-01-02' AS Date), 4, 3, CAST(750.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (7, CAST(N'2025-01-02' AS Date), 1, CAST(N'2025-01-02' AS Date), 4, 2, CAST(500.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (8, CAST(N'2025-01-02' AS Date), 1, CAST(N'2025-01-02' AS Date), 4, 3, CAST(750.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (9, CAST(N'2025-01-02' AS Date), 1, CAST(N'2025-01-02' AS Date), 4, 2, CAST(500.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (10, CAST(N'2025-01-02' AS Date), 1, CAST(N'2025-01-02' AS Date), 4, 3, CAST(750.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (11, CAST(N'2025-01-02' AS Date), 1, CAST(N'2025-01-02' AS Date), 4, 3, CAST(750.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (12, CAST(N'2025-01-02' AS Date), 1, CAST(N'2025-01-02' AS Date), 4, 3, CAST(750.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (13, CAST(N'2025-01-02' AS Date), 1, CAST(N'2025-01-02' AS Date), 4, 2, CAST(500.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (14, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-02' AS Date), 4, 5, CAST(1250.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (15, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-02' AS Date), 4, 4, CAST(1000.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (16, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-02' AS Date), 4, 4, CAST(1000.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (17, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-02' AS Date), 4, 2, CAST(500.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (18, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-02' AS Date), 4, 2, CAST(500.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (19, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-02' AS Date), 4, 4, CAST(1000.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (20, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-02' AS Date), 4, 2, CAST(500.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (21, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-02' AS Date), 4, 2, CAST(500.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (22, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-02' AS Date), 4, 2, CAST(500.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (23, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-02' AS Date), 4, 3, CAST(750.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (24, CAST(N'2025-01-02' AS Date), 2, CAST(N'2025-01-02' AS Date), 4, 2, CAST(500.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (25, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (26, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (27, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 4, CAST(1080.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (28, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 1, CAST(270.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (29, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-03' AS Date), 5, 4, CAST(1080.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (30, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 1, CAST(270.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (31, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 1, CAST(270.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (32, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 8, CAST(2160.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (33, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (34, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 4, CAST(1080.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (35, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (36, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 1, CAST(270.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (37, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (38, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 1, CAST(270.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (39, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 4, CAST(1080.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (40, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (41, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 8, CAST(2160.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (42, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (43, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 1, CAST(270.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (44, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 6, CAST(1620.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (45, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 6, CAST(1620.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (46, CAST(N'2025-01-02' AS Date), 3, CAST(N'2025-01-02' AS Date), 5, 6, CAST(1620.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (47, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (48, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (49, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 1, CAST(270.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (50, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 3, CAST(810.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (51, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (52, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 5, CAST(1350.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (53, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (54, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (55, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 1, CAST(270.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (56, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (57, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 1, CAST(270.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (58, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
INSERT [dbo].[Orders] ([ID], [DateBuy], [ID_Film], [DateSession], [ID_PriceList], [Count], [CheckSum], [ID_PaymentType], [Note]) VALUES (59, CAST(N'2025-01-02' AS Date), 4, CAST(N'2025-01-02' AS Date), 5, 2, CAST(540.00 AS Decimal(8, 2)), 1, N'')
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
SET IDENTITY_INSERT [dbo].[PaymentType] ON 

INSERT [dbo].[PaymentType] ([ID], [Title]) VALUES (1, N'Банковская карта')
INSERT [dbo].[PaymentType] ([ID], [Title]) VALUES (2, N'Внешняя оплата')
INSERT [dbo].[PaymentType] ([ID], [Title]) VALUES (3, N'Пушкинская карта')
SET IDENTITY_INSERT [dbo].[PaymentType] OFF
GO
SET IDENTITY_INSERT [dbo].[PriceList] ON 

INSERT [dbo].[PriceList] ([ID], [Price]) VALUES (1, CAST(170.00 AS Decimal(8, 2)))
INSERT [dbo].[PriceList] ([ID], [Price]) VALUES (2, CAST(200.00 AS Decimal(8, 2)))
INSERT [dbo].[PriceList] ([ID], [Price]) VALUES (3, CAST(230.00 AS Decimal(8, 2)))
INSERT [dbo].[PriceList] ([ID], [Price]) VALUES (4, CAST(250.00 AS Decimal(8, 2)))
INSERT [dbo].[PriceList] ([ID], [Price]) VALUES (5, CAST(270.00 AS Decimal(8, 2)))
SET IDENTITY_INSERT [dbo].[PriceList] OFF
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Film] FOREIGN KEY([ID_Film])
REFERENCES [dbo].[Film] ([ID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Film]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_PaymentType] FOREIGN KEY([ID_PaymentType])
REFERENCES [dbo].[PaymentType] ([ID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_PaymentType]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_PriceList] FOREIGN KEY([ID_PriceList])
REFERENCES [dbo].[PriceList] ([ID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_PriceList]
GO
