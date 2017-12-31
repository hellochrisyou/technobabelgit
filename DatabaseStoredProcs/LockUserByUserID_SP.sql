/*    ==Scripting Parameters==

    Source Database Engine Edition : Microsoft Azure SQL Database Edition
    Source Database Engine Type : Microsoft Azure SQL Database

    Target Database Engine Edition : Microsoft Azure SQL Database Edition
    Target Database Engine Type : Microsoft Azure SQL Database
*/

/****** Object:  StoredProcedure [dbo].[LockUserByUserID]    Script Date: 10/22/2017 12:37:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:      Bryan Robinson
-- Create Date: 10/16/17
-- Description: Lock User By User ID
-- =============================================
CREATE PROCEDURE [dbo].[LockUserByUserID]
(
    @UserID int
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
    if exists (Select * from UsersInformation UI
				where UI.UserID = @UserID)
	begin
		
		Insert Into UsersInformationLog
		Select UserID, CompanyID, FirstName, LastName, Email, Password, Salt, EncryptionType, 
							Active, Locked, needsPasswordReset, ModifiedDate, ModifiedUser
							from UsersInformation where @UserID = UserID

		UPDATE UI
		set UI.Locked = ~UI.Locked from 
		UsersInformation UI where UI.UserID = @UserID
	
		Select 1 as 'Result'
	end
	else
	begin
		Select -1 as 'Result'
	end
END
GO

