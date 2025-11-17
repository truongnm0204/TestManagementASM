USE [master]
GO
/****** Object:  Database [TestManagementDB]    Script Date: 11/17/2025 5:35:13 PM ******/
CREATE DATABASE [TestManagementDB]

USE [TestManagementDB]
GO
/****** Object:  Table [dbo].[Answers]    Script Date: 11/17/2025 5:35:13 PM ******/
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
/****** Object:  Table [dbo].[Classes]    Script Date: 11/17/2025 5:35:13 PM ******/
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
/****** Object:  Table [dbo].[Enrollments]    Script Date: 11/17/2025 5:35:13 PM ******/
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
/****** Object:  Table [dbo].[Questions]    Script Date: 11/17/2025 5:35:13 PM ******/
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
/****** Object:  Table [dbo].[Roles]    Script Date: 11/17/2025 5:35:13 PM ******/
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
/****** Object:  Table [dbo].[StudentAnswers]    Script Date: 11/17/2025 5:35:13 PM ******/
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
/****** Object:  Table [dbo].[Subjects]    Script Date: 11/17/2025 5:35:13 PM ******/
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
/****** Object:  Table [dbo].[TeachingAssignments]    Script Date: 11/17/2025 5:35:13 PM ******/
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
/****** Object:  Table [dbo].[TestAttempts]    Script Date: 11/17/2025 5:35:13 PM ******/
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
/****** Object:  Table [dbo].[TestQuestions]    Script Date: 11/17/2025 5:35:13 PM ******/
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
/****** Object:  Table [dbo].[Tests]    Script Date: 11/17/2025 5:35:13 PM ******/
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
/****** Object:  Table [dbo].[Users]    Script Date: 11/17/2025 5:35:13 PM ******/
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
SET IDENTITY_INSERT [dbo].[Answers] ON 
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (1, 1, N'printf()', 1, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (2, 1, N'cin>>', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (3, 1, N'scan()', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (4, 2, N'int', 1, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (5, 2, N'string', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (6, 2, N'bool', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (7, 3, N'SELECT', 1, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (8, 3, N'INSERT', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (9, 3, N'UPDATE', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (10, 4, N'Xác định duy nhất mỗi bản ghi', 1, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (11, 4, N'Lưu trữ giá trị NULL', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (12, 4, N'Giúp cập nhật dữ liệu nhanh hơn', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (13, 5, N'Ngôn ngữ hướng đối tượng', 1, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (14, 5, N'Ngôn ngữ kiểu thủ tục', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (15, 5, N'Ngôn ngữ đánh dấu', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (16, 5, N'Ngôn ngữ kịch bản', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (17, 6, N'Kế thừa', 1, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (18, 6, N'Đa hình', 1, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (19, 6, N'Định tuyến', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (20, 6, N'Đóng gói', 1, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (21, 7, N'extends', 1, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (22, 7, N'implements', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (23, 7, N'super', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (24, 7, N'this', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (25, 8, N'Không thể tạo object trực tiếp', 1, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (26, 8, N'Các phương thức mặc định là abstract', 1, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (27, 8, N'Có thể có constructor', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (28, 8, N'Hỗ trợ đa kế thừa kiểu interface', 1, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (29, 9, N'Khởi tạo đối tượng', 1, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (30, 9, N'Hủy đối tượng', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (31, 9, N'Lấy dữ liệu từ DB', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (32, 9, N'Thông báo lỗi', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (33, 10, N'Khai báo trong class', 1, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (34, 10, N'Khai báo trong method', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (35, 10, N'Khai báo ngoài file', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (36, 10, N'Không cần khai báo', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (37, 11, N'Override thay đổi hành vi method', 1, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (38, 11, N'Overload trùng tên khác tham số', 1, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (39, 11, N'Override là đa hình tĩnh', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (40, 11, N'Overload là đa hình động', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (41, 12, N'Tổ chức các lớp theo nhóm', 1, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (42, 12, N'Tạo biến toàn cục', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (43, 12, N'Tự động tối ưu hóa code', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (44, 12, N'Tạo project mới', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (45, 13, N'Không thể ghi đè', 1, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (46, 13, N'Biến không thể thay đổi giá trị', 1, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (47, 13, N'Cho phép kế thừa đa lớp', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (48, 13, N'Chỉ áp dụng cho phương thức', 0, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (49, 14, N'List', 1, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (50, 14, N'Map', 1, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (51, 14, N'Queue', 1, NULL)
GO
INSERT [dbo].[Answers] ([AnswerID], [QuestionID], [AnswerText], [IsCorrect], [Feedback]) VALUES (52, 14, N'Table', 0, NULL)
GO
SET IDENTITY_INSERT [dbo].[Answers] OFF
GO
SET IDENTITY_INSERT [dbo].[Classes] ON 
GO
INSERT [dbo].[Classes] ([ClassID], [SubjectID], [ClassName], [Semester]) VALUES (1, 1, N'PR101 - Nhóm 1', N'Fall 2025')
GO
INSERT [dbo].[Classes] ([ClassID], [SubjectID], [ClassName], [Semester]) VALUES (2, 2, N'DB201 - Nhóm 1', N'Fall 2025')
GO
SET IDENTITY_INSERT [dbo].[Classes] OFF
GO
SET IDENTITY_INSERT [dbo].[Enrollments] ON 
GO
INSERT [dbo].[Enrollments] ([EnrollmentID], [StudentID], [ClassID]) VALUES (1, 4, 1)
GO
INSERT [dbo].[Enrollments] ([EnrollmentID], [StudentID], [ClassID]) VALUES (2, 5, 1)
GO
INSERT [dbo].[Enrollments] ([EnrollmentID], [StudentID], [ClassID]) VALUES (3, 6, 2)
GO
SET IDENTITY_INSERT [dbo].[Enrollments] OFF
GO
SET IDENTITY_INSERT [dbo].[Questions] ON 
GO
INSERT [dbo].[Questions] ([QuestionID], [SubjectID], [CreatedBy_TeacherID], [QuestionText], [QuestionType], [DifficultyLevel], [Chapter]) VALUES (1, 1, 2, N'Câu lệnh để in ra màn hình trong C là gì?', N'SINGLE', 1, 1)
GO
INSERT [dbo].[Questions] ([QuestionID], [SubjectID], [CreatedBy_TeacherID], [QuestionText], [QuestionType], [DifficultyLevel], [Chapter]) VALUES (2, 1, 2, N'Kiểu dữ liệu nào để lưu số nguyên?', N'SINGLE', 1, 1)
GO
INSERT [dbo].[Questions] ([QuestionID], [SubjectID], [CreatedBy_TeacherID], [QuestionText], [QuestionType], [DifficultyLevel], [Chapter]) VALUES (3, 2, 3, N'Câu lệnh nào dùng để lấy dữ liệu trong SQL?', N'SINGLE', 1, 1)
GO
INSERT [dbo].[Questions] ([QuestionID], [SubjectID], [CreatedBy_TeacherID], [QuestionText], [QuestionType], [DifficultyLevel], [Chapter]) VALUES (4, 2, 3, N'Ràng buộc khóa chính được dùng để làm gì?', N'SINGLE', 2, 2)
GO
INSERT [dbo].[Questions] ([QuestionID], [SubjectID], [CreatedBy_TeacherID], [QuestionText], [QuestionType], [DifficultyLevel], [Chapter]) VALUES (5, 3, 2, N'Java là ngôn ngữ lập trình thuộc loại nào?', N'SINGLE', 1, 1)
GO
INSERT [dbo].[Questions] ([QuestionID], [SubjectID], [CreatedBy_TeacherID], [QuestionText], [QuestionType], [DifficultyLevel], [Chapter]) VALUES (6, 3, 2, N'Khái niệm OOP gồm những tính chất nào?', N'MULTIPLE', 2, 1)
GO
INSERT [dbo].[Questions] ([QuestionID], [SubjectID], [CreatedBy_TeacherID], [QuestionText], [QuestionType], [DifficultyLevel], [Chapter]) VALUES (7, 3, 2, N'Từ khóa nào để kế thừa một lớp trong Java?', N'SINGLE', 1, 2)
GO
INSERT [dbo].[Questions] ([QuestionID], [SubjectID], [CreatedBy_TeacherID], [QuestionText], [QuestionType], [DifficultyLevel], [Chapter]) VALUES (8, 3, 2, N'Interface trong Java có đặc điểm nào?', N'MULTIPLE', 2, 2)
GO
INSERT [dbo].[Questions] ([QuestionID], [SubjectID], [CreatedBy_TeacherID], [QuestionText], [QuestionType], [DifficultyLevel], [Chapter]) VALUES (9, 3, 2, N'Constructor dùng để làm gì?', N'SINGLE', 1, 3)
GO
INSERT [dbo].[Questions] ([QuestionID], [SubjectID], [CreatedBy_TeacherID], [QuestionText], [QuestionType], [DifficultyLevel], [Chapter]) VALUES (10, 3, 2, N'Thuộc tính của lớp được khai báo như thế nào?', N'SINGLE', 1, 1)
GO
INSERT [dbo].[Questions] ([QuestionID], [SubjectID], [CreatedBy_TeacherID], [QuestionText], [QuestionType], [DifficultyLevel], [Chapter]) VALUES (11, 3, 2, N'Override & Overload khác nhau như thế nào?', N'MULTIPLE', 3, 4)
GO
INSERT [dbo].[Questions] ([QuestionID], [SubjectID], [CreatedBy_TeacherID], [QuestionText], [QuestionType], [DifficultyLevel], [Chapter]) VALUES (12, 3, 2, N'Package trong Java có mục đích gì?', N'SINGLE', 1, 5)
GO
INSERT [dbo].[Questions] ([QuestionID], [SubjectID], [CreatedBy_TeacherID], [QuestionText], [QuestionType], [DifficultyLevel], [Chapter]) VALUES (13, 3, 2, N'Từ khóa final có ý nghĩa gì?', N'MULTIPLE', 2, 3)
GO
INSERT [dbo].[Questions] ([QuestionID], [SubjectID], [CreatedBy_TeacherID], [QuestionText], [QuestionType], [DifficultyLevel], [Chapter]) VALUES (14, 3, 2, N'Collections Framework gồm những thành phần nào?', N'MULTIPLE', 3, 6)
GO
INSERT [dbo].[Questions] ([QuestionID], [SubjectID], [CreatedBy_TeacherID], [QuestionText], [QuestionType], [DifficultyLevel], [Chapter]) VALUES (15, 4, 3, N'View dùng để làm gì?', N'SINGLE', 1, 1)
GO
INSERT [dbo].[Questions] ([QuestionID], [SubjectID], [CreatedBy_TeacherID], [QuestionText], [QuestionType], [DifficultyLevel], [Chapter]) VALUES (16, 4, 3, N'Những đặc điểm của Index?', N'MULTIPLE', 2, 1)
GO
INSERT [dbo].[Questions] ([QuestionID], [SubjectID], [CreatedBy_TeacherID], [QuestionText], [QuestionType], [DifficultyLevel], [Chapter]) VALUES (17, 4, 3, N'Triggers dùng khi nào?', N'SINGLE', 1, 2)
GO
INSERT [dbo].[Questions] ([QuestionID], [SubjectID], [CreatedBy_TeacherID], [QuestionText], [QuestionType], [DifficultyLevel], [Chapter]) VALUES (18, 4, 3, N'Các loại JOIN nâng cao?', N'MULTIPLE', 2, 2)
GO
INSERT [dbo].[Questions] ([QuestionID], [SubjectID], [CreatedBy_TeacherID], [QuestionText], [QuestionType], [DifficultyLevel], [Chapter]) VALUES (19, 4, 3, N'ACID đảm bảo điều gì?', N'SINGLE', 2, 3)
GO
INSERT [dbo].[Questions] ([QuestionID], [SubjectID], [CreatedBy_TeacherID], [QuestionText], [QuestionType], [DifficultyLevel], [Chapter]) VALUES (20, 4, 3, N'Normalization gồm những bước nào?', N'MULTIPLE', 3, 3)
GO
INSERT [dbo].[Questions] ([QuestionID], [SubjectID], [CreatedBy_TeacherID], [QuestionText], [QuestionType], [DifficultyLevel], [Chapter]) VALUES (21, 4, 3, N'Stored Procedure dùng để làm gì?', N'SINGLE', 1, 4)
GO
INSERT [dbo].[Questions] ([QuestionID], [SubjectID], [CreatedBy_TeacherID], [QuestionText], [QuestionType], [DifficultyLevel], [Chapter]) VALUES (22, 4, 3, N'Transaction dùng khi nào?', N'MULTIPLE', 2, 4)
GO
INSERT [dbo].[Questions] ([QuestionID], [SubjectID], [CreatedBy_TeacherID], [QuestionText], [QuestionType], [DifficultyLevel], [Chapter]) VALUES (23, 4, 3, N'Deadlock xảy ra khi nào?', N'SINGLE', 3, 5)
GO
INSERT [dbo].[Questions] ([QuestionID], [SubjectID], [CreatedBy_TeacherID], [QuestionText], [QuestionType], [DifficultyLevel], [Chapter]) VALUES (24, 4, 3, N'Các kiểu Partitioning?', N'MULTIPLE', 3, 6)
GO
SET IDENTITY_INSERT [dbo].[Questions] OFF
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
SET IDENTITY_INSERT [dbo].[StudentAnswers] ON 
GO
INSERT [dbo].[StudentAnswers] ([StudentAnswerID], [AttemptID], [QuestionID], [ChosenAnswerID]) VALUES (1, 1, 1, 1)
GO
INSERT [dbo].[StudentAnswers] ([StudentAnswerID], [AttemptID], [QuestionID], [ChosenAnswerID]) VALUES (2, 1, 2, 4)
GO
INSERT [dbo].[StudentAnswers] ([StudentAnswerID], [AttemptID], [QuestionID], [ChosenAnswerID]) VALUES (3, 2, 1, 2)
GO
INSERT [dbo].[StudentAnswers] ([StudentAnswerID], [AttemptID], [QuestionID], [ChosenAnswerID]) VALUES (4, 2, 2, 4)
GO
INSERT [dbo].[StudentAnswers] ([StudentAnswerID], [AttemptID], [QuestionID], [ChosenAnswerID]) VALUES (5, 4, 1, 2)
GO
INSERT [dbo].[StudentAnswers] ([StudentAnswerID], [AttemptID], [QuestionID], [ChosenAnswerID]) VALUES (6, 5, 1, 1)
GO
INSERT [dbo].[StudentAnswers] ([StudentAnswerID], [AttemptID], [QuestionID], [ChosenAnswerID]) VALUES (7, 5, 2, 4)
GO
SET IDENTITY_INSERT [dbo].[StudentAnswers] OFF
GO
SET IDENTITY_INSERT [dbo].[Subjects] ON 
GO
INSERT [dbo].[Subjects] ([SubjectID], [SubjectCode], [SubjectName], [CreatedBy_UserID], [Status]) VALUES (1, N'PR101', N'Lập Trình Cơ Bản', 1, 1)
GO
INSERT [dbo].[Subjects] ([SubjectID], [SubjectCode], [SubjectName], [CreatedBy_UserID], [Status]) VALUES (2, N'DB201', N'Cơ Sở Dữ Liệu', 1, 1)
GO
INSERT [dbo].[Subjects] ([SubjectID], [SubjectCode], [SubjectName], [CreatedBy_UserID], [Status]) VALUES (3, N'PRO192', N'Lập trình hướng đối tượng Java', 2, 1)
GO
INSERT [dbo].[Subjects] ([SubjectID], [SubjectCode], [SubjectName], [CreatedBy_UserID], [Status]) VALUES (4, N'DBI202', N'Cơ sở dữ liệu nâng cao', 2, 1)
GO
INSERT [dbo].[Subjects] ([SubjectID], [SubjectCode], [SubjectName], [CreatedBy_UserID], [Status]) VALUES (5, N'CSD201', N'Cấu trúc dữ liệu & giải thuật', 2, 1)
GO
SET IDENTITY_INSERT [dbo].[Subjects] OFF
GO
SET IDENTITY_INSERT [dbo].[TeachingAssignments] ON 
GO
INSERT [dbo].[TeachingAssignments] ([AssignmentID], [TeacherID], [ClassID]) VALUES (1, 2, 1)
GO
INSERT [dbo].[TeachingAssignments] ([AssignmentID], [TeacherID], [ClassID]) VALUES (2, 3, 2)
GO
SET IDENTITY_INSERT [dbo].[TeachingAssignments] OFF
GO
SET IDENTITY_INSERT [dbo].[TestAttempts] ON 
GO
INSERT [dbo].[TestAttempts] ([AttemptID], [StudentID], [TestID], [StartTime], [EndTime], [Score], [AttemptStatus]) VALUES (1, 4, 1, CAST(N'2025-11-17T01:04:59.7033333' AS DateTime2), CAST(N'2025-11-17T01:04:59.7033333' AS DateTime2), 2, N'Completed')
GO
INSERT [dbo].[TestAttempts] ([AttemptID], [StudentID], [TestID], [StartTime], [EndTime], [Score], [AttemptStatus]) VALUES (2, 5, 1, CAST(N'2025-11-17T01:04:59.7033333' AS DateTime2), CAST(N'2025-11-17T01:04:59.7033333' AS DateTime2), 1, N'Completed')
GO
INSERT [dbo].[TestAttempts] ([AttemptID], [StudentID], [TestID], [StartTime], [EndTime], [Score], [AttemptStatus]) VALUES (3, 6, 2, CAST(N'2025-11-17T01:04:59.7033333' AS DateTime2), NULL, NULL, N'InProgress')
GO
INSERT [dbo].[TestAttempts] ([AttemptID], [StudentID], [TestID], [StartTime], [EndTime], [Score], [AttemptStatus]) VALUES (4, 4, 3, CAST(N'2025-11-17T16:21:18.0022390' AS DateTime2), CAST(N'2025-11-17T16:21:30.5252223' AS DateTime2), 0, N'Completed')
GO
INSERT [dbo].[TestAttempts] ([AttemptID], [StudentID], [TestID], [StartTime], [EndTime], [Score], [AttemptStatus]) VALUES (5, 4, 3, CAST(N'2025-11-17T16:21:39.2184200' AS DateTime2), CAST(N'2025-11-17T16:21:50.4011492' AS DateTime2), 100, N'Completed')
GO
INSERT [dbo].[TestAttempts] ([AttemptID], [StudentID], [TestID], [StartTime], [EndTime], [Score], [AttemptStatus]) VALUES (6, 4, 4, CAST(N'2025-11-17T17:31:50.8563537' AS DateTime2), CAST(N'2025-11-17T17:33:04.8973372' AS DateTime2), 0, N'Completed')
GO
SET IDENTITY_INSERT [dbo].[TestAttempts] OFF
GO
SET IDENTITY_INSERT [dbo].[TestQuestions] ON 
GO
INSERT [dbo].[TestQuestions] ([TestQuestionID], [TestID], [QuestionID], [Points]) VALUES (1, 1, 1, 1)
GO
INSERT [dbo].[TestQuestions] ([TestQuestionID], [TestID], [QuestionID], [Points]) VALUES (2, 1, 2, 1)
GO
INSERT [dbo].[TestQuestions] ([TestQuestionID], [TestID], [QuestionID], [Points]) VALUES (3, 2, 3, 1)
GO
INSERT [dbo].[TestQuestions] ([TestQuestionID], [TestID], [QuestionID], [Points]) VALUES (4, 2, 4, 1)
GO
INSERT [dbo].[TestQuestions] ([TestQuestionID], [TestID], [QuestionID], [Points]) VALUES (5, 3, 1, 5)
GO
INSERT [dbo].[TestQuestions] ([TestQuestionID], [TestID], [QuestionID], [Points]) VALUES (6, 3, 2, 5)
GO
INSERT [dbo].[TestQuestions] ([TestQuestionID], [TestID], [QuestionID], [Points]) VALUES (7, 4, 15, 1)
GO
INSERT [dbo].[TestQuestions] ([TestQuestionID], [TestID], [QuestionID], [Points]) VALUES (8, 4, 17, 1)
GO
INSERT [dbo].[TestQuestions] ([TestQuestionID], [TestID], [QuestionID], [Points]) VALUES (9, 4, 19, 1)
GO
INSERT [dbo].[TestQuestions] ([TestQuestionID], [TestID], [QuestionID], [Points]) VALUES (10, 4, 21, 1)
GO
INSERT [dbo].[TestQuestions] ([TestQuestionID], [TestID], [QuestionID], [Points]) VALUES (11, 4, 20, 1)
GO
INSERT [dbo].[TestQuestions] ([TestQuestionID], [TestID], [QuestionID], [Points]) VALUES (12, 4, 22, 1)
GO
INSERT [dbo].[TestQuestions] ([TestQuestionID], [TestID], [QuestionID], [Points]) VALUES (13, 4, 23, 1)
GO
INSERT [dbo].[TestQuestions] ([TestQuestionID], [TestID], [QuestionID], [Points]) VALUES (14, 4, 24, 1)
GO
INSERT [dbo].[TestQuestions] ([TestQuestionID], [TestID], [QuestionID], [Points]) VALUES (15, 4, 18, 1)
GO
INSERT [dbo].[TestQuestions] ([TestQuestionID], [TestID], [QuestionID], [Points]) VALUES (16, 4, 16, 1)
GO
SET IDENTITY_INSERT [dbo].[TestQuestions] OFF
GO
SET IDENTITY_INSERT [dbo].[Tests] ON 
GO
INSERT [dbo].[Tests] ([TestID], [ClassID], [CreatedBy_TeacherID], [TestName], [DurationMinutes], [IsActive], [AvailableFrom], [AvailableTo], [CreatedAt]) VALUES (1, 1, 2, N'Kiểm tra giữa kỳ PR101', 45, 1, NULL, NULL, CAST(N'2025-11-17T01:04:59.7033333' AS DateTime2))
GO
INSERT [dbo].[Tests] ([TestID], [ClassID], [CreatedBy_TeacherID], [TestName], [DurationMinutes], [IsActive], [AvailableFrom], [AvailableTo], [CreatedAt]) VALUES (2, 2, 3, N'Kiểm tra cuối kỳ DB201', 60, 1, NULL, NULL, CAST(N'2025-11-17T01:04:59.7033333' AS DateTime2))
GO
INSERT [dbo].[Tests] ([TestID], [ClassID], [CreatedBy_TeacherID], [TestName], [DurationMinutes], [IsActive], [AvailableFrom], [AvailableTo], [CreatedAt]) VALUES (3, 1, 2, N'test123', 30, 1, CAST(N'2025-11-17T00:00:00.0000000' AS DateTime2), CAST(N'2025-11-18T00:00:00.0000000' AS DateTime2), CAST(N'2025-11-17T16:17:06.3814980' AS DateTime2))
GO
INSERT [dbo].[Tests] ([TestID], [ClassID], [CreatedBy_TeacherID], [TestName], [DurationMinutes], [IsActive], [AvailableFrom], [AvailableTo], [CreatedAt]) VALUES (4, 1, 2, N'Small Test1', 60, 1, CAST(N'2025-11-17T00:00:00.0000000' AS DateTime2), CAST(N'2025-11-18T00:00:00.0000000' AS DateTime2), CAST(N'2025-11-17T17:28:15.5976892' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[Tests] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [FullName], [Email], [RoleID], [Status], [CreatedAt]) VALUES (1, N'admin', N'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', N'Quản Trị Viên', NULL, 1, 1, CAST(N'2025-10-23T08:58:17.1833333' AS DateTime2))
GO
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [FullName], [Email], [RoleID], [Status], [CreatedAt]) VALUES (2, N'teacher1', N'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', N'Nguyễn Văn A', N'a.teacher@example.com', 2, 1, CAST(N'2025-11-17T01:04:59.6966667' AS DateTime2))
GO
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [FullName], [Email], [RoleID], [Status], [CreatedAt]) VALUES (3, N'teacher2', N'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', N'Trần Thị B', N'b.teacher@example.com', 2, 1, CAST(N'2025-11-17T01:04:59.6966667' AS DateTime2))
GO
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [FullName], [Email], [RoleID], [Status], [CreatedAt]) VALUES (4, N'student1', N'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', N'Lê Minh C', N'c.student@example.com', 3, 1, CAST(N'2025-11-17T01:04:59.7000000' AS DateTime2))
GO
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [FullName], [Email], [RoleID], [Status], [CreatedAt]) VALUES (5, N'student2', N'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', N'Phạm Thị D', N'd.student@example.com', 3, 1, CAST(N'2025-11-17T01:04:59.7000000' AS DateTime2))
GO
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [FullName], [Email], [RoleID], [Status], [CreatedAt]) VALUES (6, N'student3', N'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', N'Hoàng Văn E', N'e.student@example.com', 3, 1, CAST(N'2025-11-17T01:04:59.7000000' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
/****** Object:  Index [UK_Student_Class]    Script Date: 11/17/2025 5:35:13 PM ******/
ALTER TABLE [dbo].[Enrollments] ADD  CONSTRAINT [UK_Student_Class] UNIQUE NONCLUSTERED 
(
	[StudentID] ASC,
	[ClassID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Roles__8A2B616028740FD8]    Script Date: 11/17/2025 5:35:13 PM ******/
ALTER TABLE [dbo].[Roles] ADD UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Subjects__9F7CE1A922430F2E]    Script Date: 11/17/2025 5:35:13 PM ******/
ALTER TABLE [dbo].[Subjects] ADD UNIQUE NONCLUSTERED 
(
	[SubjectCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UK_Teacher_Class]    Script Date: 11/17/2025 5:35:13 PM ******/
ALTER TABLE [dbo].[TeachingAssignments] ADD  CONSTRAINT [UK_Teacher_Class] UNIQUE NONCLUSTERED 
(
	[TeacherID] ASC,
	[ClassID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UK_Test_Question]    Script Date: 11/17/2025 5:35:13 PM ******/
ALTER TABLE [dbo].[TestQuestions] ADD  CONSTRAINT [UK_Test_Question] UNIQUE NONCLUSTERED
(
	[TestID] ASC,
	[QuestionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UK_Attempt_Question_Answer]    Script Date: 11/17/2025 5:35:13 PM ******/
ALTER TABLE [dbo].[StudentAnswers] ADD  CONSTRAINT [UK_Attempt_Question_Answer] UNIQUE NONCLUSTERED
(
	[AttemptID] ASC,
	[QuestionID] ASC,
	[ChosenAnswerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__536C85E4826B54A9]    Script Date: 11/17/2025 5:35:13 PM ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__A9D10534C8F2A360]    Script Date: 11/17/2025 5:35:13 PM ******/
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
