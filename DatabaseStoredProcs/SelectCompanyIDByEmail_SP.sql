/*    ==Scripting Parameters==

    Source Database Engine Edition : Microsoft Azure SQL Database Edition
    Source Database Engine Type : Microsoft Azure SQL Database

    Target Database Engine Edition : Microsoft Azure SQL Database Edition
    Target Database Engine Type : Microsoft Azure SQL Database
*/

/****** Object:  StoredProcedure [dbo].[SelectCompanyIDByEmail]    Script Date: 10/22/2017 12:37:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[SelectCompanyIDByEmail]
(
    -- Add the parameters for the stored procedure here
    @email varchar(100)
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
	IF EXISTS (SELECT * FROM UsersInformation 
			WHERE Email = @email AND Active = '1')
	begin
		SELECT C.companyID FROM CompaniesInformation C
		INNER JOIN UsersInformation UC ON C.CompanyID = UC.CompanyID
		WHERE Email = @email
	end
	else
	begin --If User does not exist
		SELECT -1 AS 'Result'
	end
END
GO

