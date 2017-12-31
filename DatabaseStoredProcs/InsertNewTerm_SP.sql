/*    ==Scripting Parameters==

    Source Database Engine Edition : Microsoft Azure SQL Database Edition
    Source Database Engine Type : Microsoft Azure SQL Database

    Target Database Engine Edition : Microsoft Azure SQL Database Edition
    Target Database Engine Type : Microsoft Azure SQL Database
*/

/****** Object:  StoredProcedure [dbo].[InsertNewTerm]    Script Date: 10/22/2017 12:36:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Justin Stryjewski
-- Create Date: 10/15/2017
-- Description: This stored procedure is for when and admin adds a new term.
-- =============================================
CREATE PROCEDURE [dbo].[InsertNewTerm]
(
	--Add the parameters for the stored procedure here
	@CreatedUser varchar(100),
	@compID int, 
	@TName varchar(100),
	@TDecript varchar(100)
)
AS

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
    if exists(select * from CompaniesInformation where CompanyID=@compID)
	begin
		if exists (select * from TermsInformation TI where TI.TermName = @TName
					and TI.CompanyID = @compID)
		begin
			-- The Term already exists, cannot add.
			Select -1 as 'Result'
		end
		else
		begin
			Insert Into TermsInformation(CompanyID, TermName, TermDescription, Active, VerbalHitCount, 
									ClickHitCount, ModifiedDate, ModifiedUser, CreatedDate, CreatedUser)
			values (@compID, @TName, @TDecript, 1, 0, 0, GETDATE(), @CreatedUser, GETDATE(), @CreatedUser)

			Select 1 as 'Result'
		end
	end
	else 
	begin 
		-- The Company Does Not Exist, cannot add.
		select -2 as 'Result' 
	end 
END
GO

