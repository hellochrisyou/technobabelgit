/*    ==Scripting Parameters==

    Source Database Engine Edition : Microsoft Azure SQL Database Edition
    Source Database Engine Type : Microsoft Azure SQL Database

    Target Database Engine Edition : Microsoft Azure SQL Database Edition
    Target Database Engine Type : Microsoft Azure SQL Database
*/

/****** Object:  StoredProcedure [dbo].[GetTermListByCompanyID]    Script Date: 10/22/2017 12:31:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Keith Zane
-- Create Date: 10/15/2017
-- Description: This should be used to get a list of the terms that a company has access to.
-- =============================================
CREATE PROCEDURE [dbo].[GetTermListByCompanyID]
(
    -- Add the parameters for the stored procedure here
    @CompID int
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
    if Exists (select * from CompaniesInformation where CompanyID = @CompID)
	begin
		Select TI.TermID, TI.TermName, TI.TermDescription, TI.Active From TermsInformation TI
		where TI.CompanyID = @CompID
		order by TermName 

	end
	else
	begin
		Select -1 as 'TermID', 'ERROR' as 'TermName', 'ERROR' as 'TermDescription', Cast('FALSE' as bit) as 'Active'
	end
END
GO

