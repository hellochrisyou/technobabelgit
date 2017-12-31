/*    ==Scripting Parameters==

    Source Database Engine Edition : Microsoft Azure SQL Database Edition
    Source Database Engine Type : Microsoft Azure SQL Database

    Target Database Engine Edition : Microsoft Azure SQL Database Edition
    Target Database Engine Type : Microsoft Azure SQL Database
*/

/****** Object:  StoredProcedure [dbo].[UpdateTermByTermID]    Script Date: 10/22/2017 12:39:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[UpdateTermByTermID]
(
    -- Add the parameters for the stored procedure here
    @TermID int,
	@TDescription varchar(1000),
	@ModifiedUser varchar(100)
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
    if Exists (select * from TermsInformation TI
				where TI.TermID = @TermID)
	begin

		Insert Into TermsInformationLog
		Select TermID, CompanyID, TermName, TermDescription, Active, ModifiedDate, ModifiedUser
		from TermsInformation where TermID = @TermID

		Update TI
		set TI.TermDescription = @TDescription, TI.ModifiedDate = GETDATE(), TI.ModifiedUser = @ModifiedUser 
		from TermsInformation TI where TI.TermID = @TermID

		select 1 as 'Result'

	end
	else
	begin
		--Term doesn't Exist
		Select -1 as 'Result'
	end
END
GO

