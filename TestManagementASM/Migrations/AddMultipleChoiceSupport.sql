-- =============================================
-- Migration: Add Multiple Choice Support
-- Date: 2025-11-17
-- Description: Add UNIQUE constraint to StudentAnswers table
--              to support multiple answers per question
-- =============================================

USE [TestManagementDB]
GO

-- Check if constraint already exists
IF NOT EXISTS (
    SELECT 1 
    FROM sys.indexes 
    WHERE name = 'UK_Attempt_Question_Answer' 
    AND object_id = OBJECT_ID('dbo.StudentAnswers')
)
BEGIN
    PRINT 'Adding UNIQUE constraint UK_Attempt_Question_Answer...'
    
    ALTER TABLE [dbo].[StudentAnswers] 
    ADD CONSTRAINT [UK_Attempt_Question_Answer] UNIQUE NONCLUSTERED 
    (
        [AttemptID] ASC,
        [QuestionID] ASC,
        [ChosenAnswerID] ASC
    )
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, 
          IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, 
          ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) 
    ON [PRIMARY]
    
    PRINT 'Constraint added successfully!'
END
ELSE
BEGIN
    PRINT 'Constraint UK_Attempt_Question_Answer already exists. Skipping...'
END
GO

PRINT 'Migration completed successfully!'
GO

