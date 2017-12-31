/*    ==Scripting Parameters==

    Source Database Engine Edition : Microsoft Azure SQL Database Edition
    Source Database Engine Type : Microsoft Azure SQL Database

    Target Database Engine Edition : Microsoft Azure SQL Database Edition
    Target Database Engine Type : Microsoft Azure SQL Database
*/

/****** Object:  StoredProcedure [dbo].[SelectUsersByCompanyID]    Script Date: 10/22/2017 12:38:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Justin Stryjewski
-- Create Date: 10/142017
-- Description: This stored procedure is used to return the emails and ID's of company users for a specific company. 
--							
-- =============================================
CREATE PROCEDURE [dbo].[SelectUsersByCompanyID]
(
    @compID int
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
   If Exists(Select * From CompaniesInformation Where CompanyID=@compID)
	begin
		Select  UI.UserID, Email, UI.Active from UsersInformation UI
		inner join CompaniesInformation CI on CI.CompanyID = UI.CompanyID
		where CI.CompanyID = @compID

	end
	else
	begin
		-- This company id does not exist in the database.
		Select -1 as 'UserID', 'ERROR' as Email, 0 as 'Active'
	end
END
GO

