USE [master]
GO
/****** Object:  Database [TestManagementDB]    Script Date: 11/17/2025 12:59:27 AM ******/
CREATE DATABASE [TestManagementDB]
 
USE [TestManagementDB]
GO
/****** Object:  Table [dbo].[Answers]    Script Date: 11/17/2025 12:59:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Answers](
	[AnswerID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionID] [int] NOT NULL,
	[AnswerText] [nvarchar](1000) NOT NULL,
	[IsCorrect] [bit] NOT NULL,
	[Feedback] [nvarchar](1000) NULL,
PRIMARY KEY CLUSTERED 
(
	[AnswerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Classes]    Script Date: 11/17/2025 12:59:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Classes](
	[ClassID] [int] IDENTITY(1,1) NOT NULL,
	[SubjectID] [int] NOT NULL,
	[ClassName] [nvarchar](255) NOT NULL,
	[Semester] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ClassID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Enrollments]    Script Date: 11/17/2025 12:59:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Enrollments](
	[EnrollmentID] [int] IDENTITY(1,1) NOT NULL,
	[StudentID] [int] NOT NULL,
	[ClassID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[EnrollmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Questions]    Script Date: 11/17/2025 12:59:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Questions](
	[QuestionID] [int] IDENTITY(1,1) NOT NULL,
	[SubjectID] [int] NOT NULL,
	[CreatedBy_TeacherID] [int] NULL,
	[QuestionText] [nvarchar](max) NOT NULL,
	[QuestionType] [varchar](10) NOT NULL,
	[DifficultyLevel] [int] NULL,
	[Chapter] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[QuestionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 11/17/2025 12:59:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentAnswers]    Script Date: 11/17/2025 12:59:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentAnswers](
	[StudentAnswerID] [int] IDENTITY(1,1) NOT NULL,
	[AttemptID] [int] NOT NULL,
	[QuestionID] [int] NOT NULL,
	[ChosenAnswerID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[StudentAnswerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subjects]    Script Date: 11/17/2025 12:59:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subjects](
	[SubjectID] [int] IDENTITY(1,1) NOT NULL,
	[SubjectCode] [nvarchar](20) NOT NULL,
	[SubjectName] [nvarchar](255) NOT NULL,
	[CreatedBy_UserID] [int] NULL,
	[Status] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SubjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TeachingAssignments]    Script Date: 11/17/2025 12:59:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeachingAssignments](
	[AssignmentID] [int] IDENTITY(1,1) NOT NULL,
	[TeacherID] [int] NOT NULL,
	[ClassID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AssignmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TestAttempts]    Script Date: 11/17/2025 12:59:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestAttempts](
	[AttemptID] [int] IDENTITY(1,1) NOT NULL,
	[StudentID] [int] NOT NULL,
	[TestID] [int] NOT NULL,
	[StartTime] [datetime2](7) NOT NULL,
	[EndTime] [datetime2](7) NULL,
	[Score] [int] NULL,
	[AttemptStatus] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AttemptID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TestQuestions]    Script Date: 11/17/2025 12:59:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestQuestions](
	[TestQuestionID] [int] IDENTITY(1,1) NOT NULL,
	[TestID] [int] NOT NULL,
	[QuestionID] [int] NOT NULL,
	[Points] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TestQuestionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tests]    Script Date: 11/17/2025 12:59:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tests](
	[TestID] [int] IDENTITY(1,1) NOT NULL,
	[ClassID] [int] NOT NULL,
	[CreatedBy_TeacherID] [int] NOT NULL,
	[TestName] [nvarchar](255) NOT NULL,
	[DurationMinutes] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[AvailableFrom] [datetime2](7) NULL,
	[AvailableTo] [datetime2](7) NULL,
	[CreatedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[TestID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 11/17/2025 12:59:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[PasswordHash] [nvarchar](max) NOT NULL,
	[FullName] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NULL,
	[RoleID] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[CreatedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 
GO
INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (1, N'Admin')
GO
INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (3, N'Student')
GO
INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (2, N'Teacher')
GO
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [FullName], [Email], [RoleID], [Status], [CreatedAt]) VALUES (1, N'admin', N'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', N'Quản Trị Viên', NULL, 1, 1, CAST(N'2025-10-23T08:58:17.1833333' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
/****** Object:  Index [UK_Student_Class]    Script Date: 11/17/2025 12:59:27 AM ******/
ALTER TABLE [dbo].[Enrollments] ADD  CONSTRAINT [UK_Student_Class] UNIQUE NONCLUSTERED 
(
	[StudentID] ASC,
	[ClassID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Roles__8A2B616028740FD8]    Script Date: 11/17/2025 12:59:27 AM ******/
ALTER TABLE [dbo].[Roles] ADD UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Subjects__9F7CE1A922430F2E]    Script Date: 11/17/2025 12:59:27 AM ******/
ALTER TABLE [dbo].[Subjects] ADD UNIQUE NONCLUSTERED 
(
	[SubjectCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UK_Teacher_Class]    Script Date: 11/17/2025 12:59:27 AM ******/
ALTER TABLE [dbo].[TeachingAssignments] ADD  CONSTRAINT [UK_Teacher_Class] UNIQUE NONCLUSTERED 
(
	[TeacherID] ASC,
	[ClassID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UK_Test_Question]    Script Date: 11/17/2025 12:59:27 AM ******/
ALTER TABLE [dbo].[TestQuestions] ADD  CONSTRAINT [UK_Test_Question] UNIQUE NONCLUSTERED 
(
	[TestID] ASC,
	[QuestionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__536C85E4826B54A9]    Script Date: 11/17/2025 12:59:27 AM ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__A9D10534C8F2A360]    Script Date: 11/17/2025 12:59:27 AM ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Answers] ADD  DEFAULT ((0)) FOR [IsCorrect]
GO
ALTER TABLE [dbo].[Questions] ADD  DEFAULT ((1)) FOR [DifficultyLevel]
GO
ALTER TABLE [dbo].[Subjects] ADD  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[TestAttempts] ADD  DEFAULT (getdate()) FOR [StartTime]
GO
ALTER TABLE [dbo].[TestAttempts] ADD  DEFAULT ('InProgress') FOR [AttemptStatus]
GO
ALTER TABLE [dbo].[TestQuestions] ADD  DEFAULT ((1)) FOR [Points]
GO
ALTER TABLE [dbo].[Tests] ADD  DEFAULT ((60)) FOR [DurationMinutes]
GO
ALTER TABLE [dbo].[Tests] ADD  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Tests] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Answers]  WITH CHECK ADD  CONSTRAINT [FK_Answers_Questions] FOREIGN KEY([QuestionID])
REFERENCES [dbo].[Questions] ([QuestionID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Answers] CHECK CONSTRAINT [FK_Answers_Questions]
GO
ALTER TABLE [dbo].[Classes]  WITH CHECK ADD  CONSTRAINT [FK_Classes_Subjects] FOREIGN KEY([SubjectID])
REFERENCES [dbo].[Subjects] ([SubjectID])
GO
ALTER TABLE [dbo].[Classes] CHECK CONSTRAINT [FK_Classes_Subjects]
GO
ALTER TABLE [dbo].[Enrollments]  WITH CHECK ADD  CONSTRAINT [FK_Enrollments_Classes] FOREIGN KEY([ClassID])
REFERENCES [dbo].[Classes] ([ClassID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Enrollments] CHECK CONSTRAINT [FK_Enrollments_Classes]
GO
ALTER TABLE [dbo].[Enrollments]  WITH CHECK ADD  CONSTRAINT [FK_Enrollments_Users_Student] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Enrollments] CHECK CONSTRAINT [FK_Enrollments_Users_Student]
GO
ALTER TABLE [dbo].[Questions]  WITH CHECK ADD  CONSTRAINT [FK_Questions_Subjects] FOREIGN KEY([SubjectID])
REFERENCES [dbo].[Subjects] ([SubjectID])
GO
ALTER TABLE [dbo].[Questions] CHECK CONSTRAINT [FK_Questions_Subjects]
GO
ALTER TABLE [dbo].[Questions]  WITH CHECK ADD  CONSTRAINT [FK_Questions_Users_Teacher] FOREIGN KEY([CreatedBy_TeacherID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Questions] CHECK CONSTRAINT [FK_Questions_Users_Teacher]
GO
ALTER TABLE [dbo].[StudentAnswers]  WITH CHECK ADD  CONSTRAINT [FK_StudentAnswers_Answers] FOREIGN KEY([ChosenAnswerID])
REFERENCES [dbo].[Answers] ([AnswerID])
GO
ALTER TABLE [dbo].[StudentAnswers] CHECK CONSTRAINT [FK_StudentAnswers_Answers]
GO
ALTER TABLE [dbo].[StudentAnswers]  WITH CHECK ADD  CONSTRAINT [FK_StudentAnswers_Questions] FOREIGN KEY([QuestionID])
REFERENCES [dbo].[Questions] ([QuestionID])
GO
ALTER TABLE [dbo].[StudentAnswers] CHECK CONSTRAINT [FK_StudentAnswers_Questions]
GO
ALTER TABLE [dbo].[StudentAnswers]  WITH CHECK ADD  CONSTRAINT [FK_StudentAnswers_TestAttempts] FOREIGN KEY([AttemptID])
REFERENCES [dbo].[TestAttempts] ([AttemptID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StudentAnswers] CHECK CONSTRAINT [FK_StudentAnswers_TestAttempts]
GO
ALTER TABLE [dbo].[Subjects]  WITH CHECK ADD  CONSTRAINT [FK_Subjects_Users] FOREIGN KEY([CreatedBy_UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Subjects] CHECK CONSTRAINT [FK_Subjects_Users]
GO
ALTER TABLE [dbo].[TeachingAssignments]  WITH CHECK ADD  CONSTRAINT [FK_Assignments_Classes] FOREIGN KEY([ClassID])
REFERENCES [dbo].[Classes] ([ClassID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TeachingAssignments] CHECK CONSTRAINT [FK_Assignments_Classes]
GO
ALTER TABLE [dbo].[TeachingAssignments]  WITH CHECK ADD  CONSTRAINT [FK_Assignments_Users_Teacher] FOREIGN KEY([TeacherID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[TeachingAssignments] CHECK CONSTRAINT [FK_Assignments_Users_Teacher]
GO
ALTER TABLE [dbo].[TestAttempts]  WITH CHECK ADD  CONSTRAINT [FK_TestAttempts_Tests] FOREIGN KEY([TestID])
REFERENCES [dbo].[Tests] ([TestID])
GO
ALTER TABLE [dbo].[TestAttempts] CHECK CONSTRAINT [FK_TestAttempts_Tests]
GO
ALTER TABLE [dbo].[TestAttempts]  WITH CHECK ADD  CONSTRAINT [FK_TestAttempts_Users_Student] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[TestAttempts] CHECK CONSTRAINT [FK_TestAttempts_Users_Student]
GO
ALTER TABLE [dbo].[TestQuestions]  WITH CHECK ADD  CONSTRAINT [FK_TestQuestions_Questions] FOREIGN KEY([QuestionID])
REFERENCES [dbo].[Questions] ([QuestionID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TestQuestions] CHECK CONSTRAINT [FK_TestQuestions_Questions]
GO
ALTER TABLE [dbo].[TestQuestions]  WITH CHECK ADD  CONSTRAINT [FK_TestQuestions_Tests] FOREIGN KEY([TestID])
REFERENCES [dbo].[Tests] ([TestID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TestQuestions] CHECK CONSTRAINT [FK_TestQuestions_Tests]
GO
ALTER TABLE [dbo].[Tests]  WITH CHECK ADD  CONSTRAINT [FK_Tests_Classes] FOREIGN KEY([ClassID])
REFERENCES [dbo].[Classes] ([ClassID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Tests] CHECK CONSTRAINT [FK_Tests_Classes]
GO
ALTER TABLE [dbo].[Tests]  WITH CHECK ADD  CONSTRAINT [FK_Tests_Users_Teacher] FOREIGN KEY([CreatedBy_TeacherID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Tests] CHECK CONSTRAINT [FK_Tests_Users_Teacher]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles]
GO
ALTER TABLE [dbo].[Questions]  WITH CHECK ADD  CONSTRAINT [CHK_QuestionType_Main] CHECK  (([QuestionType]='MULTIPLE' OR [QuestionType]='SINGLE'))
GO
ALTER TABLE [dbo].[Questions] CHECK CONSTRAINT [CHK_QuestionType_Main]
GO
ALTER TABLE [dbo].[TestAttempts]  WITH CHECK ADD  CONSTRAINT [CHK_AttemptStatus] CHECK  (([AttemptStatus]='Completed' OR [AttemptStatus]='InProgress'))
GO
ALTER TABLE [dbo].[TestAttempts] CHECK CONSTRAINT [CHK_AttemptStatus]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [CHK_UserStatus] CHECK  (([Status]=(2) OR [Status]=(1) OR [Status]=(0)))
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [CHK_UserStatus]
GO
USE [master]
GO
ALTER DATABASE [TestManagementDB] SET  READ_WRITE 
GO
-- TEACHERS
INSERT INTO Users (Username, PasswordHash, FullName, Email, RoleID, Status)
VALUES 
('teacher1', 'HASH_TEACHER1', N'Nguyễn Văn A', 'a.teacher@example.com', 2, 1),
('teacher2', 'HASH_TEACHER2', N'Trần Thị B', 'b.teacher@example.com', 2, 1);

-- STUDENTS
INSERT INTO Users (Username, PasswordHash, FullName, Email, RoleID, Status)
VALUES
('student1', 'HASH_STUDENT1', N'Lê Minh C', 'c.student@example.com', 3, 1),
('student2', 'HASH_STUDENT2', N'Phạm Thị D', 'd.student@example.com', 3, 1),
('student3', 'HASH_STUDENT3', N'Hoàng Văn E', 'e.student@example.com', 3, 1);
INSERT INTO Subjects (SubjectCode, SubjectName, CreatedBy_UserID, Status)
VALUES
('PR101', N'Lập Trình Cơ Bản', 1, 1),
('DB201', N'Cơ Sở Dữ Liệu', 1, 1);
INSERT INTO Classes (SubjectID, ClassName, Semester)
VALUES
(1, N'PR101 - Nhóm 1', 'Fall 2025'),
(2, N'DB201 - Nhóm 1', 'Fall 2025');
INSERT INTO TeachingAssignments (TeacherID, ClassID)
VALUES
(2, 1),
(3, 2);
INSERT INTO Enrollments (StudentID, ClassID)
VALUES
(4, 1), -- student1 vào PR101
(5, 1), -- student2 vào PR101
(6, 2); -- student3 vào DB201
-- PR101 QUESTIONS (Created by teacher1)
INSERT INTO Questions (SubjectID, CreatedBy_TeacherID, QuestionText, QuestionType, DifficultyLevel, Chapter)
VALUES
(1, 2, N'Câu lệnh để in ra màn hình trong C là gì?', 'SINGLE', 1, 1),
(1, 2, N'Kiểu dữ liệu nào để lưu số nguyên?', 'SINGLE', 1, 1);

-- DB201 QUESTIONS (Created by teacher2)
INSERT INTO Questions (SubjectID, CreatedBy_TeacherID, QuestionText, QuestionType, DifficultyLevel, Chapter)
VALUES
(2, 3, N'Câu lệnh nào dùng để lấy dữ liệu trong SQL?', 'SINGLE', 1, 1),
(2, 3, N'Ràng buộc khóa chính được dùng để làm gì?', 'SINGLE', 2, 2);
-- Q1
INSERT INTO Answers (QuestionID, AnswerText, IsCorrect) VALUES
(1, N'printf()', 1),
(1, N'cin>>', 0),
(1, N'scan()', 0);

-- Q2
INSERT INTO Answers (QuestionID, AnswerText, IsCorrect) VALUES
(2, N'int', 1),
(2, N'string', 0),
(2, N'bool', 0);

-- Q3
INSERT INTO Answers (QuestionID, AnswerText, IsCorrect) VALUES
(3, N'SELECT', 1),
(3, N'INSERT', 0),
(3, N'UPDATE', 0);

-- Q4
INSERT INTO Answers (QuestionID, AnswerText, IsCorrect) VALUES
(4, N'Xác định duy nhất mỗi bản ghi', 1),
(4, N'Lưu trữ giá trị NULL', 0),
(4, N'Giúp cập nhật dữ liệu nhanh hơn', 0);
INSERT INTO Tests (ClassID, CreatedBy_TeacherID, TestName, DurationMinutes, IsActive)
VALUES
(1, 2, N'Kiểm tra giữa kỳ PR101', 45, 1),
(2, 3, N'Kiểm tra cuối kỳ DB201', 60, 1);
-- Test 1 (PR101)
INSERT INTO TestQuestions (TestID, QuestionID, Points)
VALUES
(1, 1, 1),
(1, 2, 1);

-- Test 2 (DB201)
INSERT INTO TestQuestions (TestID, QuestionID, Points)
VALUES
(2, 3, 1),
(2, 4, 1);
INSERT INTO TestAttempts (StudentID, TestID, StartTime, EndTime, Score, AttemptStatus)
VALUES
(4, 1, GETDATE(), GETDATE(), 2, 'Completed'),
(5, 1, GETDATE(), GETDATE(), 1, 'Completed'),
(6, 2, GETDATE(), NULL, NULL, 'InProgress');
-- Student 1 - Test 1
INSERT INTO StudentAnswers (AttemptID, QuestionID, ChosenAnswerID)
VALUES
(1, 1, 1),
(1, 2, 4);

-- Student 2 - Test 1
INSERT INTO StudentAnswers (AttemptID, QuestionID, ChosenAnswerID)
VALUES
(2, 1, 2),
(2, 2, 4);
