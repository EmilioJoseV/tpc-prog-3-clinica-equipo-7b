/*
   BASE DE DATOS: TPC_TURNOS_CLINICA_P3_DB
   SQL Server Express
   PROYECTO: UTN FRGP TUP - TPC Programacion III - Sistema de Gestion de Clinica
*/

USE master;
GO

IF DB_ID(N'TPC_TURNOS_CLINICA_P3_DB') IS NOT NULL
BEGIN
    ALTER DATABASE TPC_TURNOS_CLINICA_P3_DB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE TPC_TURNOS_CLINICA_P3_DB;
END
GO

CREATE DATABASE TPC_TURNOS_CLINICA_P3_DB;
GO

USE TPC_TURNOS_CLINICA_P3_DB;
GO

CREATE TABLE dbo.Roles
(
    IdRol INT IDENTITY(1,1) NOT NULL,
    Nombre VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(200) NULL,
    Activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_Roles PRIMARY KEY (IdRol),
    CONSTRAINT UQ_Roles_Nombre UNIQUE (Nombre),
    CONSTRAINT CK_Roles_Nombre_NoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0)
);
GO

CREATE TABLE dbo.ConfiguracionesTurno
(
    IdConfiguracionTurno INT IDENTITY(1,1) NOT NULL,
    DuracionMinutos INT NOT NULL,
    Activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_ConfiguracionesTurno PRIMARY KEY (IdConfiguracionTurno),
    CONSTRAINT CK_ConfiguracionesTurno_DuracionMinutos_Valida CHECK (DuracionMinutos > 0)
);
GO

CREATE TABLE dbo.Pacientes
(
    IdPaciente INT IDENTITY(1,1) NOT NULL,
    DNI VARCHAR(15) NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    FechaNacimiento DATE NOT NULL,
    Telefono VARCHAR(50) NULL,
    Email VARCHAR(150) NOT NULL,
    Direccion VARCHAR(200) NULL,
    Activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_Pacientes PRIMARY KEY (IdPaciente),
    CONSTRAINT UQ_Pacientes_DNI UNIQUE (DNI),
    CONSTRAINT CK_Pacientes_DNI_NoVacio CHECK (LEN(LTRIM(RTRIM(DNI))) > 0),
    CONSTRAINT CK_Pacientes_Nombre_NoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0),
    CONSTRAINT CK_Pacientes_Apellido_NoVacio CHECK (LEN(LTRIM(RTRIM(Apellido))) > 0),
    CONSTRAINT CK_Pacientes_Email_Formato CHECK (Email LIKE '%_@_%._%')
);
GO

CREATE TABLE dbo.Especialidades
(
    IdEspecialidad INT IDENTITY(1,1) NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(300) NULL,
    Activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_Especialidades PRIMARY KEY (IdEspecialidad),
    CONSTRAINT UQ_Especialidades_Nombre UNIQUE (Nombre),
    CONSTRAINT CK_Especialidades_Nombre_NoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0)
);
GO

CREATE TABLE dbo.Medicos
(
    IdMedico INT IDENTITY(1,1) NOT NULL,
    Matricula VARCHAR(30) NOT NULL,
    DNI VARCHAR(15) NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    Telefono VARCHAR(50) NULL,
    Email VARCHAR(150) NOT NULL,
    Activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_Medicos PRIMARY KEY (IdMedico),
    CONSTRAINT UQ_Medicos_Matricula UNIQUE (Matricula),
    CONSTRAINT UQ_Medicos_DNI UNIQUE (DNI),
    CONSTRAINT CK_Medicos_Matricula_NoVacia CHECK (LEN(LTRIM(RTRIM(Matricula))) > 0),
    CONSTRAINT CK_Medicos_DNI_NoVacio CHECK (LEN(LTRIM(RTRIM(DNI))) > 0),
    CONSTRAINT CK_Medicos_Nombre_NoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0),
    CONSTRAINT CK_Medicos_Apellido_NoVacio CHECK (LEN(LTRIM(RTRIM(Apellido))) > 0),
    CONSTRAINT CK_Medicos_Email_Formato CHECK (Email LIKE '%_@_%._%'),
);
GO

CREATE TABLE dbo.MedicosEspecialidades
(
    IdMedico INT NOT NULL,
    IdEspecialidad INT NOT NULL,

    CONSTRAINT PK_MedicosEspecialidades PRIMARY KEY (IdMedico, IdEspecialidad),

    CONSTRAINT FK_MedicosEspecialidades_Medicos
        FOREIGN KEY (IdMedico)
        REFERENCES dbo.Medicos(IdMedico),

    CONSTRAINT FK_MedicosEspecialidades_Especialidades
        FOREIGN KEY (IdEspecialidad)
        REFERENCES dbo.Especialidades(IdEspecialidad)
);
GO

CREATE TABLE dbo.HorariosDisponiblidadMedicos
(
    IdHorarioDisponiblidadMedico INT IDENTITY(1,1) NOT NULL,
    IdMedico INT NOT NULL,
    DiaSemana TINYINT NOT NULL,
    HoraDesde TIME NOT NULL,
    HoraHasta TIME NOT NULL,
    Activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_HorariosDisponiblidadMedicos PRIMARY KEY (IdHorarioDisponiblidadMedico),

    CONSTRAINT FK_HorariosDisponiblidadMedicos_Medicos
        FOREIGN KEY (IdMedico)
        REFERENCES dbo.Medicos(IdMedico),

    CONSTRAINT CK_HorariosDisponiblidadMedicos_DiaSemana_Valido CHECK (DiaSemana BETWEEN 1 AND 7),
    CONSTRAINT CK_HorariosDisponiblidadMedicos_HorarioValido CHECK (HoraHasta > HoraDesde)
);
GO

CREATE TABLE dbo.Usuarios
(
    IdUsuario INT IDENTITY(1,1) NOT NULL,
    NombreUsuario VARCHAR(50) NOT NULL,
    Email VARCHAR(150) NOT NULL,
    PasswordHash VARCHAR(256) NOT NULL,
    IdRol INT NOT NULL,
    IdMedico INT NULL,
    Activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_Usuarios PRIMARY KEY (IdUsuario),
    CONSTRAINT UQ_Usuarios_NombreUsuario UNIQUE (NombreUsuario),
    CONSTRAINT UQ_Usuarios_Email UNIQUE (Email),
    CONSTRAINT CK_Usuarios_NombreUsuario_NoVacio CHECK (LEN(LTRIM(RTRIM(NombreUsuario))) > 0),
    CONSTRAINT CK_Usuarios_Email_Formato CHECK (Email LIKE '%_@_%._%'),
    CONSTRAINT CK_Usuarios_PasswordHash_NoVacio CHECK (LEN(LTRIM(RTRIM(PasswordHash))) > 0),

    CONSTRAINT FK_Usuarios_Roles
        FOREIGN KEY (IdRol)
        REFERENCES dbo.Roles(IdRol),

    CONSTRAINT FK_Usuarios_Medicos
        FOREIGN KEY (IdMedico)
        REFERENCES dbo.Medicos(IdMedico)
);
GO

CREATE TABLE dbo.EstadosTurno
(
    IdEstadoTurno INT IDENTITY(1,1) NOT NULL,
    Nombre VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(200) NULL,
    EsFinal BIT NOT NULL DEFAULT 0,
    Activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_EstadosTurno PRIMARY KEY (IdEstadoTurno),
    CONSTRAINT UQ_EstadosTurno_Nombre UNIQUE (Nombre),
    CONSTRAINT CK_EstadosTurno_Nombre_NoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0)
);
GO

CREATE TABLE dbo.Turnos
(
    IdTurno INT IDENTITY(1,1) NOT NULL,
    NumeroTurno VARCHAR(20) NOT NULL,

    IdPaciente INT NOT NULL,
    IdMedico INT NOT NULL,
    IdEspecialidad INT NOT NULL,
    IdEstadoTurno INT NOT NULL,

    FechaTurno DATE NOT NULL,
    HoraInicio TIME NOT NULL,
    HoraFin TIME NOT NULL,

    Observaciones VARCHAR(500) NOT NULL,
    DiagnosticoMedico VARCHAR(1000) NULL,

    FechaAlta DATETIME NOT NULL DEFAULT GETDATE(),
    IdUsuarioAlta INT NOT NULL,
    FechaModificacion DATETIME NULL,
    IdUsuarioModificacion INT NULL,

    CONSTRAINT PK_Turnos PRIMARY KEY (IdTurno),
    CONSTRAINT UQ_Turnos_NumeroTurno UNIQUE (NumeroTurno),

    CONSTRAINT FK_Turnos_Pacientes
        FOREIGN KEY (IdPaciente)
        REFERENCES dbo.Pacientes(IdPaciente),

    CONSTRAINT FK_Turnos_Medicos
        FOREIGN KEY (IdMedico)
        REFERENCES dbo.Medicos(IdMedico),

    CONSTRAINT FK_Turnos_Especialidades
        FOREIGN KEY (IdEspecialidad)
        REFERENCES dbo.Especialidades(IdEspecialidad),

    CONSTRAINT FK_Turnos_EstadosTurno
        FOREIGN KEY (IdEstadoTurno)
        REFERENCES dbo.EstadosTurno(IdEstadoTurno),

    CONSTRAINT FK_Turnos_UsuariosAlta
        FOREIGN KEY (IdUsuarioAlta)
        REFERENCES dbo.Usuarios(IdUsuario),

    CONSTRAINT FK_Turnos_UsuariosModificacion
        FOREIGN KEY (IdUsuarioModificacion)
        REFERENCES dbo.Usuarios(IdUsuario),

    CONSTRAINT CK_Turnos_NumeroTurno_NoVacio CHECK (LEN(LTRIM(RTRIM(NumeroTurno))) > 0),
    CONSTRAINT CK_Turnos_HorarioValido CHECK (HoraFin > HoraInicio),
    CONSTRAINT CK_Turnos_Observaciones_NoVacio CHECK (LEN(LTRIM(RTRIM(Observaciones))) > 0)
);
GO

/* -------------
   DATOS DE PRUEBA
   ------------- */

/* Roles necesarios segun el enunciado */
INSERT INTO dbo.Roles (Nombre, Descripcion)
VALUES
('Administrador', 'Acceso total al sistema'),
('Recepcionista', 'Gestiona pacientes, medicos y turnos'),
('Medico', 'Consulta sus turnos y carga diagnosticos');
GO

/* Estados necesarios para el ciclo de vida del turno */
INSERT INTO dbo.EstadosTurno (Nombre, Descripcion, EsFinal)
VALUES
('Nuevo', 'Turno asignado', 0),
('Reprogramado', 'Turno reprogramado', 0),
('Cancelado', 'Turno cancelado', 1),
('NoAsistio', 'Paciente no se presento', 1),
('Cerrado', 'Turno atendido y cerrado', 1);
GO

/* Configuracion global de duracion de turnos */
INSERT INTO dbo.ConfiguracionesTurno (DuracionMinutos)
VALUES
(60);
GO

/* Especialidades */
INSERT INTO dbo.Especialidades (Nombre, Descripcion)
VALUES
('Clinica Medica', 'Atencion general'),
('Cardiologia', 'Atencion de patologias cardiovasculares'),
('Odontologia', 'Atencion odontologica'),
('Pediatria', 'Atencion medica infantil'),
('Traumatologia', 'Atencion de lesiones oseas y musculares');
GO

/* Pacientes */
INSERT INTO dbo.Pacientes
(
    DNI,
    Nombre,
    Apellido,
    FechaNacimiento,
    Telefono,
    Email,
    Direccion
)
VALUES
('30111222', 'Juan', 'Perez', '1985-04-12', '1123456789', 'juan.perez@mail.com', 'Av San Martin 1234'),
('33222333', 'Maria', 'Lopez', '1990-09-23', '1134567890', 'maria.lopez@mail.com', 'Belgrano 455'),
('28333444', 'Carlos', 'Diaz', '1978-01-05', '1145678901', 'carlos.diaz@mail.com', 'Mitre 789'),
('40555666', 'Sofia', 'Acosta', '2001-11-18', '1156789012', 'sofia.acosta@mail.com', 'Rivadavia 2300'),
('35666777', 'Lucia', 'Martinez', '1994-07-30', '1167890123', 'lucia.martinez@mail.com', 'Moreno 150');
GO

/* Medicos */
INSERT INTO dbo.Medicos
(
    Matricula,
    DNI,
    Nombre,
    Apellido,
    Telefono,
    Email
)
VALUES
('MN-10001', '20111222', 'Laura', 'Gomez', '1122001100', 'laura.gomez@clinica.local'),
('MN-10002', '21222333', 'Martin', 'Ruiz', '1133002200', 'martin.ruiz@clinica.local'),
('MN-10003', '22333444', 'Ana', 'Torres', '1144003300', 'ana.torres@clinica.local'),
('MN-10004', '23444555', 'Diego', 'Salas', '1155004400', 'diego.salas@clinica.local'),
('MN-10005', '24555666', 'Valeria', 'Molina', '1166005500', 'valeria.molina@clinica.local');
GO

/* Relacion medico - especialidad */
INSERT INTO dbo.MedicosEspecialidades
(
    IdMedico,
    IdEspecialidad
)
VALUES
(1, 1),
(2, 3),
(3, 4),
(4, 5),
(5, 2);
GO

/* Dias de atencion de los medicos */
INSERT INTO dbo.HorariosDisponiblidadMedicos
(
    IdMedico,
    DiaSemana,
    HoraDesde,
    HoraHasta
)
VALUES
(1, 1, '08:00', '14:00'),
(2, 2, '14:00', '20:00'),
(3, 3, '09:00', '13:00'),
(4, 4, '08:00', '12:00'),
(5, 5, '10:00', '16:00');
GO

/* Usuarios */
INSERT INTO dbo.Usuarios
(
    NombreUsuario,
    Email,
    PasswordHash,
    IdRol,
    IdMedico
)
VALUES
('admin', 'admin@clinica.local', 'hashadmin1', 1, NULL),
('recepcion', 'recepcion@clinica.local', 'hashrecepcion', 2, NULL),
('lgomez', 'laura.gomez@clinica.local', 'hashmedico1', 3, 1),
('mruiz', 'martin.ruiz@clinica.local', 'hasmedico2', 3, 2),
('vmolina', 'valeria.molina@clinica.local', 'hasmedico3', 3, 5);
GO

/* Turnos */
INSERT INTO dbo.Turnos
(
    NumeroTurno,
    IdPaciente,
    IdMedico,
    IdEspecialidad,
    IdEstadoTurno,
    FechaTurno,
    HoraInicio,
    HoraFin,
    Observaciones,
    DiagnosticoMedico,
    IdUsuarioAlta,
    FechaModificacion,
    IdUsuarioModificacion
)
VALUES
('T-000001', 1, 1, 1, 1, '2026-06-01', '09:00', '10:00', 'Dolor de cabeza constante', NULL, 2, NULL, NULL),
('T-000002', 2, 2, 3, 1, '2026-06-02', '14:00', '15:00', 'Dolor de muela', NULL, 2, NULL, NULL),
('T-000003', 3, 3, 4, 2, '2026-06-03', '10:00', '11:00', 'Control pediatrico reprogramado', NULL, 2, GETDATE(), 2),
('T-000004', 4, 4, 5, 3, '2026-06-04', '08:00', '09:00', 'Molestia en rodilla - turno cancelado', NULL, 2, GETDATE(), 2),
('T-000005', 5, 5, 2, 5, '2026-06-05', '10:00', '11:00', 'Control cardiologico', 'Paciente evaluada. Se solicita control posterior.', 2, GETDATE(), 5);
GO

SELECT 'Base de datos creada OK...' AS Resultado;
GO
