/*    ==Scripting Parameters==

    Source Database Engine Edition : Microsoft Azure SQL Database Edition
    Source Database Engine Type : Microsoft Azure SQL Database

    Target Database Engine Edition : Microsoft Azure SQL Database Edition
    Target Database Engine Type : Microsoft Azure SQL Database
*/

/****** Object:  StoredProcedure [dbo].[UpdateUsersByUserID]    Script Date: 10/22/2017 12:39:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[UpdateUsersByUserID]
(
    -- Add the parameters for the stored procedure here
    @UserID int,
	@Password varchar(64),
	@Salt varchar(64),
	@EncryptionType varchar(20),
	@ModifiedUser varchar(100),
	@FirstName varchar(50),
	@LastName varchar(50),
	@Admin bit
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
    if Exists (select * from UsersInformation UI
				where UI.UserID = @UserID)
	begin
		
		Insert Into UsersInformationLog
		Select UserID, CompanyID, FirstName, LastName, Email, Password, Salt, EncryptionType, 
		Active, Locked, needsPasswordReset, ModifiedDate, ModifiedUser
		from UsersInformation where @UserID = UserID


		Update UI
		set UI.Password = @Password, UI.Salt = @Salt, UI.EncryptionType = @EncryptionType, UI.ModifiedUser = @ModifiedUser, UI.ModifiedDate = GETDATE(),
							UI.FirstName = @FirstName, UI.LastName = @LastName
		from UsersInformation UI where UI.UserID = @UserID

		

		if (@Admin = (CAST('TRUE' as bit)))
		begin
			if Exists(Select * from UsersInformation U inner join UserRights UR on U.UserID = UR.UserID
							inner join Rights R on UR.RightID = R.RightID where U.UserID = @UserID and UR.RightID = (Select Top 1 RightID from Rights Where RightName = 'ADMIN')) 
			begin
				Update UR
				set UR.Active = 1 from UsersInformation U inner join UserRights UR on U.UserID = UR.UserID
							inner join Rights R on UR.RightID = R.RightID where U.UserID = @UserID and UR.RightID = (Select Top 1 RightID from Rights Where RightName = 'ADMIN')
			end
			else
			begin
				Insert into UserRights (UserID, RightID, CreatedBy, CreatedDate, Active)
				values (@UserID, (Select Top 1 RightID from Rights Where RightName = 'ADMIN'), @ModifiedUser, GETDate(), 1)
			end
		end
		else
		begin
			if Exists(Select * from UsersInformation U inner join UserRights UR on U.UserID = UR.UserID
							inner join Rights R on UR.RightID = R.RightID where U.UserID = @UserID and UR.RightID = (Select Top 1 RightID from Rights Where RightName = 'ADMIN')) 
			begin
				Update UR
				set UR.Active = 0 from UsersInformation U inner join UserRights UR on U.UserID = UR.UserID
							inner join Rights R on UR.RightID = R.RightID where U.UserID = @UserID and UR.RightID = (Select Top 1 RightID from Rights Where RightName = 'ADMIN')
			end
		end

		select 1 as 'Result'

	end
	else
	begin
		--User doesn't Exist
		Select -1 as 'Result'
	end
END
GO

