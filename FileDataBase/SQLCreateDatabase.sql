USE [master]
GO
/****** Object:  Database [ash]    Script Date: 3/17/2023 6:35:28 AM ******/
CREATE DATABASE [ash]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ash', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\ash.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ash_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\ash_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
--ALTER DATABASE [ash] SET COMPATIBILITY_LEVEL = 140
--GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ash].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ash] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ash] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ash] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ash] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ash] SET ARITHABORT OFF 
GO
ALTER DATABASE [ash] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [ash] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ash] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ash] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ash] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ash] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ash] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ash] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ash] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ash] SET  ENABLE_BROKER 
GO
ALTER DATABASE [ash] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ash] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ash] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ash] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ash] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ash] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ash] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ash] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ash] SET  MULTI_USER 
GO
ALTER DATABASE [ash] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ash] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ash] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ash] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ash] SET DELAYED_DURABILITY = DISABLED 
GO
--ALTER DATABASE [ash] SET QUERY_STORE = OFF
--GO
USE [ash]
GO
/****** Object:  Table [dbo].[advert_category]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[advert_category](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LangCode] [varchar](50) NULL,
	[Name] [nvarchar](max) NULL,
	[ParentID] [int] NULL,
	[GhiChu] [nvarchar](max) NULL,
	[Ordering] [int] NULL,
	[Status] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ListCheck] [varchar](500) NULL,
 CONSTRAINT [PK_advert_category] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[advert_category_module]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[advert_category_module](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AdvertID] [int] NOT NULL,
	[ModuleID] [int] NOT NULL,
 CONSTRAINT [PK_advert_category_module] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[contact_category]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[contact_category](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NULL,
	[Name] [nvarchar](max) NULL,
	[Photo] [nvarchar](max) NULL,
	[LangCode] [varchar](50) NULL,
	[Title] [nvarchar](max) NULL,
	[Keyword] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Ordering] [int] NULL,
	[Status] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[support_1] [nvarchar](max) NULL,
	[support_2] [nvarchar](max) NULL,
	[advert_1] [nvarchar](max) NULL,
	[advert_2] [nvarchar](max) NULL,
	[display_1] [nvarchar](max) NULL,
	[display_2] [nvarchar](max) NULL,
	[view_more_detail] [nvarchar](max) NULL,
 CONSTRAINT [PK_contact_category] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DanhMucChas]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DanhMucChas](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NULL,
 CONSTRAINT [PK_DanhMucChas] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DanhMucCons]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DanhMucCons](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[ParentID] [int] NULL,
 CONSTRAINT [PK_DanhMucCons] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[document_category]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[document_category](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NULL,
	[Name] [nvarchar](max) NULL,
	[Photo] [nvarchar](max) NULL,
	[LangCode] [varchar](50) NULL,
	[Title] [nvarchar](max) NULL,
	[Keyword] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Ordering] [int] NULL,
	[Status] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[support_1] [nvarchar](max) NULL,
	[support_2] [nvarchar](max) NULL,
	[advert_1] [nvarchar](max) NULL,
	[advert_2] [nvarchar](max) NULL,
	[display_1] [nvarchar](max) NULL,
	[display_2] [nvarchar](max) NULL,
	[view_more_detail] [nvarchar](max) NULL,
 CONSTRAINT [PK_document_category] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[document_price]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[document_price](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NULL,
	[LangCode] [varchar](50) NULL,
	[Price_min] [int] NULL,
	[Price_max] [int] NULL,
	[Ordering] [int] NULL,
	[Status] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
 CONSTRAINT [PK_document_price] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[document_publisher]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[document_publisher](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Photo] [nvarchar](max) NULL,
	[LangCode] [varchar](50) NULL,
	[KyHieu] [nvarchar](max) NULL,
	[MaSo] [nvarchar](max) NULL,
	[TKNganHang] [bigint] NULL,
	[TenNganHang] [nvarchar](max) NULL,
	[Ordering] [int] NULL,
	[Status] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
 CONSTRAINT [PK_document_publisher] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[faq_category]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[faq_category](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NULL,
	[Name] [nvarchar](max) NULL,
	[Photo] [nvarchar](max) NULL,
	[LangCode] [varchar](50) NULL,
	[Title] [nvarchar](max) NULL,
	[Keyword] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Ordering] [int] NULL,
	[Status] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[support_1] [nvarchar](max) NULL,
	[support_2] [nvarchar](max) NULL,
	[advert_1] [nvarchar](max) NULL,
	[advert_2] [nvarchar](max) NULL,
	[display_1] [nvarchar](max) NULL,
	[display_2] [nvarchar](max) NULL,
	[view_more_detail] [nvarchar](max) NULL,
 CONSTRAINT [PK_faq_category] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GroupCusUsers]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupCusUsers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NameGroup] [nvarchar](250) NOT NULL,
	[Ordering] [int] NULL,
	[Status] [bit] NOT NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_GroupCusUsers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[guide_category]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[guide_category](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NULL,
	[Name] [nvarchar](max) NULL,
	[Photo] [nvarchar](max) NULL,
	[LangCode] [varchar](50) NULL,
	[Title] [nvarchar](max) NULL,
	[Keyword] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Ordering] [int] NULL,
	[Status] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[support_1] [nvarchar](max) NULL,
	[support_2] [nvarchar](max) NULL,
	[advert_1] [nvarchar](max) NULL,
	[advert_2] [nvarchar](max) NULL,
	[display_1] [nvarchar](max) NULL,
	[display_2] [nvarchar](max) NULL,
	[view_more_detail] [nvarchar](max) NULL,
 CONSTRAINT [PK_guide_category] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[modules]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[modules](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](500) NULL,
	[Name] [nvarchar](500) NULL,
	[Ordering] [int] NULL,
	[Status] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
 CONSTRAINT [PK_modules] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[news_category]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[news_category](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NULL,
	[Name] [nvarchar](max) NULL,
	[Photo] [nvarchar](max) NULL,
	[LangCode] [varchar](50) NULL,
	[Title] [nvarchar](max) NULL,
	[Keyword] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Ordering] [int] NULL,
	[Status] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[support_1] [nvarchar](max) NULL,
	[support_2] [nvarchar](max) NULL,
	[advert_1] [nvarchar](max) NULL,
	[advert_2] [nvarchar](max) NULL,
	[display_1] [nvarchar](max) NULL,
	[display_2] [nvarchar](max) NULL,
	[view_more_detail] [nvarchar](max) NULL,
 CONSTRAINT [PK_news_category] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[qProcedures]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[qProcedures](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Code] [nvarchar](max) NULL,
	[Ordering] [int] NULL,
	[isPublish] [bit] NOT NULL,
	[LangCode] [varchar](50) NULL,
	[NameEnglish] [nvarchar](max) NULL,
 CONSTRAINT [PK_qProcedures] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[qProcesses]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[qProcesses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TableCode] [nvarchar](100) NOT NULL,
	[FKId] [int] NOT NULL,
	[FromProcedure] [int] NOT NULL,
	[FromId] [int] NOT NULL,
	[ToId] [int] NOT NULL,
	[ToProcedure] [int] NOT NULL,
	[Contents] [nvarchar](max) NULL,
	[CreateDate] [datetime] NULL,
	[Attachment] [nvarchar](max) NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_qProcesses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[quydinhpl_category]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[quydinhpl_category](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Ordering] [int] NOT NULL,
	[Status] [smallint] NOT NULL,
	[LangCode] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_quydinhpl_category] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[recruit_category]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[recruit_category](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NULL,
	[Name] [nvarchar](max) NULL,
	[Photo] [nvarchar](max) NULL,
	[LangCode] [varchar](50) NULL,
	[Title] [nvarchar](max) NULL,
	[Keyword] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Ordering] [int] NULL,
	[Status] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[support_1] [nvarchar](max) NULL,
	[support_2] [nvarchar](max) NULL,
	[advert_1] [nvarchar](max) NULL,
	[advert_2] [nvarchar](max) NULL,
	[display_1] [nvarchar](max) NULL,
	[display_2] [nvarchar](max) NULL,
	[view_more_detail] [nvarchar](max) NULL,
 CONSTRAINT [PK_recruit_category] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[support_category]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[support_category](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LangCode] [varchar](50) NULL,
	[Name] [nvarchar](max) NULL,
	[ParentID] [int] NULL,
	[GhiChu] [nvarchar](max) NULL,
	[Ordering] [int] NULL,
	[Status] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ListCheck] [varchar](500) NULL,
 CONSTRAINT [PK_support_category] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[support_category_module]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[support_category_module](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SupportID] [int] NOT NULL,
	[ModuleID] [int] NOT NULL,
 CONSTRAINT [PK_support_category_module] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[survey_category]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[survey_category](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NULL,
	[Name] [nvarchar](max) NULL,
	[Photo] [nvarchar](max) NULL,
	[LangCode] [varchar](50) NULL,
	[Title] [nvarchar](max) NULL,
	[Keyword] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Ordering] [int] NULL,
	[Status] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[support_1] [nvarchar](max) NULL,
	[support_2] [nvarchar](max) NULL,
	[advert_1] [nvarchar](max) NULL,
	[advert_2] [nvarchar](max) NULL,
	[display_1] [nvarchar](max) NULL,
	[display_2] [nvarchar](max) NULL,
	[view_more_detail] [nvarchar](max) NULL,
 CONSTRAINT [PK_survey_category] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sysdiagrams]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sysdiagrams](
	[name] [nvarchar](128) NOT NULL,
	[principal_id] [int] NOT NULL,
	[diagram_id] [int] IDENTITY(1,1) NOT NULL,
	[version] [int] NULL,
	[definition] [varbinary](max) NULL,
 CONSTRAINT [PK_sysdiagrams] PRIMARY KEY CLUSTERED 
(
	[diagram_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_AccessOnline]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_AccessOnline](
	[Session] [nvarchar](250) NOT NULL,
	[Time] [datetime] NOT NULL,
 CONSTRAINT [PK_tbl_AccessOnline] PRIMARY KEY CLUSTERED 
(
	[Session] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_AccessView]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_AccessView](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TongCong] [int] NULL,
	[NgayThongKe] [datetime] NOT NULL,
	[HomNay] [int] NULL,
	[HomTruoc] [int] NULL,
	[TuanNay] [int] NULL,
	[TuanTruoc] [int] NULL,
	[ThangNay] [int] NULL,
	[ThangTruoc] [int] NULL,
 CONSTRAINT [PK_tbl_AccessView] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_AdminMenu]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_AdminMenu](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[ParentID] [int] NOT NULL,
	[Url] [varchar](250) NOT NULL,
	[Ordering] [int] NOT NULL,
	[Active] [bit] NOT NULL,
	[Icon] [varchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[LangCode] [varchar](5) NULL,
 CONSTRAINT [PK_tbl_AdminMenu] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Advert]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Advert](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Image] [nvarchar](250) NOT NULL,
	[Url] [varchar](250) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[Status] [bit] NOT NULL,
	[Position] [varchar](max) NOT NULL,
	[Ordering] [int] NOT NULL,
	[isNewTab] [bit] NOT NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_Advert] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ApproveNews]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ApproveNews](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Ordering] [int] NULL,
 CONSTRAINT [PK_tbl_ApproveNews] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_BoxNewsConfig]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_BoxNewsConfig](
	[Code] [varchar](10) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[PageElementId] [int] NOT NULL,
	[LangCode] [varchar](5) NOT NULL,
 CONSTRAINT [PK_tbl_BoxNewsConfig] PRIMARY KEY CLUSTERED 
(
	[Code] ASC,
	[PageElementId] ASC,
	[LangCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Calendar]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Calendar](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Status] [bit] NOT NULL,
	[LangCode] [varchar](5) NOT NULL,
	[LangName] [nvarchar](50) NULL,
	[PageElementId] [int] NOT NULL,
	[Logo] [nvarchar](250) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[Details] [nvarchar](max) NULL,
 CONSTRAINT [PK_tbl_Calendar] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Category]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Category](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Position] [varchar](500) NULL,
	[Icon] [nvarchar](max) NULL,
	[Ordering] [int] NOT NULL,
	[Active] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[LangCode] [varchar](5) NULL,
	[Url] [nvarchar](500) NULL,
	[isMenu] [bit] NULL,
	[isNewTab] [bit] NOT NULL,
 CONSTRAINT [PK_tbl_Category] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Chat]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Chat](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ListIdUser] [nvarchar](max) NULL,
	[Content] [nvarchar](max) NULL,
	[Date] [datetime] NULL,
	[Count] [int] NOT NULL,
	[IdLast] [int] NULL,
	[LangCode] [varchar](50) NULL,
	[Ordering] [int] NULL,
 CONSTRAINT [PK_tbl_Chat] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ChucVu]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ChucVu](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Ordering] [int] NULL,
	[isPublish] [bit] NOT NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_ChucVu] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Config]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Config](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[ParentID] [int] NOT NULL,
	[Code] [nvarchar](500) NULL,
	[Description] [nvarchar](500) NULL,
	[Price] [int] NULL,
	[Status] [bit] NOT NULL,
	[Ordering] [int] NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_Config] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_configLuuY]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_configLuuY](
	[LuuYHoiDap] [nvarchar](max) NULL,
	[LuuYToGiac] [nvarchar](max) NULL,
	[EmailNhanYKien] [nvarchar](max) NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_configLuuY] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ConfigText]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ConfigText](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[LastModifieDate] [datetime] NULL,
	[LastModifieBy] [int] NULL,
	[LangCode] [nchar](20) NOT NULL,
	[ContentJson] [nvarchar](max) NULL,
 CONSTRAINT [PK_tbl_ConfigText] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Contact]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Contact](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FullNameOfQuestion] [nvarchar](50) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Question] [nvarchar](max) NOT NULL,
	[Answer] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[AnswerBy] [int] NULL,
	[AnswerDate] [datetime] NULL,
	[UserNameOfAnswer] [nvarchar](50) NULL,
	[Status] [bit] NOT NULL,
	[Phone] [varchar](150) NULL,
	[DiaChi] [nvarchar](max) NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_Contact] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ContactInfo]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ContactInfo](
	[NoiDung] [nvarchar](max) NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_ContactInfo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_CoQuanBanHanh]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_CoQuanBanHanh](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](250) NULL,
	[LangCode] [varchar](5) NOT NULL,
	[Ordering] [int] NOT NULL,
	[Status] [bit] NOT NULL,
	[Level] [int] NOT NULL,
 CONSTRAINT [PK_tbl_CoQuanBanHanh] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_CountAccess]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_CountAccess](
	[Count] [int] NOT NULL,
	[ID] [int] NOT NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_CountAccess] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_DanhMucTaiLieu]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_DanhMucTaiLieu](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Ordering] [int] NOT NULL,
	[Status] [bit] NOT NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_DanhMucTaiLieu] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_DMChung]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_DMChung](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NOT NULL,
	[Code] [nvarchar](250) NULL,
	[Ten] [nvarchar](500) NOT NULL,
	[Ordering] [int] NOT NULL,
	[Status] [bit] NOT NULL,
	[Level] [int] NOT NULL,
	[BCH] [nvarchar](500) NULL,
	[Phone] [nvarchar](50) NULL,
	[DiDong] [nvarchar](50) NULL,
	[Address] [nvarchar](500) NULL,
	[FeatureImage] [nvarchar](250) NULL,
	[Details] [nvarchar](max) NULL,
	[IsHome] [bit] NOT NULL,
	[CatID] [int] NOT NULL,
	[Url] [nvarchar](250) NULL,
	[IsBanChapHanh] [bit] NOT NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_DMChung] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_DMNhom]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_DMNhom](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Ordering] [int] NOT NULL,
	[Status] [bit] NOT NULL,
	[Code] [nvarchar](50) NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_DMNhom] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Document_Type]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Document_Type](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Photo] [nvarchar](max) NULL,
	[Ordering] [int] NULL,
	[Status] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[LangCode] [nchar](10) NULL,
	[Code] [nvarchar](max) NULL,
	[ParentID] [int] NOT NULL,
 CONSTRAINT [PK_tbl_Document_Type] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_DonVi]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_DonVi](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TenDonVi] [nvarchar](250) NOT NULL,
	[Ordering] [int] NULL,
	[Status] [bit] NOT NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_DonVi] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_DonViTTHC]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_DonViTTHC](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](250) NULL,
	[Ordering] [int] NOT NULL,
	[Active] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[LangCode] [varchar](5) NULL,
 CONSTRAINT [PK_tbl_DonViTTHC] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_eMagazine]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_eMagazine](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[Photo] [varchar](max) NULL,
	[Author] [nvarchar](max) NULL,
	[CTVID] [int] NULL,
	[NewMoney] [int] NULL,
	[Width] [int] NULL,
	[Background] [nvarchar](max) NULL,
	[Status] [bit] NOT NULL,
	[Source] [nvarchar](max) NULL,
	[Info] [nvarchar](max) NULL,
	[Ordering] [int] NOT NULL,
	[Color] [varchar](max) NULL,
 CONSTRAINT [PK_tbl_eMagazine] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_FeedBack]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_FeedBack](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FullNameOfQuestion] [nvarchar](50) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Question] [nvarchar](max) NOT NULL,
	[Answer] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[AnswerBy] [int] NULL,
	[AnswerDate] [datetime] NULL,
	[UserNameOfAnswer] [nvarchar](50) NULL,
	[Status] [bit] NOT NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_FeedBack] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_fieldfiles]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_fieldfiles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FieldID] [int] NULL,
	[LinkFile] [nvarchar](max) NULL,
	[NameFile] [nvarchar](500) NULL,
	[CreateDate] [datetime] NULL,
	[Status] [int] NULL,
	[ReplaceName] [nvarchar](500) NULL,
	[Size] [varchar](max) NULL,
	[Code] [nvarchar](250) NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_fieldfiles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Footer]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Footer](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[LangCode] [varchar](5) NULL,
	[Ordering] [int] NULL,
	[Status] [bit] NOT NULL,
	[LangName] [nvarchar](50) NULL,
	[FooterTextLeft] [nvarchar](max) NULL,
	[FooterTextRight] [nvarchar](max) NULL,
	[FooterTextLeftEnglish] [nvarchar](max) NULL,
	[FooterTextRightEnglish] [nvarchar](max) NULL,
 CONSTRAINT [PK_tbl_Footer] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_GroupUser]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_GroupUser](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[ExpandNews] [bit] NOT NULL,
	[Status] [bit] NOT NULL,
	[Permission] [varchar](max) NULL,
	[PermissionCatNews] [varchar](max) NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_GroupUser] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_HistoryNews]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_HistoryNews](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NewsID] [int] NOT NULL,
	[Contents] [nvarchar](max) NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_HistoryNews] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_HomeMenu]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_HomeMenu](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Url] [varchar](500) NULL,
	[Ordering] [int] NULL,
	[isNewsTab] [bit] NOT NULL,
	[Status] [bit] NOT NULL,
	[CategoryId] [int] NULL,
	[LinkType] [int] NOT NULL,
	[PageElementId] [int] NOT NULL,
	[IsHome] [smallint] NOT NULL,
	[IsMenu] [bit] NOT NULL,
	[IsPermAdd] [bit] NOT NULL,
	[FeautureImage] [nvarchar](max) NULL,
	[IsHome2] [bit] NOT NULL,
	[IsHome3] [bit] NOT NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_HomeMenu] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_HomeMenu_OLD]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_HomeMenu_OLD](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Url] [varchar](500) NULL,
	[Ordering] [int] NULL,
	[isNewsTab] [bit] NULL,
	[Status] [bit] NOT NULL,
	[CategoryId] [int] NULL,
	[LinkType] [int] NOT NULL,
	[PageElementId] [int] NOT NULL,
	[isHome] [smallint] NOT NULL,
	[isMenu] [bit] NOT NULL,
	[IsPermAdd] [bit] NOT NULL,
	[FeautureImage] [nvarchar](max) NULL,
	[IsHome2] [bit] NOT NULL,
	[IsHome3] [bit] NOT NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_HomeMenu_OLD] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Images]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Images](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NULL,
	[Images] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Url] [nvarchar](500) NULL,
	[Status] [bit] NOT NULL,
	[ImageCategoryId] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LangCode] [varchar](5) NULL,
	[ModifiedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedBy] [int] NOT NULL,
	[Ordering] [int] NOT NULL,
	[UserActId] [int] NOT NULL,
	[ProcedureId] [int] NOT NULL,
 CONSTRAINT [PK_tbl_Images] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ImagesCategory]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ImagesCategory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NULL,
	[Image] [nvarchar](500) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[LangCode] [nvarchar](10) NULL,
	[Status] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[Ordering] [int] NOT NULL,
	[ViewNumber] [bigint] NOT NULL,
	[IsHome] [bit] NOT NULL,
	[NewMoney] [int] NULL,
	[CreateBy] [int] NULL,
 CONSTRAINT [PK_tbl_ImagesCategory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Languages]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Languages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Code] [varchar](5) NOT NULL,
	[Icon] [nvarchar](250) NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_Languages] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_LichCongTac]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_LichCongTac](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LangCode] [varchar](5) NOT NULL,
	[UserID] [int] NOT NULL,
	[ngaybatdau] [datetime] NOT NULL,
	[giobatdau] [nvarchar](50) NULL,
	[ngayketthuc] [datetime] NULL,
	[noidung] [nvarchar](max) NOT NULL,
	[allDay] [tinyint] NOT NULL,
	[LVTCQ] [tinyint] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[Status] [tinyint] NOT NULL,
 CONSTRAINT [PK_tbl_LichCongTac] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Limit]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Limit](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Code] [varchar](250) NOT NULL,
	[Value] [int] NOT NULL,
	[ParentID] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[Status] [bit] NOT NULL,
	[Ordering] [int] NOT NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_Limit] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_LinhVucTTHC]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_LinhVucTTHC](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](250) NULL,
	[LangCode] [varchar](5) NOT NULL,
 CONSTRAINT [PK_tbl_LinhVucTTHC] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_LinhVucVanBan]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_LinhVucVanBan](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](250) NULL,
	[LangCode] [varchar](5) NOT NULL,
	[Ordering] [int] NOT NULL,
	[Status] [bit] NOT NULL,
 CONSTRAINT [PK_tbl_LinhVucVanBan] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_LinkWebsite]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_LinkWebsite](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Url] [nvarchar](255) NOT NULL,
	[Status] [bit] NOT NULL,
	[LangCode] [varchar](5) NULL,
	[LangName] [nvarchar](50) NULL,
	[Logo] [nvarchar](250) NULL,
	[Type] [int] NOT NULL,
	[Ordering] [int] NOT NULL,
 CONSTRAINT [PK_tbl_LinkWebsite] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_LoaiVanBan]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_LoaiVanBan](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](250) NULL,
	[LangCode] [varchar](5) NOT NULL,
	[Ordering] [int] NOT NULL,
	[Status] [bit] NOT NULL,
 CONSTRAINT [PK_tbl_LoaiVanBan] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Logs]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Logs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ContentJson] [nvarchar](max) NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_Logs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Magazine]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Magazine](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Year] [int] NULL,
	[Title] [nvarchar](max) NULL,
	[Photo] [nvarchar](max) NULL,
	[Attachment] [nvarchar](max) NULL,
	[Ordering] [int] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[Status] [bit] NOT NULL,
	[AttachmentName] [nvarchar](max) NULL,
	[AttachmentSize] [varchar](max) NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_Magazine] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ModuleCate]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ModuleCate](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Code] [varchar](max) NULL,
	[ParentID] [int] NOT NULL,
	[Status] [bit] NULL,
	[CreateDate] [datetime] NULL,
	[CreateBy] [int] NULL,
	[LangCode] [nchar](10) NULL,
	[Level] [int] NOT NULL,
	[Ordering] [int] NULL,
 CONSTRAINT [PK_tbl_ModuleCate] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_News]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_News](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[SubTitle] [nvarchar](max) NULL,
	[Photo] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Details] [nvarchar](max) NULL,
	[Status] [int] NULL,
	[IsPublish] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[CategoryIdStr] [varchar](200) NULL,
	[SourceNews] [nvarchar](max) NULL,
	[Author] [nvarchar](max) NULL,
	[LangCode] [varchar](5) NULL,
	[Tags] [nvarchar](max) NULL,
	[Type] [int] NULL,
	[Position] [varchar](250) NULL,
	[CreatedBy] [int] NULL,
	[ModifyBy] [int] NULL,
	[ViewCount] [bigint] NULL,
	[AllowComment] [tinyint] NULL,
	[IsGetNews] [tinyint] NULL,
	[IsComment] [bit] NOT NULL,
	[NewMoney] [int] NULL,
	[RelatedNews] [nvarchar](max) NULL,
	[Attachment] [nvarchar](max) NULL,
	[UserActId] [int] NOT NULL,
	[ProcedureId] [int] NOT NULL,
	[NhuanBut] [bigint] NOT NULL,
	[IdUserChoose] [int] NULL,
	[OldId] [nvarchar](max) NULL,
	[AuthorGuidID] [nvarchar](max) NULL,
	[IsDelete] [smallint] NOT NULL,
	[ZoneID] [nvarchar](max) NULL,
	[IsTraLai] [bit] NOT NULL,
	[CTVID] [int] NULL,
	[TimeChoose] [datetime] NULL,
	[IsXBDuyet] [bit] NOT NULL,
 CONSTRAINT [PK_tbl_News] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_NewsComment]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_NewsComment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NewsID] [int] NOT NULL,
	[CommentID] [int] NULL,
	[FullName] [nvarchar](500) NOT NULL,
	[Email] [nvarchar](500) NULL,
	[NoiDung] [nvarchar](max) NOT NULL,
	[CreateDate] [datetime] NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[IsNews] [bit] NOT NULL,
	[IsView] [bit] NOT NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_NewsComment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_NewsPaper]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_NewsPaper](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[NoiDung] [nvarchar](max) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Status] [int] NOT NULL,
	[CreateBy] [int] NOT NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_NewsPaper] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_NewsPaperComment]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_NewsPaperComment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NewsPaperID] [int] NOT NULL,
	[FullName] [nvarchar](500) NOT NULL,
	[Email] [nvarchar](500) NULL,
	[NoiDung] [nvarchar](max) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Phone] [int] NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_NewsPaperComment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Online]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Online](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Ip] [nvarchar](500) NULL,
	[Session] [nvarchar](500) NULL,
	[CreateDate] [datetime] NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_Online] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_PageElement]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_PageElement](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Code] [varchar](50) NULL,
	[LangCode] [varchar](5) NOT NULL,
	[Banner] [nvarchar](250) NULL,
	[Ordering] [int] NULL,
	[Description] [nvarchar](500) NULL,
	[Status] [bit] NOT NULL,
	[LangName] [nvarchar](50) NULL,
	[Subdomain] [nvarchar](50) NULL,
	[Area] [varchar](50) NULL,
	[Footer] [nvarchar](max) NULL,
 CONSTRAINT [PK_tbl_PageElement] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Position]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Position](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Code] [varchar](250) NOT NULL,
	[ParentID] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[Status] [bit] NOT NULL,
	[Ordering] [int] NOT NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_Position] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Product]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Product](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[Images] [varchar](200) NULL,
	[Description] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ViewCount] [bigint] NOT NULL,
	[Status] [bit] NOT NULL,
	[Content] [nvarchar](max) NULL,
	[Ordering] [int] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_Product] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ProductCategory]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ProductCategory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Ordering] [int] NOT NULL,
	[Status] [bit] NOT NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_ProductCategory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ProductOverView]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ProductOverView](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[Title] [nvarchar](250) NOT NULL,
	[Images] [varchar](200) NULL,
	[Description] [nvarchar](max) NULL,
	[Ordering] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[Status] [bit] NOT NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_ProductOverView] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_QALinhVuc]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_QALinhVuc](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](250) NULL,
	[LangCode] [varchar](5) NULL,
	[Ordering] [int] NULL,
	[isPublish] [bit] NOT NULL,
 CONSTRAINT [PK_tbl_QALinhVuc] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_QuestionAndAnswer]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_QuestionAndAnswer](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FullNameOfQuestion] [nvarchar](50) NOT NULL,
	[Email] [varchar](50) NULL,
	[Question] [nvarchar](max) NOT NULL,
	[Answer] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[AnswerBy] [int] NULL,
	[AnswerDate] [datetime] NULL,
	[UserNameOfAnswer] [nvarchar](50) NULL,
	[Status] [bit] NOT NULL,
	[Phone] [varchar](150) NULL,
	[DiaChi] [nvarchar](max) NULL,
	[LinhVucID] [int] NOT NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_QuestionAndAnswer] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_SlideImages]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_SlideImages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NULL,
	[Images] [nvarchar](max) NOT NULL,
	[Status] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedBy] [int] NULL,
	[PageElementId] [nvarchar](500) NULL,
	[Url] [nvarchar](250) NULL,
	[Type] [int] NOT NULL,
	[Ordering] [int] NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_SlideImages] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_SubeMagazine]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_SubeMagazine](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IDParent] [int] NULL,
	[Ordering] [int] NOT NULL,
	[StrPhoto] [nvarchar](max) NULL,
	[NumberPhoto] [int] NOT NULL,
	[isFullWidth] [bit] NOT NULL,
	[WidthPhoto] [int] NULL,
	[PositionPhoto] [int] NULL,
	[Content] [nvarchar](max) NULL,
 CONSTRAINT [PK_tbl_SubeMagazine] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_TaiLieu]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_TaiLieu](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CatID] [int] NOT NULL,
	[NameFile] [nvarchar](max) NULL,
	[LinkFile] [nvarchar](max) NULL,
	[ReplaceName] [nvarchar](max) NULL,
	[Ordering] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Status] [bit] NOT NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_TaiLieu] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ThamDoYKien]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ThamDoYKien](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Poll] [int] NOT NULL,
	[Ordering] [int] NOT NULL,
	[Status] [bit] NOT NULL,
	[IpMay] [varchar](max) NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_ThamDoYKien] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ThamDoYKien_Ip]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ThamDoYKien_Ip](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PollId] [int] NOT NULL,
	[SessionID] [varchar](100) NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_ThamDoYKien_Ip] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ToGiac]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ToGiac](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ToiPhamID] [int] NULL,
	[FullName] [nvarchar](500) NULL,
	[Email] [nvarchar](500) NULL,
	[NoiDung] [nvarchar](max) NOT NULL,
	[DiaChi] [nvarchar](max) NULL,
	[CreateDate] [datetime] NULL,
	[Phone] [int] NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_ToGiac] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Topic]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Topic](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Photo] [nvarchar](max) NULL,
	[Details] [nvarchar](max) NULL,
	[Status] [bit] NULL,
	[Comment] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[Author] [nvarchar](150) NULL,
	[LangCode] [varchar](5) NULL,
	[Tags] [nvarchar](250) NULL,
	[CreatedBy] [int] NULL,
	[ModifyBy] [int] NULL,
	[ViewCount] [bigint] NULL,
	[AllowComment] [tinyint] NULL,
	[Attachment] [nvarchar](max) NULL,
	[StartDate] [datetime] NULL,
	[ListImgSlide] [nvarchar](max) NULL,
	[IdNguoiChon] [nvarchar](max) NULL,
 CONSTRAINT [PK_tbl_Topic] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_TopicComment]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_TopicComment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TopicID] [int] NOT NULL,
	[CommentID] [int] NULL,
	[FullName] [nvarchar](500) NOT NULL,
	[Email] [nvarchar](500) NULL,
	[NoiDung] [nvarchar](max) NOT NULL,
	[CreateDate] [datetime] NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[IsView] [bit] NOT NULL,
	[IsNew] [bit] NOT NULL,
	[TraLoi] [nvarchar](max) NULL,
	[NguoiTraLoi] [nvarchar](max) NULL,
	[ThoiGianTraLoi] [datetime] NULL,
	[IdNguoiChon] [int] NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_TopicComment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_truyna]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_truyna](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NULL,
	[TenKhac] [nvarchar](255) NULL,
	[GioiTinh] [smallint] NOT NULL,
	[NamSinh] [nvarchar](50) NULL,
	[NoiSinh] [nvarchar](255) NULL,
	[HoKhauTT] [nvarchar](255) NULL,
	[NoiDKTT] [nvarchar](255) NULL,
	[QuocTich] [nvarchar](255) NULL,
	[DanToc] [nvarchar](255) NULL,
	[TenCha] [nvarchar](255) NULL,
	[TenMe] [nvarchar](255) NULL,
	[ChieuCao] [nvarchar](255) NULL,
	[MauDa] [nvarchar](255) NULL,
	[MaiToc] [nvarchar](255) NULL,
	[LongMay] [nvarchar](255) NULL,
	[DacDiemMui] [nvarchar](255) NULL,
	[DacDiemMat] [nvarchar](255) NULL,
	[DacDiemTai] [nvarchar](255) NULL,
	[DacDiemKhac] [nvarchar](255) NULL,
	[ToiDanh] [nvarchar](255) NULL,
	[HeLoaiToiDanh] [nvarchar](255) NULL,
	[LoaiTruyNa] [nvarchar](255) NULL,
	[PhamViTruyNa] [nvarchar](255) NULL,
	[QuyetDinhTruyNa] [nvarchar](255) NULL,
	[DonViRaQD] [nvarchar](255) NULL,
	[NgayTruyNaQT] [nvarchar](255) NULL,
	[HoSoTruyNaQT] [nvarchar](255) NULL,
	[QuocGiaNghiTron] [nvarchar](255) NULL,
	[BaoChoDonVi] [nvarchar](255) NULL,
	[DienThoai] [nvarchar](255) NULL,
	[Photo] [nvarchar](255) NULL,
	[CreateDate] [datetime] NOT NULL,
	[Status] [int] NOT NULL,
	[IsHome] [bit] NOT NULL,
	[IsDinhNa] [bit] NOT NULL,
	[Attachment] [nvarchar](max) NULL,
	[AttachmentTN] [nvarchar](max) NULL,
	[SmallPhoto] [nvarchar](max) NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_truyna] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_TTHC]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_TTHC](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[DonViId] [int] NULL,
	[LinhVucId] [int] NULL,
	[ThuTuc] [nvarchar](500) NULL,
	[LangCode] [varchar](5) NULL,
	[Attachment] [nvarchar](max) NULL,
	[Tieude] [nvarchar](500) NULL,
	[TrinhTuThucHien] [nvarchar](max) NULL,
	[CachThucThucHien] [nvarchar](max) NULL,
	[ThanhPhanHoSo] [nvarchar](max) NULL,
	[ThoiHan] [nvarchar](max) NULL,
	[DoiTuong] [nvarchar](max) NULL,
	[CoQuanThucHien] [nvarchar](max) NULL,
	[KetQua] [nvarchar](max) NULL,
	[LePhi] [nvarchar](max) NULL,
	[TenMauDon] [nvarchar](max) NULL,
	[YeuCauDieuKien] [nvarchar](max) NULL,
	[CoSoPhapLy] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
	[ViewCount] [bigint] NOT NULL,
	[AllowComment] [tinyint] NULL,
 CONSTRAINT [PK_tbl_TTHC] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_TTHCComment]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_TTHCComment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TTHCID] [int] NOT NULL,
	[CommentID] [int] NULL,
	[FullName] [nvarchar](500) NOT NULL,
	[Email] [nvarchar](500) NULL,
	[NoiDung] [nvarchar](max) NOT NULL,
	[CreateDate] [datetime] NULL,
	[Status] [bit] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[IsView] [bit] NOT NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_TTHCComment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_User]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_User](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](250) NOT NULL,
	[Email] [varchar](250) NULL,
	[FullName] [nvarchar](250) NULL,
	[Gender] [tinyint] NULL,
	[Photo] [nvarchar](max) NULL,
	[Address] [nvarchar](500) NULL,
	[City] [nvarchar](250) NULL,
	[District] [nvarchar](250) NULL,
	[Country] [nvarchar](150) NULL,
	[zip] [int] NULL,
	[Active] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[Password] [nvarchar](500) NOT NULL,
	[PasswordQuestion] [nvarchar](500) NULL,
	[PasswordAnswer] [nvarchar](500) NULL,
	[Telephone] [varchar](20) NULL,
	[Phone] [varchar](20) NULL,
	[DonviId] [int] NOT NULL,
	[ChucVuId] [int] NOT NULL,
	[UserType] [int] NOT NULL,
	[TimeLogin] [datetime] NULL,
	[IPLogin] [nvarchar](500) NULL,
	[GroupUserID] [nvarchar](50) NULL,
	[NoiBo] [tinyint] NULL,
	[PageNoiBo] [nvarchar](250) NULL,
	[isAdmin] [bit] NOT NULL,
	[IsBanChapHanh] [bit] NOT NULL,
	[IsShow] [bit] NOT NULL,
	[SoTaiKhoan] [nvarchar](100) NULL,
	[isDuyetComment] [bit] NOT NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_UserAdmin]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_UserAdmin](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](250) NOT NULL,
	[Email] [varchar](250) NOT NULL,
	[FullName] [nvarchar](250) NULL,
	[Gender] [tinyint] NULL,
	[Photo] [nvarchar](max) NULL,
	[Address] [nvarchar](500) NULL,
	[City] [nvarchar](250) NULL,
	[District] [nvarchar](250) NULL,
	[Country] [nvarchar](150) NULL,
	[zip] [int] NULL,
	[Active] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[Password] [nvarchar](500) NOT NULL,
	[PasswordQuestion] [nvarchar](500) NULL,
	[PasswordAnswer] [nvarchar](500) NULL,
	[Phone] [varchar](20) NULL,
	[TimeLogin] [datetime] NULL,
	[IPLogin] [nvarchar](500) NULL,
	[GroupUserID] [nvarchar](250) NULL,
	[PageElementID] [varchar](250) NULL,
	[QuyTrinhXuatBanID] [varchar](250) NULL,
	[isAdmin] [bit] NOT NULL,
	[SoTaiKhoan] [nvarchar](100) NULL,
	[IsDuyetComment] [bit] NOT NULL,
	[LastOnline] [datetime] NULL,
	[isOnline] [bit] NULL,
	[NganHang] [nvarchar](max) NULL,
	[LangCode] [varchar](50) NULL,
	[IsCTV] [bit] NULL,
 CONSTRAINT [PK_tbl_UserAdmin] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_VanBan]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_VanBan](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[Status] [bit] NOT NULL,
	[SoHieu] [nvarchar](50) NULL,
	[NgayHieuLuc] [datetime] NULL,
	[NgayHieuLucVARCHAR] [nvarchar](50) NULL,
	[NguoiKy] [nvarchar](150) NULL,
	[CoQuanBanHanhId] [int] NOT NULL,
	[LoaiVanBanId] [int] NOT NULL,
	[LinhVucVanBanId] [int] NOT NULL,
	[TrichYeu] [nvarchar](500) NOT NULL,
	[LangCode] [varchar](5) NOT NULL,
	[Attachment] [nvarchar](max) NULL,
	[isNoiBo] [int] NULL,
	[IsHome] [bit] NULL,
	[NgayBanHanh] [datetime] NULL,
	[HetHieuLuc] [bit] NULL,
	[IsRight] [bit] NULL,
	[IsSendMail] [bit] NULL,
	[IsSendSMS] [bit] NULL,
	[Description] [nvarchar](2000) NULL,
	[CateID] [int] NULL,
	[FileID] [int] NOT NULL,
	[ExpiryDate] [nvarchar](50) NULL,
	[Agencies] [nvarchar](200) NULL,
	[ManCreate] [int] NULL,
	[AttachmentName] [nvarchar](max) NULL,
	[AttachmentSize] [varchar](max) NULL,
 CONSTRAINT [PK_tbl_VanBan] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Video]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Video](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Images] [nvarchar](max) NULL,
	[Description] [nvarchar](500) NULL,
	[Url] [nvarchar](500) NULL,
	[IsHot] [bit] NOT NULL,
	[Status] [bit] NOT NULL,
	[VideoCategoryId] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LangCode] [varchar](5) NULL,
	[CreatedBy] [varchar](50) NULL,
	[Type] [int] NOT NULL,
	[Ordering] [int] NULL,
	[ViewNumber] [bigint] NOT NULL,
	[Duration] [nvarchar](200) NULL,
	[IsHome] [bit] NOT NULL,
	[UserActId] [int] NOT NULL,
	[ProcedureId] [int] NOT NULL,
	[NewMoney] [int] NULL,
	[CreateById] [int] NULL,
 CONSTRAINT [PK_tbl_Video] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_VideoCategory]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_VideoCategory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[Url] [nvarchar](250) NULL,
	[Images] [nvarchar](250) NULL,
	[Description] [nvarchar](500) NULL,
	[Status] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [varchar](50) NOT NULL,
	[ModifiedBy] [varchar](50) NULL,
	[Type] [int] NOT NULL,
	[Ordering] [int] NULL,
	[LangCode] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_VideoCategory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[test_category]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[test_category](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NULL,
	[Name] [nvarchar](max) NULL,
	[Photo] [nvarchar](max) NULL,
	[LangCode] [varchar](50) NULL,
	[Title] [nvarchar](max) NULL,
	[Keyword] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Ordering] [int] NULL,
	[Status] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[support_1] [nvarchar](max) NULL,
	[support_2] [nvarchar](max) NULL,
	[advert_1] [nvarchar](max) NULL,
	[advert_2] [nvarchar](max) NULL,
	[display_1] [nvarchar](max) NULL,
	[display_2] [nvarchar](max) NULL,
	[view_more_detail] [nvarchar](max) NULL,
 CONSTRAINT [PK_test_category] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[view_Category_Languages]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[view_Category_Languages](
	[ID] [int] NOT NULL,
	[ParentID] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](250) NULL,
	[Icon] [nvarchar](max) NULL,
	[Color] [varchar](50) NULL,
	[Background] [varchar](50) NULL,
	[Ordering] [int] NOT NULL,
	[Active] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[LangCode] [varchar](5) NULL,
	[LangName] [nvarchar](250) NOT NULL,
	[Code] [varchar](5) NOT NULL,
	[LangIcon] [nvarchar](250) NULL,
	[PageElementId] [int] NOT NULL,
	[Slug] [nvarchar](50) NOT NULL,
	[FeautureImage] [nvarchar](max) NULL,
 CONSTRAINT [PK_view_Category_Languages] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[ParentID] ASC,
	[Name] ASC,
	[Ordering] ASC,
	[Active] ASC,
	[LangName] ASC,
	[Code] ASC,
	[PageElementId] ASC,
	[Slug] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[View_DataHome]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[View_DataHome](
	[ID] [int] NOT NULL,
	[Code] [nvarchar](max) NULL,
	[isPublish] [bit] NOT NULL,
	[LangCode] [varchar](50) NULL,
	[Name] [nvarchar](max) NULL,
	[NameEnglish] [nvarchar](max) NULL,
	[Ordering] [int] NULL,
	[SoLuong] [int] NULL,
 CONSTRAINT [PK_View_DataHome] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[isPublish] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[View_Gallery_Category]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[View_Gallery_Category](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](250) NULL,
	[Images] [nvarchar](max) NOT NULL,
	[Status] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedBy] [int] NULL,
	[PageElementId] [int] NOT NULL,
	[Url] [nvarchar](250) NULL,
	[Type] [int] NOT NULL,
	[Ordering] [int] NULL,
	[CategoryId] [int] NULL,
	[CatName] [nvarchar](500) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[view_Images_ImageCategory]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[view_Images_ImageCategory](
	[Title] [nvarchar](max) NOT NULL,
	[Images] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Url] [nvarchar](500) NULL,
	[Status] [bit] NOT NULL,
	[Ordering] [int] NULL,
	[CateName] [nvarchar](500) NOT NULL,
	[ImageCategoryId] [varchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[view_News]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[view_News](
	[CreatedDate] [datetime] NOT NULL,
	[ViewCount] [bigint] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [int] NULL,
	[Author] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[Title] [nvarchar](max) NOT NULL,
	[SubTitle] [nvarchar](max) NULL,
	[Photo] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[IsPublish] [bit] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[CategoryIdStr] [varchar](200) NULL,
	[SourceNews] [nvarchar](max) NULL,
	[LangCode] [varchar](5) NULL,
	[Type] [int] NULL,
	[Tags] [nvarchar](max) NULL,
	[Position] [varchar](250) NULL,
	[ModifyBy] [int] NULL,
	[AllowComment] [tinyint] NULL,
	[IsGetNews] [tinyint] NULL,
	[IsComment] [bit] NOT NULL,
	[NewMoney] [int] NULL,
	[RelatedNews] [nvarchar](max) NULL,
	[Attachment] [nvarchar](max) NULL,
	[UserActId] [int] NOT NULL,
	[ProcedureId] [int] NOT NULL,
	[NhuanBut] [bigint] NOT NULL,
	[IdUserChoose] [int] NULL,
	[OldId] [nvarchar](max) NULL,
	[AuthorGuidID] [nvarchar](max) NULL,
	[ZoneID] [nvarchar](max) NULL,
	[IsDelete] [smallint] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[view_News_HomeCategory]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[view_News_HomeCategory](
	[ID] [int] NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[SubTitle] [nvarchar](max) NULL,
	[Photo] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Status] [int] NULL,
	[IsPublish] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[CategoryIdStr] [varchar](200) NULL,
	[SourceNews] [nvarchar](max) NULL,
	[Author] [nvarchar](max) NULL,
	[LangCode] [varchar](5) NULL,
	[Tags] [nvarchar](max) NULL,
	[Type] [int] NULL,
	[Position] [varchar](250) NULL,
	[CreatedBy] [int] NULL,
	[ModifyBy] [int] NULL,
	[ViewCount] [bigint] NULL,
	[AllowComment] [tinyint] NULL,
	[IsGetNews] [tinyint] NULL,
	[IsComment] [bit] NOT NULL,
	[NewMoney] [int] NULL,
	[RelatedNews] [nvarchar](max) NULL,
	[Attachment] [nvarchar](max) NULL,
	[UserActId] [int] NOT NULL,
	[ProcedureId] [int] NOT NULL,
	[NhuanBut] [bigint] NOT NULL,
	[IdUserChoose] [int] NULL,
	[Expr3] [varchar](500) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[view_News_hotAndUnderhot]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[view_News_hotAndUnderhot](
	[CreatedDate] [datetime] NOT NULL,
	[ViewCount] [bigint] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [int] NULL,
	[Author] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[Title] [nvarchar](max) NOT NULL,
	[SubTitle] [nvarchar](max) NULL,
	[Photo] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[IsPublish] [bit] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[CategoryIdStr] [varchar](200) NULL,
	[SourceNews] [nvarchar](max) NULL,
	[LangCode] [varchar](5) NULL,
	[Type] [int] NULL,
	[Tags] [nvarchar](max) NULL,
	[Position] [varchar](250) NULL,
	[ModifyBy] [int] NULL,
	[AllowComment] [tinyint] NULL,
	[IsGetNews] [tinyint] NULL,
	[IsComment] [bit] NOT NULL,
	[NewMoney] [int] NULL,
	[RelatedNews] [nvarchar](max) NULL,
	[Attachment] [nvarchar](max) NULL,
	[UserActId] [int] NOT NULL,
	[ProcedureId] [int] NOT NULL,
	[NhuanBut] [bigint] NOT NULL,
	[IdUserChoose] [int] NULL,
	[OldId] [nvarchar](max) NULL,
	[AuthorGuidID] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[view_News_Limit]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[view_News_Limit](
	[CreatedDate] [datetime] NOT NULL,
	[ViewCount] [bigint] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [int] NULL,
	[Author] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[Title] [nvarchar](max) NOT NULL,
	[SubTitle] [nvarchar](max) NULL,
	[Photo] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[IsPublish] [bit] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[CategoryIdStr] [varchar](200) NULL,
	[SourceNews] [nvarchar](max) NULL,
	[LangCode] [varchar](5) NULL,
	[Type] [int] NULL,
	[Tags] [nvarchar](max) NULL,
	[Position] [varchar](250) NULL,
	[ModifyBy] [int] NULL,
	[AllowComment] [tinyint] NULL,
	[IsGetNews] [tinyint] NULL,
	[IsComment] [bit] NOT NULL,
	[NewMoney] [int] NULL,
	[RelatedNews] [nvarchar](max) NULL,
	[Attachment] [nvarchar](max) NULL,
	[UserActId] [int] NOT NULL,
	[ProcedureId] [int] NOT NULL,
	[NhuanBut] [bigint] NOT NULL,
	[IdUserChoose] [int] NULL,
	[OldId] [nvarchar](max) NULL,
	[AuthorGuidID] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[view_NEWS_LISTDATA]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[view_NEWS_LISTDATA](
	[CreatedBy] [int] NULL,
	[CategoryId] [int] NOT NULL,
	[NguoiTao] [varchar](250) NULL,
	[ID] [int] NOT NULL,
	[TieuDeChinh] [nvarchar](500) NOT NULL,
	[Status] [int] NULL,
	[AnhDaiDien] [nvarchar](250) NULL,
	[LangCode] [varchar](5) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[Author] [nvarchar](50) NULL,
	[CatName] [nvarchar](250) NULL,
	[CatImage] [nvarchar](max) NULL,
	[CatImage2] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[IsGetNews] [tinyint] NULL,
	[AllowComment] [tinyint] NULL,
	[ViewCount] [bigint] NULL,
	[ModifyBy] [int] NULL,
	[Tags] [nvarchar](250) NULL,
	[NewMoney] [int] NULL,
	[MainTitle] [nvarchar](500) NOT NULL,
	[Avatar] [nvarchar](250) NULL,
 CONSTRAINT [PK_view_NEWS_LISTDATA] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC,
	[ID] ASC,
	[TieuDeChinh] ASC,
	[CreatedDate] ASC,
	[MainTitle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[view_NEWS_LISTDETAIL]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[view_NEWS_LISTDETAIL](
	[CreatedBy] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[NguoiTao] [varchar](250) NULL,
	[ID] [int] NOT NULL,
	[Title] [nvarchar](250) NOT NULL,
	[Status] [int] NULL,
	[FeautureImage] [nvarchar](max) NULL,
	[LangCode] [varchar](5) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[DisplayZone] [varchar](100) NULL,
	[PageElementId] [int] NULL,
	[ModifyDate] [datetime] NULL,
	[Author] [nvarchar](50) NULL,
	[NoiBo] [tinyint] NULL,
	[IsHot] [tinyint] NULL,
	[IsSlide] [tinyint] NULL,
	[IsNew] [tinyint] NULL,
	[CatName] [nvarchar](250) NULL,
	[CatImage] [nvarchar](max) NULL,
	[CatImage2] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[CropImage] [nvarchar](max) NULL,
	[MorePageId] [nvarchar](250) NULL,
	[MoreCategoryId] [nvarchar](250) NULL,
	[MoreDisplayZone] [nvarchar](250) NULL,
	[IsGetNews] [tinyint] NULL,
	[AllowComment] [tinyint] NULL,
	[ViewCount] [bigint] NOT NULL,
	[ModifyBy] [int] NULL,
	[Tags] [nvarchar](250) NULL,
	[KeyWords] [nvarchar](250) NULL,
	[IsRightSlide] [tinyint] NULL,
	[Details] [nvarchar](max) NULL,
 CONSTRAINT [PK_view_NEWS_LISTDETAIL] PRIMARY KEY CLUSTERED 
(
	[CreatedBy] ASC,
	[CategoryId] ASC,
	[ID] ASC,
	[Title] ASC,
	[CreatedDate] ASC,
	[ViewCount] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[view_News_MultiCate]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[view_News_MultiCate](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[SubTitle] [nvarchar](max) NULL,
	[CateComma] [varchar](202) NULL,
	[Photo] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Status] [int] NULL,
	[IsPublish] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[CategoryIdStr] [varchar](200) NULL,
	[SourceNews] [nvarchar](max) NULL,
	[Author] [nvarchar](max) NULL,
	[LangCode] [varchar](5) NULL,
	[Tags] [nvarchar](max) NULL,
	[Type] [int] NULL,
	[Position] [varchar](250) NULL,
	[CreatedBy] [int] NULL,
	[ModifyBy] [int] NULL,
	[ViewCount] [bigint] NULL,
	[AllowComment] [tinyint] NULL,
	[IsGetNews] [tinyint] NULL,
	[IsComment] [bit] NOT NULL,
	[NewMoney] [int] NULL,
	[RelatedNews] [nvarchar](max) NULL,
	[Attachment] [nvarchar](max) NULL,
	[UserActId] [int] NOT NULL,
	[ProcedureId] [int] NOT NULL,
	[NhuanBut] [bigint] NOT NULL,
	[IdUserChoose] [int] NULL,
	[IsDelete] [smallint] NOT NULL,
	[IsTraLai] [bit] NOT NULL,
	[CTVID] [int] NULL,
	[TimeChoose] [datetime] NULL,
	[IsXBDuyet] [bit] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[view_News_noPublic]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[view_News_noPublic](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[SubTitle] [nvarchar](max) NULL,
	[Photo] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Details] [nvarchar](max) NULL,
	[Status] [int] NULL,
	[IsPublish] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[CategoryIdStr] [varchar](200) NULL,
	[SourceNews] [nvarchar](max) NULL,
	[Author] [nvarchar](max) NULL,
	[LangCode] [varchar](5) NULL,
	[Tags] [nvarchar](max) NULL,
	[Type] [int] NULL,
	[Position] [varchar](250) NULL,
	[CreatedBy] [int] NULL,
	[ModifyBy] [int] NULL,
	[ViewCount] [bigint] NULL,
	[AllowComment] [tinyint] NULL,
	[IsGetNews] [tinyint] NULL,
	[IsComment] [bit] NOT NULL,
	[NewMoney] [int] NULL,
	[RelatedNews] [nvarchar](max) NULL,
	[Attachment] [nvarchar](max) NULL,
	[UserActId] [int] NOT NULL,
	[ProcedureId] [int] NOT NULL,
	[NhuanBut] [bigint] NOT NULL,
	[IdUserChoose] [int] NULL,
	[OldId] [nvarchar](max) NULL,
	[AuthorGuidID] [nvarchar](max) NULL,
	[IsDelete] [smallint] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[view_News_Public]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[view_News_Public](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[SubTitle] [nvarchar](max) NULL,
	[Photo] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Details] [nvarchar](max) NULL,
	[Status] [int] NULL,
	[IsPublish] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[CategoryIdStr] [varchar](200) NULL,
	[SourceNews] [nvarchar](max) NULL,
	[Author] [nvarchar](max) NULL,
	[LangCode] [varchar](5) NULL,
	[Tags] [nvarchar](max) NULL,
	[Type] [int] NULL,
	[Position] [varchar](250) NULL,
	[CreatedBy] [int] NULL,
	[ModifyBy] [int] NULL,
	[ViewCount] [bigint] NULL,
	[AllowComment] [tinyint] NULL,
	[IsGetNews] [tinyint] NULL,
	[IsComment] [bit] NOT NULL,
	[NewMoney] [int] NULL,
	[RelatedNews] [nvarchar](max) NULL,
	[Attachment] [nvarchar](max) NULL,
	[UserActId] [int] NOT NULL,
	[ProcedureId] [int] NOT NULL,
	[NhuanBut] [bigint] NOT NULL,
	[IdUserChoose] [int] NULL,
	[OldId] [nvarchar](max) NULL,
	[AuthorGuidID] [nvarchar](max) NULL,
	[IsDelete] [smallint] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[view_news_search]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[view_news_search](
	[Title] [nvarchar](250) NOT NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[KeyWords] [nvarchar](250) NULL,
	[Tags] [nvarchar](250) NULL,
	[PageElementId] [int] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[FeautureImage] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[NoiBo] [tinyint] NULL,
	[Status] [int] NULL,
	[IsSlide] [tinyint] NULL,
	[IsHot] [tinyint] NULL,
	[IsNew] [tinyint] NULL,
	[CropImage] [nvarchar](max) NULL,
 CONSTRAINT [PK_view_news_search] PRIMARY KEY CLUSTERED 
(
	[Title] ASC,
	[ID] ASC,
	[CategoryId] ASC,
	[CreatedDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[view_News_Sukien]    Script Date: 3/17/2023 6:35:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[view_News_Sukien](
	[CreatedDate] [datetime] NOT NULL,
	[ViewCount] [bigint] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [int] NULL,
	[Author] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[Title] [nvarchar](max) NOT NULL,
	[SubTitle] [nvarchar](max) NULL,
	[Photo] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[IsPublish] [bit] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[CategoryIdStr] [varchar](200) NULL,
	[SourceNews] [nvarchar](max) NULL,
	[LangCode] [varchar](5) NULL,
	[Type] [int] NULL,
	[Tags] [nvarchar](max) NULL,
	[Position] [varchar](250) NULL,
	[ModifyBy] [int] NULL,
	[AllowComment] [tinyint] NULL,
	[IsGetNews] [tinyint] NULL,
	[IsComment] [bit] NOT NULL,
	[NewMoney] [int] NULL,
	[RelatedNews] [nvarchar](max) NULL,
	[Attachment] [nvarchar](max) NULL,
	[UserActId] [int] NOT NULL,
	[ProcedureId] [int] NOT NULL,
	[NhuanBut] [bigint] NOT NULL,
	[IdUserChoose] [int] NULL,
	[OldId] [nvarchar](max) NULL,
	[AuthorGuidID] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tbl_ConfigText] ON 

INSERT [dbo].[tbl_ConfigText] ([ID], [CreatedDate], [CreatedBy], [LastModifieDate], [LastModifieBy], [LangCode], [ContentJson]) VALUES (1, CAST(N'2023-03-15T00:00:00.000' AS DateTime), 0, CAST(N'2023-03-15T23:24:18.780' AS DateTime), NULL, N'vn                  ', N'[{"ID":"d807af3f-9dab-4258-935f-7b8a8c2a0990","Field":"truyenhinh","Value":"","Note":null,"ActionBy":null,"ActionTime":"2023-03-15T23:24:18.746444+07:00","LangCode":"vn","NameLangCode":null,"NgonNgu":null},{"ID":"877f76df-58f5-46fc-821b-f33b93c2fa17","Field":"ThuVienAnhHome","Value":"","Note":null,"ActionBy":null,"ActionTime":"2023-03-15T23:24:20.0480335+07:00","LangCode":"vn","NameLangCode":null,"NgonNgu":null},{"ID":"1ef93fe7-6251-44b9-bcc2-e76a7ba575d3","Field":"LienKetWebSite","Value":"","Note":null,"ActionBy":null,"ActionTime":"2023-03-15T23:24:20.4162473+07:00","LangCode":"vn","NameLangCode":null,"NgonNgu":null},{"ID":"7c7cdfc8-eede-4b11-a5fb-14750e004933","Field":"ChonLienKetWebSite","Value":"","Note":null,"ActionBy":null,"ActionTime":"2023-03-15T23:24:20.4182521+07:00","LangCode":"vn","NameLangCode":null,"NgonNgu":null},{"ID":"f200e1a6-74ad-44e8-b75f-30bd9760190d","Field":"Title_Site","Value":"","Note":null,"ActionBy":null,"ActionTime":"2023-03-15T23:24:21.9304685+07:00","LangCode":"vn","NameLangCode":null,"NgonNgu":null},{"ID":"3853caca-dcb8-4896-8949-0c62dd29da49","Field":"thongbao","Value":"Thông báo","Note":null,"ActionBy":null,"ActionTime":"2023-03-15T23:26:11.827619+07:00","LangCode":"vn","NameLangCode":null,"NgonNgu":null},{"ID":"75e100a3-8d31-4518-801a-92c42dd77e5b","Field":"KetNoiVoiChungToi","Value":"","Note":null,"ActionBy":null,"ActionTime":"2023-03-15T23:26:12.35756+07:00","LangCode":"vn","NameLangCode":null,"NgonNgu":null},{"ID":"64b3d3a2-2ebe-4101-b579-5be43200b0f6","Field":"magazine","Value":"","Note":null,"ActionBy":null,"ActionTime":"2023-03-15T23:26:16.2596521+07:00","LangCode":"vn","NameLangCode":null,"NgonNgu":null},{"ID":"6bae9abb-5e1b-43c4-8816-5de565d480e8","Field":"NamHome","Value":"","Note":null,"ActionBy":null,"ActionTime":"2023-03-15T23:26:16.8996337+07:00","LangCode":"vn","NameLangCode":null,"NgonNgu":null},{"ID":"946f3c0a-aeaa-499b-835c-40b12ef6018b","Field":"TapChiHome","Value":"","Note":null,"ActionBy":null,"ActionTime":"2023-03-15T23:26:16.9012982+07:00","LangCode":"vn","NameLangCode":null,"NgonNgu":null},{"ID":"7c0846d6-6319-4f9a-a091-0ccd29ed24d6","Field":"sitekeylocal","Value":"","Note":null,"ActionBy":null,"ActionTime":"2023-03-15T23:27:56.6953848+07:00","LangCode":"vn","NameLangCode":null,"NgonNgu":null},{"ID":"b85c2b50-36bd-4fe4-a777-5b3d0a0d5bb9","Field":"recapcha","Value":"","Note":null,"ActionBy":null,"ActionTime":"2023-03-15T23:27:56.6995288+07:00","LangCode":"vn","NameLangCode":null,"NgonNgu":null},{"ID":"4b8bdfd2-e49c-4527-8e1b-d3130780fed0","Field":"title_AdminSite","Value":"","Note":null,"ActionBy":null,"ActionTime":"2023-03-15T23:32:06.3952265+07:00","LangCode":"vn","NameLangCode":null,"NgonNgu":null},{"ID":"95e3f24e-8fb3-42e0-86bf-d67334a9f471","Field":"title_logoAdmin","Value":"","Note":null,"ActionBy":null,"ActionTime":"2023-03-15T23:32:06.6884234+07:00","LangCode":"vn","NameLangCode":null,"NgonNgu":null},{"ID":"487d5b43-bc7f-4034-a767-723f12e2934e","Field":"timeoutlogin","Value":"","Note":null,"ActionBy":null,"ActionTime":"2023-03-15T23:32:31.2967881+07:00","LangCode":"vn","NameLangCode":null,"NgonNgu":null},{"ID":"5c9193aa-c691-413b-b515-54067e1cb23f","Field":"DonViCha","Value":"","Note":null,"ActionBy":null,"ActionTime":"2023-03-15T23:34:12.9601701+07:00","LangCode":"vn","NameLangCode":null,"NgonNgu":null},{"ID":"df644952-2ede-4e88-9789-790108e05f03","Field":"TenDonVi","Value":"","Note":null,"ActionBy":null,"ActionTime":"2023-03-15T23:34:12.9638741+07:00","LangCode":"vn","NameLangCode":null,"NgonNgu":null},{"ID":"b25a2af9-d97b-48f1-a24f-d7ef7d246639","Field":"DuongDan","Value":"","Note":null,"ActionBy":null,"ActionTime":"2023-03-15T23:34:12.966975+07:00","LangCode":"vn","NameLangCode":null,"NgonNgu":null},{"ID":"ca893f59-a40b-427d-9fe5-75ad19009bcb","Field":"NoiDung","Value":"","Note":null,"ActionBy":null,"ActionTime":"2023-03-15T23:34:12.9689465+07:00","LangCode":"vn","NameLangCode":null,"NgonNgu":null},{"ID":"212587c8-2467-4354-b424-a265183ad1db","Field":"ThuTu","Value":"","Note":null,"ActionBy":null,"ActionTime":"2023-03-15T23:34:12.9736354+07:00","LangCode":"vn","NameLangCode":null,"NgonNgu":null},{"ID":"642a6c1a-2916-4215-ad97-699b6ff02ba8","Field":"KichHoat","Value":"","Note":null,"ActionBy":null,"ActionTime":"2023-03-15T23:34:12.9785053+07:00","LangCode":"vn","NameLangCode":null,"NgonNgu":null},{"ID":"3a79bfb7-6480-42ff-8425-5405d87f7e58","Field":"ThemMoi","Value":"","Note":null,"ActionBy":null,"ActionTime":"2023-03-15T23:34:12.9793297+07:00","LangCode":"vn","NameLangCode":null,"NgonNgu":null}]')
SET IDENTITY_INSERT [dbo].[tbl_ConfigText] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_Logs] ON 

INSERT [dbo].[tbl_Logs] ([ID], [CreatedDate], [ContentJson], [LangCode]) VALUES (1, CAST(N'2023-03-15T00:00:00.000' AS DateTime), N'[{"ID":"dd448345-624d-4a58-aece-bccde92724b1","ParentID":0,"Contents":"Đăng Nhập","ActionBy":"admin","ActionTime":"2023-03-15T23:32:05.6783771+07:00","isLogin":false,"BrowserType":"Chrome","DeviceType":"Desktop"},{"ID":"c84b159e-8c83-4d4c-9607-84c5207031b3","ParentID":0,"Contents":"Đăng Xuất","ActionBy":"admin","ActionTime":"2023-03-15T23:58:43.9837008+07:00","isLogin":false,"BrowserType":null,"DeviceType":null}]', NULL)
SET IDENTITY_INSERT [dbo].[tbl_Logs] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_UserAdmin] ON 

INSERT [dbo].[tbl_UserAdmin] ([ID], [UserName], [Email], [FullName], [Gender], [Photo], [Address], [City], [District], [Country], [zip], [Active], [CreatedDate], [Password], [PasswordQuestion], [PasswordAnswer], [Phone], [TimeLogin], [IPLogin], [GroupUserID], [PageElementID], [QuyTrinhXuatBanID], [isAdmin], [SoTaiKhoan], [IsDuyetComment], [LastOnline], [isOnline], [NganHang], [LangCode], [IsCTV]) VALUES (1, N'admin', N'admin', N'admin', 0, N'/Upload/Images/Normal/2023/3/ab1a0974eb6f20933c3c32ee96ae146b-MicrosoftTeamsimage-4.png', N'1', N'', N'', N'', 0, 1, CAST(N'2023-03-15T23:30:37.897' AS DateTime), N'21232f297a57a5a743894a0e4a801fc3', N'', N'', N'1', CAST(N'2023-03-15T23:32:05.640' AS DateTime), N'14.170.159.147, 172.69.22.110', N'', N'', N'', 1, N'11', 1, CAST(N'2023-03-15T23:58:05.867' AS DateTime), 0, N'1', N'', 1)
SET IDENTITY_INSERT [dbo].[tbl_UserAdmin] OFF
GO
/****** Object:  Index [IX_FK_tbl_LichCongTac_tbl_UserAdmin1]    Script Date: 3/17/2023 6:35:28 AM ******/
CREATE NONCLUSTERED INDEX [IX_FK_tbl_LichCongTac_tbl_UserAdmin1] ON [dbo].[tbl_LichCongTac]
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_tbl_NewsPaperComment_tbl_NewsPaper]    Script Date: 3/17/2023 6:35:28 AM ******/
CREATE NONCLUSTERED INDEX [IX_FK_tbl_NewsPaperComment_tbl_NewsPaper] ON [dbo].[tbl_NewsPaperComment]
(
	[NewsPaperID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_LichCongTac]  WITH CHECK ADD  CONSTRAINT [FK_tbl_LichCongTac_tbl_UserAdmin1] FOREIGN KEY([UserID])
REFERENCES [dbo].[tbl_UserAdmin] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbl_LichCongTac] CHECK CONSTRAINT [FK_tbl_LichCongTac_tbl_UserAdmin1]
GO
ALTER TABLE [dbo].[tbl_NewsPaperComment]  WITH CHECK ADD  CONSTRAINT [FK_tbl_NewsPaperComment_tbl_NewsPaper] FOREIGN KEY([NewsPaperID])
REFERENCES [dbo].[tbl_NewsPaper] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbl_NewsPaperComment] CHECK CONSTRAINT [FK_tbl_NewsPaperComment_tbl_NewsPaper]
GO
USE [master]
GO
ALTER DATABASE [ash] SET  READ_WRITE 
GO
