/*    ==Scripting Parameters==

    Source Database Engine Edition : Microsoft Azure SQL Database Edition
    Source Database Engine Type : Microsoft Azure SQL Database

    Target Database Engine Edition : Microsoft Azure SQL Database Edition
    Target Database Engine Type : Microsoft Azure SQL Database
*/

/****** Object:  StoredProcedure [dbo].[SelectUserByUserID]    Script Date: 10/22/2017 12:38:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Bryan Robinson
-- Create Date: 10/16/17	
-- Description: Select User By User ID
-- =============================================
CREATE PROCEDURE [dbo].[SelectUserByUserID]
(
    -- Add the parameters for the stored procedure here
    @UserID int
)
AS
BEGIN
    SET NOCOUNT ON

    IF Exists (SELECT * FROM UsersInformation UI
				WHERE UI.UserID = @UserID)
	BEGIN
		SELECT FirstName, LastName, Email, 
			CASE WHEN (EXISTS(select * from UsersInformation U inner join UserRights UR on u.UserID = ur.UserID
									where u.userID = @UserID and UR.Active = 1 and UR.RightID = (Select RightID from Rights where RightName = 'ADMIN'))) THEN 1 ELSE 0 END as 'isAdmin' 
			FROM UsersInformation UI WHERE UI.UserID = @UserID 
	END
	ELSE
	BEGIN
		SELECT 'ERROR' AS 'FirstName', 'ERROR' AS'LastName', 'ERROR' AS 'Email', '0' as 'isAdmin' 
	end
END
GO

