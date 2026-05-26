-- ============================================================
--  CafeteriaDB — Crear tablas en base de datos existente
--  Ejecutar directamente sobre la base de datos CafeteriaDB en AWS RDS
--  MySQL Workbench: conectar al RDS, seleccionar CafeteriaDB, ejecutar
-- ============================================================

USE CafeteriaDB;

-- ── TABLAS ──────────────────────────────────────────────────

CREATE TABLE IF NOT EXISTS Roles (
    id     INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS Usuarios (
    id              INT AUTO_INCREMENT PRIMARY KEY,
    rol_id          INT          NOT NULL,
    nombre_completo VARCHAR(200) NOT NULL,
    username        VARCHAR(100) NOT NULL UNIQUE,
    password_hash   VARCHAR(500) NOT NULL,
    FOREIGN KEY (rol_id) REFERENCES Roles(id)
);

CREATE TABLE IF NOT EXISTS MetodosPago (
    id     INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS Categorias (
    id          INT AUTO_INCREMENT PRIMARY KEY,
    nombre      VARCHAR(200) NOT NULL,
    descripcion TEXT
);

CREATE TABLE IF NOT EXISTS Clientes (
    id             INT AUTO_INCREMENT PRIMARY KEY,
    nombre         VARCHAR(200) NOT NULL,
    correo         VARCHAR(200) NULL,
    puntos_lealtad INT NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS Productos (
    id           INT AUTO_INCREMENT PRIMARY KEY,
    categoria_id INT          NOT NULL,
    nombre       VARCHAR(200) NOT NULL,
    descripcion  TEXT,
    esta_activo  TINYINT(1)   NOT NULL DEFAULT 1,
    imagen_url   VARCHAR(500) NULL,
    FOREIGN KEY (categoria_id) REFERENCES Categorias(id)
);

CREATE TABLE IF NOT EXISTS Presentaciones (
    id          INT AUTO_INCREMENT PRIMARY KEY,
    producto_id INT            NOT NULL,
    tamano      VARCHAR(100)   NOT NULL,
    precio      DECIMAL(10,2)  NOT NULL,
    FOREIGN KEY (producto_id) REFERENCES Productos(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Ordenes (
    id             INT AUTO_INCREMENT PRIMARY KEY,
    usuario_id     INT           NOT NULL,
    cliente_id     INT           NULL,
    metodo_pago_id INT           NOT NULL,
    total          DECIMAL(10,2) NOT NULL,
    estado         VARCHAR(50)   NOT NULL DEFAULT 'Pendiente',
    fecha          DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (usuario_id)     REFERENCES Usuarios(id),
    FOREIGN KEY (cliente_id)     REFERENCES Clientes(id),
    FOREIGN KEY (metodo_pago_id) REFERENCES MetodosPago(id)
);

CREATE TABLE IF NOT EXISTS DetalleOrden (
    id              INT AUTO_INCREMENT PRIMARY KEY,
    orden_id        INT           NOT NULL,
    presentacion_id INT           NOT NULL,
    cantidad        INT           NOT NULL,
    precio_unitario DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (orden_id)        REFERENCES Ordenes(id) ON DELETE CASCADE,
    FOREIGN KEY (presentacion_id) REFERENCES Presentaciones(id)
);

-- ── DATOS INICIALES (solo si las tablas estan vacias) ───────

INSERT INTO Roles (nombre)
SELECT * FROM (SELECT 'Administrador' UNION SELECT 'Barista' UNION SELECT 'Cajero') AS tmp
WHERE NOT EXISTS (SELECT 1 FROM Roles LIMIT 1);

INSERT INTO MetodosPago (nombre)
SELECT * FROM (SELECT 'Efectivo' UNION SELECT 'Tarjeta') AS tmp
WHERE NOT EXISTS (SELECT 1 FROM MetodosPago LIMIT 1);

INSERT INTO Categorias (nombre, descripcion)
SELECT * FROM (
    SELECT 'Cafés Calientes',      'Espressos, lattes, cappuccinos y más'  UNION ALL
    SELECT 'Bebidas Frías',        'Frapés, cold brew, smoothies'          UNION ALL
    SELECT 'Postres y Repostería', 'Pasteles, muffins, croissants'         UNION ALL
    SELECT 'Snacks',               'Bocadillos y antojitos'
) AS tmp
WHERE NOT EXISTS (SELECT 1 FROM Categorias LIMIT 1);

-- ── VERIFICACIÓN ─────────────────────────────────────────────
SELECT 'Roles'          AS Tabla, COUNT(*) AS Registros FROM Roles          UNION ALL
SELECT 'Usuarios',       COUNT(*) FROM Usuarios                              UNION ALL
SELECT 'MetodosPago',    COUNT(*) FROM MetodosPago                           UNION ALL
SELECT 'Categorias',     COUNT(*) FROM Categorias                            UNION ALL
SELECT 'Productos',      COUNT(*) FROM Productos                             UNION ALL
SELECT 'Presentaciones', COUNT(*) FROM Presentaciones                        UNION ALL
SELECT 'Ordenes',        COUNT(*) FROM Ordenes                               UNION ALL
SELECT 'DetalleOrden',   COUNT(*) FROM DetalleOrden;
