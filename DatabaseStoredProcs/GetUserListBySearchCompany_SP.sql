/*    ==Scripting Parameters==

    Source Database Engine Edition : Microsoft Azure SQL Database Edition
    Source Database Engine Type : Microsoft Azure SQL Database

    Target Database Engine Edition : Microsoft Azure SQL Database Edition
    Target Database Engine Type : Microsoft Azure SQL Database
*/

/****** Object:  StoredProcedure [dbo].[GetUserListBySearchCompany]    Script Date: 10/22/2017 12:34:26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[GetUserListBySearchCompany]
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

		if Exists( select * from UsersInformation where Email like @Search+'%' )
		begin
			Select UserID, Email, Active from UsersInformation where Email like @Search+'%' 
		end
		else
		begin
			Select -2 as 'UserID', 'ERROR' as 'Email', Cast('FALSE' as bit) as 'Active'
		end
	END
	else
	begin
		Select -1 as 'UserID', 'ERROR' as 'Email', Cast('FALSE' as bit) as 'Active'
	end
END
GO

