/*    ==Scripting Parameters==

    Source Database Engine Edition : Microsoft Azure SQL Database Edition
    Source Database Engine Type : Microsoft Azure SQL Database

    Target Database Engine Edition : Microsoft Azure SQL Database Edition
    Target Database Engine Type : Microsoft Azure SQL Database
*/

/****** Object:  StoredProcedure [dbo].[SelectTermByTermID]    Script Date: 10/22/2017 12:38:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[SelectTermByTermID]
(
    -- Add the parameters for the stored procedure here
    @TermID int
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
    if Exists (Select * from TermsInformation TI 
				where TI.TermID = @TermID)
	begin
		Select TermName, TermDescription From TermsInformation TI where TI.TermID = @TermID 
	end
	else
	begin
		Select 'ERROR' as 'TermName', 'ERROR' as 'TermDescription'
	end
END
GO

