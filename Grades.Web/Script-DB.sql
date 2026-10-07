-- Base de datos de la solución Grades
-- Este script se puede ejecutar varias veces: solo crea lo que falta.
IF DB_ID('GradesDB') IS NULL
    CREATE DATABASE GradesDB;
GO
USE GradesDB;
GO

-- =============================================================
-- TABLAS
-- =============================================================

-- Estudiantes. La columna IsActive permite la eliminación lógica.
IF OBJECT_ID('Students') IS NULL
    CREATE TABLE Students (
        StudentId  INT IDENTITY(1,1) PRIMARY KEY,
        Code       NVARCHAR(10)  NOT NULL UNIQUE,
        FirstName  NVARCHAR(50)  NOT NULL,
        LastName   NVARCHAR(50)  NOT NULL,
        Email      NVARCHAR(100) NULL,
        IsActive   BIT           NOT NULL DEFAULT 1
    );
GO

-- Notas por curso. Un estudiante tiene muchas notas (relación 1 a N).
-- El promedio y la condición NO se guardan: los calcula la aplicación.
IF OBJECT_ID('Grades') IS NULL
    CREATE TABLE Grades (
        GradeId    INT IDENTITY(1,1) PRIMARY KEY,
        StudentId  INT           NOT NULL REFERENCES Students(StudentId),
        Course     NVARCHAR(100) NOT NULL,
        Score1     DECIMAL(4,2)  NOT NULL,
        Score2     DECIMAL(4,2)  NOT NULL,
        Score3     DECIMAL(4,2)  NOT NULL,
        IsActive   BIT           NOT NULL DEFAULT 1
    );
GO

-- =============================================================
-- DATOS DE PRUEBA (solo si las tablas están vacías)
-- =============================================================
IF NOT EXISTS (SELECT 1 FROM Students)
BEGIN
    INSERT INTO Students (Code, FirstName, LastName, Email) VALUES
        (N'U2024001', N'Ana',   N'Torres', N'ana.torres@mail.com'),
        (N'U2024002', N'Luis',  N'Ramos',  N'luis.ramos@mail.com'),
        (N'U2024003', N'Carla', N'Vega',   NULL);

    INSERT INTO Grades (StudentId, Course, Score1, Score2, Score3) VALUES
        (1, N'Programming I', 16, 14, 15),
        (1, N'Databases',     12, 13, 14),
        (2, N'Programming I', 10, 12,  9),
        (2, N'Mathematics',   11, 13, 12),
        (3, N'Programming I', 18, 19, 17),
        (3, N'Databases',     17, 16, 18);
END
GO

-- =============================================================
-- STORED PROCEDURES: Students
-- =============================================================

CREATE OR ALTER PROCEDURE usp_Students_List
AS
BEGIN
    SELECT StudentId, Code, FirstName, LastName, Email
    FROM Students
    WHERE IsActive = 1
    ORDER BY LastName, FirstName;
END
GO

CREATE OR ALTER PROCEDURE usp_Students_GetById
    @StudentId INT
AS
BEGIN
    SELECT StudentId, Code, FirstName, LastName, Email
    FROM Students
    WHERE StudentId = @StudentId AND IsActive = 1;
END
GO

CREATE OR ALTER PROCEDURE usp_Students_Insert
    @Code      NVARCHAR(10),
    @FirstName NVARCHAR(50),
    @LastName  NVARCHAR(50),
    @Email     NVARCHAR(100)
AS
BEGIN
    INSERT INTO Students (Code, FirstName, LastName, Email)
    VALUES (@Code, @FirstName, @LastName, @Email);
END
GO

CREATE OR ALTER PROCEDURE usp_Students_Update
    @StudentId INT,
    @Code      NVARCHAR(10),
    @FirstName NVARCHAR(50),
    @LastName  NVARCHAR(50),
    @Email     NVARCHAR(100)
AS
BEGIN
    UPDATE Students
    SET Code = @Code,
        FirstName = @FirstName,
        LastName = @LastName,
        Email = @Email
    WHERE StudentId = @StudentId;
END
GO

-- Eliminación lógica: desactiva al estudiante y también sus notas.
CREATE OR ALTER PROCEDURE usp_Students_Delete
    @StudentId INT
AS
BEGIN
    UPDATE Grades   SET IsActive = 0 WHERE StudentId = @StudentId;
    UPDATE Students SET IsActive = 0 WHERE StudentId = @StudentId;
END
GO

-- Devuelve cuántos estudiantes (distintos a @StudentId) ya usan ese código.
-- Al crear se envía @StudentId = 0.
CREATE OR ALTER PROCEDURE usp_Students_ExistsCode
    @Code      NVARCHAR(10),
    @StudentId INT
AS
BEGIN
    SELECT COUNT(*)
    FROM Students
    WHERE Code = @Code AND StudentId <> @StudentId;
END
GO

-- =============================================================
-- STORED PROCEDURES: Grades
-- =============================================================

CREATE OR ALTER PROCEDURE usp_Grades_List
AS
BEGIN
    SELECT g.GradeId, g.StudentId, g.Course, g.Score1, g.Score2, g.Score3,
           s.FirstName + ' ' + s.LastName AS StudentName
    FROM Grades g
    INNER JOIN Students s ON s.StudentId = g.StudentId
    WHERE g.IsActive = 1
    ORDER BY s.LastName, s.FirstName, g.Course;
END
GO

CREATE OR ALTER PROCEDURE usp_Grades_GetById
    @GradeId INT
AS
BEGIN
    SELECT GradeId, StudentId, Course, Score1, Score2, Score3
    FROM Grades
    WHERE GradeId = @GradeId AND IsActive = 1;
END
GO

CREATE OR ALTER PROCEDURE usp_Grades_Insert
    @StudentId INT,
    @Course    NVARCHAR(100),
    @Score1    DECIMAL(4,2),
    @Score2    DECIMAL(4,2),
    @Score3    DECIMAL(4,2)
AS
BEGIN
    INSERT INTO Grades (StudentId, Course, Score1, Score2, Score3)
    VALUES (@StudentId, @Course, @Score1, @Score2, @Score3);
END
GO

CREATE OR ALTER PROCEDURE usp_Grades_Update
    @GradeId   INT,
    @StudentId INT,
    @Course    NVARCHAR(100),
    @Score1    DECIMAL(4,2),
    @Score2    DECIMAL(4,2),
    @Score3    DECIMAL(4,2)
AS
BEGIN
    UPDATE Grades
    SET StudentId = @StudentId,
        Course = @Course,
        Score1 = @Score1,
        Score2 = @Score2,
        Score3 = @Score3
    WHERE GradeId = @GradeId;
END
GO

-- Eliminación lógica de una nota.
CREATE OR ALTER PROCEDURE usp_Grades_Delete
    @GradeId INT
AS
BEGIN
    UPDATE Grades SET IsActive = 0 WHERE GradeId = @GradeId;
END
GO
