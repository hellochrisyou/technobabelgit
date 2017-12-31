/*    ==Scripting Parameters==

    Source Database Engine Edition : Microsoft Azure SQL Database Edition
    Source Database Engine Type : Microsoft Azure SQL Database

    Target Database Engine Edition : Microsoft Azure SQL Database Edition
    Target Database Engine Type : Microsoft Azure SQL Database
*/

/****** Object:  StoredProcedure [dbo].[GetTopTenClickedTermsByCompID]    Script Date: 10/22/2017 12:32:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[GetTopTenClickedTermsByCompID]
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
    if Exists ( Select * from CompaniesInformation where CompanyID = @CompID)
	begin
		Select TOP 10 TI.TermID, TI.TermName, TI.VerbalHitCount, TI.ClickHitCount 
				From TermsInformation TI where TI.CompanyID = @CompID
				order by ClickHitCount DESC
	end
	else
	begin
		Select -1 as 'TermID', 'ERROR' as 'TermName', '-1' as 'VerbalHitCount', '-1' as 'ClickHitCount' 
	end
END
GO

