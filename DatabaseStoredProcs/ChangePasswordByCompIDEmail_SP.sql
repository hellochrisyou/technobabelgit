/*    ==Scripting Parameters==

    Source Database Engine Edition : Microsoft Azure SQL Database Edition
    Source Database Engine Type : Microsoft Azure SQL Database

    Target Database Engine Edition : Microsoft Azure SQL Database Edition
    Target Database Engine Type : Microsoft Azure SQL Database
*/

/****** Object:  StoredProcedure [dbo].[ChangePasswordByCompIDEmail]    Script Date: 10/22/2017 12:28:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[ChangePasswordByCompIDEmail]
(
    -- Add the parameters for the stored procedure here
    @CompID int,
	@Email varchar(100),
	@pass varchar(64),
	@salt varchar(64),
	@UserEmail varchar(100)
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
    If exists(select * from CompaniesInformation where CompanyID = @CompID)
	begin
		If Exists (select * from UsersInformation where CompanyID = @CompID and Email = @Email)
		begin
			Insert into UsersInformationLog
			Select UserID, CompanyID, FirstName, LastName, Email, Password, Salt, EncryptionType, 
			Active, Locked, needsPasswordReset, ModifiedDate, ModifiedUser from UsersInformation where CompanyID = @CompID and Email = @Email

			Update UI
			set UI.Password = @pass, UI.salt = @salt, UI.ModifiedDate = GETDATE(), UI.ModifiedUser = @UserEmail
			from UsersInformation UI where Ui.CompanyID = @CompID and Ui.Email = @Email

			select 1 as 'Result'
		end
		else
		begin
			select -2 as 'Result'
		end
	end
	else
	begin
		select -1 as 'Result'
	end
END
GO

