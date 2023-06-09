
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([Id], [ParentId], [Name], [Description], [IsActive], [DateCreated], [DateUpdated]) VALUES (1, 0, N'Branding', N'Branding', 1, CAST(N'2023-04-05T10:35:09.777' AS DateTime), CAST(N'2023-04-05T10:35:09.777' AS DateTime))
INSERT [dbo].[Categories] ([Id], [ParentId], [Name], [Description], [IsActive], [DateCreated], [DateUpdated]) VALUES (2, 0, N'Design', N'Design', 1, CAST(N'2023-04-05T10:35:09.777' AS DateTime), CAST(N'2023-04-05T10:35:09.777' AS DateTime))
INSERT [dbo].[Categories] ([Id], [ParentId], [Name], [Description], [IsActive], [DateCreated], [DateUpdated]) VALUES (3, 0, N'Photo', N'Photo', 1, CAST(N'2023-04-05T10:35:09.777' AS DateTime), CAST(N'2023-04-05T10:35:09.777' AS DateTime))
INSERT [dbo].[Categories] ([Id], [ParentId], [Name], [Description], [IsActive], [DateCreated], [DateUpdated]) VALUES (4, 0, N'Coffee', N'Coffee', 1, CAST(N'2023-04-05T10:35:09.777' AS DateTime), CAST(N'2023-04-05T10:35:09.777' AS DateTime))
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
----------
SET IDENTITY_INSERT [dbo].[Menus] ON 

INSERT [dbo].[Menus] ([Id], [Name], [ParentId], [Controller], [Action], [Slug], [Params], [Title], [IsActive], [IsHorizontal], [DateCreated], [DateUpdated]) VALUES (1, N'Trang chủ', 0, N'Home', N'Index', N'/Home/Index', N'', N'Trang chủ', 1, 1, CAST(N'2023-04-05T10:35:09.773' AS DateTime), CAST(N'2023-04-05T10:35:09.773' AS DateTime))
INSERT [dbo].[Menus] ([Id], [Name], [ParentId], [Controller], [Action], [Slug], [Params], [Title], [IsActive], [IsHorizontal], [DateCreated], [DateUpdated]) VALUES (2, N'Dự án', 0, N'Home', N'Project', N'/Home/Project', N'', N'Dự án', 1, 1, CAST(N'2023-04-05T10:35:09.773' AS DateTime), CAST(N'2023-04-05T10:35:09.773' AS DateTime))
INSERT [dbo].[Menus] ([Id], [Name], [ParentId], [Controller], [Action], [Slug], [Params], [Title], [IsActive], [IsHorizontal], [DateCreated], [DateUpdated]) VALUES (3, N'Về chúng tôi', 0, N'Home', N'About', N'/Home/About', N'', N'Về chúng tôi', 1, 1, CAST(N'2023-04-05T10:35:09.773' AS DateTime), CAST(N'2023-04-05T10:35:09.773' AS DateTime))
SET IDENTITY_INSERT [dbo].[Menus] OFF
GO
----------
SET IDENTITY_INSERT [dbo].[Projects] ON 

INSERT [dbo].[Projects] ([Id], [CategoryId], [Name], [Seo], [Keyword], [Title], [Description], [ShortDesc], [Created], [IsActive], [Investor], [Address], [LandArea], [ConstructionArea], [YearOfCompletion], [Architect], [Intro], [IntroContent], [Intro1], [Intro1Content], [Intro2], [Intro2Content], [Picture], [Picture1], [Picture2], [Picture3], [Picture4], [CategoryClasses], [DateCreated], [DateUpdated]) VALUES (1, 1, N'Chung cư Tecco Garden', N'Best web solution', N'Interior Design', N'Project title 1', N'Chung cư Tecco Garden', N'Lorem ipsum dolor sit amet consec adipiscing nulla quis fermentum hendrerit nisi diam viverra.', CAST(N'2023-03-27T07:54:08.417' AS DateTime), 1, N'Tecco Group', N'Tứ Hiệp Thanh Trì', CAST(14470.00 AS Decimal(18, 2)), CAST(6531.00 AS Decimal(18, 2)), 2020, N'Ngô Quang Mạnh', N'Aenean suscipit eget mi act', N'<p>Aenean suscipit eget mi act fermentum phasellus vulputate turpis tincidunt. Aenean suscipit eget mi act fermentum phasellus vulputate turpis tincidunt. Aenean suscipit eget mi act fermentum phasellus vulputate turpis tincidunt.  Aenean suscipit eget mi act fermentum phasellus vulputate turpis tincidunt.</p><p>Aenean suscipit eget mi act fermentum phasellus vulputate turpis tincidunt. Aenean suscipit eget mi act fermentum phasellus   vulputate turpis tincidunt. Aenean suscipit eget mi act fermentum phasellus vulputate turpis tincidunt. Aenean suscipit eget mi act fermentum phasellus vulputate turpis tincidunt.</p>', N'Aenean suscipit eget mi act', N'<p>Aenean suscipit eget mi act fermentum phasellus vulputate turpis tincidunt. Aenean suscipit eget mi act fermentum phasellus vulputate turpis tincidunt. Aenean suscipit eget mi act fermentum phasellus vulputate turpis tincidunt. Aenean suscipit eget mi act fermentum phasellus.</p>', N'Aenean suscipit eget mi act', N'<p>Aenean suscipit eget mi act fermentum phasellus vulputate turpis tincidunt. Aenean suscipit eget mi act fermentum phasellus vulputate turpis tincidunt. Aenean suscipit eget mi act fermentum phasellus vulputate turpis tincidunt. Aenean suscipit eget mi act fermentum phasellus.</p>', N'assets/images/parallax1.jpg', N'assets/images/popup/small-1-1.jpg', N'assets/images/popup/small-2-1.jpg', N'assets/images/popup/small-3-1.jpg', N'assets/images/popup/small-4-1.jpg', NULL, CAST(N'2023-04-05T10:35:09.780' AS DateTime), CAST(N'2023-04-05T10:35:09.780' AS DateTime))
SET IDENTITY_INSERT [dbo].[Projects] OFF
GO
----------
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([RoleId], [RoleName]) VALUES (1, N'Admin')
INSERT [dbo].[Roles] ([RoleId], [RoleName]) VALUES (2, N'User')
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
----------
INSERT [dbo].[UserRoles] ([UserId], [RoleId]) VALUES (1, 1)
INSERT [dbo].[UserRoles] ([UserId], [RoleId]) VALUES (1, 2)
GO
----------
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserId], [Username], [FirstName], [LastName], [Email], [Password], [IsActive], [ActivationCode]) VALUES (1, N'admin', N'admin', N'admin', N'info@ktq.vn', N'25d55ad283aa400af464c76d713c07ad', 1, N'136ba3a5-bd1a-4a98-8891-b49978dc5503')
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
----------