/*    ==Scripting Parameters==

    Source Database Engine Edition : Microsoft Azure SQL Database Edition
    Source Database Engine Type : Microsoft Azure SQL Database

    Target Database Engine Edition : Microsoft Azure SQL Database Edition
    Target Database Engine Type : Microsoft Azure SQL Database
*/

/****** Object:  StoredProcedure [dbo].[GetSaltByEmail]    Script Date: 10/22/2017 12:30:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Keith Zane
-- Create Date: 10/14/17
-- Description: Takes in the email and gives back the salt so that we can add it to the password check.
-- =============================================
CREATE PROCEDURE [dbo].[GetSaltByEmail]
(
    -- Add the parameters for the stored procedure here
    @Email varchar(100)
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
    if EXISTS (Select * from UsersInformation UI
				where UI.Email = @email)
	begin
		Select UI.Salt from UsersInformation UI
				where UI.Email = @email
	end
	else
	begin
		Select '-1' as 'Salt'
	end
END
GO

