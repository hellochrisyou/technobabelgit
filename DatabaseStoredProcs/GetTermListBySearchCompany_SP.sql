/*    ==Scripting Parameters==

    Source Database Engine Edition : Microsoft Azure SQL Database Edition
    Source Database Engine Type : Microsoft Azure SQL Database

    Target Database Engine Edition : Microsoft Azure SQL Database Edition
    Target Database Engine Type : Microsoft Azure SQL Database
*/

/****** Object:  StoredProcedure [dbo].[GetTermListBySearchCompany]    Script Date: 10/22/2017 12:32:31 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[GetTermListBySearchCompany]
(
    -- Add the parameters for the stored procedure here
    @Search varchar(100),
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

		if Exists( select * from TermsInformation where TermName like @Search+'%' )
		begin
			Select TermID, TermName, TermDescription, Active from TermsInformation where TermName like @Search+'%' 
			order by TermName
		end
		else
		begin
			Select -2 as 'TermID', 'ERROR' as 'TermName', 'ERROR' as 'TermDescription', Cast('FALSE' as bit) as 'Active'
		end
	END
	else
	begin
		Select -1 as 'TermID', 'ERROR' as 'TermName', 'ERROR' as 'TermDescription', Cast('FALSE' as bit) as 'Active'
	end
END
GO

