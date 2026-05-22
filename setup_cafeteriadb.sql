-- ============================================================
--  SETUP COMPLETO — cafeteriadb
--
--  Elimina y recrea la base de datos desde cero.
--
--  Cómo ejecutar en MySQL Workbench:
--    File → Open SQL Script → selecciona este archivo → Ctrl+Shift+Enter
-- ============================================================

DROP DATABASE IF EXISTS cafeteriadb;
CREATE DATABASE cafeteriadb
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_unicode_ci;

USE cafeteriadb;

-- ── TABLAS ──────────────────────────────────────────────────

CREATE TABLE Roles (
    id     INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL
);

CREATE TABLE Usuarios (
    id              INT AUTO_INCREMENT PRIMARY KEY,
    rol_id          INT          NOT NULL,
    nombre_completo VARCHAR(200) NOT NULL,
    username        VARCHAR(100) NOT NULL UNIQUE,
    password_hash   VARCHAR(500) NOT NULL,
    FOREIGN KEY (rol_id) REFERENCES Roles(id)
);

CREATE TABLE MetodosPago (
    id     INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL
);

CREATE TABLE Categorias (
    id          INT AUTO_INCREMENT PRIMARY KEY,
    nombre      VARCHAR(200) NOT NULL,
    descripcion TEXT
);

CREATE TABLE Clientes (
    id              INT AUTO_INCREMENT PRIMARY KEY,
    nombre          VARCHAR(200) NOT NULL,
    correo          VARCHAR(200) NULL,
    puntos_lealtad  INT NOT NULL DEFAULT 0
);

CREATE TABLE Productos (
    id           INT AUTO_INCREMENT PRIMARY KEY,
    categoria_id INT          NOT NULL,
    nombre       VARCHAR(200) NOT NULL,
    descripcion  TEXT,
    esta_activo  TINYINT(1)   NOT NULL DEFAULT 1,
    imagen_url   VARCHAR(500) NULL,
    FOREIGN KEY (categoria_id) REFERENCES Categorias(id)
);

CREATE TABLE Presentaciones (
    id          INT AUTO_INCREMENT PRIMARY KEY,
    producto_id INT            NOT NULL,
    tamano      VARCHAR(100)   NOT NULL,
    precio      DECIMAL(10,2)  NOT NULL,
    FOREIGN KEY (producto_id) REFERENCES Productos(id) ON DELETE CASCADE
);

CREATE TABLE Ordenes (
    id              INT AUTO_INCREMENT PRIMARY KEY,
    usuario_id      INT           NOT NULL,
    cliente_id      INT           NULL,
    metodo_pago_id  INT           NOT NULL,
    total           DECIMAL(10,2) NOT NULL,
    estado          VARCHAR(50)   NOT NULL DEFAULT 'Pendiente',
    fecha           DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (usuario_id)     REFERENCES Usuarios(id),
    FOREIGN KEY (cliente_id)     REFERENCES Clientes(id),
    FOREIGN KEY (metodo_pago_id) REFERENCES MetodosPago(id)
);

CREATE TABLE DetalleOrden (
    id               INT AUTO_INCREMENT PRIMARY KEY,
    orden_id         INT           NOT NULL,
    presentacion_id  INT           NOT NULL,
    cantidad         INT           NOT NULL,
    precio_unitario  DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (orden_id)        REFERENCES Ordenes(id)       ON DELETE CASCADE,
    FOREIGN KEY (presentacion_id) REFERENCES Presentaciones(id)
);

-- ── DATOS INICIALES ─────────────────────────────────────────

INSERT INTO Roles (nombre) VALUES
    ('Administrador'),
    ('Barista'),
    ('Cajero');

-- Usuario: admin / 1234  (hash BCrypt de "1234")
INSERT INTO Usuarios (nombre_completo, username, password_hash, rol_id) VALUES
    ('Administrador del Sistema', 'admin',
     '$2a$11$K/cqBuZAK6Q8jVF9g5kMsOv.QRkGrLZa5XrFPB5pRvfAoHj6RJlXW', 1);

INSERT INTO MetodosPago (nombre) VALUES
    ('Efectivo'),
    ('Tarjeta');

INSERT INTO Categorias (nombre, descripcion) VALUES
    ('Cafés Calientes',      'Espressos, lattes, cappuccinos y más'),
    ('Bebidas Frías',        'Frapés, cold brew, smoothies'),
    ('Postres y Repostería', 'Pasteles, muffins, croissants'),
    ('Snacks',               'Bocadillos y antojitos');

-- ── VERIFICACIÓN ─────────────────────────────────────────────
SELECT 'Roles'          AS Tabla, COUNT(*) AS Registros FROM Roles          UNION ALL
SELECT 'Usuarios',       COUNT(*) FROM Usuarios       UNION ALL
SELECT 'MetodosPago',    COUNT(*) FROM MetodosPago    UNION ALL
SELECT 'Categorias',     COUNT(*) FROM Categorias     UNION ALL
SELECT 'Productos',      COUNT(*) FROM Productos      UNION ALL
SELECT 'Presentaciones', COUNT(*) FROM Presentaciones UNION ALL
SELECT 'Ordenes',        COUNT(*) FROM Ordenes        UNION ALL
SELECT 'DetalleOrden',   COUNT(*) FROM DetalleOrden;
