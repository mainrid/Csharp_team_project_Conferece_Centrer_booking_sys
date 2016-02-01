USE [master]
GO
/****** Object:  Database [Keskus_baas]    Script Date: 17-Nov-15 11:39:16 PM ******/
CREATE DATABASE [Keskus_baas]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Keskus_baas', FILENAME = N'C:\Users\alisa\Keskus_baas.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Keskus_baas_log', FILENAME = N'C:\Users\alisa\Keskus_baas_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Keskus_baas] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Keskus_baas].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Keskus_baas] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Keskus_baas] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Keskus_baas] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Keskus_baas] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Keskus_baas] SET ARITHABORT OFF 
GO
ALTER DATABASE [Keskus_baas] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Keskus_baas] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Keskus_baas] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Keskus_baas] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Keskus_baas] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Keskus_baas] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Keskus_baas] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Keskus_baas] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Keskus_baas] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Keskus_baas] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Keskus_baas] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Keskus_baas] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Keskus_baas] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Keskus_baas] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Keskus_baas] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Keskus_baas] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Keskus_baas] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Keskus_baas] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Keskus_baas] SET  MULTI_USER 
GO
ALTER DATABASE [Keskus_baas] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Keskus_baas] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Keskus_baas] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Keskus_baas] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [Keskus_baas] SET DELAYED_DURABILITY = DISABLED 
GO
USE [Keskus_baas]
GO
/****** Object:  Table [dbo].[Administraator]    Script Date: 17-Nov-15 11:39:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Administraator](
	[AdminID] [int] IDENTITY(1,1) NOT NULL,
	[Nimi] [nvarchar](50) NOT NULL,
	[Muutmisoigus] [bit] NOT NULL,
 CONSTRAINT [PK_Administraator] PRIMARY KEY CLUSTERED 
(
	[AdminID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Broneering]    Script Date: 17-Nov-15 11:39:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Broneering](
	[BroneeringID] [int] IDENTITY(1,1) NOT NULL,
	[Kuupaev] [date] NOT NULL,
	[RuumID] [int] NOT NULL,
	[KlientID] [int] NOT NULL,
	[OsalejateArv] [int] NOT NULL,
	[Loodud] [datetime] NOT NULL,
	[Kinnitatud] [bit] NOT NULL,
	[AdminID] [int] NOT NULL,
 CONSTRAINT [PK_Broneering] PRIMARY KEY CLUSTERED 
(
	[BroneeringID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Klient]    Script Date: 17-Nov-15 11:39:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Klient](
	[KlientID] [int] IDENTITY(1,1) NOT NULL,
	[OrgNimetus] [nvarchar](50) NOT NULL,
	[Kontaktisik] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Klient] PRIMARY KEY CLUSTERED 
(
	[KlientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Kontakt]    Script Date: 17-Nov-15 11:39:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Kontakt](
	[KontaktID] [int] IDENTITY(1,1) NOT NULL,
	[KlientID] [int] NOT NULL,
	[KontaktliikID] [int] NOT NULL,
	[Vaartus] [nvarchar](50) NULL,
	[Loodud] [datetime] NULL,
 CONSTRAINT [PK_Kontakt] PRIMARY KEY CLUSTERED 
(
	[KontaktID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Kontaktliik]    Script Date: 17-Nov-15 11:39:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Kontaktliik](
	[KontaktliikID] [int] IDENTITY(1,1) NOT NULL,
	[Nimetus] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_Kontaktliik] PRIMARY KEY CLUSTERED 
(
	[KontaktliikID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Ruum]    Script Date: 17-Nov-15 11:39:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ruum](
	[RuumID] [int] IDENTITY(1,1) NOT NULL,
	[Nimetus] [nvarchar](50) NOT NULL,
	[Istekohad] [int] NOT NULL,
	[Aktiivne] [bit] NOT NULL,
 CONSTRAINT [PK_Ruum] PRIMARY KEY CLUSTERED 
(
	[RuumID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Broneering]  WITH CHECK ADD  CONSTRAINT [FK_Broneering_Administraator] FOREIGN KEY([AdminID])
REFERENCES [dbo].[Administraator] ([AdminID])
GO
ALTER TABLE [dbo].[Broneering] CHECK CONSTRAINT [FK_Broneering_Administraator]
GO
ALTER TABLE [dbo].[Broneering]  WITH CHECK ADD  CONSTRAINT [FK_Broneering_Klient] FOREIGN KEY([KlientID])
REFERENCES [dbo].[Klient] ([KlientID])
GO
ALTER TABLE [dbo].[Broneering] CHECK CONSTRAINT [FK_Broneering_Klient]
GO
ALTER TABLE [dbo].[Broneering]  WITH CHECK ADD  CONSTRAINT [FK_Broneering_Ruum] FOREIGN KEY([RuumID])
REFERENCES [dbo].[Ruum] ([RuumID])
GO
ALTER TABLE [dbo].[Broneering] CHECK CONSTRAINT [FK_Broneering_Ruum]
GO
ALTER TABLE [dbo].[Kontakt]  WITH CHECK ADD  CONSTRAINT [FK_Kontakt_Klient] FOREIGN KEY([KlientID])
REFERENCES [dbo].[Klient] ([KlientID])
GO
ALTER TABLE [dbo].[Kontakt] CHECK CONSTRAINT [FK_Kontakt_Klient]
GO
ALTER TABLE [dbo].[Kontakt]  WITH CHECK ADD  CONSTRAINT [FK_Kontakt_Kontaktliik] FOREIGN KEY([KontaktliikID])
REFERENCES [dbo].[Kontaktliik] ([KontaktliikID])
GO
ALTER TABLE [dbo].[Kontakt] CHECK CONSTRAINT [FK_Kontakt_Kontaktliik]
GO
USE [master]
GO
ALTER DATABASE [Keskus_baas] SET  READ_WRITE 
GO
