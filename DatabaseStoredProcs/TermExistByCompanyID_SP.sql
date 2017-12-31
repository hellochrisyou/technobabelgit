/*    ==Scripting Parameters==

    Source Database Engine Edition : Microsoft Azure SQL Database Edition
    Source Database Engine Type : Microsoft Azure SQL Database

    Target Database Engine Edition : Microsoft Azure SQL Database Edition
    Target Database Engine Type : Microsoft Azure SQL Database
*/

/****** Object:  StoredProcedure [dbo].[TermExistByCompanyID]    Script Date: 10/22/2017 12:39:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Bryan Robinson
-- Create Date: 10/14/17
-- Description: Select Term By CompanyID
-- =============================================
CREATE PROCEDURE [dbo].[TermExistByCompanyID]
(
	@term varchar(100),
    @compID int    
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON
	
	IF EXISTS (SELECT * FROM TermsInformation TI
			WHERE CompanyID = @compID and TI.TermName = @term and TI.Active = '1')
	begin
		SELECT 1 AS 'Result'
	end
	else
	begin --If User does not exist
		SELECT 0 AS 'Result'
	end
   
    
END
GO

