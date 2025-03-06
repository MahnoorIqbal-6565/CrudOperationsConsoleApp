/
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
/*Pointer used for text / image updates. This might not be needed, but is declared here just in case*/
DECLARE @pv binary(16)
BEGIN TRANSACTION
ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [FK_Employees_Departments_DepartmentId]
DELETE FROM [dbo].[Employees] WHERE [EmployeeId]=6
SET IDENTITY_INSERT [dbo].[Employees] ON
INSERT INTO [dbo].[Employees] ([EmployeeId], [Name], [Email], [PhoneNumber], [Address], [City], [DepartmentId]) VALUES (4, N'mahn', N'mah@gmail.com', N'03030030', N'mayamr', N'Karaci', 1)
SET IDENTITY_INSERT [dbo].[Employees] OFF
ALTER TABLE [dbo].[Employees]
    ADD CONSTRAINT [FK_Employees_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[Departments] ([DepartmentId])
COMMIT TRANSACTION
