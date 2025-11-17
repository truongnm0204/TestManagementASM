# Database Migration Guide

## Multiple Choice Support Migration

### Overview
This migration adds support for Multiple Choice questions where students can select multiple correct answers.

### What Changed

#### 1. **Database Schema**
- Added UNIQUE constraint `UK_Attempt_Question_Answer` on `StudentAnswers` table
- Prevents duplicate (AttemptID, QuestionID, ChosenAnswerID) combinations
- Allows multiple StudentAnswer records for the same question (for MULTIPLE type questions)

#### 2. **Application Code**
- **TakeTestViewModel**: Changed from `Dictionary<int, int>` to `Dictionary<int, HashSet<int>>`
- **TestAttemptService**: Added `SaveStudentAnswersAsync()` method for multiple answers
- **Scoring Logic**: Updated to handle both SINGLE and MULTIPLE question types
  - SINGLE: Award full points if correct answer selected
  - MULTIPLE: All-or-nothing scoring (must select ALL correct answers, NO incorrect answers)
- **UI**: Dynamic RadioButton (SINGLE) or CheckBox (MULTIPLE) based on QuestionType

### How to Apply Migration

#### Option 1: Using SQL Server Management Studio (SSMS)
1. Open SSMS and connect to your SQL Server
2. Open the file `AddMultipleChoiceSupport.sql`
3. Execute the script
4. Verify the constraint was added:
   ```sql
   SELECT * FROM sys.indexes 
   WHERE name = 'UK_Attempt_Question_Answer' 
   AND object_id = OBJECT_ID('dbo.StudentAnswers')
   ```

#### Option 2: Using sqlcmd (Command Line)
```bash
sqlcmd -S localhost -d TestManagementDB -i AddMultipleChoiceSupport.sql
```

#### Option 3: Using Azure Data Studio
1. Open Azure Data Studio
2. Connect to your database
3. Open `AddMultipleChoiceSupport.sql`
4. Click "Run" button

### Verification

After applying the migration, verify:

1. **Check Constraint Exists**:
   ```sql
   SELECT * FROM sys.indexes 
   WHERE name = 'UK_Attempt_Question_Answer'
   ```

2. **Test Multiple Answers**:
   ```sql
   -- This should work (different answers for same question)
   INSERT INTO StudentAnswers (AttemptID, QuestionID, ChosenAnswerID)
   VALUES (1, 6, 17), (1, 6, 18), (1, 6, 20)
   
   -- This should fail (duplicate)
   INSERT INTO StudentAnswers (AttemptID, QuestionID, ChosenAnswerID)
   VALUES (1, 6, 17)  -- Error: Duplicate key
   ```

### Rollback (If Needed)

If you need to rollback this migration:

```sql
USE [TestManagementDB]
GO

-- Drop the constraint
IF EXISTS (
    SELECT 1 FROM sys.indexes 
    WHERE name = 'UK_Attempt_Question_Answer' 
    AND object_id = OBJECT_ID('dbo.StudentAnswers')
)
BEGIN
    ALTER TABLE [dbo].[StudentAnswers] 
    DROP CONSTRAINT [UK_Attempt_Question_Answer]
    
    PRINT 'Constraint dropped successfully!'
END
GO
```

### Testing the Feature

1. **Login as Teacher**
2. **Create a test with MULTIPLE type questions**:
   - Question Type: MULTIPLE
   - Add 4 answers, mark 2-3 as correct
3. **Login as Student**
4. **Take the test**:
   - SINGLE questions → RadioButton (select one)
   - MULTIPLE questions → CheckBox (select multiple)
5. **Submit and check score**:
   - SINGLE: Correct if selected the right answer
   - MULTIPLE: Correct only if selected ALL correct answers and NO incorrect answers

### Scoring Logic

#### SINGLE Choice:
- ✅ Selected correct answer → Full points
- ❌ Selected wrong answer → 0 points
- ❌ No answer selected → 0 points

#### MULTIPLE Choice (All-or-nothing):
- ✅ Selected ALL correct answers AND NO incorrect answers → Full points
- ❌ Missing any correct answer → 0 points
- ❌ Selected any incorrect answer → 0 points
- ❌ No answers selected → 0 points

### Notes

- Existing data is NOT affected by this migration
- The constraint only prevents future duplicates
- Old test attempts will continue to work
- New test attempts will support multiple answers for MULTIPLE type questions

### Support

If you encounter any issues:
1. Check SQL Server error logs
2. Verify database connection string
3. Ensure you have ALTER TABLE permissions
4. Contact the development team

