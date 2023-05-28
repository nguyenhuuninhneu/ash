
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/15/2023 22:58:09
-- Generated from EDMX file: D:\MyWorkspace\Job\VietESoft\AMBN_new\Web.Model\Model1.edmx
-- --------------------------------------------------
--Use master
--GO
--Create DAtabase [AMBN]

--GO
SET QUOTED_IDENTIFIER OFF;
GO
USE [AMBN];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_tbl_LichCongTac_tbl_UserAdmin1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_LichCongTac] DROP CONSTRAINT [FK_tbl_LichCongTac_tbl_UserAdmin1];
GO
IF OBJECT_ID(N'[dbo].[FK_tbl_NewsPaperComment_tbl_NewsPaper]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_NewsPaperComment] DROP CONSTRAINT [FK_tbl_NewsPaperComment_tbl_NewsPaper];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[advert_category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[advert_category];
GO
IF OBJECT_ID(N'[dbo].[advert_category_module]', 'U') IS NOT NULL
    DROP TABLE [dbo].[advert_category_module];
GO
IF OBJECT_ID(N'[dbo].[contact_category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[contact_category];
GO
IF OBJECT_ID(N'[dbo].[DanhMucCon]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DanhMucCon];
GO
IF OBJECT_ID(N'[dbo].[DanhMucCha]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DanhMucCha];
GO
IF OBJECT_ID(N'[dbo].[document_category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[document_category];
GO
IF OBJECT_ID(N'[dbo].[document_price]', 'U') IS NOT NULL
    DROP TABLE [dbo].[document_price];
GO
IF OBJECT_ID(N'[dbo].[document_publisher]', 'U') IS NOT NULL
    DROP TABLE [dbo].[document_publisher];
GO
IF OBJECT_ID(N'[dbo].[faq_category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[faq_category];
GO
IF OBJECT_ID(N'[dbo].[GroupCusUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GroupCusUser];
GO
IF OBJECT_ID(N'[dbo].[guide_category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[guide_category];
GO
IF OBJECT_ID(N'[dbo].[module]', 'U') IS NOT NULL
    DROP TABLE [dbo].[module];
GO
IF OBJECT_ID(N'[dbo].[news_category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[news_category];
GO
IF OBJECT_ID(N'[dbo].[qProcedure]', 'U') IS NOT NULL
    DROP TABLE [dbo].[qProcedure];
GO
IF OBJECT_ID(N'[dbo].[qProcess]', 'U') IS NOT NULL
    DROP TABLE [dbo].[qProcess];
GO
IF OBJECT_ID(N'[dbo].[quydinhpl_category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[quydinhpl_category];
GO
IF OBJECT_ID(N'[dbo].[recruit_category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[recruit_category];
GO
IF OBJECT_ID(N'[dbo].[support_category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[support_category];
GO
IF OBJECT_ID(N'[dbo].[support_category_module]', 'U') IS NOT NULL
    DROP TABLE [dbo].[support_category_module];
GO
IF OBJECT_ID(N'[dbo].[survey_category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[survey_category];
GO
IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO
IF OBJECT_ID(N'[dbo].[tbl_AccessOnline]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_AccessOnline];
GO
IF OBJECT_ID(N'[dbo].[tbl_AccessView]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_AccessView];
GO
IF OBJECT_ID(N'[dbo].[tbl_AdminMenu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_AdminMenu];
GO
IF OBJECT_ID(N'[dbo].[tbl_Advert]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_Advert];
GO
IF OBJECT_ID(N'[dbo].[tbl_ApproveNews]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_ApproveNews];
GO
IF OBJECT_ID(N'[dbo].[tbl_BoxNewsConfig]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_BoxNewsConfig];
GO
IF OBJECT_ID(N'[dbo].[tbl_Calendar]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_Calendar];
GO
IF OBJECT_ID(N'[dbo].[tbl_Category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_Category];
GO
IF OBJECT_ID(N'[dbo].[tbl_Config]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_Config];
GO
IF OBJECT_ID(N'[dbo].[tbl_configLuuY]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_configLuuY];
GO
IF OBJECT_ID(N'[dbo].[tbl_ConfigText]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_ConfigText];
GO
IF OBJECT_ID(N'[dbo].[tbl_Contact]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_Contact];
GO
IF OBJECT_ID(N'[dbo].[tbl_ContactInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_ContactInfo];
GO
IF OBJECT_ID(N'[dbo].[tbl_CoQuanBanHanh]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_CoQuanBanHanh];
GO
IF OBJECT_ID(N'[dbo].[tbl_CountAccess]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_CountAccess];
GO
IF OBJECT_ID(N'[dbo].[tbl_Chat]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_Chat];
GO
IF OBJECT_ID(N'[dbo].[tbl_ChucVu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_ChucVu];
GO
IF OBJECT_ID(N'[dbo].[tbl_DanhMucTaiLieu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_DanhMucTaiLieu];
GO
IF OBJECT_ID(N'[dbo].[tbl_DMChung]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_DMChung];
GO
IF OBJECT_ID(N'[dbo].[tbl_DMNhom]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_DMNhom];
GO
IF OBJECT_ID(N'[dbo].[tbl_Document_Type]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_Document_Type];
GO
IF OBJECT_ID(N'[dbo].[tbl_DonVi]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_DonVi];
GO
IF OBJECT_ID(N'[dbo].[tbl_DonViTTHC]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_DonViTTHC];
GO
IF OBJECT_ID(N'[dbo].[tbl_eMagazine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_eMagazine];
GO
IF OBJECT_ID(N'[dbo].[tbl_FeedBack]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_FeedBack];
GO
IF OBJECT_ID(N'[dbo].[tbl_fieldfiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_fieldfiles];
GO
IF OBJECT_ID(N'[dbo].[tbl_Footer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_Footer];
GO
IF OBJECT_ID(N'[dbo].[tbl_GroupUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_GroupUser];
GO
IF OBJECT_ID(N'[dbo].[tbl_HistoryNews]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_HistoryNews];
GO
IF OBJECT_ID(N'[dbo].[tbl_HomeMenu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_HomeMenu];
GO
IF OBJECT_ID(N'[dbo].[tbl_HomeMenu_OLD]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_HomeMenu_OLD];
GO
IF OBJECT_ID(N'[dbo].[tbl_Images]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_Images];
GO
IF OBJECT_ID(N'[dbo].[tbl_ImagesCategory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_ImagesCategory];
GO
IF OBJECT_ID(N'[dbo].[tbl_Languages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_Languages];
GO
IF OBJECT_ID(N'[dbo].[tbl_LichCongTac]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_LichCongTac];
GO
IF OBJECT_ID(N'[dbo].[tbl_Limit]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_Limit];
GO
IF OBJECT_ID(N'[dbo].[tbl_LinkWebsite]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_LinkWebsite];
GO
IF OBJECT_ID(N'[dbo].[tbl_LinhVucTTHC]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_LinhVucTTHC];
GO
IF OBJECT_ID(N'[dbo].[tbl_LinhVucVanBan]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_LinhVucVanBan];
GO
IF OBJECT_ID(N'[dbo].[tbl_LoaiVanBan]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_LoaiVanBan];
GO
IF OBJECT_ID(N'[dbo].[tbl_Logs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_Logs];
GO
IF OBJECT_ID(N'[dbo].[tbl_Magazine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_Magazine];
GO
IF OBJECT_ID(N'[dbo].[tbl_ModuleCate]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_ModuleCate];
GO
IF OBJECT_ID(N'[dbo].[tbl_News]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_News];
GO
IF OBJECT_ID(N'[dbo].[tbl_NewsComment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_NewsComment];
GO
IF OBJECT_ID(N'[dbo].[tbl_NewsPaper]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_NewsPaper];
GO
IF OBJECT_ID(N'[dbo].[tbl_NewsPaperComment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_NewsPaperComment];
GO
IF OBJECT_ID(N'[dbo].[tbl_Online]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_Online];
GO
IF OBJECT_ID(N'[dbo].[tbl_PageElement]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_PageElement];
GO
IF OBJECT_ID(N'[dbo].[tbl_Position]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_Position];
GO
IF OBJECT_ID(N'[dbo].[tbl_Product]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_Product];
GO
IF OBJECT_ID(N'[dbo].[tbl_ProductCategory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_ProductCategory];
GO
IF OBJECT_ID(N'[dbo].[tbl_ProductOverView]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_ProductOverView];
GO
IF OBJECT_ID(N'[dbo].[tbl_QALinhVuc]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_QALinhVuc];
GO
IF OBJECT_ID(N'[dbo].[tbl_QuestionAndAnswer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_QuestionAndAnswer];
GO
IF OBJECT_ID(N'[dbo].[tbl_SlideImages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_SlideImages];
GO
IF OBJECT_ID(N'[dbo].[tbl_SubeMagazine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_SubeMagazine];
GO
IF OBJECT_ID(N'[dbo].[tbl_TaiLieu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_TaiLieu];
GO
IF OBJECT_ID(N'[dbo].[tbl_ToGiac]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_ToGiac];
GO
IF OBJECT_ID(N'[dbo].[tbl_Topic]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_Topic];
GO
IF OBJECT_ID(N'[dbo].[tbl_TopicComment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_TopicComment];
GO
IF OBJECT_ID(N'[dbo].[tbl_TTHC]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_TTHC];
GO
IF OBJECT_ID(N'[dbo].[tbl_TTHCComment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_TTHCComment];
GO
IF OBJECT_ID(N'[dbo].[tbl_ThamDoYKien]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_ThamDoYKien];
GO
IF OBJECT_ID(N'[dbo].[tbl_ThamDoYKien_Ip]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_ThamDoYKien_Ip];
GO
IF OBJECT_ID(N'[dbo].[tbl_truyna]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_truyna];
GO
IF OBJECT_ID(N'[dbo].[tbl_User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_User];
GO
IF OBJECT_ID(N'[dbo].[tbl_UserAdmin]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_UserAdmin];
GO
IF OBJECT_ID(N'[dbo].[tbl_VanBan]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_VanBan];
GO
IF OBJECT_ID(N'[dbo].[tbl_Video]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_Video];
GO
IF OBJECT_ID(N'[dbo].[tbl_VideoCategory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_VideoCategory];
GO
IF OBJECT_ID(N'[dbo].[test_category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[test_category];
GO
IF OBJECT_ID(N'[tuyengiaovnModelStoreContainer].[view_Category_Languages]', 'U') IS NOT NULL
    DROP TABLE [tuyengiaovnModelStoreContainer].[view_Category_Languages];
GO
IF OBJECT_ID(N'[tuyengiaovnModelStoreContainer].[View_DataHome]', 'U') IS NOT NULL
    DROP TABLE [tuyengiaovnModelStoreContainer].[View_DataHome];
GO
IF OBJECT_ID(N'[tuyengiaovnModelStoreContainer].[View_Gallery_Category]', 'U') IS NOT NULL
    DROP TABLE [tuyengiaovnModelStoreContainer].[View_Gallery_Category];
GO
IF OBJECT_ID(N'[tuyengiaovnModelStoreContainer].[view_Images_ImageCategory]', 'U') IS NOT NULL
    DROP TABLE [tuyengiaovnModelStoreContainer].[view_Images_ImageCategory];
GO
IF OBJECT_ID(N'[tuyengiaovnModelStoreContainer].[view_News]', 'U') IS NOT NULL
    DROP TABLE [tuyengiaovnModelStoreContainer].[view_News];
GO
IF OBJECT_ID(N'[tuyengiaovnModelStoreContainer].[view_News_HomeCategory]', 'U') IS NOT NULL
    DROP TABLE [tuyengiaovnModelStoreContainer].[view_News_HomeCategory];
GO
IF OBJECT_ID(N'[tuyengiaovnModelStoreContainer].[view_News_hotAndUnderhot]', 'U') IS NOT NULL
    DROP TABLE [tuyengiaovnModelStoreContainer].[view_News_hotAndUnderhot];
GO
IF OBJECT_ID(N'[tuyengiaovnModelStoreContainer].[view_News_Limit]', 'U') IS NOT NULL
    DROP TABLE [tuyengiaovnModelStoreContainer].[view_News_Limit];
GO
IF OBJECT_ID(N'[tuyengiaovnModelStoreContainer].[view_NEWS_LISTDATA]', 'U') IS NOT NULL
    DROP TABLE [tuyengiaovnModelStoreContainer].[view_NEWS_LISTDATA];
GO
IF OBJECT_ID(N'[tuyengiaovnModelStoreContainer].[view_NEWS_LISTDETAIL]', 'U') IS NOT NULL
    DROP TABLE [tuyengiaovnModelStoreContainer].[view_NEWS_LISTDETAIL];
GO
IF OBJECT_ID(N'[tuyengiaovnModelStoreContainer].[view_News_MultiCate]', 'U') IS NOT NULL
    DROP TABLE [tuyengiaovnModelStoreContainer].[view_News_MultiCate];
GO
IF OBJECT_ID(N'[tuyengiaovnModelStoreContainer].[view_News_noPublic]', 'U') IS NOT NULL
    DROP TABLE [tuyengiaovnModelStoreContainer].[view_News_noPublic];
GO
IF OBJECT_ID(N'[tuyengiaovnModelStoreContainer].[view_News_Public]', 'U') IS NOT NULL
    DROP TABLE [tuyengiaovnModelStoreContainer].[view_News_Public];
GO
IF OBJECT_ID(N'[tuyengiaovnModelStoreContainer].[view_news_search]', 'U') IS NOT NULL
    DROP TABLE [tuyengiaovnModelStoreContainer].[view_news_search];
GO
IF OBJECT_ID(N'[tuyengiaovnModelStoreContainer].[view_News_Sukien]', 'U') IS NOT NULL
    DROP TABLE [tuyengiaovnModelStoreContainer].[view_News_Sukien];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'advert_category'
CREATE TABLE [dbo].[advert_category] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [LangCode] varchar(50)  NULL,
    [Name] nvarchar(max)  NULL,
    [ParentID] int  NULL,
    [GhiChu] nvarchar(max)  NULL,
    [Ordering] int  NULL,
    [Status] bit  NOT NULL,
    [CreatedDate] datetime  NULL,
    [CreatedBy] int  NULL,
    [ListCheck] varchar(500)  NULL
);
GO

-- Creating table 'advert_category_module'
CREATE TABLE [dbo].[advert_category_module] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [AdvertID] int  NOT NULL,
    [ModuleID] int  NOT NULL
);
GO

-- Creating table 'contact_category'
CREATE TABLE [dbo].[contact_category] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ParentID] int  NULL,
    [Name] nvarchar(max)  NULL,
    [Photo] nvarchar(max)  NULL,
    [LangCode] varchar(50)  NULL,
    [Title] nvarchar(max)  NULL,
    [Keyword] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Ordering] int  NULL,
    [Status] bit  NOT NULL,
    [CreatedDate] datetime  NULL,
    [CreatedBy] int  NULL,
    [support_1] nvarchar(max)  NULL,
    [support_2] nvarchar(max)  NULL,
    [advert_1] nvarchar(max)  NULL,
    [advert_2] nvarchar(max)  NULL,
    [display_1] nvarchar(max)  NULL,
    [display_2] nvarchar(max)  NULL,
    [view_more_detail] nvarchar(max)  NULL
);
GO

-- Creating table 'DanhMucCons'
CREATE TABLE [dbo].[DanhMucCons] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NULL,
    [ParentID] int  NULL
);
GO

-- Creating table 'DanhMucChas'
CREATE TABLE [dbo].[DanhMucChas] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(500)  NULL
);
GO

-- Creating table 'document_category'
CREATE TABLE [dbo].[document_category] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ParentID] int  NULL,
    [Name] nvarchar(max)  NULL,
    [Photo] nvarchar(max)  NULL,
    [LangCode] varchar(50)  NULL,
    [Title] nvarchar(max)  NULL,
    [Keyword] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Ordering] int  NULL,
    [Status] bit  NOT NULL,
    [CreatedDate] datetime  NULL,
    [CreatedBy] int  NULL,
    [support_1] nvarchar(max)  NULL,
    [support_2] nvarchar(max)  NULL,
    [advert_1] nvarchar(max)  NULL,
    [advert_2] nvarchar(max)  NULL,
    [display_1] nvarchar(max)  NULL,
    [display_2] nvarchar(max)  NULL,
    [view_more_detail] nvarchar(max)  NULL
);
GO

-- Creating table 'document_price'
CREATE TABLE [dbo].[document_price] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(500)  NULL,
    [LangCode] varchar(50)  NULL,
    [Price_min] int  NULL,
    [Price_max] int  NULL,
    [Ordering] int  NULL,
    [Status] bit  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] int  NOT NULL
);
GO

-- Creating table 'document_publisher'
CREATE TABLE [dbo].[document_publisher] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ParentID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Photo] nvarchar(max)  NULL,
    [LangCode] varchar(50)  NULL,
    [KyHieu] nvarchar(max)  NULL,
    [MaSo] nvarchar(max)  NULL,
    [TKNganHang] bigint  NULL,
    [TenNganHang] nvarchar(max)  NULL,
    [Ordering] int  NULL,
    [Status] bit  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] int  NOT NULL
);
GO

-- Creating table 'faq_category'
CREATE TABLE [dbo].[faq_category] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ParentID] int  NULL,
    [Name] nvarchar(max)  NULL,
    [Photo] nvarchar(max)  NULL,
    [LangCode] varchar(50)  NULL,
    [Title] nvarchar(max)  NULL,
    [Keyword] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Ordering] int  NULL,
    [Status] bit  NOT NULL,
    [CreatedDate] datetime  NULL,
    [CreatedBy] int  NULL,
    [support_1] nvarchar(max)  NULL,
    [support_2] nvarchar(max)  NULL,
    [advert_1] nvarchar(max)  NULL,
    [advert_2] nvarchar(max)  NULL,
    [display_1] nvarchar(max)  NULL,
    [display_2] nvarchar(max)  NULL,
    [view_more_detail] nvarchar(max)  NULL
);
GO

-- Creating table 'GroupCusUsers'
CREATE TABLE [dbo].[GroupCusUsers] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [NameGroup] nvarchar(250)  NOT NULL,
    [Ordering] int  NULL,
    [Status] bit  NOT NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'guide_category'
CREATE TABLE [dbo].[guide_category] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ParentID] int  NULL,
    [Name] nvarchar(max)  NULL,
    [Photo] nvarchar(max)  NULL,
    [LangCode] varchar(50)  NULL,
    [Title] nvarchar(max)  NULL,
    [Keyword] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Ordering] int  NULL,
    [Status] bit  NOT NULL,
    [CreatedDate] datetime  NULL,
    [CreatedBy] int  NULL,
    [support_1] nvarchar(max)  NULL,
    [support_2] nvarchar(max)  NULL,
    [advert_1] nvarchar(max)  NULL,
    [advert_2] nvarchar(max)  NULL,
    [display_1] nvarchar(max)  NULL,
    [display_2] nvarchar(max)  NULL,
    [view_more_detail] nvarchar(max)  NULL
);
GO

-- Creating table 'modules'
CREATE TABLE [dbo].[modules] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(500)  NULL,
    [Name] nvarchar(500)  NULL,
    [Ordering] int  NULL,
    [Status] bit  NOT NULL,
    [CreatedDate] datetime  NULL,
    [CreatedBy] int  NULL
);
GO

-- Creating table 'news_category'
CREATE TABLE [dbo].[news_category] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ParentID] int  NULL,
    [Name] nvarchar(max)  NULL,
    [Photo] nvarchar(max)  NULL,
    [LangCode] varchar(50)  NULL,
    [Title] nvarchar(max)  NULL,
    [Keyword] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Ordering] int  NULL,
    [Status] bit  NOT NULL,
    [CreatedDate] datetime  NULL,
    [CreatedBy] int  NULL,
    [support_1] nvarchar(max)  NULL,
    [support_2] nvarchar(max)  NULL,
    [advert_1] nvarchar(max)  NULL,
    [advert_2] nvarchar(max)  NULL,
    [display_1] nvarchar(max)  NULL,
    [display_2] nvarchar(max)  NULL,
    [view_more_detail] nvarchar(max)  NULL
);
GO

-- Creating table 'qProcedures'
CREATE TABLE [dbo].[qProcedures] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Code] nvarchar(max)  NULL,
    [Ordering] int  NULL,
    [isPublish] bit  NOT NULL,
    [LangCode] varchar(50)  NULL,
    [NameEnglish] nvarchar(max)  NULL
);
GO

-- Creating table 'qProcesses'
CREATE TABLE [dbo].[qProcesses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TableCode] nvarchar(100)  NOT NULL,
    [FKId] int  NOT NULL,
    [FromProcedure] int  NOT NULL,
    [FromId] int  NOT NULL,
    [ToId] int  NOT NULL,
    [ToProcedure] int  NOT NULL,
    [Contents] nvarchar(max)  NULL,
    [CreateDate] datetime  NULL,
    [Attachment] nvarchar(max)  NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'quydinhpl_category'
CREATE TABLE [dbo].[quydinhpl_category] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ParentID] int  NOT NULL,
    [Name] nvarchar(255)  NULL,
    [Ordering] int  NOT NULL,
    [Status] smallint  NOT NULL,
    [LangCode] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'support_category'
CREATE TABLE [dbo].[support_category] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [LangCode] varchar(50)  NULL,
    [Name] nvarchar(max)  NULL,
    [ParentID] int  NULL,
    [GhiChu] nvarchar(max)  NULL,
    [Ordering] int  NULL,
    [Status] bit  NOT NULL,
    [CreatedDate] datetime  NULL,
    [CreatedBy] int  NULL,
    [ListCheck] varchar(500)  NULL
);
GO

-- Creating table 'support_category_module'
CREATE TABLE [dbo].[support_category_module] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [SupportID] int  NOT NULL,
    [ModuleID] int  NOT NULL
);
GO

-- Creating table 'survey_category'
CREATE TABLE [dbo].[survey_category] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ParentID] int  NULL,
    [Name] nvarchar(max)  NULL,
    [Photo] nvarchar(max)  NULL,
    [LangCode] varchar(50)  NULL,
    [Title] nvarchar(max)  NULL,
    [Keyword] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Ordering] int  NULL,
    [Status] bit  NOT NULL,
    [CreatedDate] datetime  NULL,
    [CreatedBy] int  NULL,
    [support_1] nvarchar(max)  NULL,
    [support_2] nvarchar(max)  NULL,
    [advert_1] nvarchar(max)  NULL,
    [advert_2] nvarchar(max)  NULL,
    [display_1] nvarchar(max)  NULL,
    [display_2] nvarchar(max)  NULL,
    [view_more_detail] nvarchar(max)  NULL
);
GO

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- Creating table 'tbl_AccessOnline'
CREATE TABLE [dbo].[tbl_AccessOnline] (
    [Session] nvarchar(250)  NOT NULL,
    [Time] datetime  NOT NULL
);
GO

-- Creating table 'tbl_AccessView'
CREATE TABLE [dbo].[tbl_AccessView] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [TongCong] int  NULL,
    [NgayThongKe] datetime  NOT NULL,
    [HomNay] int  NULL,
    [HomTruoc] int  NULL,
    [TuanNay] int  NULL,
    [TuanTruoc] int  NULL,
    [ThangNay] int  NULL,
    [ThangTruoc] int  NULL
);
GO

-- Creating table 'tbl_AdminMenu'
CREATE TABLE [dbo].[tbl_AdminMenu] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [ParentID] int  NOT NULL,
    [Url] varchar(250)  NOT NULL,
    [Ordering] int  NOT NULL,
    [Active] bit  NOT NULL,
    [Icon] varchar(50)  NULL,
    [CreatedDate] datetime  NULL,
    [LangCode] varchar(5)  NULL
);
GO

-- Creating table 'tbl_Advert'
CREATE TABLE [dbo].[tbl_Advert] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [Image] nvarchar(250)  NOT NULL,
    [Url] varchar(250)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] int  NULL,
    [ModifiedDate] datetime  NULL,
    [ModifiedBy] int  NULL,
    [Status] bit  NOT NULL,
    [Position] varchar(max)  NOT NULL,
    [Ordering] int  NOT NULL,
    [isNewTab] bit  NOT NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_ApproveNews'
CREATE TABLE [dbo].[tbl_ApproveNews] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NULL,
    [Ordering] int  NULL
);
GO

-- Creating table 'tbl_BoxNewsConfig'
CREATE TABLE [dbo].[tbl_BoxNewsConfig] (
    [Code] varchar(10)  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [PageElementId] int  NOT NULL,
    [LangCode] varchar(5)  NOT NULL
);
GO

-- Creating table 'tbl_Calendar'
CREATE TABLE [dbo].[tbl_Calendar] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(500)  NOT NULL,
    [Status] bit  NOT NULL,
    [LangCode] varchar(5)  NOT NULL,
    [LangName] nvarchar(50)  NULL,
    [PageElementId] int  NOT NULL,
    [Logo] nvarchar(250)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [Details] nvarchar(max)  NULL
);
GO

-- Creating table 'tbl_Category'
CREATE TABLE [dbo].[tbl_Category] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ParentID] int  NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Position] varchar(500)  NULL,
    [Icon] nvarchar(max)  NULL,
    [Ordering] int  NOT NULL,
    [Active] bit  NOT NULL,
    [CreatedDate] datetime  NULL,
    [LangCode] varchar(5)  NULL,
    [Url] nvarchar(500)  NULL,
    [isMenu] bit  NULL,
    [isNewTab] bit  NOT NULL
);
GO

-- Creating table 'tbl_Config'
CREATE TABLE [dbo].[tbl_Config] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(500)  NOT NULL,
    [ParentID] int  NOT NULL,
    [Code] nvarchar(500)  NULL,
    [Description] nvarchar(500)  NULL,
    [Price] int  NULL,
    [Status] bit  NOT NULL,
    [Ordering] int  NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_configLuuY'
CREATE TABLE [dbo].[tbl_configLuuY] (
    [LuuYHoiDap] nvarchar(max)  NULL,
    [LuuYToGiac] nvarchar(max)  NULL,
    [EmailNhanYKien] nvarchar(max)  NULL,
    [ID] int IDENTITY(1,1) NOT NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_ConfigText'
CREATE TABLE [dbo].[tbl_ConfigText] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [LastModifieDate] datetime  NULL,
    [LastModifieBy] int  NULL,
    [LangCode] nchar(20)  NOT NULL,
    [ContentJson] nvarchar(max)  NULL
);
GO

-- Creating table 'tbl_Contact'
CREATE TABLE [dbo].[tbl_Contact] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [FullNameOfQuestion] nvarchar(50)  NOT NULL,
    [Email] varchar(50)  NOT NULL,
    [Question] nvarchar(max)  NOT NULL,
    [Answer] nvarchar(max)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [AnswerBy] int  NULL,
    [AnswerDate] datetime  NULL,
    [UserNameOfAnswer] nvarchar(50)  NULL,
    [Status] bit  NOT NULL,
    [Phone] varchar(150)  NULL,
    [DiaChi] nvarchar(max)  NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_ContactInfo'
CREATE TABLE [dbo].[tbl_ContactInfo] (
    [NoiDung] nvarchar(max)  NULL,
    [ID] int IDENTITY(1,1) NOT NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_CoQuanBanHanh'
CREATE TABLE [dbo].[tbl_CoQuanBanHanh] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ParentID] int  NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [Description] nvarchar(250)  NULL,
    [LangCode] varchar(5)  NOT NULL,
    [Ordering] int  NOT NULL,
    [Status] bit  NOT NULL,
    [Level] int  NOT NULL
);
GO

-- Creating table 'tbl_CountAccess'
CREATE TABLE [dbo].[tbl_CountAccess] (
    [Count] int  NOT NULL,
    [ID] int  NOT NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_Chat'
CREATE TABLE [dbo].[tbl_Chat] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ListIdUser] nvarchar(max)  NULL,
    [Content] nvarchar(max)  NULL,
    [Date] datetime  NULL,
    [Count] int  NOT NULL,
    [IdLast] int  NULL,
    [LangCode] varchar(50)  NULL,
    [Ordering] int  NULL
);
GO

-- Creating table 'tbl_ChucVu'
CREATE TABLE [dbo].[tbl_ChucVu] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NULL,
    [Ordering] int  NULL,
    [isPublish] bit  NOT NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_DanhMucTaiLieu'
CREATE TABLE [dbo].[tbl_DanhMucTaiLieu] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ParentID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Ordering] int  NOT NULL,
    [Status] bit  NOT NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_DMChung'
CREATE TABLE [dbo].[tbl_DMChung] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ParentID] int  NOT NULL,
    [Code] nvarchar(250)  NULL,
    [Ten] nvarchar(500)  NOT NULL,
    [Ordering] int  NOT NULL,
    [Status] bit  NOT NULL,
    [Level] int  NOT NULL,
    [BCH] nvarchar(500)  NULL,
    [Phone] nvarchar(50)  NULL,
    [DiDong] nvarchar(50)  NULL,
    [Address] nvarchar(500)  NULL,
    [FeatureImage] nvarchar(250)  NULL,
    [Details] nvarchar(max)  NULL,
    [IsHome] bit  NOT NULL,
    [CatID] int  NOT NULL,
    [Url] nvarchar(250)  NULL,
    [IsBanChapHanh] bit  NOT NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_DMNhom'
CREATE TABLE [dbo].[tbl_DMNhom] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NULL,
    [Ordering] int  NOT NULL,
    [Status] bit  NOT NULL,
    [Code] nvarchar(50)  NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_Document_Type'
CREATE TABLE [dbo].[tbl_Document_Type] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Photo] nvarchar(max)  NULL,
    [Ordering] int  NULL,
    [Status] bit  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [LangCode] nchar(10)  NULL,
    [Code] nvarchar(max)  NULL,
    [ParentID] int  NOT NULL
);
GO

-- Creating table 'tbl_DonVi'
CREATE TABLE [dbo].[tbl_DonVi] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TenDonVi] nvarchar(250)  NOT NULL,
    [Ordering] int  NULL,
    [Status] bit  NOT NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_DonViTTHC'
CREATE TABLE [dbo].[tbl_DonViTTHC] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ParentID] int  NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [Description] nvarchar(250)  NULL,
    [Ordering] int  NOT NULL,
    [Active] bit  NOT NULL,
    [CreatedDate] datetime  NULL,
    [LangCode] varchar(5)  NULL
);
GO

-- Creating table 'tbl_eMagazine'
CREATE TABLE [dbo].[tbl_eMagazine] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [CreatedDate] datetime  NULL,
    [CreatedBy] int  NULL,
    [Photo] varchar(max)  NULL,
    [Author] nvarchar(max)  NULL,
    [CTVID] int  NULL,
    [NewMoney] int  NULL,
    [Width] int  NULL,
    [Background] nvarchar(max)  NULL,
    [Status] bit  NOT NULL,
    [Source] nvarchar(max)  NULL,
    [Info] nvarchar(max)  NULL,
    [Ordering] int  NOT NULL,
    [Color] varchar(max)  NULL
);
GO

-- Creating table 'tbl_FeedBack'
CREATE TABLE [dbo].[tbl_FeedBack] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [FullNameOfQuestion] nvarchar(50)  NOT NULL,
    [Email] varchar(50)  NOT NULL,
    [Question] nvarchar(max)  NOT NULL,
    [Answer] nvarchar(max)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [AnswerBy] int  NULL,
    [AnswerDate] datetime  NULL,
    [UserNameOfAnswer] nvarchar(50)  NULL,
    [Status] bit  NOT NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_fieldfiles'
CREATE TABLE [dbo].[tbl_fieldfiles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FieldID] int  NULL,
    [LinkFile] nvarchar(max)  NULL,
    [NameFile] nvarchar(500)  NULL,
    [CreateDate] datetime  NULL,
    [Status] int  NULL,
    [ReplaceName] nvarchar(500)  NULL,
    [Size] varchar(max)  NULL,
    [Code] nvarchar(250)  NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_Footer'
CREATE TABLE [dbo].[tbl_Footer] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [LangCode] varchar(5)  NULL,
    [Ordering] int  NULL,
    [Status] bit  NOT NULL,
    [LangName] nvarchar(50)  NULL,
    [FooterTextLeft] nvarchar(max)  NULL,
    [FooterTextRight] nvarchar(max)  NULL,
    [FooterTextLeftEnglish] nvarchar(max)  NULL,
    [FooterTextRightEnglish] nvarchar(max)  NULL
);
GO

-- Creating table 'tbl_GroupUser'
CREATE TABLE [dbo].[tbl_GroupUser] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(500)  NOT NULL,
    [ExpandNews] bit  NOT NULL,
    [Status] bit  NOT NULL,
    [Permission] varchar(max)  NULL,
    [PermissionCatNews] varchar(max)  NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_HistoryNews'
CREATE TABLE [dbo].[tbl_HistoryNews] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [NewsID] int  NOT NULL,
    [Contents] nvarchar(max)  NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_HomeMenu'
CREATE TABLE [dbo].[tbl_HomeMenu] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ParentID] int  NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [Url] varchar(500)  NULL,
    [Ordering] int  NULL,
    [isNewsTab] bit  NOT NULL,
    [Status] bit  NOT NULL,
    [CategoryId] int  NULL,
    [LinkType] int  NOT NULL,
    [PageElementId] int  NOT NULL,
    [IsHome] smallint  NOT NULL,
    [IsMenu] bit  NOT NULL,
    [IsPermAdd] bit  NOT NULL,
    [FeautureImage] nvarchar(max)  NULL,
    [IsHome2] bit  NOT NULL,
    [IsHome3] bit  NOT NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_HomeMenu_OLD'
CREATE TABLE [dbo].[tbl_HomeMenu_OLD] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ParentID] int  NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [Url] varchar(500)  NULL,
    [Ordering] int  NULL,
    [isNewsTab] bit  NULL,
    [Status] bit  NOT NULL,
    [CategoryId] int  NULL,
    [LinkType] int  NOT NULL,
    [PageElementId] int  NOT NULL,
    [isHome] smallint  NOT NULL,
    [isMenu] bit  NOT NULL,
    [IsPermAdd] bit  NOT NULL,
    [FeautureImage] nvarchar(max)  NULL,
    [IsHome2] bit  NOT NULL,
    [IsHome3] bit  NOT NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_Images'
CREATE TABLE [dbo].[tbl_Images] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NULL,
    [Images] nvarchar(max)  NOT NULL,
    [Description] nvarchar(500)  NULL,
    [Url] nvarchar(500)  NULL,
    [Status] bit  NOT NULL,
    [ImageCategoryId] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [LangCode] varchar(5)  NULL,
    [ModifiedDate] datetime  NULL,
    [CreatedBy] int  NULL,
    [ModifiedBy] int  NOT NULL,
    [Ordering] int  NOT NULL,
    [UserActId] int  NOT NULL,
    [ProcedureId] int  NOT NULL
);
GO

-- Creating table 'tbl_ImagesCategory'
CREATE TABLE [dbo].[tbl_ImagesCategory] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(500)  NULL,
    [Image] nvarchar(500)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [LangCode] nvarchar(10)  NULL,
    [Status] bit  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [Ordering] int  NOT NULL,
    [ViewNumber] bigint  NOT NULL,
    [IsHome] bit  NOT NULL,
    [NewMoney] int  NULL,
    [CreateBy] int  NULL
);
GO

-- Creating table 'tbl_Languages'
CREATE TABLE [dbo].[tbl_Languages] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [Code] varchar(5)  NOT NULL,
    [Icon] nvarchar(250)  NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_LichCongTac'
CREATE TABLE [dbo].[tbl_LichCongTac] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [LangCode] varchar(5)  NOT NULL,
    [UserID] int  NOT NULL,
    [ngaybatdau] datetime  NOT NULL,
    [giobatdau] nvarchar(50)  NULL,
    [ngayketthuc] datetime  NULL,
    [noidung] nvarchar(max)  NOT NULL,
    [allDay] tinyint  NOT NULL,
    [LVTCQ] tinyint  NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [ModifyDate] datetime  NULL,
    [Status] tinyint  NOT NULL
);
GO

-- Creating table 'tbl_Limit'
CREATE TABLE [dbo].[tbl_Limit] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [Code] varchar(250)  NOT NULL,
    [Value] int  NOT NULL,
    [ParentID] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] int  NULL,
    [ModifiedDate] datetime  NULL,
    [ModifiedBy] int  NULL,
    [Status] bit  NOT NULL,
    [Ordering] int  NOT NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_LinkWebsite'
CREATE TABLE [dbo].[tbl_LinkWebsite] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Url] nvarchar(255)  NOT NULL,
    [Status] bit  NOT NULL,
    [LangCode] varchar(5)  NULL,
    [LangName] nvarchar(50)  NULL,
    [Logo] nvarchar(250)  NULL,
    [Type] int  NOT NULL,
    [Ordering] int  NOT NULL
);
GO

-- Creating table 'tbl_LinhVucTTHC'
CREATE TABLE [dbo].[tbl_LinhVucTTHC] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [Description] nvarchar(250)  NULL,
    [LangCode] varchar(5)  NOT NULL
);
GO

-- Creating table 'tbl_LinhVucVanBan'
CREATE TABLE [dbo].[tbl_LinhVucVanBan] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [Description] nvarchar(250)  NULL,
    [LangCode] varchar(5)  NOT NULL,
    [Ordering] int  NOT NULL,
    [Status] bit  NOT NULL
);
GO

-- Creating table 'tbl_LoaiVanBan'
CREATE TABLE [dbo].[tbl_LoaiVanBan] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [Description] nvarchar(250)  NULL,
    [LangCode] varchar(5)  NOT NULL,
    [Ordering] int  NOT NULL,
    [Status] bit  NOT NULL
);
GO

-- Creating table 'tbl_Logs'
CREATE TABLE [dbo].[tbl_Logs] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [ContentJson] nvarchar(max)  NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_Magazine'
CREATE TABLE [dbo].[tbl_Magazine] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Year] int  NULL,
    [Title] nvarchar(max)  NULL,
    [Photo] nvarchar(max)  NULL,
    [Attachment] nvarchar(max)  NULL,
    [Ordering] int  NOT NULL,
    [CreatedDate] datetime  NULL,
    [CreatedBy] int  NULL,
    [Status] bit  NOT NULL,
    [AttachmentName] nvarchar(max)  NULL,
    [AttachmentSize] varchar(max)  NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_ModuleCate'
CREATE TABLE [dbo].[tbl_ModuleCate] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Code] varchar(max)  NULL,
    [ParentID] int  NOT NULL,
    [Status] bit  NULL,
    [CreateDate] datetime  NULL,
    [CreateBy] int  NULL,
    [LangCode] nchar(10)  NULL,
    [Level] int  NOT NULL,
    [Ordering] int  NULL
);
GO

-- Creating table 'tbl_News'
CREATE TABLE [dbo].[tbl_News] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [SubTitle] nvarchar(max)  NULL,
    [Photo] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Details] nvarchar(max)  NULL,
    [Status] int  NULL,
    [IsPublish] bit  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [ModifyDate] datetime  NULL,
    [CategoryIdStr] varchar(200)  NULL,
    [SourceNews] nvarchar(max)  NULL,
    [Author] nvarchar(max)  NULL,
    [LangCode] varchar(5)  NULL,
    [Tags] nvarchar(max)  NULL,
    [Type] int  NULL,
    [Position] varchar(250)  NULL,
    [CreatedBy] int  NULL,
    [ModifyBy] int  NULL,
    [ViewCount] bigint  NULL,
    [AllowComment] tinyint  NULL,
    [IsGetNews] tinyint  NULL,
    [IsComment] bit  NOT NULL,
    [NewMoney] int  NULL,
    [RelatedNews] nvarchar(max)  NULL,
    [Attachment] nvarchar(max)  NULL,
    [UserActId] int  NOT NULL,
    [ProcedureId] int  NOT NULL,
    [NhuanBut] bigint  NOT NULL,
    [IdUserChoose] int  NULL,
    [OldId] nvarchar(max)  NULL,
    [AuthorGuidID] nvarchar(max)  NULL,
    [IsDelete] smallint  NOT NULL,
    [ZoneID] nvarchar(max)  NULL,
    [IsTraLai] bit  NOT NULL,
    [CTVID] int  NULL,
    [TimeChoose] datetime  NULL,
    [IsXBDuyet] bit  NOT NULL
);
GO

-- Creating table 'tbl_NewsComment'
CREATE TABLE [dbo].[tbl_NewsComment] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [NewsID] int  NOT NULL,
    [CommentID] int  NULL,
    [FullName] nvarchar(500)  NOT NULL,
    [Email] nvarchar(500)  NULL,
    [NoiDung] nvarchar(max)  NOT NULL,
    [CreateDate] datetime  NULL,
    [Status] bit  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [IsNews] bit  NOT NULL,
    [IsView] bit  NOT NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_NewsPaper'
CREATE TABLE [dbo].[tbl_NewsPaper] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [NoiDung] nvarchar(max)  NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [Status] int  NOT NULL,
    [CreateBy] int  NOT NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_NewsPaperComment'
CREATE TABLE [dbo].[tbl_NewsPaperComment] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [NewsPaperID] int  NOT NULL,
    [FullName] nvarchar(500)  NOT NULL,
    [Email] nvarchar(500)  NULL,
    [NoiDung] nvarchar(max)  NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [Phone] int  NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_Online'
CREATE TABLE [dbo].[tbl_Online] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Ip] nvarchar(500)  NULL,
    [Session] nvarchar(500)  NULL,
    [CreateDate] datetime  NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_PageElement'
CREATE TABLE [dbo].[tbl_PageElement] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [Code] varchar(50)  NULL,
    [LangCode] varchar(5)  NOT NULL,
    [Banner] nvarchar(250)  NULL,
    [Ordering] int  NULL,
    [Description] nvarchar(500)  NULL,
    [Status] bit  NOT NULL,
    [LangName] nvarchar(50)  NULL,
    [Subdomain] nvarchar(50)  NULL,
    [Area] varchar(50)  NULL,
    [Footer] nvarchar(max)  NULL
);
GO

-- Creating table 'tbl_Position'
CREATE TABLE [dbo].[tbl_Position] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [Code] varchar(250)  NOT NULL,
    [ParentID] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] int  NULL,
    [ModifiedDate] datetime  NULL,
    [ModifiedBy] int  NULL,
    [Status] bit  NOT NULL,
    [Ordering] int  NOT NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_Product'
CREATE TABLE [dbo].[tbl_Product] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CategoryId] int  NOT NULL,
    [Images] varchar(200)  NULL,
    [Description] nvarchar(max)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [ViewCount] bigint  NOT NULL,
    [Status] bit  NOT NULL,
    [Content] nvarchar(max)  NULL,
    [Ordering] int  NOT NULL,
    [Name] nvarchar(200)  NOT NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_ProductCategory'
CREATE TABLE [dbo].[tbl_ProductCategory] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ParentID] int  NOT NULL,
    [Name] nvarchar(200)  NOT NULL,
    [Ordering] int  NOT NULL,
    [Status] bit  NOT NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_ProductOverView'
CREATE TABLE [dbo].[tbl_ProductOverView] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ProductId] int  NOT NULL,
    [Title] nvarchar(250)  NOT NULL,
    [Images] varchar(200)  NULL,
    [Description] nvarchar(max)  NULL,
    [Ordering] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [Status] bit  NOT NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_QALinhVuc'
CREATE TABLE [dbo].[tbl_QALinhVuc] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [Description] nvarchar(250)  NULL,
    [LangCode] varchar(5)  NULL,
    [Ordering] int  NULL,
    [isPublish] bit  NOT NULL
);
GO

-- Creating table 'tbl_QuestionAndAnswer'
CREATE TABLE [dbo].[tbl_QuestionAndAnswer] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [FullNameOfQuestion] nvarchar(50)  NOT NULL,
    [Email] varchar(50)  NULL,
    [Question] nvarchar(max)  NOT NULL,
    [Answer] nvarchar(max)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [AnswerBy] int  NULL,
    [AnswerDate] datetime  NULL,
    [UserNameOfAnswer] nvarchar(50)  NULL,
    [Status] bit  NOT NULL,
    [Phone] varchar(150)  NULL,
    [DiaChi] nvarchar(max)  NULL,
    [LinhVucID] int  NOT NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_SlideImages'
CREATE TABLE [dbo].[tbl_SlideImages] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(250)  NULL,
    [Images] nvarchar(max)  NOT NULL,
    [Status] bit  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [ModifiedDate] datetime  NULL,
    [CreatedBy] int  NULL,
    [ModifiedBy] int  NULL,
    [PageElementId] nvarchar(500)  NULL,
    [Url] nvarchar(250)  NULL,
    [Type] int  NOT NULL,
    [Ordering] int  NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_SubeMagazine'
CREATE TABLE [dbo].[tbl_SubeMagazine] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [IDParent] int  NULL,
    [Ordering] int  NOT NULL,
    [StrPhoto] nvarchar(max)  NULL,
    [NumberPhoto] int  NOT NULL,
    [isFullWidth] bit  NOT NULL,
    [WidthPhoto] int  NULL,
    [PositionPhoto] int  NULL,
    [Content] nvarchar(max)  NULL
);
GO

-- Creating table 'tbl_TaiLieu'
CREATE TABLE [dbo].[tbl_TaiLieu] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CatID] int  NOT NULL,
    [NameFile] nvarchar(max)  NULL,
    [LinkFile] nvarchar(max)  NULL,
    [ReplaceName] nvarchar(max)  NULL,
    [Ordering] int  NOT NULL,
    [CreateDate] datetime  NOT NULL,
    [Status] bit  NOT NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_ToGiac'
CREATE TABLE [dbo].[tbl_ToGiac] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ToiPhamID] int  NULL,
    [FullName] nvarchar(500)  NULL,
    [Email] nvarchar(500)  NULL,
    [NoiDung] nvarchar(max)  NOT NULL,
    [DiaChi] nvarchar(max)  NULL,
    [CreateDate] datetime  NULL,
    [Phone] int  NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_Topic'
CREATE TABLE [dbo].[tbl_Topic] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Photo] nvarchar(max)  NULL,
    [Details] nvarchar(max)  NULL,
    [Status] bit  NULL,
    [Comment] nvarchar(max)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [ModifyDate] datetime  NULL,
    [Author] nvarchar(150)  NULL,
    [LangCode] varchar(5)  NULL,
    [Tags] nvarchar(250)  NULL,
    [CreatedBy] int  NULL,
    [ModifyBy] int  NULL,
    [ViewCount] bigint  NULL,
    [AllowComment] tinyint  NULL,
    [Attachment] nvarchar(max)  NULL,
    [StartDate] datetime  NULL,
    [ListImgSlide] nvarchar(max)  NULL,
    [IdNguoiChon] nvarchar(max)  NULL
);
GO

-- Creating table 'tbl_TopicComment'
CREATE TABLE [dbo].[tbl_TopicComment] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [TopicID] int  NOT NULL,
    [CommentID] int  NULL,
    [FullName] nvarchar(500)  NOT NULL,
    [Email] nvarchar(500)  NULL,
    [NoiDung] nvarchar(max)  NOT NULL,
    [CreateDate] datetime  NULL,
    [Status] bit  NOT NULL,
    [CreatedBy] int  NULL,
    [IsView] bit  NOT NULL,
    [IsNew] bit  NOT NULL,
    [TraLoi] nvarchar(max)  NULL,
    [NguoiTraLoi] nvarchar(max)  NULL,
    [ThoiGianTraLoi] datetime  NULL,
    [IdNguoiChon] int  NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_TTHC'
CREATE TABLE [dbo].[tbl_TTHC] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [DonViId] int  NULL,
    [LinhVucId] int  NULL,
    [ThuTuc] nvarchar(500)  NULL,
    [LangCode] varchar(5)  NULL,
    [Attachment] nvarchar(max)  NULL,
    [Tieude] nvarchar(500)  NULL,
    [TrinhTuThucHien] nvarchar(max)  NULL,
    [CachThucThucHien] nvarchar(max)  NULL,
    [ThanhPhanHoSo] nvarchar(max)  NULL,
    [ThoiHan] nvarchar(max)  NULL,
    [DoiTuong] nvarchar(max)  NULL,
    [CoQuanThucHien] nvarchar(max)  NULL,
    [KetQua] nvarchar(max)  NULL,
    [LePhi] nvarchar(max)  NULL,
    [TenMauDon] nvarchar(max)  NULL,
    [YeuCauDieuKien] nvarchar(max)  NULL,
    [CoSoPhapLy] nvarchar(max)  NULL,
    [Status] int  NOT NULL,
    [ViewCount] bigint  NOT NULL,
    [AllowComment] tinyint  NULL
);
GO

-- Creating table 'tbl_TTHCComment'
CREATE TABLE [dbo].[tbl_TTHCComment] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [TTHCID] int  NOT NULL,
    [CommentID] int  NULL,
    [FullName] nvarchar(500)  NOT NULL,
    [Email] nvarchar(500)  NULL,
    [NoiDung] nvarchar(max)  NOT NULL,
    [CreateDate] datetime  NULL,
    [Status] bit  NOT NULL,
    [CreatedBy] int  NOT NULL,
    [IsView] bit  NOT NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_ThamDoYKien'
CREATE TABLE [dbo].[tbl_ThamDoYKien] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ParentID] int  NOT NULL,
    [Name] nvarchar(200)  NOT NULL,
    [Poll] int  NOT NULL,
    [Ordering] int  NOT NULL,
    [Status] bit  NOT NULL,
    [IpMay] varchar(max)  NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_ThamDoYKien_Ip'
CREATE TABLE [dbo].[tbl_ThamDoYKien_Ip] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [PollId] int  NOT NULL,
    [SessionID] varchar(100)  NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_truyna'
CREATE TABLE [dbo].[tbl_truyna] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(255)  NULL,
    [TenKhac] nvarchar(255)  NULL,
    [GioiTinh] smallint  NOT NULL,
    [NamSinh] nvarchar(50)  NULL,
    [NoiSinh] nvarchar(255)  NULL,
    [HoKhauTT] nvarchar(255)  NULL,
    [NoiDKTT] nvarchar(255)  NULL,
    [QuocTich] nvarchar(255)  NULL,
    [DanToc] nvarchar(255)  NULL,
    [TenCha] nvarchar(255)  NULL,
    [TenMe] nvarchar(255)  NULL,
    [ChieuCao] nvarchar(255)  NULL,
    [MauDa] nvarchar(255)  NULL,
    [MaiToc] nvarchar(255)  NULL,
    [LongMay] nvarchar(255)  NULL,
    [DacDiemMui] nvarchar(255)  NULL,
    [DacDiemMat] nvarchar(255)  NULL,
    [DacDiemTai] nvarchar(255)  NULL,
    [DacDiemKhac] nvarchar(255)  NULL,
    [ToiDanh] nvarchar(255)  NULL,
    [HeLoaiToiDanh] nvarchar(255)  NULL,
    [LoaiTruyNa] nvarchar(255)  NULL,
    [PhamViTruyNa] nvarchar(255)  NULL,
    [QuyetDinhTruyNa] nvarchar(255)  NULL,
    [DonViRaQD] nvarchar(255)  NULL,
    [NgayTruyNaQT] nvarchar(255)  NULL,
    [HoSoTruyNaQT] nvarchar(255)  NULL,
    [QuocGiaNghiTron] nvarchar(255)  NULL,
    [BaoChoDonVi] nvarchar(255)  NULL,
    [DienThoai] nvarchar(255)  NULL,
    [Photo] nvarchar(255)  NULL,
    [CreateDate] datetime  NOT NULL,
    [Status] int  NOT NULL,
    [IsHome] bit  NOT NULL,
    [IsDinhNa] bit  NOT NULL,
    [Attachment] nvarchar(max)  NULL,
    [AttachmentTN] nvarchar(max)  NULL,
    [SmallPhoto] nvarchar(max)  NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_User'
CREATE TABLE [dbo].[tbl_User] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [UserName] varchar(250)  NOT NULL,
    [Email] varchar(250)  NULL,
    [FullName] nvarchar(250)  NULL,
    [Gender] tinyint  NULL,
    [Photo] nvarchar(max)  NULL,
    [Address] nvarchar(500)  NULL,
    [City] nvarchar(250)  NULL,
    [District] nvarchar(250)  NULL,
    [Country] nvarchar(150)  NULL,
    [zip] int  NULL,
    [Active] bit  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [Password] nvarchar(500)  NOT NULL,
    [PasswordQuestion] nvarchar(500)  NULL,
    [PasswordAnswer] nvarchar(500)  NULL,
    [Telephone] varchar(20)  NULL,
    [Phone] varchar(20)  NULL,
    [DonviId] int  NOT NULL,
    [ChucVuId] int  NOT NULL,
    [UserType] int  NOT NULL,
    [TimeLogin] datetime  NULL,
    [IPLogin] nvarchar(500)  NULL,
    [GroupUserID] nvarchar(50)  NULL,
    [NoiBo] tinyint  NULL,
    [PageNoiBo] nvarchar(250)  NULL,
    [isAdmin] bit  NOT NULL,
    [IsBanChapHanh] bit  NOT NULL,
    [IsShow] bit  NOT NULL,
    [SoTaiKhoan] nvarchar(100)  NULL,
    [isDuyetComment] bit  NOT NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'tbl_UserAdmin'
CREATE TABLE [dbo].[tbl_UserAdmin] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [UserName] varchar(250)  NOT NULL,
    [Email] varchar(250)  NOT NULL,
    [FullName] nvarchar(250)  NULL,
    [Gender] tinyint  NULL,
    [Photo] nvarchar(max)  NULL,
    [Address] nvarchar(500)  NULL,
    [City] nvarchar(250)  NULL,
    [District] nvarchar(250)  NULL,
    [Country] nvarchar(150)  NULL,
    [zip] int  NULL,
    [Active] bit  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [Password] nvarchar(500)  NOT NULL,
    [PasswordQuestion] nvarchar(500)  NULL,
    [PasswordAnswer] nvarchar(500)  NULL,
    [Phone] varchar(20)  NULL,
    [TimeLogin] datetime  NULL,
    [IPLogin] nvarchar(500)  NULL,
    [GroupUserID] nvarchar(250)  NULL,
    [PageElementID] varchar(250)  NULL,
    [QuyTrinhXuatBanID] varchar(250)  NULL,
    [isAdmin] bit  NOT NULL,
    [SoTaiKhoan] nvarchar(100)  NULL,
    [IsDuyetComment] bit  NOT NULL,
    [LastOnline] datetime  NULL,
    [isOnline] bit  NULL,
    [NganHang] nvarchar(max)  NULL,
    [LangCode] varchar(50)  NULL,
    [IsCTV] bit  NULL
);
GO

-- Creating table 'tbl_VanBan'
CREATE TABLE [dbo].[tbl_VanBan] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [Status] bit  NOT NULL,
    [SoHieu] nvarchar(50)  NULL,
    [NgayHieuLuc] datetime  NULL,
    [NgayHieuLucVARCHAR] nvarchar(50)  NULL,
    [NguoiKy] nvarchar(150)  NULL,
    [CoQuanBanHanhId] int  NOT NULL,
    [LoaiVanBanId] int  NOT NULL,
    [LinhVucVanBanId] int  NOT NULL,
    [TrichYeu] nvarchar(500)  NOT NULL,
    [LangCode] varchar(5)  NOT NULL,
    [Attachment] nvarchar(max)  NULL,
    [isNoiBo] int  NULL,
    [IsHome] bit  NULL,
    [NgayBanHanh] datetime  NULL,
    [HetHieuLuc] bit  NULL,
    [IsRight] bit  NULL,
    [IsSendMail] bit  NULL,
    [IsSendSMS] bit  NULL,
    [Description] nvarchar(2000)  NULL,
    [CateID] int  NULL,
    [FileID] int  NOT NULL,
    [ExpiryDate] nvarchar(50)  NULL,
    [Agencies] nvarchar(200)  NULL,
    [ManCreate] int  NULL,
    [AttachmentName] nvarchar(max)  NULL,
    [AttachmentSize] varchar(max)  NULL
);
GO

-- Creating table 'tbl_Video'
CREATE TABLE [dbo].[tbl_Video] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Images] nvarchar(max)  NULL,
    [Description] nvarchar(500)  NULL,
    [Url] nvarchar(500)  NULL,
    [IsHot] bit  NOT NULL,
    [Status] bit  NOT NULL,
    [VideoCategoryId] varchar(max)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [LangCode] varchar(5)  NULL,
    [CreatedBy] varchar(50)  NULL,
    [Type] int  NOT NULL,
    [Ordering] int  NULL,
    [ViewNumber] bigint  NOT NULL,
    [Duration] nvarchar(200)  NULL,
    [IsHome] bit  NOT NULL,
    [UserActId] int  NOT NULL,
    [ProcedureId] int  NOT NULL,
    [NewMoney] int  NULL,
    [CreateById] int  NULL
);
GO

-- Creating table 'tbl_VideoCategory'
CREATE TABLE [dbo].[tbl_VideoCategory] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(150)  NOT NULL,
    [Url] nvarchar(250)  NULL,
    [Images] nvarchar(250)  NULL,
    [Description] nvarchar(500)  NULL,
    [Status] bit  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [CreatedBy] varchar(50)  NOT NULL,
    [ModifiedBy] varchar(50)  NULL,
    [Type] int  NOT NULL,
    [Ordering] int  NULL,
    [LangCode] varchar(50)  NULL
);
GO

-- Creating table 'test_category'
CREATE TABLE [dbo].[test_category] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ParentID] int  NULL,
    [Name] nvarchar(max)  NULL,
    [Photo] nvarchar(max)  NULL,
    [LangCode] varchar(50)  NULL,
    [Title] nvarchar(max)  NULL,
    [Keyword] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Ordering] int  NULL,
    [Status] bit  NOT NULL,
    [CreatedDate] datetime  NULL,
    [CreatedBy] int  NULL,
    [support_1] nvarchar(max)  NULL,
    [support_2] nvarchar(max)  NULL,
    [advert_1] nvarchar(max)  NULL,
    [advert_2] nvarchar(max)  NULL,
    [display_1] nvarchar(max)  NULL,
    [display_2] nvarchar(max)  NULL,
    [view_more_detail] nvarchar(max)  NULL
);
GO

-- Creating table 'view_Category_Languages'
CREATE TABLE [dbo].[view_Category_Languages] (
    [ID] int  NOT NULL,
    [ParentID] int  NOT NULL,
    [Name] nvarchar(250)  NOT NULL,
    [Description] nvarchar(250)  NULL,
    [Icon] nvarchar(max)  NULL,
    [Color] varchar(50)  NULL,
    [Background] varchar(50)  NULL,
    [Ordering] int  NOT NULL,
    [Active] bit  NOT NULL,
    [CreatedDate] datetime  NULL,
    [LangCode] varchar(5)  NULL,
    [LangName] nvarchar(250)  NOT NULL,
    [Code] varchar(5)  NOT NULL,
    [LangIcon] nvarchar(250)  NULL,
    [PageElementId] int  NOT NULL,
    [Slug] nvarchar(50)  NOT NULL,
    [FeautureImage] nvarchar(max)  NULL
);
GO

-- Creating table 'View_DataHome'
CREATE TABLE [dbo].[View_DataHome] (
    [ID] int  NOT NULL,
    [Code] nvarchar(max)  NULL,
    [isPublish] bit  NOT NULL,
    [LangCode] varchar(50)  NULL,
    [Name] nvarchar(max)  NULL,
    [NameEnglish] nvarchar(max)  NULL,
    [Ordering] int  NULL,
    [SoLuong] int  NULL
);
GO

-- Creating table 'View_Gallery_Category'
CREATE TABLE [dbo].[View_Gallery_Category] (
    [ID] int  NOT NULL,
    [Name] nvarchar(250)  NULL,
    [Images] nvarchar(max)  NOT NULL,
    [Status] bit  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [ModifiedDate] datetime  NULL,
    [CreatedBy] int  NULL,
    [ModifiedBy] int  NULL,
    [PageElementId] int  NOT NULL,
    [Url] nvarchar(250)  NULL,
    [Type] int  NOT NULL,
    [Ordering] int  NULL,
    [CategoryId] int  NULL,
    [CatName] nvarchar(500)  NOT NULL
);
GO

-- Creating table 'view_Images_ImageCategory'
CREATE TABLE [dbo].[view_Images_ImageCategory] (
    [Title] nvarchar(max)  NOT NULL,
    [Images] nvarchar(max)  NOT NULL,
    [Description] nvarchar(500)  NULL,
    [Url] nvarchar(500)  NULL,
    [Status] bit  NOT NULL,
    [Ordering] int  NULL,
    [CateName] nvarchar(500)  NOT NULL,
    [ImageCategoryId] varchar(max)  NOT NULL
);
GO

-- Creating table 'view_News'
CREATE TABLE [dbo].[view_News] (
    [CreatedDate] datetime  NOT NULL,
    [ViewCount] bigint  NULL,
    [ID] int IDENTITY(1,1) NOT NULL,
    [Status] int  NULL,
    [Author] nvarchar(max)  NULL,
    [CreatedBy] int  NULL,
    [Title] nvarchar(max)  NOT NULL,
    [SubTitle] nvarchar(max)  NULL,
    [Photo] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [IsPublish] bit  NOT NULL,
    [ModifyDate] datetime  NULL,
    [CategoryIdStr] varchar(200)  NULL,
    [SourceNews] nvarchar(max)  NULL,
    [LangCode] varchar(5)  NULL,
    [Type] int  NULL,
    [Tags] nvarchar(max)  NULL,
    [Position] varchar(250)  NULL,
    [ModifyBy] int  NULL,
    [AllowComment] tinyint  NULL,
    [IsGetNews] tinyint  NULL,
    [IsComment] bit  NOT NULL,
    [NewMoney] int  NULL,
    [RelatedNews] nvarchar(max)  NULL,
    [Attachment] nvarchar(max)  NULL,
    [UserActId] int  NOT NULL,
    [ProcedureId] int  NOT NULL,
    [NhuanBut] bigint  NOT NULL,
    [IdUserChoose] int  NULL,
    [OldId] nvarchar(max)  NULL,
    [AuthorGuidID] nvarchar(max)  NULL,
    [ZoneID] nvarchar(max)  NULL,
    [IsDelete] smallint  NOT NULL
);
GO

-- Creating table 'view_News_HomeCategory'
CREATE TABLE [dbo].[view_News_HomeCategory] (
    [ID] int  NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [SubTitle] nvarchar(max)  NULL,
    [Photo] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Status] int  NULL,
    [IsPublish] bit  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [ModifyDate] datetime  NULL,
    [CategoryIdStr] varchar(200)  NULL,
    [SourceNews] nvarchar(max)  NULL,
    [Author] nvarchar(max)  NULL,
    [LangCode] varchar(5)  NULL,
    [Tags] nvarchar(max)  NULL,
    [Type] int  NULL,
    [Position] varchar(250)  NULL,
    [CreatedBy] int  NULL,
    [ModifyBy] int  NULL,
    [ViewCount] bigint  NULL,
    [AllowComment] tinyint  NULL,
    [IsGetNews] tinyint  NULL,
    [IsComment] bit  NOT NULL,
    [NewMoney] int  NULL,
    [RelatedNews] nvarchar(max)  NULL,
    [Attachment] nvarchar(max)  NULL,
    [UserActId] int  NOT NULL,
    [ProcedureId] int  NOT NULL,
    [NhuanBut] bigint  NOT NULL,
    [IdUserChoose] int  NULL,
    [Expr3] varchar(500)  NULL
);
GO

-- Creating table 'view_News_hotAndUnderhot'
CREATE TABLE [dbo].[view_News_hotAndUnderhot] (
    [CreatedDate] datetime  NOT NULL,
    [ViewCount] bigint  NULL,
    [ID] int IDENTITY(1,1) NOT NULL,
    [Status] int  NULL,
    [Author] nvarchar(max)  NULL,
    [CreatedBy] int  NULL,
    [Title] nvarchar(max)  NOT NULL,
    [SubTitle] nvarchar(max)  NULL,
    [Photo] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [IsPublish] bit  NOT NULL,
    [ModifyDate] datetime  NULL,
    [CategoryIdStr] varchar(200)  NULL,
    [SourceNews] nvarchar(max)  NULL,
    [LangCode] varchar(5)  NULL,
    [Type] int  NULL,
    [Tags] nvarchar(max)  NULL,
    [Position] varchar(250)  NULL,
    [ModifyBy] int  NULL,
    [AllowComment] tinyint  NULL,
    [IsGetNews] tinyint  NULL,
    [IsComment] bit  NOT NULL,
    [NewMoney] int  NULL,
    [RelatedNews] nvarchar(max)  NULL,
    [Attachment] nvarchar(max)  NULL,
    [UserActId] int  NOT NULL,
    [ProcedureId] int  NOT NULL,
    [NhuanBut] bigint  NOT NULL,
    [IdUserChoose] int  NULL,
    [OldId] nvarchar(max)  NULL,
    [AuthorGuidID] nvarchar(max)  NULL
);
GO

-- Creating table 'view_News_Limit'
CREATE TABLE [dbo].[view_News_Limit] (
    [CreatedDate] datetime  NOT NULL,
    [ViewCount] bigint  NULL,
    [ID] int IDENTITY(1,1) NOT NULL,
    [Status] int  NULL,
    [Author] nvarchar(max)  NULL,
    [CreatedBy] int  NULL,
    [Title] nvarchar(max)  NOT NULL,
    [SubTitle] nvarchar(max)  NULL,
    [Photo] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [IsPublish] bit  NOT NULL,
    [ModifyDate] datetime  NULL,
    [CategoryIdStr] varchar(200)  NULL,
    [SourceNews] nvarchar(max)  NULL,
    [LangCode] varchar(5)  NULL,
    [Type] int  NULL,
    [Tags] nvarchar(max)  NULL,
    [Position] varchar(250)  NULL,
    [ModifyBy] int  NULL,
    [AllowComment] tinyint  NULL,
    [IsGetNews] tinyint  NULL,
    [IsComment] bit  NOT NULL,
    [NewMoney] int  NULL,
    [RelatedNews] nvarchar(max)  NULL,
    [Attachment] nvarchar(max)  NULL,
    [UserActId] int  NOT NULL,
    [ProcedureId] int  NOT NULL,
    [NhuanBut] bigint  NOT NULL,
    [IdUserChoose] int  NULL,
    [OldId] nvarchar(max)  NULL,
    [AuthorGuidID] nvarchar(max)  NULL
);
GO

-- Creating table 'view_NEWS_LISTDATA'
CREATE TABLE [dbo].[view_NEWS_LISTDATA] (
    [CreatedBy] int  NULL,
    [CategoryId] int  NOT NULL,
    [NguoiTao] varchar(250)  NULL,
    [ID] int  NOT NULL,
    [TieuDeChinh] nvarchar(500)  NOT NULL,
    [Status] int  NULL,
    [AnhDaiDien] nvarchar(250)  NULL,
    [LangCode] varchar(5)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [ModifyDate] datetime  NULL,
    [Author] nvarchar(50)  NULL,
    [CatName] nvarchar(250)  NULL,
    [CatImage] nvarchar(max)  NULL,
    [CatImage2] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [IsGetNews] tinyint  NULL,
    [AllowComment] tinyint  NULL,
    [ViewCount] bigint  NULL,
    [ModifyBy] int  NULL,
    [Tags] nvarchar(250)  NULL,
    [NewMoney] int  NULL,
    [MainTitle] nvarchar(500)  NOT NULL,
    [Avatar] nvarchar(250)  NULL
);
GO

-- Creating table 'view_NEWS_LISTDETAIL'
CREATE TABLE [dbo].[view_NEWS_LISTDETAIL] (
    [CreatedBy] int  NOT NULL,
    [CategoryId] int  NOT NULL,
    [NguoiTao] varchar(250)  NULL,
    [ID] int  NOT NULL,
    [Title] nvarchar(250)  NOT NULL,
    [Status] int  NULL,
    [FeautureImage] nvarchar(max)  NULL,
    [LangCode] varchar(5)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [DisplayZone] varchar(100)  NULL,
    [PageElementId] int  NULL,
    [ModifyDate] datetime  NULL,
    [Author] nvarchar(50)  NULL,
    [NoiBo] tinyint  NULL,
    [IsHot] tinyint  NULL,
    [IsSlide] tinyint  NULL,
    [IsNew] tinyint  NULL,
    [CatName] nvarchar(250)  NULL,
    [CatImage] nvarchar(max)  NULL,
    [CatImage2] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [CropImage] nvarchar(max)  NULL,
    [MorePageId] nvarchar(250)  NULL,
    [MoreCategoryId] nvarchar(250)  NULL,
    [MoreDisplayZone] nvarchar(250)  NULL,
    [IsGetNews] tinyint  NULL,
    [AllowComment] tinyint  NULL,
    [ViewCount] bigint  NOT NULL,
    [ModifyBy] int  NULL,
    [Tags] nvarchar(250)  NULL,
    [KeyWords] nvarchar(250)  NULL,
    [IsRightSlide] tinyint  NULL,
    [Details] nvarchar(max)  NULL
);
GO

-- Creating table 'view_News_MultiCate'
CREATE TABLE [dbo].[view_News_MultiCate] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [SubTitle] nvarchar(max)  NULL,
    [CateComma] varchar(202)  NULL,
    [Photo] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Status] int  NULL,
    [IsPublish] bit  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [ModifyDate] datetime  NULL,
    [CategoryIdStr] varchar(200)  NULL,
    [SourceNews] nvarchar(max)  NULL,
    [Author] nvarchar(max)  NULL,
    [LangCode] varchar(5)  NULL,
    [Tags] nvarchar(max)  NULL,
    [Type] int  NULL,
    [Position] varchar(250)  NULL,
    [CreatedBy] int  NULL,
    [ModifyBy] int  NULL,
    [ViewCount] bigint  NULL,
    [AllowComment] tinyint  NULL,
    [IsGetNews] tinyint  NULL,
    [IsComment] bit  NOT NULL,
    [NewMoney] int  NULL,
    [RelatedNews] nvarchar(max)  NULL,
    [Attachment] nvarchar(max)  NULL,
    [UserActId] int  NOT NULL,
    [ProcedureId] int  NOT NULL,
    [NhuanBut] bigint  NOT NULL,
    [IdUserChoose] int  NULL,
    [IsDelete] smallint  NOT NULL,
    [IsTraLai] bit  NOT NULL,
    [CTVID] int  NULL,
    [TimeChoose] datetime  NULL,
    [IsXBDuyet] bit  NOT NULL
);
GO

-- Creating table 'view_News_noPublic'
CREATE TABLE [dbo].[view_News_noPublic] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [SubTitle] nvarchar(max)  NULL,
    [Photo] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Details] nvarchar(max)  NULL,
    [Status] int  NULL,
    [IsPublish] bit  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [ModifyDate] datetime  NULL,
    [CategoryIdStr] varchar(200)  NULL,
    [SourceNews] nvarchar(max)  NULL,
    [Author] nvarchar(max)  NULL,
    [LangCode] varchar(5)  NULL,
    [Tags] nvarchar(max)  NULL,
    [Type] int  NULL,
    [Position] varchar(250)  NULL,
    [CreatedBy] int  NULL,
    [ModifyBy] int  NULL,
    [ViewCount] bigint  NULL,
    [AllowComment] tinyint  NULL,
    [IsGetNews] tinyint  NULL,
    [IsComment] bit  NOT NULL,
    [NewMoney] int  NULL,
    [RelatedNews] nvarchar(max)  NULL,
    [Attachment] nvarchar(max)  NULL,
    [UserActId] int  NOT NULL,
    [ProcedureId] int  NOT NULL,
    [NhuanBut] bigint  NOT NULL,
    [IdUserChoose] int  NULL,
    [OldId] nvarchar(max)  NULL,
    [AuthorGuidID] nvarchar(max)  NULL,
    [IsDelete] smallint  NOT NULL
);
GO

-- Creating table 'view_News_Public'
CREATE TABLE [dbo].[view_News_Public] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [SubTitle] nvarchar(max)  NULL,
    [Photo] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Details] nvarchar(max)  NULL,
    [Status] int  NULL,
    [IsPublish] bit  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [ModifyDate] datetime  NULL,
    [CategoryIdStr] varchar(200)  NULL,
    [SourceNews] nvarchar(max)  NULL,
    [Author] nvarchar(max)  NULL,
    [LangCode] varchar(5)  NULL,
    [Tags] nvarchar(max)  NULL,
    [Type] int  NULL,
    [Position] varchar(250)  NULL,
    [CreatedBy] int  NULL,
    [ModifyBy] int  NULL,
    [ViewCount] bigint  NULL,
    [AllowComment] tinyint  NULL,
    [IsGetNews] tinyint  NULL,
    [IsComment] bit  NOT NULL,
    [NewMoney] int  NULL,
    [RelatedNews] nvarchar(max)  NULL,
    [Attachment] nvarchar(max)  NULL,
    [UserActId] int  NOT NULL,
    [ProcedureId] int  NOT NULL,
    [NhuanBut] bigint  NOT NULL,
    [IdUserChoose] int  NULL,
    [OldId] nvarchar(max)  NULL,
    [AuthorGuidID] nvarchar(max)  NULL,
    [IsDelete] smallint  NOT NULL
);
GO

-- Creating table 'view_news_search'
CREATE TABLE [dbo].[view_news_search] (
    [Title] nvarchar(250)  NOT NULL,
    [ID] int IDENTITY(1,1) NOT NULL,
    [CategoryId] int  NOT NULL,
    [KeyWords] nvarchar(250)  NULL,
    [Tags] nvarchar(250)  NULL,
    [PageElementId] int  NULL,
    [CreatedDate] datetime  NOT NULL,
    [FeautureImage] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [NoiBo] tinyint  NULL,
    [Status] int  NULL,
    [IsSlide] tinyint  NULL,
    [IsHot] tinyint  NULL,
    [IsNew] tinyint  NULL,
    [CropImage] nvarchar(max)  NULL
);
GO

-- Creating table 'view_News_Sukien'
CREATE TABLE [dbo].[view_News_Sukien] (
    [CreatedDate] datetime  NOT NULL,
    [ViewCount] bigint  NULL,
    [ID] int IDENTITY(1,1) NOT NULL,
    [Status] int  NULL,
    [Author] nvarchar(max)  NULL,
    [CreatedBy] int  NULL,
    [Title] nvarchar(max)  NOT NULL,
    [SubTitle] nvarchar(max)  NULL,
    [Photo] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [IsPublish] bit  NOT NULL,
    [ModifyDate] datetime  NULL,
    [CategoryIdStr] varchar(200)  NULL,
    [SourceNews] nvarchar(max)  NULL,
    [LangCode] varchar(5)  NULL,
    [Type] int  NULL,
    [Tags] nvarchar(max)  NULL,
    [Position] varchar(250)  NULL,
    [ModifyBy] int  NULL,
    [AllowComment] tinyint  NULL,
    [IsGetNews] tinyint  NULL,
    [IsComment] bit  NOT NULL,
    [NewMoney] int  NULL,
    [RelatedNews] nvarchar(max)  NULL,
    [Attachment] nvarchar(max)  NULL,
    [UserActId] int  NOT NULL,
    [ProcedureId] int  NOT NULL,
    [NhuanBut] bigint  NOT NULL,
    [IdUserChoose] int  NULL,
    [OldId] nvarchar(max)  NULL,
    [AuthorGuidID] nvarchar(max)  NULL
);
GO

-- Creating table 'recruit_category'
CREATE TABLE [dbo].[recruit_category] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ParentID] int  NULL,
    [Name] nvarchar(max)  NULL,
    [Photo] nvarchar(max)  NULL,
    [LangCode] varchar(50)  NULL,
    [Title] nvarchar(max)  NULL,
    [Keyword] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Ordering] int  NULL,
    [Status] bit  NOT NULL,
    [CreatedDate] datetime  NULL,
    [CreatedBy] int  NULL,
    [support_1] nvarchar(max)  NULL,
    [support_2] nvarchar(max)  NULL,
    [advert_1] nvarchar(max)  NULL,
    [advert_2] nvarchar(max)  NULL,
    [display_1] nvarchar(max)  NULL,
    [display_2] nvarchar(max)  NULL,
    [view_more_detail] nvarchar(max)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'advert_category'
ALTER TABLE [dbo].[advert_category]
ADD CONSTRAINT [PK_advert_category]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'advert_category_module'
ALTER TABLE [dbo].[advert_category_module]
ADD CONSTRAINT [PK_advert_category_module]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'contact_category'
ALTER TABLE [dbo].[contact_category]
ADD CONSTRAINT [PK_contact_category]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DanhMucCons'
ALTER TABLE [dbo].[DanhMucCons]
ADD CONSTRAINT [PK_DanhMucCons]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DanhMucChas'
ALTER TABLE [dbo].[DanhMucChas]
ADD CONSTRAINT [PK_DanhMucChas]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'document_category'
ALTER TABLE [dbo].[document_category]
ADD CONSTRAINT [PK_document_category]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'document_price'
ALTER TABLE [dbo].[document_price]
ADD CONSTRAINT [PK_document_price]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'document_publisher'
ALTER TABLE [dbo].[document_publisher]
ADD CONSTRAINT [PK_document_publisher]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'faq_category'
ALTER TABLE [dbo].[faq_category]
ADD CONSTRAINT [PK_faq_category]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'GroupCusUsers'
ALTER TABLE [dbo].[GroupCusUsers]
ADD CONSTRAINT [PK_GroupCusUsers]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'guide_category'
ALTER TABLE [dbo].[guide_category]
ADD CONSTRAINT [PK_guide_category]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'modules'
ALTER TABLE [dbo].[modules]
ADD CONSTRAINT [PK_modules]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'news_category'
ALTER TABLE [dbo].[news_category]
ADD CONSTRAINT [PK_news_category]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'qProcedures'
ALTER TABLE [dbo].[qProcedures]
ADD CONSTRAINT [PK_qProcedures]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Id] in table 'qProcesses'
ALTER TABLE [dbo].[qProcesses]
ADD CONSTRAINT [PK_qProcesses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ID] in table 'quydinhpl_category'
ALTER TABLE [dbo].[quydinhpl_category]
ADD CONSTRAINT [PK_quydinhpl_category]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'support_category'
ALTER TABLE [dbo].[support_category]
ADD CONSTRAINT [PK_support_category]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'support_category_module'
ALTER TABLE [dbo].[support_category_module]
ADD CONSTRAINT [PK_support_category_module]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'survey_category'
ALTER TABLE [dbo].[survey_category]
ADD CONSTRAINT [PK_survey_category]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- Creating primary key on [Session] in table 'tbl_AccessOnline'
ALTER TABLE [dbo].[tbl_AccessOnline]
ADD CONSTRAINT [PK_tbl_AccessOnline]
    PRIMARY KEY CLUSTERED ([Session] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_AccessView'
ALTER TABLE [dbo].[tbl_AccessView]
ADD CONSTRAINT [PK_tbl_AccessView]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_AdminMenu'
ALTER TABLE [dbo].[tbl_AdminMenu]
ADD CONSTRAINT [PK_tbl_AdminMenu]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_Advert'
ALTER TABLE [dbo].[tbl_Advert]
ADD CONSTRAINT [PK_tbl_Advert]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_ApproveNews'
ALTER TABLE [dbo].[tbl_ApproveNews]
ADD CONSTRAINT [PK_tbl_ApproveNews]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Code], [PageElementId], [LangCode] in table 'tbl_BoxNewsConfig'
ALTER TABLE [dbo].[tbl_BoxNewsConfig]
ADD CONSTRAINT [PK_tbl_BoxNewsConfig]
    PRIMARY KEY CLUSTERED ([Code], [PageElementId], [LangCode] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_Calendar'
ALTER TABLE [dbo].[tbl_Calendar]
ADD CONSTRAINT [PK_tbl_Calendar]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_Category'
ALTER TABLE [dbo].[tbl_Category]
ADD CONSTRAINT [PK_tbl_Category]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_Config'
ALTER TABLE [dbo].[tbl_Config]
ADD CONSTRAINT [PK_tbl_Config]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_configLuuY'
ALTER TABLE [dbo].[tbl_configLuuY]
ADD CONSTRAINT [PK_tbl_configLuuY]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_ConfigText'
ALTER TABLE [dbo].[tbl_ConfigText]
ADD CONSTRAINT [PK_tbl_ConfigText]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_Contact'
ALTER TABLE [dbo].[tbl_Contact]
ADD CONSTRAINT [PK_tbl_Contact]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_ContactInfo'
ALTER TABLE [dbo].[tbl_ContactInfo]
ADD CONSTRAINT [PK_tbl_ContactInfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_CoQuanBanHanh'
ALTER TABLE [dbo].[tbl_CoQuanBanHanh]
ADD CONSTRAINT [PK_tbl_CoQuanBanHanh]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_CountAccess'
ALTER TABLE [dbo].[tbl_CountAccess]
ADD CONSTRAINT [PK_tbl_CountAccess]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_Chat'
ALTER TABLE [dbo].[tbl_Chat]
ADD CONSTRAINT [PK_tbl_Chat]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_ChucVu'
ALTER TABLE [dbo].[tbl_ChucVu]
ADD CONSTRAINT [PK_tbl_ChucVu]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_DanhMucTaiLieu'
ALTER TABLE [dbo].[tbl_DanhMucTaiLieu]
ADD CONSTRAINT [PK_tbl_DanhMucTaiLieu]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_DMChung'
ALTER TABLE [dbo].[tbl_DMChung]
ADD CONSTRAINT [PK_tbl_DMChung]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_DMNhom'
ALTER TABLE [dbo].[tbl_DMNhom]
ADD CONSTRAINT [PK_tbl_DMNhom]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_Document_Type'
ALTER TABLE [dbo].[tbl_Document_Type]
ADD CONSTRAINT [PK_tbl_Document_Type]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_DonVi'
ALTER TABLE [dbo].[tbl_DonVi]
ADD CONSTRAINT [PK_tbl_DonVi]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_DonViTTHC'
ALTER TABLE [dbo].[tbl_DonViTTHC]
ADD CONSTRAINT [PK_tbl_DonViTTHC]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_eMagazine'
ALTER TABLE [dbo].[tbl_eMagazine]
ADD CONSTRAINT [PK_tbl_eMagazine]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_FeedBack'
ALTER TABLE [dbo].[tbl_FeedBack]
ADD CONSTRAINT [PK_tbl_FeedBack]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_fieldfiles'
ALTER TABLE [dbo].[tbl_fieldfiles]
ADD CONSTRAINT [PK_tbl_fieldfiles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_Footer'
ALTER TABLE [dbo].[tbl_Footer]
ADD CONSTRAINT [PK_tbl_Footer]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_GroupUser'
ALTER TABLE [dbo].[tbl_GroupUser]
ADD CONSTRAINT [PK_tbl_GroupUser]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_HistoryNews'
ALTER TABLE [dbo].[tbl_HistoryNews]
ADD CONSTRAINT [PK_tbl_HistoryNews]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_HomeMenu'
ALTER TABLE [dbo].[tbl_HomeMenu]
ADD CONSTRAINT [PK_tbl_HomeMenu]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_HomeMenu_OLD'
ALTER TABLE [dbo].[tbl_HomeMenu_OLD]
ADD CONSTRAINT [PK_tbl_HomeMenu_OLD]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_Images'
ALTER TABLE [dbo].[tbl_Images]
ADD CONSTRAINT [PK_tbl_Images]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_ImagesCategory'
ALTER TABLE [dbo].[tbl_ImagesCategory]
ADD CONSTRAINT [PK_tbl_ImagesCategory]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_Languages'
ALTER TABLE [dbo].[tbl_Languages]
ADD CONSTRAINT [PK_tbl_Languages]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_LichCongTac'
ALTER TABLE [dbo].[tbl_LichCongTac]
ADD CONSTRAINT [PK_tbl_LichCongTac]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_Limit'
ALTER TABLE [dbo].[tbl_Limit]
ADD CONSTRAINT [PK_tbl_Limit]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_LinkWebsite'
ALTER TABLE [dbo].[tbl_LinkWebsite]
ADD CONSTRAINT [PK_tbl_LinkWebsite]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_LinhVucTTHC'
ALTER TABLE [dbo].[tbl_LinhVucTTHC]
ADD CONSTRAINT [PK_tbl_LinhVucTTHC]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_LinhVucVanBan'
ALTER TABLE [dbo].[tbl_LinhVucVanBan]
ADD CONSTRAINT [PK_tbl_LinhVucVanBan]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_LoaiVanBan'
ALTER TABLE [dbo].[tbl_LoaiVanBan]
ADD CONSTRAINT [PK_tbl_LoaiVanBan]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_Logs'
ALTER TABLE [dbo].[tbl_Logs]
ADD CONSTRAINT [PK_tbl_Logs]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_Magazine'
ALTER TABLE [dbo].[tbl_Magazine]
ADD CONSTRAINT [PK_tbl_Magazine]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_ModuleCate'
ALTER TABLE [dbo].[tbl_ModuleCate]
ADD CONSTRAINT [PK_tbl_ModuleCate]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_News'
ALTER TABLE [dbo].[tbl_News]
ADD CONSTRAINT [PK_tbl_News]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_NewsComment'
ALTER TABLE [dbo].[tbl_NewsComment]
ADD CONSTRAINT [PK_tbl_NewsComment]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_NewsPaper'
ALTER TABLE [dbo].[tbl_NewsPaper]
ADD CONSTRAINT [PK_tbl_NewsPaper]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_NewsPaperComment'
ALTER TABLE [dbo].[tbl_NewsPaperComment]
ADD CONSTRAINT [PK_tbl_NewsPaperComment]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_Online'
ALTER TABLE [dbo].[tbl_Online]
ADD CONSTRAINT [PK_tbl_Online]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_PageElement'
ALTER TABLE [dbo].[tbl_PageElement]
ADD CONSTRAINT [PK_tbl_PageElement]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_Position'
ALTER TABLE [dbo].[tbl_Position]
ADD CONSTRAINT [PK_tbl_Position]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_Product'
ALTER TABLE [dbo].[tbl_Product]
ADD CONSTRAINT [PK_tbl_Product]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_ProductCategory'
ALTER TABLE [dbo].[tbl_ProductCategory]
ADD CONSTRAINT [PK_tbl_ProductCategory]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_ProductOverView'
ALTER TABLE [dbo].[tbl_ProductOverView]
ADD CONSTRAINT [PK_tbl_ProductOverView]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_QALinhVuc'
ALTER TABLE [dbo].[tbl_QALinhVuc]
ADD CONSTRAINT [PK_tbl_QALinhVuc]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_QuestionAndAnswer'
ALTER TABLE [dbo].[tbl_QuestionAndAnswer]
ADD CONSTRAINT [PK_tbl_QuestionAndAnswer]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_SlideImages'
ALTER TABLE [dbo].[tbl_SlideImages]
ADD CONSTRAINT [PK_tbl_SlideImages]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_SubeMagazine'
ALTER TABLE [dbo].[tbl_SubeMagazine]
ADD CONSTRAINT [PK_tbl_SubeMagazine]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_TaiLieu'
ALTER TABLE [dbo].[tbl_TaiLieu]
ADD CONSTRAINT [PK_tbl_TaiLieu]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_ToGiac'
ALTER TABLE [dbo].[tbl_ToGiac]
ADD CONSTRAINT [PK_tbl_ToGiac]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_Topic'
ALTER TABLE [dbo].[tbl_Topic]
ADD CONSTRAINT [PK_tbl_Topic]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_TopicComment'
ALTER TABLE [dbo].[tbl_TopicComment]
ADD CONSTRAINT [PK_tbl_TopicComment]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_TTHC'
ALTER TABLE [dbo].[tbl_TTHC]
ADD CONSTRAINT [PK_tbl_TTHC]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_TTHCComment'
ALTER TABLE [dbo].[tbl_TTHCComment]
ADD CONSTRAINT [PK_tbl_TTHCComment]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_ThamDoYKien'
ALTER TABLE [dbo].[tbl_ThamDoYKien]
ADD CONSTRAINT [PK_tbl_ThamDoYKien]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_ThamDoYKien_Ip'
ALTER TABLE [dbo].[tbl_ThamDoYKien_Ip]
ADD CONSTRAINT [PK_tbl_ThamDoYKien_Ip]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_truyna'
ALTER TABLE [dbo].[tbl_truyna]
ADD CONSTRAINT [PK_tbl_truyna]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_User'
ALTER TABLE [dbo].[tbl_User]
ADD CONSTRAINT [PK_tbl_User]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_UserAdmin'
ALTER TABLE [dbo].[tbl_UserAdmin]
ADD CONSTRAINT [PK_tbl_UserAdmin]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_VanBan'
ALTER TABLE [dbo].[tbl_VanBan]
ADD CONSTRAINT [PK_tbl_VanBan]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_Video'
ALTER TABLE [dbo].[tbl_Video]
ADD CONSTRAINT [PK_tbl_Video]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tbl_VideoCategory'
ALTER TABLE [dbo].[tbl_VideoCategory]
ADD CONSTRAINT [PK_tbl_VideoCategory]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'test_category'
ALTER TABLE [dbo].[test_category]
ADD CONSTRAINT [PK_test_category]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID], [ParentID], [Name], [Ordering], [Active], [LangName], [Code], [PageElementId], [Slug] in table 'view_Category_Languages'
ALTER TABLE [dbo].[view_Category_Languages]
ADD CONSTRAINT [PK_view_Category_Languages]
    PRIMARY KEY CLUSTERED ([ID], [ParentID], [Name], [Ordering], [Active], [LangName], [Code], [PageElementId], [Slug] ASC);
GO

-- Creating primary key on [ID], [isPublish] in table 'View_DataHome'
ALTER TABLE [dbo].[View_DataHome]
ADD CONSTRAINT [PK_View_DataHome]
    PRIMARY KEY CLUSTERED ([ID], [isPublish] ASC);
GO

-- Creating primary key on [ID], [Images], [Status], [CreatedDate], [PageElementId], [Type], [CatName] in table 'View_Gallery_Category'
ALTER TABLE [dbo].[View_Gallery_Category]
ADD CONSTRAINT [PK_View_Gallery_Category]
    PRIMARY KEY CLUSTERED ([ID], [Images], [Status], [CreatedDate], [PageElementId], [Type], [CatName] ASC);
GO

-- Creating primary key on [Title], [Images], [Status], [CateName], [ImageCategoryId] in table 'view_Images_ImageCategory'
ALTER TABLE [dbo].[view_Images_ImageCategory]
ADD CONSTRAINT [PK_view_Images_ImageCategory]
    PRIMARY KEY CLUSTERED ([Title], [Images], [Status], [CateName], [ImageCategoryId] ASC);
GO

-- Creating primary key on [CreatedDate], [ID], [Title], [IsPublish], [IsComment], [UserActId], [ProcedureId], [NhuanBut], [IsDelete] in table 'view_News'
ALTER TABLE [dbo].[view_News]
ADD CONSTRAINT [PK_view_News]
    PRIMARY KEY CLUSTERED ([CreatedDate], [ID], [Title], [IsPublish], [IsComment], [UserActId], [ProcedureId], [NhuanBut], [IsDelete] ASC);
GO

-- Creating primary key on [ID], [Title], [IsPublish], [CreatedDate], [IsComment], [UserActId], [ProcedureId], [NhuanBut] in table 'view_News_HomeCategory'
ALTER TABLE [dbo].[view_News_HomeCategory]
ADD CONSTRAINT [PK_view_News_HomeCategory]
    PRIMARY KEY CLUSTERED ([ID], [Title], [IsPublish], [CreatedDate], [IsComment], [UserActId], [ProcedureId], [NhuanBut] ASC);
GO

-- Creating primary key on [CreatedDate], [ID], [Title], [IsPublish], [IsComment], [UserActId], [ProcedureId], [NhuanBut] in table 'view_News_hotAndUnderhot'
ALTER TABLE [dbo].[view_News_hotAndUnderhot]
ADD CONSTRAINT [PK_view_News_hotAndUnderhot]
    PRIMARY KEY CLUSTERED ([CreatedDate], [ID], [Title], [IsPublish], [IsComment], [UserActId], [ProcedureId], [NhuanBut] ASC);
GO

-- Creating primary key on [CreatedDate], [ID], [Title], [IsPublish], [IsComment], [UserActId], [ProcedureId], [NhuanBut] in table 'view_News_Limit'
ALTER TABLE [dbo].[view_News_Limit]
ADD CONSTRAINT [PK_view_News_Limit]
    PRIMARY KEY CLUSTERED ([CreatedDate], [ID], [Title], [IsPublish], [IsComment], [UserActId], [ProcedureId], [NhuanBut] ASC);
GO

-- Creating primary key on [CategoryId], [ID], [TieuDeChinh], [CreatedDate], [MainTitle] in table 'view_NEWS_LISTDATA'
ALTER TABLE [dbo].[view_NEWS_LISTDATA]
ADD CONSTRAINT [PK_view_NEWS_LISTDATA]
    PRIMARY KEY CLUSTERED ([CategoryId], [ID], [TieuDeChinh], [CreatedDate], [MainTitle] ASC);
GO

-- Creating primary key on [CreatedBy], [CategoryId], [ID], [Title], [CreatedDate], [ViewCount] in table 'view_NEWS_LISTDETAIL'
ALTER TABLE [dbo].[view_NEWS_LISTDETAIL]
ADD CONSTRAINT [PK_view_NEWS_LISTDETAIL]
    PRIMARY KEY CLUSTERED ([CreatedBy], [CategoryId], [ID], [Title], [CreatedDate], [ViewCount] ASC);
GO

-- Creating primary key on [ID], [Title], [IsPublish], [CreatedDate], [IsComment], [UserActId], [ProcedureId], [NhuanBut], [IsDelete], [IsTraLai], [IsXBDuyet] in table 'view_News_MultiCate'
ALTER TABLE [dbo].[view_News_MultiCate]
ADD CONSTRAINT [PK_view_News_MultiCate]
    PRIMARY KEY CLUSTERED ([ID], [Title], [IsPublish], [CreatedDate], [IsComment], [UserActId], [ProcedureId], [NhuanBut], [IsDelete], [IsTraLai], [IsXBDuyet] ASC);
GO

-- Creating primary key on [ID], [Title], [IsPublish], [CreatedDate], [IsComment], [UserActId], [ProcedureId], [NhuanBut], [IsDelete] in table 'view_News_noPublic'
ALTER TABLE [dbo].[view_News_noPublic]
ADD CONSTRAINT [PK_view_News_noPublic]
    PRIMARY KEY CLUSTERED ([ID], [Title], [IsPublish], [CreatedDate], [IsComment], [UserActId], [ProcedureId], [NhuanBut], [IsDelete] ASC);
GO

-- Creating primary key on [ID], [Title], [IsPublish], [CreatedDate], [IsComment], [UserActId], [ProcedureId], [NhuanBut], [IsDelete] in table 'view_News_Public'
ALTER TABLE [dbo].[view_News_Public]
ADD CONSTRAINT [PK_view_News_Public]
    PRIMARY KEY CLUSTERED ([ID], [Title], [IsPublish], [CreatedDate], [IsComment], [UserActId], [ProcedureId], [NhuanBut], [IsDelete] ASC);
GO

-- Creating primary key on [Title], [ID], [CategoryId], [CreatedDate] in table 'view_news_search'
ALTER TABLE [dbo].[view_news_search]
ADD CONSTRAINT [PK_view_news_search]
    PRIMARY KEY CLUSTERED ([Title], [ID], [CategoryId], [CreatedDate] ASC);
GO

-- Creating primary key on [CreatedDate], [ID], [Title], [IsPublish], [IsComment], [UserActId], [ProcedureId], [NhuanBut] in table 'view_News_Sukien'
ALTER TABLE [dbo].[view_News_Sukien]
ADD CONSTRAINT [PK_view_News_Sukien]
    PRIMARY KEY CLUSTERED ([CreatedDate], [ID], [Title], [IsPublish], [IsComment], [UserActId], [ProcedureId], [NhuanBut] ASC);
GO

-- Creating primary key on [ID] in table 'recruit_category'
ALTER TABLE [dbo].[recruit_category]
ADD CONSTRAINT [PK_recruit_category]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserID] in table 'tbl_LichCongTac'
ALTER TABLE [dbo].[tbl_LichCongTac]
ADD CONSTRAINT [FK_tbl_LichCongTac_tbl_UserAdmin1]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[tbl_UserAdmin]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_LichCongTac_tbl_UserAdmin1'
CREATE INDEX [IX_FK_tbl_LichCongTac_tbl_UserAdmin1]
ON [dbo].[tbl_LichCongTac]
    ([UserID]);
GO

-- Creating foreign key on [NewsPaperID] in table 'tbl_NewsPaperComment'
ALTER TABLE [dbo].[tbl_NewsPaperComment]
ADD CONSTRAINT [FK_tbl_NewsPaperComment_tbl_NewsPaper]
    FOREIGN KEY ([NewsPaperID])
    REFERENCES [dbo].[tbl_NewsPaper]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_NewsPaperComment_tbl_NewsPaper'
CREATE INDEX [IX_FK_tbl_NewsPaperComment_tbl_NewsPaper]
ON [dbo].[tbl_NewsPaperComment]
    ([NewsPaperID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------