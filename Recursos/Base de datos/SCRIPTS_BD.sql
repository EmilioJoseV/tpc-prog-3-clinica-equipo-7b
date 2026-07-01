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

-- Trigger para validar solapamientos de turnos por medico y paciente
CREATE TRIGGER TR_Turnos_ValidarSolapamientos
ON dbo.Turnos
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM inserted I
        INNER JOIN dbo.EstadosTurno EI
            ON EI.IdEstadoTurno = I.IdEstadoTurno
        INNER JOIN dbo.Turnos T
            ON T.IdMedico = I.IdMedico
            AND T.FechaTurno = I.FechaTurno
            AND I.HoraInicio < T.HoraFin
            AND T.HoraInicio < I.HoraFin
            AND T.IdTurno <> I.IdTurno
        INNER JOIN dbo.EstadosTurno ET
            ON ET.IdEstadoTurno = T.IdEstadoTurno
        WHERE EI.EsFinal = 0
            AND ET.EsFinal = 0
    )
    BEGIN
        THROW 50001, 'El medico ya tiene un turno en ese horario', 1;
    END;

    IF EXISTS
    (
        SELECT 1
        FROM inserted I
        INNER JOIN dbo.EstadosTurno EI
            ON EI.IdEstadoTurno = I.IdEstadoTurno
        INNER JOIN dbo.Turnos T
            ON T.IdPaciente = I.IdPaciente
            AND T.FechaTurno = I.FechaTurno
            AND I.HoraInicio < T.HoraFin
            AND T.HoraInicio < I.HoraFin
            AND T.IdTurno <> I.IdTurno
        INNER JOIN dbo.EstadosTurno ET
            ON ET.IdEstadoTurno = T.IdEstadoTurno
        WHERE EI.EsFinal = 0
            AND ET.EsFinal = 0
    )
    BEGIN
        THROW 50002, 'El paciente ya tiene un turno en ese horario', 1;
    END;
END;
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
('Clinica Medica', 'Atencion general de adultos'),
('Cardiologia', 'Atencion a problemas del corazon'),
('Odontologia', 'Atencion de dientes, muelas y encias'),
('Pediatria', 'Atencion de ninos y adolescentes'),
('Traumatologia', 'Atencion de golpes, fracturas y lesiones'),
('Dermatologia', 'Atencion de piel, manchas y lunares'),
('Oftalmologia', 'Atencion de vista y ojos'),
('Neurologia', 'Atencion de cerebro, nervios y dolores raros'),
('Gastroenterologia', 'Atencion de panza, digestion y acidez'),
('Kinesiologia', 'Atencion de rehabilitacion y movilidad'),
('Endocrinologia', 'Atencion de hormonas y metabolismo'),
('Otorrinolaringologia', 'Atencion de oido, nariz y garganta');
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
('50100001', 'Clark', 'Kent', '1150100001', 'clark.kent@mail.com'),
('50100002', 'Will', 'Smith', '1150100002', 'will.smith@mail.com'),
('50100003', 'John', 'Doe', '1150100003', 'john.doe@mail.com'),
('50100004', 'Satoshi', 'Nakamoto', '1150100004', 'satoshi.nakamoto@mail.com'),
('50100005', 'Bill', 'Gates', '1150100005', 'bill.gates@mail.com'),
('50100006', 'Marco Antonio', 'Solis', '1150100006', 'marco.solis@mail.com'),
('50100007', 'Bruce', 'Wayne', '1150100007', 'bruce.wayne@mail.com'),
('50100008', 'Diana', 'Prince', '1150100008', 'diana.prince@mail.com'),
('50100009', 'Peter', 'Parker', '1150100009', 'peter.parker@mail.com'),
('50100010', 'Tony', 'Stark', '1150100010', 'tony.stark@mail.com'),
('50100011', 'Steve', 'Rogers', '1150100011', 'steve.rogers@mail.com'),
('50100012', 'Natasha', 'Romanoff', '1150100012', 'natasha.romanoff@mail.com'),
('50100013', 'Pepe', 'Argento', '1150100013', 'pepe.argento@mail.com'),
('50100014', 'Moni', 'Argento', '1150100014', 'moni.argento@mail.com'),
('50100015', 'Don', 'Ramon', '1150100015', 'don.ramon@mail.com'),
('50100016', 'Marta', 'Fierro', '1150100016', 'marta.fierro@mail.com'),
('50100017', 'Sherlock', 'Holmes', '1150100017', 'sherlock.holmes@mail.com'),
('50100018', 'Sarah', 'Connor', '1150100018', 'sarah.connor@mail.com'),
('50200001', 'Gregorio', 'House', '1150200001', 'gregorio.house@clinica.local'),
('50200002', 'Ada', 'Lovelace', '1150200002', 'ada.lovelace@clinica.local'),
('50200003', 'Florinda', 'Madrigal', '1150200003', 'florinda.madrigal@clinica.local'),
('50200004', 'Cacho', 'Buenavista', '1150200004', 'cacho.buenavista@clinica.local'),
('50200005', 'Nora', 'Lenteja', '1150200005', 'nora.lenteja@clinica.local'),
('50200006', 'Emmett', 'Brown', '1150200006', 'emmett.brown@clinica.local'),
('50200007', 'Lisa', 'Simpson', '1150200007', 'lisa.simpson@clinica.local'),
('50200008', 'Meredith', 'Grey', '1150200008', 'meredith.grey@clinica.local'),
('50300001', 'Admin', 'Sistema', '1150300001', 'admin@clinica.local'),
('50300002', 'Ramona', 'Mostrador', '1150300002', 'recepcion@clinica.local'),
('50300003', 'Anacleto', 'Sistema', '1150300003', 'anacleto.sistema@clinica.local'),
('50300004', 'Pancho', 'Pendiente', '1150300004', 'pancho.pendiente@clinica.local'),
('50300005', 'Berta', 'Clave', '1150300005', 'berta.clave@clinica.local'),
('50300006', 'Beto', 'Bloqueado', '1150300006', 'beto.bloqueado@clinica.local'),
('50300007', 'Lola', 'Inactiva', '1150300007', 'lola.inactiva@clinica.local'),
('50300008', 'Fermin', 'Sinmatricula', '1150300008', 'fermin.sinmatricula@clinica.local');
GO

INSERT INTO dbo.Pacientes
(
    IdPersona,
    FechaNacimiento,
    Direccion
)
VALUES
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50100001'), '1980-06-18', 'Av Metropolis 1938'),
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50100002'), '1968-09-25', 'Bel Air 1990'),
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50100003'), '1990-01-01', 'Calle Generica 123'),
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50100004'), '1975-04-05', 'Blockchain 21'),
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50100005'), '1955-10-28', 'Windows 95'),
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50100006'), '1959-12-29', 'Av Cancion Triste 100'),
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50100007'), '1972-02-19', 'Mansion Wayne 1'),
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50100008'), '1984-03-22', 'Isla Paraiso 8'),
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50100009'), '2001-08-10', 'Queens 15'),
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50100010'), '1970-05-29', 'Torre Stark 3000'),
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50100011'), '1918-07-04', 'Brooklyn 40'),
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50100012'), '1984-11-22', 'Budapest 77'),
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50100013'), '1972-05-14', 'Pasaje Campeon 100'),
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50100014'), '1975-08-22', 'Calle Liquidacion 222'),
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50100015'), '1964-09-02', 'Vecindad 71'),
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50100016'), '1981-12-01', 'San Martin 1810'),
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50100017'), '1976-01-06', 'Baker Street 221B'),
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50100018'), '1979-11-13', 'Resistencia 2029');
GO

INSERT INTO dbo.Medicos
(
    IdPersona,
    Matricula
)
VALUES
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50200001'), 'MN-20001'),
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50200002'), 'MN-20002'),
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50200003'), 'MN-20003'),
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50200004'), 'MN-20004'),
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50200005'), 'MN-20005'),
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50200006'), 'MN-20006'),
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50200007'), 'MN-20007'),
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50200008'), 'MN-20008');
GO

DECLARE @IdClinica INT = (SELECT IdEspecialidad FROM dbo.Especialidades WHERE Nombre = 'Clinica Medica');
DECLARE @IdCardiologia INT = (SELECT IdEspecialidad FROM dbo.Especialidades WHERE Nombre = 'Cardiologia');
DECLARE @IdOdontologia INT = (SELECT IdEspecialidad FROM dbo.Especialidades WHERE Nombre = 'Odontologia');
DECLARE @IdPediatria INT = (SELECT IdEspecialidad FROM dbo.Especialidades WHERE Nombre = 'Pediatria');
DECLARE @IdTraumatologia INT = (SELECT IdEspecialidad FROM dbo.Especialidades WHERE Nombre = 'Traumatologia');
DECLARE @IdDermatologia INT = (SELECT IdEspecialidad FROM dbo.Especialidades WHERE Nombre = 'Dermatologia');
DECLARE @IdOftalmologia INT = (SELECT IdEspecialidad FROM dbo.Especialidades WHERE Nombre = 'Oftalmologia');
DECLARE @IdNeurologia INT = (SELECT IdEspecialidad FROM dbo.Especialidades WHERE Nombre = 'Neurologia');
DECLARE @IdGastro INT = (SELECT IdEspecialidad FROM dbo.Especialidades WHERE Nombre = 'Gastroenterologia');
DECLARE @IdKinesiologia INT = (SELECT IdEspecialidad FROM dbo.Especialidades WHERE Nombre = 'Kinesiologia');
DECLARE @IdEndocrino INT = (SELECT IdEspecialidad FROM dbo.Especialidades WHERE Nombre = 'Endocrinologia');
DECLARE @IdOtorrino INT = (SELECT IdEspecialidad FROM dbo.Especialidades WHERE Nombre = 'Otorrinolaringologia');
DECLARE @IdHouse INT = (SELECT IdMedico FROM dbo.Medicos WHERE Matricula = 'MN-20001');
DECLARE @IdAda INT = (SELECT IdMedico FROM dbo.Medicos WHERE Matricula = 'MN-20002');
DECLARE @IdFlorinda INT = (SELECT IdMedico FROM dbo.Medicos WHERE Matricula = 'MN-20003');
DECLARE @IdCacho INT = (SELECT IdMedico FROM dbo.Medicos WHERE Matricula = 'MN-20004');
DECLARE @IdNora INT = (SELECT IdMedico FROM dbo.Medicos WHERE Matricula = 'MN-20005');
DECLARE @IdBrown INT = (SELECT IdMedico FROM dbo.Medicos WHERE Matricula = 'MN-20006');
DECLARE @IdLisa INT = (SELECT IdMedico FROM dbo.Medicos WHERE Matricula = 'MN-20007');
DECLARE @IdGrey INT = (SELECT IdMedico FROM dbo.Medicos WHERE Matricula = 'MN-20008');

INSERT INTO dbo.MedicosEspecialidades
(
    IdMedico,
    IdEspecialidad
)
VALUES
(@IdHouse, @IdClinica),
(@IdHouse, @IdNeurologia),
(@IdAda, @IdCardiologia),
(@IdAda, @IdEndocrino),
(@IdFlorinda, @IdPediatria),
(@IdFlorinda, @IdOtorrino),
(@IdCacho, @IdTraumatologia),
(@IdCacho, @IdKinesiologia),
(@IdNora, @IdOftalmologia),
(@IdNora, @IdDermatologia),
(@IdBrown, @IdOdontologia),
(@IdBrown, @IdGastro),
(@IdLisa, @IdPediatria),
(@IdLisa, @IdClinica),
(@IdGrey, @IdDermatologia),
(@IdGrey, @IdCardiologia);
GO

DECLARE @AyerHorario DATE = DATEADD(DAY, -1, CAST(GETDATE() AS DATE));
DECLARE @HoyHorario DATE = CAST(GETDATE() AS DATE);
DECLARE @MananaHorario DATE = DATEADD(DAY, 1, @HoyHorario);
DECLARE @MasDosHorario DATE = DATEADD(DAY, 2, @HoyHorario);
DECLARE @MasTresHorario DATE = DATEADD(DAY, 3, @HoyHorario);
DECLARE @MasCuatroHorario DATE = DATEADD(DAY, 4, @HoyHorario);
DECLARE @MasCincoHorario DATE = DATEADD(DAY, 5, @HoyHorario);
DECLARE @DiaAyer TINYINT = CONVERT(TINYINT, (DATEDIFF(DAY, '19000101', @AyerHorario) % 7) + 1);
DECLARE @DiaHoy TINYINT = CONVERT(TINYINT, (DATEDIFF(DAY, '19000101', @HoyHorario) % 7) + 1);
DECLARE @DiaManana TINYINT = CONVERT(TINYINT, (DATEDIFF(DAY, '19000101', @MananaHorario) % 7) + 1);
DECLARE @DiaMasDos TINYINT = CONVERT(TINYINT, (DATEDIFF(DAY, '19000101', @MasDosHorario) % 7) + 1);
DECLARE @DiaMasTres TINYINT = CONVERT(TINYINT, (DATEDIFF(DAY, '19000101', @MasTresHorario) % 7) + 1);
DECLARE @DiaMasCuatro TINYINT = CONVERT(TINYINT, (DATEDIFF(DAY, '19000101', @MasCuatroHorario) % 7) + 1);
DECLARE @DiaMasCinco TINYINT = CONVERT(TINYINT, (DATEDIFF(DAY, '19000101', @MasCincoHorario) % 7) + 1);

INSERT INTO dbo.HorariosDisponiblidadMedicos
(
    IdMedico,
    DiaSemana,
    HoraDesde,
    HoraHasta
)
SELECT M.IdMedico, D.DiaSemana, M.HoraDesde, M.HoraHasta
FROM
(
    VALUES
    (@DiaAyer),
    (@DiaHoy),
    (@DiaManana),
    (@DiaMasDos),
    (@DiaMasTres),
    (@DiaMasCuatro),
    (@DiaMasCinco)
) D(DiaSemana)
CROSS JOIN
(
    VALUES
    ((SELECT IdMedico FROM dbo.Medicos WHERE Matricula = 'MN-20001'), CAST('09:00' AS TIME), CAST('15:00' AS TIME)),
    ((SELECT IdMedico FROM dbo.Medicos WHERE Matricula = 'MN-20002'), CAST('10:00' AS TIME), CAST('16:00' AS TIME)),
    ((SELECT IdMedico FROM dbo.Medicos WHERE Matricula = 'MN-20003'), CAST('08:00' AS TIME), CAST('14:00' AS TIME)),
    ((SELECT IdMedico FROM dbo.Medicos WHERE Matricula = 'MN-20004'), CAST('11:00' AS TIME), CAST('17:00' AS TIME)),
    ((SELECT IdMedico FROM dbo.Medicos WHERE Matricula = 'MN-20005'), CAST('09:00' AS TIME), CAST('13:00' AS TIME)),
    ((SELECT IdMedico FROM dbo.Medicos WHERE Matricula = 'MN-20006'), CAST('14:00' AS TIME), CAST('20:00' AS TIME)),
    ((SELECT IdMedico FROM dbo.Medicos WHERE Matricula = 'MN-20007'), CAST('12:00' AS TIME), CAST('18:00' AS TIME)),
    ((SELECT IdMedico FROM dbo.Medicos WHERE Matricula = 'MN-20008'), CAST('08:00' AS TIME), CAST('12:00' AS TIME))
) M(IdMedico, HoraDesde, HoraHasta);
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
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50300001'), 'admin', '3b612c75a7b5048a435fb6ec81e52ff92d6d795a8b5a9c17070f6a63c97a53b2', NULL, 1, 2), -- Admin123
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50300002'), 'recepcion', '0c01954c0f4f6bbda12d86eaecbd6a524225a0cad0fac52e6aaf4c237f7f9cbe', NULL, 2, 2), -- Recep123
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50300003'), 'admin.anacleto', '3b612c75a7b5048a435fb6ec81e52ff92d6d795a8b5a9c17070f6a63c97a53b2', NULL, 1, 2), -- Admin123
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50200001'), 'dr.house', '4537a9fc80c4e8dfc60b7f4728fa77d654fa730ded77f2d201091a3418a27b93', NULL, 3, 2), -- Medico123
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50200002'), 'dra.ada', '4537a9fc80c4e8dfc60b7f4728fa77d654fa730ded77f2d201091a3418a27b93', NULL, 3, 2), -- Medico123
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50200003'), 'dra.florinda', '4537a9fc80c4e8dfc60b7f4728fa77d654fa730ded77f2d201091a3418a27b93', NULL, 3, 2), -- Medico123
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50200004'), 'dr.cacho', '4537a9fc80c4e8dfc60b7f4728fa77d654fa730ded77f2d201091a3418a27b93', NULL, 3, 2), -- Medico123
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50200005'), 'dra.nora', '4537a9fc80c4e8dfc60b7f4728fa77d654fa730ded77f2d201091a3418a27b93', NULL, 3, 2), -- Medico123
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50200006'), 'dr.brown', '4537a9fc80c4e8dfc60b7f4728fa77d654fa730ded77f2d201091a3418a27b93', NULL, 3, 2), -- Medico123
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50200007'), 'dra.lisa', '4537a9fc80c4e8dfc60b7f4728fa77d654fa730ded77f2d201091a3418a27b93', NULL, 3, 2), -- Medico123
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50200008'), 'dra.grey', '4537a9fc80c4e8dfc60b7f4728fa77d654fa730ded77f2d201091a3418a27b93', NULL, 3, 2), -- Medico123
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50300006'), 'beto.bloqueado', '0c01954c0f4f6bbda12d86eaecbd6a524225a0cad0fac52e6aaf4c237f7f9cbe', NULL, 2, 3), -- Recep123
((SELECT IdPersona FROM dbo.Personas WHERE DNI = '50300007'), 'lola.inactiva', '3b612c75a7b5048a435fb6ec81e52ff92d6d795a8b5a9c17070f6a63c97a53b2', NULL, 1, 4); -- Admin123
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
SELECT
    P.IdPersona,
    NULL,
    NULL,
    NULL,
    (SELECT IdRol FROM dbo.Roles WHERE Nombre = 'Paciente'),
    (SELECT IdEstadoUsuario FROM dbo.EstadosUsuario WHERE Nombre = 'Pendiente')
FROM dbo.Pacientes P;
GO

DECLARE @AyerTurno DATE = DATEADD(DAY, -1, CAST(GETDATE() AS DATE));
DECLARE @HoyTurno DATE = CAST(GETDATE() AS DATE);
DECLARE @MananaTurno DATE = DATEADD(DAY, 1, @HoyTurno);
DECLARE @MasDosTurno DATE = DATEADD(DAY, 2, @HoyTurno);
DECLARE @MasTresTurno DATE = DATEADD(DAY, 3, @HoyTurno);
DECLARE @MasCuatroTurno DATE = DATEADD(DAY, 4, @HoyTurno);
DECLARE @MasCincoTurno DATE = DATEADD(DAY, 5, @HoyTurno);
DECLARE @IdUsuarioRecepcion INT = (SELECT IdUsuario FROM dbo.Usuarios WHERE NombreUsuario = 'recepcion');
DECLARE @IdUsuarioDrHouse INT = (SELECT IdUsuario FROM dbo.Usuarios WHERE NombreUsuario = 'dr.house');
DECLARE @IdUsuarioDrBrown INT = (SELECT IdUsuario FROM dbo.Usuarios WHERE NombreUsuario = 'dr.brown');
DECLARE @IdEstadoNuevo INT = (SELECT IdEstadoTurno FROM dbo.EstadosTurno WHERE Nombre = 'Nuevo');
DECLARE @IdEstadoReprogramado INT = (SELECT IdEstadoTurno FROM dbo.EstadosTurno WHERE Nombre = 'Reprogramado');
DECLARE @IdEstadoCancelado INT = (SELECT IdEstadoTurno FROM dbo.EstadosTurno WHERE Nombre = 'Cancelado');
DECLARE @IdEstadoNoAsistio INT = (SELECT IdEstadoTurno FROM dbo.EstadosTurno WHERE Nombre = 'NoAsistio');
DECLARE @IdEstadoCerrado INT = (SELECT IdEstadoTurno FROM dbo.EstadosTurno WHERE Nombre = 'Cerrado');
DECLARE @IdClinicaTurno INT = (SELECT IdEspecialidad FROM dbo.Especialidades WHERE Nombre = 'Clinica Medica');
DECLARE @IdCardiologiaTurno INT = (SELECT IdEspecialidad FROM dbo.Especialidades WHERE Nombre = 'Cardiologia');
DECLARE @IdOdontologiaTurno INT = (SELECT IdEspecialidad FROM dbo.Especialidades WHERE Nombre = 'Odontologia');
DECLARE @IdPediatriaTurno INT = (SELECT IdEspecialidad FROM dbo.Especialidades WHERE Nombre = 'Pediatria');
DECLARE @IdTraumatologiaTurno INT = (SELECT IdEspecialidad FROM dbo.Especialidades WHERE Nombre = 'Traumatologia');
DECLARE @IdDermatologiaTurno INT = (SELECT IdEspecialidad FROM dbo.Especialidades WHERE Nombre = 'Dermatologia');
DECLARE @IdOftalmologiaTurno INT = (SELECT IdEspecialidad FROM dbo.Especialidades WHERE Nombre = 'Oftalmologia');
DECLARE @IdNeurologiaTurno INT = (SELECT IdEspecialidad FROM dbo.Especialidades WHERE Nombre = 'Neurologia');
DECLARE @IdGastroTurno INT = (SELECT IdEspecialidad FROM dbo.Especialidades WHERE Nombre = 'Gastroenterologia');
DECLARE @IdKinesiologiaTurno INT = (SELECT IdEspecialidad FROM dbo.Especialidades WHERE Nombre = 'Kinesiologia');
DECLARE @IdEndocrinoTurno INT = (SELECT IdEspecialidad FROM dbo.Especialidades WHERE Nombre = 'Endocrinologia');
DECLARE @IdOtorrinoTurno INT = (SELECT IdEspecialidad FROM dbo.Especialidades WHERE Nombre = 'Otorrinolaringologia');
DECLARE @IdMedicoHouseTurno INT = (SELECT IdMedico FROM dbo.Medicos WHERE Matricula = 'MN-20001');
DECLARE @IdMedicoAdaTurno INT = (SELECT IdMedico FROM dbo.Medicos WHERE Matricula = 'MN-20002');
DECLARE @IdMedicoFlorindaTurno INT = (SELECT IdMedico FROM dbo.Medicos WHERE Matricula = 'MN-20003');
DECLARE @IdMedicoCachoTurno INT = (SELECT IdMedico FROM dbo.Medicos WHERE Matricula = 'MN-20004');
DECLARE @IdMedicoNoraTurno INT = (SELECT IdMedico FROM dbo.Medicos WHERE Matricula = 'MN-20005');
DECLARE @IdMedicoBrownTurno INT = (SELECT IdMedico FROM dbo.Medicos WHERE Matricula = 'MN-20006');
DECLARE @IdMedicoLisaTurno INT = (SELECT IdMedico FROM dbo.Medicos WHERE Matricula = 'MN-20007');
DECLARE @IdMedicoGreyTurno INT = (SELECT IdMedico FROM dbo.Medicos WHERE Matricula = 'MN-20008');
DECLARE @IdClark INT = (SELECT P.IdPaciente FROM dbo.Pacientes P INNER JOIN dbo.Personas PE ON PE.IdPersona = P.IdPersona WHERE PE.DNI = '50100001');
DECLARE @IdWill INT = (SELECT P.IdPaciente FROM dbo.Pacientes P INNER JOIN dbo.Personas PE ON PE.IdPersona = P.IdPersona WHERE PE.DNI = '50100002');
DECLARE @IdJohn INT = (SELECT P.IdPaciente FROM dbo.Pacientes P INNER JOIN dbo.Personas PE ON PE.IdPersona = P.IdPersona WHERE PE.DNI = '50100003');
DECLARE @IdSatoshi INT = (SELECT P.IdPaciente FROM dbo.Pacientes P INNER JOIN dbo.Personas PE ON PE.IdPersona = P.IdPersona WHERE PE.DNI = '50100004');
DECLARE @IdBill INT = (SELECT P.IdPaciente FROM dbo.Pacientes P INNER JOIN dbo.Personas PE ON PE.IdPersona = P.IdPersona WHERE PE.DNI = '50100005');
DECLARE @IdMarco INT = (SELECT P.IdPaciente FROM dbo.Pacientes P INNER JOIN dbo.Personas PE ON PE.IdPersona = P.IdPersona WHERE PE.DNI = '50100006');
DECLARE @IdBruce INT = (SELECT P.IdPaciente FROM dbo.Pacientes P INNER JOIN dbo.Personas PE ON PE.IdPersona = P.IdPersona WHERE PE.DNI = '50100007');
DECLARE @IdDiana INT = (SELECT P.IdPaciente FROM dbo.Pacientes P INNER JOIN dbo.Personas PE ON PE.IdPersona = P.IdPersona WHERE PE.DNI = '50100008');
DECLARE @IdPeter INT = (SELECT P.IdPaciente FROM dbo.Pacientes P INNER JOIN dbo.Personas PE ON PE.IdPersona = P.IdPersona WHERE PE.DNI = '50100009');
DECLARE @IdTony INT = (SELECT P.IdPaciente FROM dbo.Pacientes P INNER JOIN dbo.Personas PE ON PE.IdPersona = P.IdPersona WHERE PE.DNI = '50100010');
DECLARE @IdSteve INT = (SELECT P.IdPaciente FROM dbo.Pacientes P INNER JOIN dbo.Personas PE ON PE.IdPersona = P.IdPersona WHERE PE.DNI = '50100011');
DECLARE @IdNatasha INT = (SELECT P.IdPaciente FROM dbo.Pacientes P INNER JOIN dbo.Personas PE ON PE.IdPersona = P.IdPersona WHERE PE.DNI = '50100012');
DECLARE @IdPepe INT = (SELECT P.IdPaciente FROM dbo.Pacientes P INNER JOIN dbo.Personas PE ON PE.IdPersona = P.IdPersona WHERE PE.DNI = '50100013');
DECLARE @IdMoni INT = (SELECT P.IdPaciente FROM dbo.Pacientes P INNER JOIN dbo.Personas PE ON PE.IdPersona = P.IdPersona WHERE PE.DNI = '50100014');
DECLARE @IdDonRamon INT = (SELECT P.IdPaciente FROM dbo.Pacientes P INNER JOIN dbo.Personas PE ON PE.IdPersona = P.IdPersona WHERE PE.DNI = '50100015');
DECLARE @IdMarta INT = (SELECT P.IdPaciente FROM dbo.Pacientes P INNER JOIN dbo.Personas PE ON PE.IdPersona = P.IdPersona WHERE PE.DNI = '50100016');
DECLARE @IdSherlock INT = (SELECT P.IdPaciente FROM dbo.Pacientes P INNER JOIN dbo.Personas PE ON PE.IdPersona = P.IdPersona WHERE PE.DNI = '50100017');
DECLARE @IdSarah INT = (SELECT P.IdPaciente FROM dbo.Pacientes P INNER JOIN dbo.Personas PE ON PE.IdPersona = P.IdPersona WHERE PE.DNI = '50100018');

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
(@IdClark, @IdMedicoHouseTurno, @IdClinicaTurno, @IdEstadoCerrado, @AyerTurno, '09:00', '10:00', 'Dolor de espalda despues de mudanza express.', 'Contractura leve. Indico calor local y control.', @IdUsuarioRecepcion, GETDATE(), @IdUsuarioDrHouse),
(@IdWill, @IdMedicoAdaTurno, @IdCardiologiaTurno, @IdEstadoNoAsistio, @AyerTurno, '10:00', '11:00', 'Control de presion arterial.', NULL, @IdUsuarioRecepcion, GETDATE(), @IdUsuarioRecepcion),
(@IdJohn, @IdMedicoFlorindaTurno, @IdPediatriaTurno, @IdEstadoCancelado, @AyerTurno, '08:00', '09:00', 'Turno cancelado por el paciente.', NULL, @IdUsuarioRecepcion, GETDATE(), @IdUsuarioRecepcion),
(@IdSatoshi, @IdMedicoBrownTurno, @IdOdontologiaTurno, @IdEstadoCerrado, @AyerTurno, '14:00', '15:00', 'Dolor de muela al tomar frio.', 'Caries pequena. Se deriva para arreglo.', @IdUsuarioRecepcion, GETDATE(), @IdUsuarioDrBrown),
(@IdBill, @IdMedicoHouseTurno, @IdNeurologiaTurno, @IdEstadoNuevo, @HoyTurno, '09:00', '10:00', 'Dolor de cabeza despues de muchas reuniones.', NULL, @IdUsuarioRecepcion, NULL, NULL),
(@IdMarco, @IdMedicoAdaTurno, @IdEndocrinoTurno, @IdEstadoNuevo, @HoyTurno, '10:00', '11:00', 'Control de glucemia y cansancio.', NULL, @IdUsuarioRecepcion, NULL, NULL),
(@IdBruce, @IdMedicoCachoTurno, @IdTraumatologiaTurno, @IdEstadoReprogramado, @HoyTurno, '11:00', '12:00', 'Golpe en hombro entrenando de noche.', NULL, @IdUsuarioRecepcion, GETDATE(), @IdUsuarioRecepcion),
(@IdDiana, @IdMedicoNoraTurno, @IdOftalmologiaTurno, @IdEstadoNuevo, @HoyTurno, '09:00', '10:00', 'Vision borrosa para leer de cerca.', NULL, @IdUsuarioRecepcion, NULL, NULL),
(@IdPeter, @IdMedicoGreyTurno, @IdDermatologiaTurno, @IdEstadoNuevo, @MananaTurno, '08:00', '09:00', 'Picazon en brazo despues de picadura.', NULL, @IdUsuarioRecepcion, NULL, NULL),
(@IdTony, @IdMedicoLisaTurno, @IdClinicaTurno, @IdEstadoNuevo, @MananaTurno, '12:00', '13:00', 'Chequeo general por cansancio.', NULL, @IdUsuarioRecepcion, NULL, NULL),
(@IdSteve, @IdMedicoFlorindaTurno, @IdOtorrinoTurno, @IdEstadoReprogramado, @MananaTurno, '08:00', '09:00', 'Dolor de garganta reprogramado.', NULL, @IdUsuarioRecepcion, GETDATE(), @IdUsuarioRecepcion),
(@IdNatasha, @IdMedicoBrownTurno, @IdGastroTurno, @IdEstadoNuevo, @MananaTurno, '14:00', '15:00', 'Acidez despues de comer picante.', NULL, @IdUsuarioRecepcion, NULL, NULL),
(@IdPepe, @IdMedicoHouseTurno, @IdClinicaTurno, @IdEstadoNuevo, @MasDosTurno, '10:00', '11:00', 'Dice que le duele todo menos el orgullo.', NULL, @IdUsuarioRecepcion, NULL, NULL),
(@IdMoni, @IdMedicoCachoTurno, @IdKinesiologiaTurno, @IdEstadoNuevo, @MasDosTurno, '11:00', '12:00', 'Molestia lumbar por ordenar placares.', NULL, @IdUsuarioRecepcion, NULL, NULL),
(@IdDonRamon, @IdMedicoNoraTurno, @IdDermatologiaTurno, @IdEstadoNuevo, @MasDosTurno, '09:00', '10:00', 'Mancha en la piel que quiere controlar.', NULL, @IdUsuarioRecepcion, NULL, NULL),
(@IdMarta, @IdMedicoAdaTurno, @IdCardiologiaTurno, @IdEstadoNuevo, @MasTresTurno, '10:00', '11:00', 'Control cardiologico anual.', NULL, @IdUsuarioRecepcion, NULL, NULL),
(@IdSherlock, @IdMedicoGreyTurno, @IdCardiologiaTurno, @IdEstadoNuevo, @MasTresTurno, '08:00', '09:00', 'Palpitaciones despues de mucho cafe.', NULL, @IdUsuarioRecepcion, NULL, NULL),
(@IdSarah, @IdMedicoFlorindaTurno, @IdPediatriaTurno, @IdEstadoNuevo, @MasTresTurno, '09:00', '10:00', 'Consulta familiar por control escolar.', NULL, @IdUsuarioRecepcion, NULL, NULL),
(@IdClark, @IdMedicoBrownTurno, @IdOdontologiaTurno, @IdEstadoNuevo, @MasCuatroTurno, '15:00', '16:00', 'Limpieza dental programada.', NULL, @IdUsuarioRecepcion, NULL, NULL),
(@IdWill, @IdMedicoLisaTurno, @IdPediatriaTurno, @IdEstadoNuevo, @MasCuatroTurno, '12:00', '13:00', 'Consulta por control de adolescente.', NULL, @IdUsuarioRecepcion, NULL, NULL),
(@IdJohn, @IdMedicoCachoTurno, @IdTraumatologiaTurno, @IdEstadoNuevo, @MasCuatroTurno, '12:00', '13:00', 'Dolor de rodilla al subir escaleras.', NULL, @IdUsuarioRecepcion, NULL, NULL),
(@IdSatoshi, @IdMedicoAdaTurno, @IdEndocrinoTurno, @IdEstadoNuevo, @MasCincoTurno, '11:00', '12:00', 'Control metabolico de rutina.', NULL, @IdUsuarioRecepcion, NULL, NULL),
(@IdBill, @IdMedicoNoraTurno, @IdOftalmologiaTurno, @IdEstadoNuevo, @MasCincoTurno, '10:00', '11:00', 'Revisar graduacion de anteojos.', NULL, @IdUsuarioRecepcion, NULL, NULL),
(@IdMarco, @IdMedicoHouseTurno, @IdClinicaTurno, @IdEstadoNuevo, @MasCincoTurno, '11:00', '12:00', 'Chequeo general antes de viaje.', NULL, @IdUsuarioRecepcion, NULL, NULL);
GO

SELECT 'Base de datos creada OK...' AS Resultado;
GO
