/*    ==Scripting Parameters==

    Source Database Engine Edition : Microsoft Azure SQL Database Edition
    Source Database Engine Type : Microsoft Azure SQL Database

    Target Database Engine Edition : Microsoft Azure SQL Database Edition
    Target Database Engine Type : Microsoft Azure SQL Database
*/

/****** Object:  StoredProcedure [dbo].[LogErrorMessage]    Script Date: 10/22/2017 12:37:26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Keith Zane
-- Create Date: 10/15/17
-- Description: This is used to log the error messages that we get back from the technobabel application.
-- =============================================
CREATE PROCEDURE [dbo].[LogErrorMessage]
(
    -- Add the parameters for the stored procedure here
    @ErrorMessage varchar(MAX),
	@user varchar(100)
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
    insert into Errors (ErrorDescription, ErrorTime, UserName)
	values (@ErrorMessage, GETDATE(), @user)
END
GO

