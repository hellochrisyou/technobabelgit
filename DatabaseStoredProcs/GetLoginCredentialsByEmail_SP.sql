/*    ==Scripting Parameters==

    Source Database Engine Edition : Microsoft Azure SQL Database Edition
    Source Database Engine Type : Microsoft Azure SQL Database

    Target Database Engine Edition : Microsoft Azure SQL Database Edition
    Target Database Engine Type : Microsoft Azure SQL Database
*/

/****** Object:  StoredProcedure [dbo].[GetLoginCredentialsByEmail]    Script Date: 10/22/2017 12:29:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Keith Zane
-- Create Date: 10/15/17
-- Description: This will give back the company id, the company name, as well as the name of the right's that this user has.
-- =============================================
CREATE PROCEDURE [dbo].[GetLoginCredentialsByEmail]
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
    if Exists (Select * from UsersInformation UI
				where UI.Email = @Email)
	begin
		Select CI.CompanyID, CI.CompanyName, R.RightName, UI.FirstName, UI.LastName, UR.Active From UserRights UR
		inner join Rights R on UR.RightID = R.RightID
		inner join UsersInformation UI on UR.UserID = UI.UserID
		inner Join CompaniesInformation CI on CI.CompanyID = UI.CompanyID
		where UI.Email = @Email
	end
	else
	begin
		Select -1 as 'CompanyID', 'ERROR' as 'CompanyName', 'ERROR' as 'RightName', 'ERROR' as 'FirstName', 'ERROR' as 'LastName', Cast('FALSE' as bit) as 'Active'
	end
END
GO

