/*
   BASE DE DATOS: TPC_CLINICA_MEDICA_P3_DB
   SQL Server Express
   PROYECTO: UTN FRGP TUP - TPC Programacion III - Sistema de Gestion de Clinica
*/

USE master;
GO

IF DB_ID(N'TPC_CLINICA_MEDICA_P3_DB') IS NOT NULL
BEGIN
    ALTER DATABASE TPC_CLINICA_MEDICA_P3_DB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE TPC_CLINICA_MEDICA_P3_DB;
END
GO

CREATE DATABASE TPC_CLINICA_MEDICA_P3_DB;
GO

USE TPC_CLINICA_MEDICA_P3_DB;
GO

/* -------------
   1 - ROLES
   ------------- */

CREATE TABLE dbo.Rol
(
    IdRol INT IDENTITY(1,1) NOT NULL,
    Nombre VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(200) NULL,
    Activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_Rol PRIMARY KEY (IdRol),
    CONSTRAINT UQ_Rol_Nombre UNIQUE (Nombre),
    CONSTRAINT CK_Rol_Nombre_NoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0)
);
GO

/* -------------
   2 - TURNOS DE TRABAJO
   ------------- */

CREATE TABLE dbo.TurnoTrabajo
(
    IdTurnoTrabajo INT IDENTITY(1,1) NOT NULL,
    Nombre VARCHAR(80) NOT NULL,
    HoraEntrada TIME NOT NULL,
    HoraSalida TIME NOT NULL,
    Activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_TurnoTrabajo PRIMARY KEY (IdTurnoTrabajo),
    CONSTRAINT UQ_TurnoTrabajo_Nombre UNIQUE (Nombre),
    CONSTRAINT CK_TurnoTrabajo_Nombre_NoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0),
    CONSTRAINT CK_TurnoTrabajo_HorarioValido CHECK (HoraSalida > HoraEntrada)
);
GO

/* -------------
   3 - CONFIGURACION DE TURNO
   ------------- */

CREATE TABLE dbo.ConfiguracionTurno
(
    IdConfiguracionTurno INT IDENTITY(1,1) NOT NULL,
    DuracionMinutos INT NOT NULL,
    Activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_ConfiguracionTurno PRIMARY KEY (IdConfiguracionTurno),
    CONSTRAINT CK_ConfiguracionTurno_DuracionMinutos_Valida CHECK (DuracionMinutos > 0)
);
GO

/* -------------
   4 - PACIENTES
   ------------- */

CREATE TABLE dbo.Paciente
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

    CONSTRAINT PK_Paciente PRIMARY KEY (IdPaciente),
    CONSTRAINT UQ_Paciente_DNI UNIQUE (DNI),
    CONSTRAINT CK_Paciente_DNI_NoVacio CHECK (LEN(LTRIM(RTRIM(DNI))) > 0),
    CONSTRAINT CK_Paciente_Nombre_NoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0),
    CONSTRAINT CK_Paciente_Apellido_NoVacio CHECK (LEN(LTRIM(RTRIM(Apellido))) > 0),
    CONSTRAINT CK_Paciente_Email_Formato CHECK (Email LIKE '%_@_%._%')
);
GO

/* -------------
   5 - ESPECIALIDADES
   ------------- */

CREATE TABLE dbo.Especialidad
(
    IdEspecialidad INT IDENTITY(1,1) NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(300) NULL,
    Activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_Especialidad PRIMARY KEY (IdEspecialidad),
    CONSTRAINT UQ_Especialidad_Nombre UNIQUE (Nombre),
    CONSTRAINT CK_Especialidad_Nombre_NoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0)
);
GO

/* -------------
   6 - MEDICOS
   ------------- */

CREATE TABLE dbo.Medico
(
    IdMedico INT IDENTITY(1,1) NOT NULL,
    Matricula VARCHAR(30) NOT NULL,
    DNI VARCHAR(15) NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    Telefono VARCHAR(50) NULL,
    Email VARCHAR(150) NOT NULL,
    IdTurnoTrabajo INT NOT NULL,
    Activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_Medico PRIMARY KEY (IdMedico),
    CONSTRAINT UQ_Medico_Matricula UNIQUE (Matricula),
    CONSTRAINT UQ_Medico_DNI UNIQUE (DNI),
    CONSTRAINT CK_Medico_Matricula_NoVacia CHECK (LEN(LTRIM(RTRIM(Matricula))) > 0),
    CONSTRAINT CK_Medico_DNI_NoVacio CHECK (LEN(LTRIM(RTRIM(DNI))) > 0),
    CONSTRAINT CK_Medico_Nombre_NoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0),
    CONSTRAINT CK_Medico_Apellido_NoVacio CHECK (LEN(LTRIM(RTRIM(Apellido))) > 0),
    CONSTRAINT CK_Medico_Email_Formato CHECK (Email LIKE '%_@_%._%'),

    CONSTRAINT FK_Medico_TurnoTrabajo
        FOREIGN KEY (IdTurnoTrabajo)
        REFERENCES dbo.TurnoTrabajo(IdTurnoTrabajo)
);
GO

/* -------------
   7 - MEDICO - ESPECIALIDAD
   ------------- */

CREATE TABLE dbo.MedicoEspecialidad
(
    IdMedico INT NOT NULL,
    IdEspecialidad INT NOT NULL,

    CONSTRAINT PK_MedicoEspecialidad PRIMARY KEY (IdMedico, IdEspecialidad),

    CONSTRAINT FK_MedicoEspecialidad_Medico
        FOREIGN KEY (IdMedico)
        REFERENCES dbo.Medico(IdMedico),

    CONSTRAINT FK_MedicoEspecialidad_Especialidad
        FOREIGN KEY (IdEspecialidad)
        REFERENCES dbo.Especialidad(IdEspecialidad)
);
GO

/* -------------
   8 - DIA DE ATENCION DEL MEDICO
   ------------- */

CREATE TABLE dbo.DiaAtencionMedico
(
    IdDiaAtencionMedico INT IDENTITY(1,1) NOT NULL,
    IdMedico INT NOT NULL,
    DiaSemana TINYINT NOT NULL,
    HoraDesde TIME NOT NULL,
    HoraHasta TIME NOT NULL,
    Activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_DiaAtencionMedico PRIMARY KEY (IdDiaAtencionMedico),

    CONSTRAINT FK_DiaAtencionMedico_Medico
        FOREIGN KEY (IdMedico)
        REFERENCES dbo.Medico(IdMedico),

    CONSTRAINT CK_DiaAtencionMedico_DiaSemana_Valido CHECK (DiaSemana BETWEEN 1 AND 7),
    CONSTRAINT CK_DiaAtencionMedico_HorarioValido CHECK (HoraHasta > HoraDesde)
);
GO

/* -------------
   9 - USUARIOS
   ------------- */

CREATE TABLE dbo.Usuario
(
    IdUsuario INT IDENTITY(1,1) NOT NULL,
    NombreUsuario VARCHAR(50) NOT NULL,
    Email VARCHAR(150) NOT NULL,
    PasswordHash VARCHAR(256) NOT NULL,
    IdRol INT NOT NULL,
    IdMedico INT NULL,
    Activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_Usuario PRIMARY KEY (IdUsuario),
    CONSTRAINT UQ_Usuario_NombreUsuario UNIQUE (NombreUsuario),
    CONSTRAINT UQ_Usuario_Email UNIQUE (Email),
    CONSTRAINT CK_Usuario_NombreUsuario_NoVacio CHECK (LEN(LTRIM(RTRIM(NombreUsuario))) > 0),
    CONSTRAINT CK_Usuario_Email_Formato CHECK (Email LIKE '%_@_%._%'),
    CONSTRAINT CK_Usuario_PasswordHash_NoVacio CHECK (LEN(LTRIM(RTRIM(PasswordHash))) > 0),

    CONSTRAINT FK_Usuario_Rol
        FOREIGN KEY (IdRol)
        REFERENCES dbo.Rol(IdRol),

    CONSTRAINT FK_Usuario_Medico
        FOREIGN KEY (IdMedico)
        REFERENCES dbo.Medico(IdMedico)
);
GO

/* -------------
   10 - ESTADOS DE TURNO
   ------------- */

CREATE TABLE dbo.EstadoTurno
(
    IdEstadoTurno INT IDENTITY(1,1) NOT NULL,
    Nombre VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(200) NULL,
    EsFinal BIT NOT NULL DEFAULT 0,
    Activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_EstadoTurno PRIMARY KEY (IdEstadoTurno),
    CONSTRAINT UQ_EstadoTurno_Nombre UNIQUE (Nombre),
    CONSTRAINT CK_EstadoTurno_Nombre_NoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0)
);
GO

/* -------------
   11 - TURNOS
   ------------- */

CREATE TABLE dbo.Turno
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

    CONSTRAINT PK_Turno PRIMARY KEY (IdTurno),
    CONSTRAINT UQ_Turno_NumeroTurno UNIQUE (NumeroTurno),

    CONSTRAINT FK_Turno_Paciente
        FOREIGN KEY (IdPaciente)
        REFERENCES dbo.Paciente(IdPaciente),

    CONSTRAINT FK_Turno_Medico
        FOREIGN KEY (IdMedico)
        REFERENCES dbo.Medico(IdMedico),

    CONSTRAINT FK_Turno_Especialidad
        FOREIGN KEY (IdEspecialidad)
        REFERENCES dbo.Especialidad(IdEspecialidad),

    CONSTRAINT FK_Turno_EstadoTurno
        FOREIGN KEY (IdEstadoTurno)
        REFERENCES dbo.EstadoTurno(IdEstadoTurno),

    CONSTRAINT FK_Turno_UsuarioAlta
        FOREIGN KEY (IdUsuarioAlta)
        REFERENCES dbo.Usuario(IdUsuario),

    CONSTRAINT FK_Turno_UsuarioModificacion
        FOREIGN KEY (IdUsuarioModificacion)
        REFERENCES dbo.Usuario(IdUsuario),

    CONSTRAINT CK_Turno_NumeroTurno_NoVacio CHECK (LEN(LTRIM(RTRIM(NumeroTurno))) > 0),
    CONSTRAINT CK_Turno_HorarioValido CHECK (HoraFin > HoraInicio),
    CONSTRAINT CK_Turno_Observaciones_NoVacio CHECK (LEN(LTRIM(RTRIM(Observaciones))) > 0)
);
GO

/* -------------
   DATOS DE PRUEBA
   ------------- */

/* Roles necesarios segun el enunciado */
INSERT INTO dbo.Rol (Nombre, Descripcion)
VALUES
('Administrador', 'Acceso total al sistema'),
('Recepcionista', 'Gestiona pacientes, medicos y turnos'),
('Medico', 'Consulta sus turnos y carga diagnosticos');
GO

/* Estados necesarios para el ciclo de vida del turno */
INSERT INTO dbo.EstadoTurno (Nombre, Descripcion, EsFinal)
VALUES
('Nuevo', 'Turno asignado', 0),
('Reprogramado', 'Turno reprogramado', 0),
('Cancelado', 'Turno cancelado', 1),
('NoAsistio', 'Paciente ausente', 1),
('Cerrado', 'Turno atendido y cerrado', 1);
GO

/* Turnos de trabajo */
INSERT INTO dbo.TurnoTrabajo (Nombre, HoraEntrada, HoraSalida)
VALUES
('Maniana', '08:00', '14:00'),
('Tarde', '14:00', '20:00'),
('Jornada Completa', '08:00', '20:00'),
('Intermedio', '10:00', '16:00'),
('Noche', '18:00', '23:00');
GO

/* Configuracion global de duracion de turnos */
INSERT INTO dbo.ConfiguracionTurno (DuracionMinutos)
VALUES
(60);
GO

/* Especialidades */
INSERT INTO dbo.Especialidad (Nombre, Descripcion)
VALUES
('Clinica Medica', 'Atencion general'),
('Cardiologia', 'Atencion de patologias cardiovasculares'),
('Odontologia', 'Atencion odontologica'),
('Pediatria', 'Atencion medica infantil'),
('Traumatologia', 'Atencion de lesiones oseas y musculares');
GO

/* Pacientes */
INSERT INTO dbo.Paciente
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
INSERT INTO dbo.Medico
(
    Matricula,
    DNI,
    Nombre,
    Apellido,
    Telefono,
    Email,
    IdTurnoTrabajo
)
VALUES
('MN-10001', '20111222', 'Laura', 'Gomez', '1122001100', 'laura.gomez@clinica.local', 1),
('MN-10002', '21222333', 'Martin', 'Ruiz', '1133002200', 'martin.ruiz@clinica.local', 2),
('MN-10003', '22333444', 'Ana', 'Torres', '1144003300', 'ana.torres@clinica.local', 3),
('MN-10004', '23444555', 'Diego', 'Salas', '1155004400', 'diego.salas@clinica.local', 1),
('MN-10005', '24555666', 'Valeria', 'Molina', '1166005500', 'valeria.molina@clinica.local', 4);
GO

/* Relacion medico - especialidad */
INSERT INTO dbo.MedicoEspecialidad
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
INSERT INTO dbo.DiaAtencionMedico
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
INSERT INTO dbo.Usuario
(
    NombreUsuario,
    Email,
    PasswordHash,
    IdRol,
    IdMedico
)
VALUES
('admin', 'admin@clinica.local', 'HASH_ADMIN', 1, NULL),
('recepcion', 'recepcion@clinica.local', 'HASH_RECEPCION', 2, NULL),
('lgomez', 'laura.gomez@clinica.local', 'HASH_MEDICO_1', 3, 1),
('mruiz', 'martin.ruiz@clinica.local', 'HASH_MEDICO_2', 3, 2),
('vmolina', 'valeria.molina@clinica.local', 'HASH_MEDICO_3', 3, 5);
GO

/* Turnos */
INSERT INTO dbo.Turno
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
