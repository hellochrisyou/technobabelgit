/*    ==Scripting Parameters==

    Source Database Engine Edition : Microsoft Azure SQL Database Edition
    Source Database Engine Type : Microsoft Azure SQL Database

    Target Database Engine Edition : Microsoft Azure SQL Database Edition
    Target Database Engine Type : Microsoft Azure SQL Database
*/

/****** Object:  StoredProcedure [dbo].[GetUserListByPageAndCompanyID]    Script Date: 10/22/2017 12:33:51 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
Create PROCEDURE [dbo].[GetUserListByPageAndCompanyID]
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

	declare @U table (
		UserID int,
		Email varchar(100),
		Active bit,
		RN int
	)

	insert into @U
	SELECT UsersInformation.UserID, UsersInformation.Email, UsersInformation.Active,
       row_number() OVER (ORDER BY Email) AS RN
	FROM UsersInformation
	ORDER BY Email

	if Exists (select * from CompaniesInformation where CompanyID = @CompID)
	begin

		if Exists(
		SELECT TOP 8
			UserID AS 'UserID',
			Email as 'Email',
			Active as 'Active'
		FROM @U      
		WHERE RN > (@varib*(@Page-1)) and RN <= (@varib*(@Page))
		)
		begin
			SELECT TOP 8
				UserID AS 'UserID',
				Email as 'Email',
				Active as 'Active'
			FROM @U        
			WHERE RN > (@varib*(@Page-1)) and RN <= (@varib*(@Page))
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

