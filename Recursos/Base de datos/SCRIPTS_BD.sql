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

CREATE TABLE dbo.EstadosUsuario
(
    IdEstadoUsuario INT IDENTITY(1,1) NOT NULL,
    Nombre VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(200) NULL,
    Activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_EstadosUsuario PRIMARY KEY (IdEstadoUsuario),
    CONSTRAINT UQ_EstadosUsuario_Nombre UNIQUE (Nombre),
    CONSTRAINT CK_EstadosUsuario_Nombre_NoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0)
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

CREATE TABLE dbo.Personas
(
    IdPersona INT IDENTITY(1,1) NOT NULL,
    DNI VARCHAR(15) NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    Telefono VARCHAR(50) NULL,
    Email VARCHAR(150) NOT NULL,

    CONSTRAINT PK_Personas PRIMARY KEY (IdPersona),
    CONSTRAINT UQ_Personas_DNI UNIQUE (DNI),
    CONSTRAINT UQ_Personas_Email UNIQUE (Email),
    CONSTRAINT CK_Personas_DNI_NoVacio CHECK (LEN(LTRIM(RTRIM(DNI))) > 0),
    CONSTRAINT CK_Personas_Nombre_NoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0),
    CONSTRAINT CK_Personas_Apellido_NoVacio CHECK (LEN(LTRIM(RTRIM(Apellido))) > 0),
    CONSTRAINT CK_Personas_Email_Formato CHECK (Email LIKE '%_@_%._%')
);
GO

CREATE TABLE dbo.Pacientes
(
    IdPaciente INT IDENTITY(1,1) NOT NULL,
    IdPersona INT NOT NULL,
    FechaNacimiento DATE NOT NULL,
    Direccion VARCHAR(200) NULL,
    Activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_Pacientes PRIMARY KEY (IdPaciente),
    CONSTRAINT UQ_Pacientes_IdPersona UNIQUE (IdPersona),
    CONSTRAINT FK_Pacientes_Personas
        FOREIGN KEY (IdPersona)
        REFERENCES dbo.Personas(IdPersona)
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
    IdPersona INT NOT NULL,
    Matricula VARCHAR(30) NOT NULL,
    Activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT PK_Medicos PRIMARY KEY (IdMedico),
    CONSTRAINT UQ_Medicos_IdPersona UNIQUE (IdPersona),
    CONSTRAINT UQ_Medicos_Matricula UNIQUE (Matricula),
    CONSTRAINT CK_Medicos_Matricula_NoVacia CHECK (LEN(LTRIM(RTRIM(Matricula))) > 0),
    CONSTRAINT FK_Medicos_Personas
        FOREIGN KEY (IdPersona)
        REFERENCES dbo.Personas(IdPersona)
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

    CONSTRAINT PK_HorariosDisponiblidadMedicos PRIMARY KEY (IdHorarioDisponiblidadMedico),
    CONSTRAINT UQ_HorariosDisponiblidadMedicos_Horario UNIQUE (IdMedico, DiaSemana, HoraDesde, HoraHasta),

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
    IdPersona INT NOT NULL,
    NombreUsuario VARCHAR(50) NULL,
    PasswordHash VARCHAR(256) NULL,
    Imagen VARBINARY(MAX) NULL,
    IdRol INT NOT NULL,
    IdEstadoUsuario INT NOT NULL,

    CONSTRAINT PK_Usuarios PRIMARY KEY (IdUsuario),
    CONSTRAINT UQ_Usuarios_IdPersona UNIQUE (IdPersona),

    CONSTRAINT FK_Usuarios_Roles
        FOREIGN KEY (IdRol)
        REFERENCES dbo.Roles(IdRol),

    CONSTRAINT FK_Usuarios_EstadosUsuario
        FOREIGN KEY (IdEstadoUsuario)
        REFERENCES dbo.EstadosUsuario(IdEstadoUsuario),

    CONSTRAINT FK_Usuarios_Personas
        FOREIGN KEY (IdPersona)
        REFERENCES dbo.Personas(IdPersona)
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

    CONSTRAINT CK_Turnos_HorarioValido CHECK (HoraFin > HoraInicio),
    CONSTRAINT CK_Turnos_Observaciones_NoVacio CHECK (LEN(LTRIM(RTRIM(Observaciones))) > 0)
);
GO

-- DATOS DE PRUEBA
INSERT INTO dbo.Roles (Nombre, Descripcion)
VALUES
('Administrador', 'Acceso total al sistema'),
('Recepcionista', 'Gestiona pacientes, medicos y turnos'),
('Medico', 'Consulta sus turnos y carga diagnosticos'),
('Paciente', 'Consulta sus turnos y registra informacion basica');
GO

INSERT INTO dbo.EstadosUsuario (Nombre, Descripcion)
VALUES
('Pendiente', 'Cuenta creada pero aun no activada'),
('Activo', 'Cuenta habilitada para uso normal'),
('Bloqueado', 'Cuenta bloqueada temporalmente'),
('Inactivo', 'Cuenta deshabilitada'),
('CambioClavePendiente', 'Debe cambiar la clave en el proximo ingreso');
GO

INSERT INTO dbo.EstadosTurno (Nombre, Descripcion, EsFinal)
VALUES
('Nuevo', 'Turno asignado', 0),
('Reprogramado', 'Turno reprogramado', 0),
('Cancelado', 'Turno cancelado', 1),
('NoAsistio', 'Paciente no se presento', 1),
('Cerrado', 'Turno atendido y cerrado', 1);
GO

INSERT INTO dbo.ConfiguracionesTurno (DuracionMinutos)
VALUES
(60);
GO

INSERT INTO dbo.Especialidades (Nombre, Descripcion)
VALUES
('Clinica Medica', 'Atencion general'),
('Cardiologia', 'Atencion de patologias cardiovasculares'),
('Odontologia', 'Atencion odontologica'),
('Pediatria', 'Atencion medica infantil'),
('Traumatologia', 'Atencion de lesiones oseas y musculares');
GO

INSERT INTO dbo.Personas
(
    DNI,
    Nombre,
    Apellido,
    Telefono,
    Email
)
VALUES
('30111222', 'Juan', 'Perez', '1123456789', 'juan.perez@mail.com'),
('33222333', 'Maria', 'Lopez', '1134567890', 'maria.lopez@mail.com'),
('28333444', 'Carlos', 'Diaz', '1145678901', 'carlos.diaz@mail.com'),
('40555666', 'Sofia', 'Acosta', '1156789012', 'sofia.acosta@mail.com'),
('35666777', 'Lucia', 'Martinez', '1167890123', 'lucia.martinez@mail.com');
GO

INSERT INTO dbo.Pacientes
(
    IdPersona,
    FechaNacimiento,
    Direccion
)
VALUES
 (1, '1985-04-12', 'Av San Martin 1234'),
 (2, '1990-09-23', 'Belgrano 455'),
 (3, '1978-01-05', 'Mitre 789'),
 (4, '2001-11-18', 'Rivadavia 2300'),
 (5, '1994-07-30', 'Moreno 150');
GO

INSERT INTO dbo.Personas
(
    DNI,
    Nombre,
    Apellido,
    Telefono,
    Email
)
VALUES
('20111222', 'Laura', 'Gomez', '1122001100', 'laura.gomez@clinica.local'),
('21222333', 'Martin', 'Ruiz', '1133002200', 'martin.ruiz@clinica.local'),
('22333444', 'Ana', 'Torres', '1144003300', 'ana.torres@clinica.local'),
('23444555', 'Diego', 'Salas', '1155004400', 'diego.salas@clinica.local'),
('24555666', 'Valeria', 'Molina', '1166005500', 'valeria.molina@clinica.local');
GO

INSERT INTO dbo.Medicos
(
    IdPersona,
    Matricula
)
VALUES
(6, 'MN-10001'),
(7, 'MN-10002'),
(8, 'MN-10003'),
(9, 'MN-10004'),
(10, 'MN-10005');
GO

INSERT INTO dbo.Personas
(
    DNI,
    Nombre,
    Apellido,
    Telefono,
    Email
)
VALUES
('30100001', 'Admin', 'Sistema', '1111111111', 'admin@clinica.local'),
('30100002', 'Recepcion', 'Clinica', '1111111112', 'recepcion@clinica.local');
GO

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

INSERT INTO dbo.Usuarios
(
    IdPersona,
    NombreUsuario,
    PasswordHash,
    Imagen,
    IdRol,
    IdEstadoUsuario
)
VALUES
(11, 'admin', '3b612c75a7b5048a435fb6ec81e52ff92d6d795a8b5a9c17070f6a63c97a53b2', NULL, 1, 2), -- La clave sin encriptar para pruebitas es: Admin123
(12, 'recepcion', '0c01954c0f4f6bbda12d86eaecbd6a524225a0cad0fac52e6aaf4c237f7f9cbe', NULL, 2, 2), -- La clave sin encriptar para pruebitas es: Recep123
(6, 'medico1', '4537a9fc80c4e8dfc60b7f4728fa77d654fa730ded77f2d201091a3418a27b93', NULL, 3, 2), -- La clave sin encriptar para pruebitas es: Medico123
(7, 'medico2', '4537a9fc80c4e8dfc60b7f4728fa77d654fa730ded77f2d201091a3418a27b93', NULL, 3, 2), -- La clave sin encriptar para pruebitas es: Medico123
(8, 'medico3', '4537a9fc80c4e8dfc60b7f4728fa77d654fa730ded77f2d201091a3418a27b93', NULL, 3, 2), -- La clave sin encriptar para pruebitas es: Medico123
(9, 'medico4', '4537a9fc80c4e8dfc60b7f4728fa77d654fa730ded77f2d201091a3418a27b93', NULL, 3, 2), -- La clave sin encriptar para pruebitas es: Medico123
(10, 'medico5', '4537a9fc80c4e8dfc60b7f4728fa77d654fa730ded77f2d201091a3418a27b93', NULL, 3, 2), -- La clave sin encriptar para pruebitas es: Medico123
(1, 'paciente1', '1d11d88472cc3a3b6e3f3df865f0ca13716e1c0d4552a1f4d8c9bc429fa2ceca', NULL, 4, 2), -- La clave sin encriptar para pruebitas es: Paciente123
(2, 'paciente2', '1d11d88472cc3a3b6e3f3df865f0ca13716e1c0d4552a1f4d8c9bc429fa2ceca', NULL, 4, 2), -- La clave sin encriptar para pruebitas es: Paciente123
(3, 'paciente3', '1d11d88472cc3a3b6e3f3df865f0ca13716e1c0d4552a1f4d8c9bc429fa2ceca', NULL, 4, 2), -- La clave sin encriptar para pruebitas es: Paciente123
(4, 'paciente4', '1d11d88472cc3a3b6e3f3df865f0ca13716e1c0d4552a1f4d8c9bc429fa2ceca', NULL, 4, 2), -- La clave sin encriptar para pruebitas es: Paciente123
(5, 'paciente5', '1d11d88472cc3a3b6e3f3df865f0ca13716e1c0d4552a1f4d8c9bc429fa2ceca', NULL, 4, 2); -- La clave sin encriptar para pruebitas es: Paciente123
GO

INSERT INTO dbo.Turnos
(
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
(1, 1, 1, 1, '2026-06-01', '09:00', '10:00', 'Dolor de cabeza constante', NULL, 2, NULL, NULL), -- Numero de turno: T-001
(2, 2, 3, 1, '2026-06-02', '14:00', '15:00', 'Dolor de muela', NULL, 2, NULL, NULL), -- Numero de turno: T-002
(3, 3, 4, 2, '2026-06-03', '10:00', '11:00', 'Control pediatrico reprogramado', NULL, 2, GETDATE(), 2), -- Numero de turno: T-003
(4, 4, 5, 3, '2026-06-04', '08:00', '09:00', 'Molestia en rodilla - turno cancelado', NULL, 2, GETDATE(), 2), -- Numero de turno: T-004
(5, 5, 2, 5, '2026-06-05', '10:00', '11:00', 'Control cardiologico', 'Paciente evaluada. Se solicita control posterior.', 2, GETDATE(), 5); -- Numero de turno: T-005
GO

SELECT 'Base de datos creada OK...' AS Resultado;
GO

