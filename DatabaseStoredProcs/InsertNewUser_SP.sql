/*    ==Scripting Parameters==

    Source Database Engine Edition : Microsoft Azure SQL Database Edition
    Source Database Engine Type : Microsoft Azure SQL Database

    Target Database Engine Edition : Microsoft Azure SQL Database Edition
    Target Database Engine Type : Microsoft Azure SQL Database
*/

/****** Object:  StoredProcedure [dbo].[InsertNewUser]    Script Date: 10/22/2017 12:36:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Keith Zane
-- Create Date: 10/13/17
-- Description: This Stored Procedure is used for when the Admin adds a new User
--				This will help create the reference that we need.
-- =============================================
CREATE PROCEDURE [dbo].[InsertNewUser]
(
    -- Add the parameters for the stored procedure here
    @CreatedUser varchar(100),
	@CompID int,
	@Email varchar(100),
	@Pass varchar(64),
	@Salt varchar(64),
	@FirstName varchar(50),
	@LastName varchar(50),
	@EncryptType varchar(20),
	@IsAdmin bit

)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

	Declare @userIDInserted int

    -- Insert statements for procedure here
	if EXISTS(SELECT * FROM CompaniesInformation where CompanyID = @CompID)
	begin
		if EXISTS(Select * From UsersInformation UI
					where UI.Email = @Email)
		begin
			select -1 as 'Result'
		end
		else
		begin
			Insert Into UsersInformation (CompanyID, FirstName, LastName, Email, Password, Salt, EncryptionType,
									Active, Locked, needsPasswordReset, ModifiedDate, ModifiedUser, CreatedDate, CreatedUser)
			values (@CompID, @FirstName, @LastName, @Email, @Pass, @Salt, @EncryptType, 1, 0, 0, GETDATE(), @CreatedUser, GETDATE(), @CreatedUser)


			SET @userIDInserted = SCOPE_IDENTITY()

			Insert Into UserRights (UserID, RightID, CreatedBy, CreatedDate, Active)
			values (@userIDInserted, (Select RightID FROM Rights where RightName = 'STANDARD'), @CreatedUser, GETDATE(), '1')

			if (@IsAdmin = 1)
			begin
				Insert Into UserRights (UserID, RightID, CreatedBy, CreatedDate, Active)
				values (@userIDInserted, (Select RightID FROM Rights where RightName = 'ADMIN'), @CreatedUser, GETDATE(), '1')
			end

			Select 1 as 'Result'
		end
	end
	else
	begin
		Select -2 as 'Result'
	end
END
GO

