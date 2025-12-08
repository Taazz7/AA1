CREATE DATABASE AA1;

USE AA1;


-- 1. Eliminar todas las tablas en orden correcto (respetando foreign keys)
DROP TABLE IF EXISTS MANTENIMIENTOS;
DROP TABLE IF EXISTS MATERIALES;
DROP TABLE IF EXISTS RESERVAS;
DROP TABLE IF EXISTS PISTAS;
DROP TABLE IF EXISTS USUARIOS;


-- 2. Recrear las tablas con IDENTITY en las Primary Keys

CREATE TABLE USUARIOS (
    idUsuario INT PRIMARY KEY IDENTITY(1,1),  -- Auto-incremental
    nombre VARCHAR(50),
    apellido VARCHAR(50),
    telefono INT,
    direccion VARCHAR(100),
    fechaNac DATE
);

CREATE TABLE PISTAS (
    idPista INT PRIMARY KEY IDENTITY(1,1),  -- Auto-incremental
    nombre VARCHAR(50),
    tipo VARCHAR(30),
    direccion VARCHAR(255),
    activa BIT,
    precioHora INT
);

CREATE TABLE RESERVAS (
    idReserva INT PRIMARY KEY IDENTITY(1,1),  -- Auto-incremental
    idUsuario INT,
    idPista INT,
    fecha DATE,
    horas INT,
    precio INT,
    FOREIGN KEY (idUsuario) REFERENCES USUARIOS(idUsuario),
    FOREIGN KEY (idPista) REFERENCES PISTAS(idPista)
);

CREATE TABLE MATERIALES (
    idMaterial INT PRIMARY KEY IDENTITY(1,1),  -- Auto-incremental
    nombre VARCHAR(50),
    cantidad INT,
    disponibilidad BIT,
    idPista INT,
    fechaAct DATE,
    FOREIGN KEY (idPista) REFERENCES PISTAS(idPista)
);

CREATE TABLE MANTENIMIENTOS (
    idMantenimiento INT PRIMARY KEY IDENTITY(1,1),  -- Auto-incremental
    nombre VARCHAR(50),
    tlfn INT,
    cif INT,
    idPista INT,
    correo VARCHAR(100),
    FOREIGN KEY (idPista) REFERENCES PISTAS(idPista)
);


-- 3. Insertar datos de ejemplo (sin especificar los IDs)

-- Usuarios
INSERT INTO USUARIOS (nombre, apellido, telefono, direccion, fechaNac) 
VALUES ('Ana', 'García', 600123456, 'Calle Mayor 10', '1990-05-15');

INSERT INTO USUARIOS (nombre, apellido, telefono, direccion, fechaNac) 
VALUES ('Luis', 'Martínez', 600654321, 'Av. Goya 22', '1985-11-30');

-- Pistas
INSERT INTO PISTAS (nombre, tipo, direccion, activa, precioHora) 
VALUES ('Pista Central', 'Tenis', 'Pista de tierra batida', 1, 25);

INSERT INTO PISTAS (nombre, tipo, direccion, activa, precioHora) 
VALUES ('Pista Norte', 'Padel', 'Pista cubierta', 1, 20);

-- Reservas
INSERT INTO RESERVAS (idUsuario, idPista, fecha, horas, precio) 
VALUES (2, 1, '2025-11-20', 1, 20);

INSERT INTO RESERVAS (idUsuario, idPista, fecha, horas, precio) 
VALUES (1, 2, '2025-11-21', 3, 28);

INSERT INTO RESERVAS (idUsuario, idPista, fecha, horas, precio) 
VALUES (1, 2, '2025-12-01', 1, 18);

-- Materiales
INSERT INTO MATERIALES (nombre, cantidad, disponibilidad, idPista, fechaAct) 
VALUES ('Raquetas', 10, 1, 1, '2025-01-20');

INSERT INTO MATERIALES (nombre, cantidad, disponibilidad, idPista, fechaAct) 
VALUES ('Pelotas', 30, 1, 2, '2023-05-05');

-- Mantenimiento
INSERT INTO MANTENIMIENTOS (nombre, tlfn, cif, idPista, correo) 
VALUES ('Revisión red', 152847563, 258, 1, 'mantenimiento@club.com');

INSERT INTO MANTENIMIENTOS (nombre, tlfn, cif, idPista, correo) 
VALUES ('Cambio focos', 611259566, 364, 2, 'soporte@club.com');


-- Verificacion
SELECT 'USUARIOS' AS Tabla, COUNT(*) AS Registros FROM USUARIOS
UNION ALL
SELECT 'PISTAS', COUNT(*) FROM PISTAS
UNION ALL
SELECT 'RESERVAS', COUNT(*) FROM RESERVAS
UNION ALL
SELECT 'MATERIALES', COUNT(*) FROM MATERIALES
UNION ALL
SELECT 'MANTENIMIENTOS', COUNT(*) FROM MANTENIMIENTOS;
