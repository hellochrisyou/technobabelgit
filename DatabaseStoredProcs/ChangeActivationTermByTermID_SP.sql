/*    ==Scripting Parameters==

    Source Database Engine Edition : Microsoft Azure SQL Database Edition
    Source Database Engine Type : Microsoft Azure SQL Database

    Target Database Engine Edition : Microsoft Azure SQL Database Edition
    Target Database Engine Type : Microsoft Azure SQL Database
*/

/****** Object:  StoredProcedure [dbo].[ChangeActivationTermByTermID]    Script Date: 10/22/2017 12:25:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[ChangeActivationTermByTermID]
(
    @TermID int,
	@User varchar(100)
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
    if exists (Select * from TermsInformation T where T.TermID = @TermID)
	begin
		Insert Into TermsInformationLog
		Select TermID, CompanyID, TermName, TermDescription, Active, ModifiedDate, ModifiedUser
		from TermsInformation where TermID = @TermID
		
		UPDATE TI
		set TI.Active = ~TI.Active, TI.ModifiedDate = GETDATE(), TI.ModifiedUser = @User from 
		TermsInformation TI where TI.TermID = @TermID
	
		Select 1 as 'Result'
	end
	else
	begin
		Select -1 as 'Result'
	end
END
GO

