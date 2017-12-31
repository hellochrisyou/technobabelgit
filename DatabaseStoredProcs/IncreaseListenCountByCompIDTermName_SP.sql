/*    ==Scripting Parameters==

    Source Database Engine Edition : Microsoft Azure SQL Database Edition
    Source Database Engine Type : Microsoft Azure SQL Database

    Target Database Engine Edition : Microsoft Azure SQL Database Edition
    Target Database Engine Type : Microsoft Azure SQL Database
*/

/****** Object:  StoredProcedure [dbo].[IncreaseListenCountByCompIDTermName]    Script Date: 10/22/2017 12:35:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[IncreaseListenCountByCompIDTermName]
(
    -- Add the parameters for the stored procedure here
	@CompID int,
    @TermName varchar(100)
	
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
    if Exists (Select * from CompaniesInformation where CompanyID = @CompID)
	begin
		if Exists (Select * from TermsInformation TI where TI.CompanyID = @CompID and TI.TermName = @TermName)
		begin
			Update TI
			SET TI.VerbalHitCount = TI.VerbalHitCount + 1
			from TermsInformation TI where TI.CompanyID = @CompID and TI.TermName = @TermName

			select  1 as 'Result'
		end
		else
		begin
			--The Term Doesn't exist for that company.
			select -2 as 'Result'
		end
	end
	else
	begin
		-- Company Does not Exist.
		Select -1 as 'Result'
	end
END
GO

