-- ============================================================
--  CoffeeDB — Script de limpieza y datos iniciales
--
--  Opción A – MySQL Workbench:
--    Abre este archivo y ejecuta con Ctrl+Shift+Enter
--
--  Opción B – Terminal:
--    mysql -u root -p12345678 CoffeeDB < reset_database.sql
-- ============================================================

USE cafeteriadb;

-- ── 1. DESHABILITAR FK TEMPORALMENTE ────────────────────────
SET FOREIGN_KEY_CHECKS = 0;

-- ── 2. VACIAR TABLAS Y RESETEAR AUTO_INCREMENT ──────────────
TRUNCATE TABLE DetalleOrden;
TRUNCATE TABLE Ordenes;
TRUNCATE TABLE Presentaciones;
TRUNCATE TABLE Productos;
TRUNCATE TABLE Categorias;
TRUNCATE TABLE Usuarios;
TRUNCATE TABLE Roles;
TRUNCATE TABLE Clientes;
TRUNCATE TABLE MetodosPago;

-- ── 3. RE-HABILITAR FK ──────────────────────────────────────
SET FOREIGN_KEY_CHECKS = 1;

-- ============================================================
--  DATOS BASE
-- ============================================================

-- Roles
INSERT INTO Roles (nombre) VALUES
    ('Administrador'),
    ('Barista'),
    ('Cajero');

-- Usuario admin  (login: admin / 1234)
-- El sistema acepta admin/1234 por validación directa.
-- El hash es BCrypt de "1234" por si se usa verificación normal.
INSERT INTO Usuarios (nombre_completo, username, password_hash, rol_id) VALUES
    ('Administrador del Sistema', 'admin',
     '$2a$11$K/cqBuZAK6Q8jVF9g5kMsOv.QRkGrLZa5XrFPB5pRvfAoHj6RJlXW', 1);

-- Métodos de pago
INSERT INTO MetodosPago (nombre) VALUES
    ('Efectivo'),
    ('Tarjeta');

-- Categorías iniciales
INSERT INTO Categorias (nombre, descripcion) VALUES
    ('Cafés Calientes',       'Espressos, lattes, cappuccinos y más'),
    ('Bebidas Frías',         'Frapés, cold brew, smoothies'),
    ('Postres y Repostería',  'Pasteles, muffins, croissants'),
    ('Snacks',                'Bocadillos y antojitos');

-- ============================================================
--  VERIFICACIÓN — muestra cuántos registros quedaron
-- ============================================================
SELECT 'Roles'          AS Tabla, COUNT(*) AS Registros FROM Roles         UNION ALL
SELECT 'Usuarios',       COUNT(*) FROM Usuarios      UNION ALL
SELECT 'MetodosPago',    COUNT(*) FROM MetodosPago   UNION ALL
SELECT 'Categorias',     COUNT(*) FROM Categorias    UNION ALL
SELECT 'Productos',      COUNT(*) FROM Productos     UNION ALL
SELECT 'Presentaciones', COUNT(*) FROM Presentaciones UNION ALL
SELECT 'Ordenes',        COUNT(*) FROM Ordenes       UNION ALL
SELECT 'DetalleOrden',   COUNT(*) FROM DetalleOrden;
