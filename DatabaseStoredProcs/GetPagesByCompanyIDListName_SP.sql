/*    ==Scripting Parameters==

    Source Database Engine Edition : Microsoft Azure SQL Database Edition
    Source Database Engine Type : Microsoft Azure SQL Database

    Target Database Engine Edition : Microsoft Azure SQL Database Edition
    Target Database Engine Type : Microsoft Azure SQL Database
*/

/****** Object:  StoredProcedure [dbo].[GetPagesByCompanyIDListName]    Script Date: 10/22/2017 12:30:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[GetPagesByCompanyIDListName]
(
    -- Add the parameters for the stored procedure here
    @compID int,
	@listname varchar(100)
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON
	declare @countOfItems int
    -- Insert statements for procedure here
    if exists(select * from CompaniesInformation where CompanyID = @compID)
	begin
		if @listname = 'userlist'
		begin
			set @countOfItems = (select Count(userid)  from UsersInformation where CompanyID = @compID group by CompanyID)
			if @countOfItems > 0
			begin
				select ((@countOfItems-1)/8) + 1 as 'Result'
			end
			else
			begin
				select -2 as 'Result'
			end
		end
		else if @listname = 'termlist'
		begin
			set @countOfItems = (select Count(TermID)  from TermsInformation where CompanyID = @compID group by CompanyID)
			if @countOfItems > 0
			begin
				select ((@countOfItems-1)/8) + 1 as 'Result'
			end
			else
			begin
				select -2 as 'Result'
			end
		end
		else
		begin
			select -3 as 'Result'
		end
	end
	else
	begin
		select -1 as 'Result'
	end
END
GO

