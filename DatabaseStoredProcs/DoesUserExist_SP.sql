/*    ==Scripting Parameters==

    Source Database Engine Edition : Microsoft Azure SQL Database Edition
    Source Database Engine Type : Microsoft Azure SQL Database

    Target Database Engine Edition : Microsoft Azure SQL Database Edition
    Target Database Engine Type : Microsoft Azure SQL Database
*/

/****** Object:  StoredProcedure [dbo].[DoesUserExist]    Script Date: 10/22/2017 12:29:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Keith Zane
-- Create Date: 10/13/2017
-- Description: This stored procedure is used to check to see if the user exists in the database
--				It will give error codes back based on the information given
--					Error Codes:
--						-1 (The User Is Locked Out Of Their Account)
--						-2 (The User Is Not Active)
--						-3 (The User Typed in the wrong password)
--						-10000 (Fatal Error)
--
--					Results:
--						1 (The User Exists and Can Log In)
--						0 (The User Does Not Exist)
-- =============================================
CREATE PROCEDURE [dbo].[DoesUserExist]
(
    -- Add the parameters for the stored procedure here
    @EmailAddress varchar(100),
	@Pass varchar(64)
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
    If Exists(Select * from UsersInformation
				where Email = @EmailAddress and Password = @Pass and Active = 1 and Locked = 0)
	begin
		-- User exists, isn't locked out, and is active.
		Select 1 as 'Result'
	end
	else
	begin
		if Exists(Select * from UsersInformation
				where Email = @EmailAddress and Password = @Pass and Active = 1 and Locked = 1)
		begin
			-- User Exists Right information, but is locked out
			Select -1 as 'Result'
		end
		else
		begin
			if Exists(Select * from UsersInformation
				where Email = @EmailAddress and Password = @Pass and Active = 0)
			begin
			-- User Exists Right Information, but isn't active
				Select -2 as 'Result'
			end
			else
			begin
				if Exists(Select * from UsersInformation
				where Email = @EmailAddress and Password <> @Pass)
				begin
					-- User Exists Wrong Information
					Select -3 as 'Result'
				end
				else
				begin
					if (not Exists(Select * from UsersInformation
					where Email = @EmailAddress))
					begin
					-- User Does Not Exist
						Select 0  as 'Result'
					end
					else
					begin
						--FATAL ERROR!!! 
						Select -10000  as 'Result'
					end
				end
			end
		end
	end
END
GO

