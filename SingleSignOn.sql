USE [SingleSignOn]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 4/29/2018 6:23:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [uniqueidentifier] NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[DisplayName] [nvarchar](50) NULL,
	[Role] [nvarchar](50) NOT NULL,
	[SubjectId] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

USE [SingleSignOn]
GO
INSERT [dbo].[Users] ([Id], [Username], [Password], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [DisplayName], [Role], [SubjectId]) VALUES (N'41bee8a3-0734-4793-84e5-449a141846f3', N'admin123', N'admin123', N'admin@admin.com', CAST(N'2018-04-29T03:03:13.533' AS DateTime), N'admin@admin.com', CAST(N'2018-04-29T03:03:13.533' AS DateTime), N'admin', N'Admin', N'2f70abed-b7e8-41bd-9197-cbe4419a509b')
GO
INSERT [dbo].[Users] ([Id], [Username], [Password], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [DisplayName], [Role], [SubjectId]) VALUES (N'cf40ea14-f862-43a1-8f65-4bbad46d463f', N'business123', N'business123', N'admin', CAST(N'2018-04-29T18:08:48.947' AS DateTime), N'admin', CAST(N'2018-04-29T18:08:48.947' AS DateTime), N'business', N'BusinessUser', N'f995eca0-6311-431c-af17-a7b92e703c52')
GO
INSERT [dbo].[Users] ([Id], [Username], [Password], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [DisplayName], [Role], [SubjectId]) VALUES (N'2cc6b80f-59dd-4734-8081-52b463d20054', N'guest123', N'guest123', N'admin@admin.com', CAST(N'2018-04-29T00:22:29.743' AS DateTime), N'admin@admin.com', CAST(N'2018-04-29T00:22:29.743' AS DateTime), N'Guest', N'Guest', N'2cc6b80f-59dd-4734-8081-52b463d20054')
GO
