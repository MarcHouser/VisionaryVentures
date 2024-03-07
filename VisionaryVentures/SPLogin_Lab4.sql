SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Elliott Samuel Tulchinsky
-- Create date: 2/29/2024
-- Description:	Stored Procedure to login into our ASP.NET Razor Page for Lab3
-- =============================================
CREATE PROCEDURE sp_Lab4Login
	@Username AS NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT COUNT(*)
	FROM HashedCredentials
	WHERE Username = @Username
END;
GO
