USE [master]
GO
/****** Object:  Database [Achievments]    Script Date: 13.03.2016 20:58:03 ******/
CREATE DATABASE [Achievments]
GO
USE [Achievments]
GO
/****** Object:  Table [dbo].[AchieveInfo]    Script Date: 13.03.2016 20:58:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AchieveInfo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ThemeID] [int] NOT NULL,
	[SubthemeID] [int] NOT NULL,
	[Date] [date] NOT NULL,
	[Points] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Subscribe] [ntext] NULL,
	[PersonID] [int] NOT NULL,
 CONSTRAINT [PK_AchieveInfo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Password]    Script Date: 13.03.2016 20:58:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Password](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Password] [varbinary](50) NOT NULL,
 CONSTRAINT [PK_Password] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Subtheme]    Script Date: 13.03.2016 20:58:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subtheme](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Subtheme] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SubThemeRel]    Script Date: 13.03.2016 20:58:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubThemeRel](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SubThemeID] [int] NOT NULL,
	[ThemeID] [int] NOT NULL,
 CONSTRAINT [PK_SubThemeRel] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Theme]    Script Date: 13.03.2016 20:58:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Theme](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Theme] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[AchieveInfo]  WITH CHECK ADD  CONSTRAINT [FK_AchieveInfo_Password] FOREIGN KEY([PersonID])
REFERENCES [dbo].[Password] ([ID])
GO
ALTER TABLE [dbo].[AchieveInfo] CHECK CONSTRAINT [FK_AchieveInfo_Password]
GO
ALTER TABLE [dbo].[AchieveInfo]  WITH CHECK ADD  CONSTRAINT [FK_AchieveInfo_Subtheme] FOREIGN KEY([SubthemeID])
REFERENCES [dbo].[Subtheme] ([ID])
GO
ALTER TABLE [dbo].[AchieveInfo] CHECK CONSTRAINT [FK_AchieveInfo_Subtheme]
GO
ALTER TABLE [dbo].[AchieveInfo]  WITH CHECK ADD  CONSTRAINT [FK_AchieveInfo_Theme] FOREIGN KEY([ThemeID])
REFERENCES [dbo].[Theme] ([ID])
GO
ALTER TABLE [dbo].[AchieveInfo] CHECK CONSTRAINT [FK_AchieveInfo_Theme]
GO
ALTER TABLE [dbo].[SubThemeRel]  WITH CHECK ADD  CONSTRAINT [FK_SubThemeRel_Subtheme] FOREIGN KEY([SubThemeID])
REFERENCES [dbo].[Subtheme] ([ID])
GO
ALTER TABLE [dbo].[SubThemeRel] CHECK CONSTRAINT [FK_SubThemeRel_Subtheme]
GO
ALTER TABLE [dbo].[SubThemeRel]  WITH CHECK ADD  CONSTRAINT [FK_SubThemeRel_Theme] FOREIGN KEY([ThemeID])
REFERENCES [dbo].[Theme] ([ID])
GO
ALTER TABLE [dbo].[SubThemeRel] CHECK CONSTRAINT [FK_SubThemeRel_Theme]
GO
USE [master]
GO
ALTER DATABASE [Achievments] SET  READ_WRITE 
GO
USE [Achievments]
GO
INSERT INTO [dbo].[Subtheme]
           ([Name])
     VALUES
           ('Личное')
GO

USE [Achievments]
GO
INSERT INTO [dbo].[Theme]
           ([Name])
     VALUES
           ('Личное')
GO

USE [Achievments]
GO

INSERT INTO [dbo].[SubThemeRel]
           ([SubThemeID]
           ,[ThemeID])
     VALUES
           (1
           ,1)
GO



