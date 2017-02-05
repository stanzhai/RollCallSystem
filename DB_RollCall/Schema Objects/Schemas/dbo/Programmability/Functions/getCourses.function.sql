/*-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	获取一个班级的所有课程
-- =============================================
CREATE FUNCTION [dbo].[getCourses] 
(	
	@ClassID int
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT ID,CourseName FROM Course WHERE ClassID=@ClassID
)*/