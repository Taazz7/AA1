-- Creates

CREATE DATABASE AA1

USE AA1

CREATE TABLE USUARIOS (
    idUsuario INT PRIMARY KEY,
    nombre VARCHAR(50),
    apellido VARCHAR(50),
    telefono INT,
    direccion VARCHAR(100),
    fechaNac DATE
);

CREATE TABLE PISTAS (
    idPista INT PRIMARY KEY,
    nombre VARCHAR(50),
    tipo VARCHAR(30),
    direccion VARCHAR(255),
    activa BIT,
    precioHora DECIMAL(10,2)
);

CREATE TABLE RESERVAS (
    idReserva INT PRIMARY KEY,
    idUsuario INT,
    idPista INT,
    fecha DATE,
    horas int,
    precio int,
    FOREIGN KEY (idUsuario) REFERENCES USUARIOS(idUsuario),
    FOREIGN KEY (idPista) REFERENCES PISTAS(idPista)
);

CREATE TABLE MATERIALES (
    idMaterial INT PRIMARY KEY,
    nombre VARCHAR(50),
    cantidad INT,
    disponibilidad BIT,
    idPista INT,
    fechaAct DATE,
    FOREIGN KEY (idPista) REFERENCES PISTAS(idPista)
);

CREATE TABLE MANTENIMIENTOS (
    idMantenimiento INT PRIMARY KEY,
    nombre VARCHAR(50),
    tlfn INT,
    cif INT,
    idPista INT,
    correo VARCHAR(100),
    FOREIGN KEY (idPista) REFERENCES PISTAS(idPista)
);

-- Inserts

-- Usuarios
INSERT INTO USUARIOS VALUES (1, 'Ana', 'García', 600123456, 'Calle Mayor 10', '1990-05-15');
INSERT INTO USUARIOS VALUES (2, 'Luis', 'Martínez', 600654321, 'Av. Goya 22', '1985-11-30');

-- Pistas
INSERT INTO PISTAS VALUES (1, 'Pista Central', 'Tenis', 'Pista de tierra batida', 1, 25.00);
INSERT INTO PISTAS VALUES (2, 'Pista Norte', 'Padel', 'Pista cubierta', 1, 20.00);

-- Reservas
INSERT INTO RESERVAS VALUES (1, 2, 1, '2025-11-20', 1, 20);
INSERT INTO RESERVAS VALUES (2, 1, 2, '2025-11-21', 3, 28);

-- Materiales
INSERT INTO MATERIALES VALUES (1, 'Raquetas', 10, 1, 1, '2025-1-20');
INSERT INTO MATERIALES VALUES (2, 'Pelotas', 30, 1, 2, '2023-5-5');

-- Mantenimiento
INSERT INTO MANTENIMIENTOS VALUES (1, 'Revisión red', '152847563', '258', 1, 'mantenimiento@club.com');
INSERT INTO MANTENIMIENTOS VALUES (2, 'Cambio focos', '611259566', '364', 2, 'soporte@club.com');