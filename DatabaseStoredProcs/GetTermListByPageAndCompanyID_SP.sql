/*    ==Scripting Parameters==

    Source Database Engine Edition : Microsoft Azure SQL Database Edition
    Source Database Engine Type : Microsoft Azure SQL Database

    Target Database Engine Edition : Microsoft Azure SQL Database Edition
    Target Database Engine Type : Microsoft Azure SQL Database
*/

/****** Object:  StoredProcedure [dbo].[GetTermListByPageAndCompanyID]    Script Date: 10/22/2017 12:32:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[GetTermListByPageAndCompanyID]
(
    -- Add the parameters for the stored procedure here
    @Page int,
	@CompID int
	
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
    declare @varib int;
	set @varib = 8;

	declare @T table (
		TermID int,
		TermName varchar(100),
		TermDescription varchar(1000),
		Active bit,
		RN int
	)

	insert into @T
	SELECT TermsInformation.TermID, TermsInformation.TermName, TermsInformation.TermDescription, TermsInformation.Active, 
       row_number() OVER (ORDER BY TermName) AS RN
	FROM TermsInformation
	ORDER BY TermName

	if Exists (select * from CompaniesInformation where CompanyID = @CompID)
	begin

		if Exists(
		SELECT TOP 8
			TermID AS 'TermID',
			TermName as 'TermName',
			TermDescription as 'TermDescription',
			Active as 'Active'
		FROM @T       
		WHERE RN > (@varib*(@Page-1)) and RN <= (@varib*(@Page))
		)
		begin
			SELECT TOP 8
				TermID AS 'TermID',
				TermName as 'TermName',
				TermDescription as 'TermDescription',
				Active as 'Active'
			FROM @T       
			WHERE RN > (@varib*(@Page-1)) and RN <= (@varib*(@Page))
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

