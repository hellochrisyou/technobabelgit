/*    ==Scripting Parameters==

    Source Database Engine Edition : Microsoft Azure SQL Database Edition
    Source Database Engine Type : Microsoft Azure SQL Database

    Target Database Engine Edition : Microsoft Azure SQL Database Edition
    Target Database Engine Type : Microsoft Azure SQL Database
*/

/****** Object:  StoredProcedure [dbo].[GetTermIDByTermNameCompID]    Script Date: 10/22/2017 12:31:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[GetTermIDByTermNameCompID]
(
    -- Add the parameters for the stored procedure here
    @TermName varchar(100),
	@CompID int
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
    if Exists(Select * from CompaniesInformation where CompanyID = @CompID)
	begin
		if Exists (select * from TermsInformation TI
					where TI.TermName = @TermName and TI.CompanyID = @CompID and TI.Active = (CAST('TRUE' as bit)))
		begin
			select TI.TermID from TermsInformation TI
					where TI.TermName = @TermName and TI.CompanyID = @CompID and TI.Active = (CAST('TRUE' as bit))
		end
		else
		begin
			select -2 as 'TermID'
		end

    end
	else
	begin
		select -1 as 'TermID'
	end
END
GO

