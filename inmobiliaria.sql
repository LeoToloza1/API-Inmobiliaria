-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 01-05-2024 a las 23:04:20
-- Versión del servidor: 10.4.25-MariaDB
-- Versión de PHP: 8.1.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliaria`
--
CREATE DATABASE IF NOT EXISTS `inmobiliaria` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `inmobiliaria`;

DELIMITER $$
--
-- Procedimientos
--
DROP PROCEDURE IF EXISTS `auditoria_pagos`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `auditoria_pagos` ()   BEGIN
SELECT
    alquiler.direccion,
    alquiler.uso,
    t.tipo,
    i.nombre as nombre_inquilino,
    i.apellido as apellido_inquilino,
    p.fecha_pago, 
    p.importe, 
    p.numero_pago,
    p.detalle, 
    p.creado_fecha, 
    u.nombre, 
    u.apellido,
    u.email
FROM pago as p
INNER JOIN usuario as u ON p.creado_usuario = u.id
INNER JOIN contrato c ON p.id_contrato = c.id
INNER JOIN inquilino i ON c.id_inquilino = i.id
INNER JOIN inmueble alquiler ON c.id_inmueble = alquiler.id
INNER JOIN tipo_inmueble as t ON alquiler.id_tipo = t.id
INNER JOIN propietario pro ON alquiler.id_propietario = pro.id
WHERE u.id = p.creado_usuario;
END$$

DROP PROCEDURE IF EXISTS `listarCiudades`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `listarCiudades` ()   BEGIN
    select * from ciudad;
END$$

DROP PROCEDURE IF EXISTS `listarUsuarios`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `listarUsuarios` ()   BEGIN
    SELECT id,nombre,apellido,dni,email,rol,avatarUrl,borrado FROM usuario;
END$$

DROP PROCEDURE IF EXISTS `listarZonas`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `listarZonas` ()   BEGIN
    select * from zona;
END$$

DROP PROCEDURE IF EXISTS `listar_inmuebles`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `listar_inmuebles` ()   BEGIN 
SELECT
    i.id,
    i.direccion,
    i.uso,
    i.id_tipo,
    i.ambientes,
    i.coordenadas,
    i.latitud,
    i.longitud,
    i.precio,
    i.id_propietario,
    i.estado,
    i.id_ciudad,
    i.id_zona,
    i.borrado,
    i.descripcion,
    t.id AS t_id_tipo ,
    t.tipo AS tipo_inmueble,
    p.id AS p_id,
    p.nombre AS nombre_propietario,
    p.apellido AS apellido_propietario, 
    c.ciudad ,
    z.zona 
FROM inmueble AS i 
   INNER JOIN tipo_inmueble AS t
   ON i.id_tipo = t.id
   INNER JOIN propietario AS p 
   ON i.id_propietario = p.id 
   JOIN ciudad AS c 
   ON c.id = i.id_ciudad 
   JOIN zona AS z ON z.id = i.id_zona 
WHERE i.borrado = 0  ;
END$$

DROP PROCEDURE IF EXISTS `listar_inmuebles_2`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `listar_inmuebles_2` ()   BEGIN
SELECT
	i.id,
    i.direccion,
    i.uso,
    i.id_tipo,
    i.ambientes,
    i.coordenadas,
    i.latitud,
    i.longitud,
    i.precio,
    i.id_propietario,
    i.estado,
    i.id_ciudad,
    i.id_zona,
    i.borrado,
    i.descripcion,
    t.id AS t_id_tipo ,
    t.tipo AS tipo_inmueble,
    p.id AS p_id,
    p.nombre AS nombre_propietario,
    p.apellido AS apellido_propietario, 
    c.ciudad ,
    z.zona 
FROM inmueble AS i 
   INNER JOIN tipo_inmueble AS t
   ON i.id_tipo = t.id
   INNER JOIN propietario AS p 
   ON i.id_propietario = p.id 
   JOIN ciudad AS c 
   ON c.id = i.id_ciudad 
   JOIN zona AS z
   ON z.id = i.id_zona 
   JOIN contrato AS cont
   ON i.id = cont.id_inmueble
WHERE i.borrado = 0  ;
END$$

DROP PROCEDURE IF EXISTS `listar_inmuebles_3`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `listar_inmuebles_3` ()   BEGIN
SET @f_inicio = '2024-04-01';
SET @f_fin = '2024-04-10';
SELECT
	i.id,
    i.direccion,
    i.uso,
    i.id_tipo,
    i.ambientes,
    i.coordenadas,
    i.latitud,
    i.longitud,
    i.precio,
    i.id_propietario,
    i.estado,
    i.id_ciudad,
    i.id_zona,
    i.borrado,
    i.descripcion,
    t.id AS t_id_tipo ,
    t.tipo AS tipo_inmueble,
    p.id AS p_id,
    p.nombre AS nombre_propietario,
    p.apellido AS apellido_propietario, 
    c.ciudad ,
    z.zona 
FROM inmueble AS i 
   INNER JOIN tipo_inmueble AS t
   ON i.id_tipo = t.id
   INNER JOIN propietario AS p 
   ON i.id_propietario = p.id 
   JOIN ciudad AS c 
   ON c.id = i.id_ciudad 
   JOIN zona AS z
   ON z.id = i.id_zona 
   JOIN contrato AS cont
   ON i.id = cont.id_inmueble
WHERE i.borrado = 0   
	AND (@f_inicio < cont.fecha_fin AND @f_fin > cont.fecha_inicio) ;

END$$

DROP PROCEDURE IF EXISTS `listar_inmuebles_4`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `listar_inmuebles_4` (IN `f_inicio` DATE, IN `f_fin` DATE)  SQL SECURITY INVOKER BEGIN

SELECT
    i.id,
    i.direccion,
    i.uso,
    i.id_tipo,
    i.ambientes,
    i.coordenadas,
    i.latitud,
    i.longitud,
    i.precio,
    i.id_propietario,
    i.estado,
    i.id_ciudad,
    i.id_zona,
    i.borrado,
    i.descripcion,
    t.id AS t_id_tipo,
    t.tipo AS tipo_inmueble,
    p.id AS p_id,
    p.nombre AS nombre_propietario,
    p.apellido AS apellido_propietario,
    c.ciudad,
    z.zona
FROM
    inmueble AS i
    INNER JOIN tipo_inmueble AS t ON i.id_tipo = t.id
    INNER JOIN propietario AS p ON i.id_propietario = p.id
    JOIN ciudad AS c ON c.id = i.id_ciudad
    JOIN zona AS z ON z.id = i.id_zona
    LEFT JOIN contrato AS cont ON i.id = cont.id_inmueble
        AND (
            cont.fecha_inicio <= f_fin
            AND cont.fecha_fin >= f_inicio
        )
WHERE
    i.borrado = 0
     AND cont.id_inmueble IS NULL
    ;

END$$

DROP PROCEDURE IF EXISTS `listar_inmuebles_5`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `listar_inmuebles_5` (IN `f_inicio` DATE, IN `f_fin` DATE)  SQL SECURITY INVOKER BEGIN

    SELECT
        i.id,
        i.direccion,
        i.uso,
        i.id_tipo,
        i.ambientes,
        i.coordenadas,
        i.latitud,
        i.longitud,
        i.precio,
        i.id_propietario,
        i.estado,
        i.id_ciudad,
        i.id_zona,
        i.borrado,
        i.descripcion,
        t.id AS t_id_tipo,
        t.tipo AS tipo_inmueble,
        p.id AS p_id,
        p.nombre AS nombre_propietario,
        p.apellido AS apellido_propietario,
        c.ciudad,
        z.zona
    FROM
        inmueble AS i
        INNER JOIN tipo_inmueble AS t ON i.id_tipo = t.id
        INNER JOIN propietario AS p ON i.id_propietario = p.id
        JOIN ciudad AS c ON c.id = i.id_ciudad
        JOIN zona AS z ON z.id = i.id_zona
        LEFT JOIN contrato AS cont 
            ON i.id = cont.id_inmueble
            AND (
	                f_fin <= cont.fecha_inicio 
                OR
					f_inicio >= cont.fecha_fin
            )
    WHERE
        i.borrado = 0
        AND cont.id_inmueble IS NULL;

END$$

DROP PROCEDURE IF EXISTS `listar_inmuebles_6`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `listar_inmuebles_6` (IN `f_inicio` DATE, IN `f_fin` DATE)  SQL SECURITY INVOKER BEGIN

    SELECT
        i.id,
        i.direccion,
        i.uso,
        i.id_tipo,
        i.ambientes,
        i.coordenadas,
        i.latitud,
        i.longitud,
        i.precio,
        i.id_propietario,
        i.estado,
        i.id_ciudad,
        i.id_zona,
        i.borrado,
        i.descripcion,
        t.id AS t_id_tipo,
        t.tipo AS tipo_inmueble,
        p.id AS p_id,
        p.nombre AS nombre_propietario,
        p.apellido AS apellido_propietario,
        c.ciudad,
        z.zona
    FROM
        inmueble AS i
        INNER JOIN tipo_inmueble AS t ON i.id_tipo = t.id
        INNER JOIN propietario AS p ON i.id_propietario = p.id
        JOIN ciudad AS c ON c.id = i.id_ciudad
        JOIN zona AS z ON z.id = i.id_zona
        LEFT JOIN contrato AS cont 
            ON i.id = cont.id_inmueble
            AND (
                (f_fin <= cont.fecha_inicio) 
                OR (f_inicio >= cont.fecha_fin)
            )
        LEFT JOIN contrato AS cont2 
            ON i.id = cont2.id_inmueble
            AND (cont2.fecha_inicio IS NULL OR cont2.fecha_inicio <= f_fin)
            AND (cont2.fecha_fin IS NULL OR cont2.fecha_fin >= f_inicio)
    WHERE
        i.borrado = 0
        AND (cont2.id_inmueble IS NULL)
    ;

END$$

DROP PROCEDURE IF EXISTS `verify_contrato`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `verify_contrato` (IN `f_inicio` DATE, IN `f_fin` DATE, IN `id_prop` INT)   BEGIN
-- SET @id :=1;
-- SET @in := '2024-04-01';
-- SET @out := '2024-05-01';
 IF (f_inicio < f_fin) THEN
SELECT id
FROM contrato
WHERE
id_inmueble= id_prop
AND NOT (
    (f_fin < fecha_inicio )
OR
    (fecha_fin < f_inicio)
    )
    LIMIT 1
;
ELSE
SET @MESSAGE_TEXT = 'Fecha Inicio Mayor que Fecha Fin';
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = @MESSAGE_TEXT;
    END IF;
END$$

--
-- Funciones
--
DROP FUNCTION IF EXISTS `validarFechas`$$
CREATE DEFINER=`root`@`localhost` FUNCTION `validarFechas` (`f_inicio` DATE, `f_fin` DATE) RETURNS INT(11)  BEGIN
    DECLARE resultado INT;

    IF (f_inicio < f_fin) THEN
        SELECT id INTO resultado
        FROM contrato
        WHERE
            id_inmueble = 1 
            AND (
                (f_fin < fecha_inicio )
                OR
                (fecha_fin < f_inicio)
            );  
    ELSE   
        SET @MESSAGE_TEXT = 'Fecha Inicio Mayor que Fecha Fin';
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = @MESSAGE_TEXT;
    END IF;

    RETURN resultado;
END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contrato`
--

DROP TABLE IF EXISTS `contrato`;
CREATE TABLE IF NOT EXISTS `contrato` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_inquilino` int(11) NOT NULL,
  `id_inmueble` int(11) NOT NULL,
  `fecha_inicio` date DEFAULT NULL,
  `fecha_fin` date DEFAULT NULL,
  `fecha_efectiva` date DEFAULT NULL,
  `monto` decimal(9,2) DEFAULT NULL,
  `borrado` tinyint(1) DEFAULT 0,
  `creado_fecha` datetime NOT NULL DEFAULT current_timestamp(),
  `creado_usuario` int(11) NOT NULL,
  `cancelado_fecha` datetime DEFAULT NULL,
  `cancelado_usuario` int(11) NOT NULL,
  `editado_usuario` int(2) DEFAULT NULL,
  `editado_fecha` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `contrato_inmueble_idx` (`id_inmueble`),
  KEY `contrato_inquilino_idx` (`id_inquilino`),
  KEY `fecha_inicio` (`fecha_inicio`,`fecha_fin`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4;

--
-- RELACIONES PARA LA TABLA `contrato`:
--   `id_inmueble`
--       `inmueble` -> `id`
--   `id_inquilino`
--       `inquilino` -> `id`
--

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`id`, `id_inquilino`, `id_inmueble`, `fecha_inicio`, `fecha_fin`, `fecha_efectiva`, `monto`, `borrado`, `creado_fecha`, `creado_usuario`, `cancelado_fecha`, `cancelado_usuario`, `editado_usuario`, `editado_fecha`) VALUES
(1, 1, 1, '2023-04-23', '2024-04-30', '2024-04-30', '1500.00', 0, '2024-04-23 01:47:34', 0, '2024-04-24 16:31:44', 0, 0, NULL),
(2, 3, 1, '2023-04-23', '2025-04-23', '2025-04-23', '23425.00', 0, '2024-04-23 01:49:19', 0, '2024-04-23 01:53:57', 0, 0, NULL),
(3, 1, 1, '2024-04-26', '2025-04-25', '2025-04-25', '2500.00', 0, '2024-04-23 02:04:46', 0, '2024-04-23 02:04:46', 0, 0, NULL),
(4, 4, 3, '2024-04-25', '2024-04-30', '2024-04-30', '1000.00', 0, '2024-04-24 22:10:10', 0, NULL, 0, NULL, NULL),
(5, 4, 10, '2024-05-01', '2024-05-01', '2024-05-01', '5000.00', 1, '2024-04-25 12:15:10', 2, NULL, 0, 2, '2024-04-25 12:22:08'),
(6, 1, 1, '2024-05-01', '2025-05-01', '0001-01-01', '6000.00', 0, '2024-05-01 17:31:34', 0, NULL, 0, NULL, NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmueble`
--

DROP TABLE IF EXISTS `inmueble`;
CREATE TABLE IF NOT EXISTS `inmueble` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `direccion` varchar(100) NOT NULL COMMENT 'calle y altura',
  `uso` enum('Comercial','Residencial') NOT NULL DEFAULT 'Comercial',
  `id_tipo` int(11) NOT NULL,
  `ambientes` tinyint(2) NOT NULL DEFAULT 1,
  `coordenadas` varchar(100) DEFAULT NULL,
  `precio` decimal(11,2) DEFAULT NULL,
  `id_propietario` int(11) NOT NULL,
  `estado` enum('Disponible','Retirado') NOT NULL DEFAULT 'Disponible',
  `borrado` tinyint(1) NOT NULL DEFAULT 0,
  `descripcion` text DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `inmueble_tipo_idx` (`id_tipo`),
  KEY `propietario_inmueble_idx` (`id_propietario`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4;

--
-- RELACIONES PARA LA TABLA `inmueble`:
--   `id_tipo`
--       `tipo_inmueble` -> `id`
--   `id_propietario`
--       `propietario` -> `id`
--

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`id`, `direccion`, `uso`, `id_tipo`, `ambientes`, `coordenadas`, `precio`, `id_propietario`, `estado`, `borrado`, `descripcion`) VALUES
(1, 'san martin 45', 'Residencial', 1, 9, '-32.414566613131946, -65.00877119828924', '1.26', 1, 'Disponible', 0, 'Casa  de 2 ambientes'),
(2, 'av Los Mandarinos 566', 'Comercial', 4, 0, '-32.4239588393048, -65.01171109578154', '2.00', 3, 'Disponible', 0, 'Oficina de 250m2'),
(3, 'Av. Illia 1234', 'Residencial', 2, 0, '-32.35045498578529, -65.01366813627159', '3.00', 1, 'Disponible', 0, 'Local con dependencias, oficina de 12m2'),
(4, 'Junin 345', 'Comercial', 2, 0, '-32.35045498578529, -65.01366813627159', '4.00', 8, 'Disponible', 0, 'Local 232 metros, con entrepiso  2 baños'),
(5, 'Junin 345', 'Comercial', 1, 5, '-33.25, -66.36', '55.00', 3, 'Disponible', 0, 'Casa 2 plantas, techo tecja, con cochera 3 autos'),
(6, 'Junin 345', 'Comercial', 2, 9, '-33.25, -66.36', '2635.52', 3, 'Disponible', 0, 'Dpto 5º piso,  3 dormitorios, uno en suite. '),
(7, 'Junin 345', 'Comercial', 1, 0, '-33.25, -66.36', '5555.00', 3, 'Disponible', 0, '55dthysh'),
(8, 'nose', 'Comercial', 2, 0, '-33.25, -66.36', '150000.00', 3, 'Disponible', 0, '6666666666666'),
(9, 'Junin 345', 'Comercial', 1, 5, '-33.25, -66.36', '5625.00', 3, 'Disponible', 0, 'csa de 2 planta, 3 dormitorios, uno en suite\r\n'),
(10, 'Junin 345', 'Comercial', 1, 4, '-33.25, -66.36', '666.00', 1, 'Disponible', 0, 'ssaf'),
(11, 'Av SiempreViva', 'Residencial', 1, 3, '-33.25, -66.36', '5500.00', 1, 'Disponible', 0, 'Casa de 3 ambientes');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilino`
--

DROP TABLE IF EXISTS `inquilino`;
CREATE TABLE IF NOT EXISTS `inquilino` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(45) NOT NULL,
  `apellido` varchar(45) NOT NULL,
  `dni` varchar(11) NOT NULL,
  `email` varchar(45) NOT NULL,
  `telefono` varchar(45) NOT NULL,
  `borrado` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`),
  UNIQUE KEY `email_UNIQUE` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4;

--
-- RELACIONES PARA LA TABLA `inquilino`:
--

--
-- Volcado de datos para la tabla `inquilino`
--

INSERT INTO `inquilino` (`id`, `nombre`, `apellido`, `dni`, `email`, `telefono`, `borrado`) VALUES
(1, 'Marcelo', 'Jofre', '1256555', 'b1729985-ffa2-11ee-a424-b8aeedb3ac9e', 'b1729993-ffa2-11ee-a424-b8aeedb3ac9e', 0),
(2, 'Jorge', 'Mendez', '1256555', '905b3f1b-ee14-11ee-8ebc-b8aeedb3ac9e', '905b3f29-ee14-11ee-8ebc-b8aeedb3ac9e', 0),
(3, 'Natalia', 'Gomez', '1256555', '0f078f6f-ff9f-11ee-a424-b8aeedb3ac9e', '0f078f7c-ff9f-11ee-a424-b8aeedb3ac9e', 0),
(4, 'Maria Florencia', 'Fernandez', '1256555', 'd3d99b4c-0053-11ef-a424-b8aeedb3ac9e', 'd3d99b62-0053-11ef-a424-b8aeedb3ac9e', 0),
(6, 'Eduardo', 'Maldonado', '1234567890', 'ajfb@sas', '12345678', 0),
(7, 'Milton', 'Hersheys', '123456789', 'milton@mail.com', '2664112233', 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

DROP TABLE IF EXISTS `pago`;
CREATE TABLE IF NOT EXISTS `pago` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_contrato` int(11) NOT NULL,
  `fecha_pago` date NOT NULL DEFAULT current_timestamp(),
  `importe` decimal(11,2) NOT NULL COMMENT 'si es negativo es una nota de credito',
  `estado` tinyint(2) DEFAULT 0,
  `numero_pago` int(10) UNSIGNED NOT NULL DEFAULT 1,
  `detalle` varchar(150) NOT NULL COMMENT 'aca van los detalles de cada abono -> paga el mes x - abono mes x',
  `creado_fecha` datetime NOT NULL DEFAULT current_timestamp(),
  `creado_usuario` int(11) NOT NULL,
  `editado_usuario` int(2) DEFAULT NULL,
  `editado_fecha` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_contrato` (`id_contrato`,`numero_pago`),
  KEY `pago_contrato_idx` (`id_contrato`),
  KEY `usuario_creado` (`creado_usuario`),
  KEY `usuario_editado` (`editado_usuario`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4;

--
-- RELACIONES PARA LA TABLA `pago`:
--   `id_contrato`
--       `contrato` -> `id`
--   `creado_usuario`
--       `usuario` -> `id`
--   `editado_usuario`
--       `usuario` -> `id`
--

--
-- Volcado de datos para la tabla `pago`
--

INSERT INTO `pago` (`id`, `id_contrato`, `fecha_pago`, `importe`, `estado`, `numero_pago`, `detalle`, `creado_fecha`, `creado_usuario`, `editado_usuario`, `editado_fecha`) VALUES
(2, 1, '2024-04-24', '1500.00', 0, 1, 'pago de mayo', '2024-04-24 21:52:00', 1, NULL, NULL),
(3, 1, '2024-04-24', '1500.00', 0, 2, 'pago adelantado de junio', '2024-04-24 23:10:21', 1, NULL, NULL),
(4, 1, '2024-04-24', '-500.00', 0, 3, 'Nota de crédito: reposición de un caño ', '2024-04-24 23:11:21', 1, NULL, NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietario`
--

DROP TABLE IF EXISTS `propietario`;
CREATE TABLE IF NOT EXISTS `propietario` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(45) NOT NULL,
  `apellido` varchar(45) NOT NULL,
  `dni` varchar(45) NOT NULL,
  `email` varchar(100) NOT NULL,
  `telefono` varchar(45) NOT NULL,
  `borrado` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`),
  UNIQUE KEY `email_UNIQUE` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4;

--
-- RELACIONES PARA LA TABLA `propietario`:
--

--
-- Volcado de datos para la tabla `propietario`
--

INSERT INTO `propietario` (`id`, `nombre`, `apellido`, `dni`, `email`, `telefono`, `borrado`) VALUES
(1, 'Jose', 'Perez', '12345678', '64771466-ff83-11ee-a424-b8aeedb3ac9e', '64771473-ff83-11ee-a424-b8aeedb3ac9e', 0),
(3, 'Marcelo', 'JOFE', '12345678', '372d0744-ff9d-11ee-a424-b8aeedb3ac9e', '372d0751-ff9d-11ee-a424-b8aeedb3ac9e', 0),
(8, 'Jose', 'Perez', '12345678', 'aaa@aa.coms', '1234', 0),
(9, 'Marcelo', 'JOFE', '12345678', 'cbd8df41-faa2-11ee-9c9d-b8aeedb3', 'cbd8df4b-faa2-11ee-9c9d-b8aeedb3', 0),
(10, 'Pedro', 'Blanco', '12569865', '4bd9ea81-7f90-4358-8b67-2bc6e78f7f16', 'b2944aa0-0053-11ef-a424-b8aeedb3ac9e', 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipo_inmueble`
--

DROP TABLE IF EXISTS `tipo_inmueble`;
CREATE TABLE IF NOT EXISTS `tipo_inmueble` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tipo` varchar(200) NOT NULL,
  `borrado` tinyint(1) DEFAULT 0,
  PRIMARY KEY (`id`),
  UNIQUE KEY `tipo` (`tipo`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4;

--
-- RELACIONES PARA LA TABLA `tipo_inmueble`:
--

--
-- Volcado de datos para la tabla `tipo_inmueble`
--

INSERT INTO `tipo_inmueble` (`id`, `tipo`, `borrado`) VALUES
(1, 'Casa', 0),
(2, 'Departamento', 0),
(3, 'Deposito', 0),
(4, 'Local', 0),
(5, 'Cabaña', 0),
(6, 'Quintas', 0),
(7, 'hostel', 0),
(8, 'CAMPING', 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

DROP TABLE IF EXISTS `usuario`;
CREATE TABLE IF NOT EXISTS `usuario` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(45) NOT NULL,
  `apellido` varchar(45) NOT NULL,
  `dni` varchar(45) NOT NULL,
  `email` varchar(45) NOT NULL,
  `password` varchar(250) NOT NULL,
  `rol` enum('usuario','administrador') NOT NULL DEFAULT 'usuario' COMMENT 'solo vamos a usar 2 tipos de usuarios.\\n- usuario normal de la plataforma\\n- un  administrador',
  `avatarUrl` varchar(100) DEFAULT NULL,
  `borrado` tinyint(1) DEFAULT 0 COMMENT '0 para activo, 1 para inactivo',
  `update_at` datetime NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COMMENT='tabla para usuarios internos del sistema';

--
-- RELACIONES PARA LA TABLA `usuario`:
--

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`id`, `nombre`, `apellido`, `dni`, `email`, `password`, `rol`, `avatarUrl`, `borrado`, `update_at`) VALUES
(1, 'Leonel', 'Toloza', '123456789', 'admin@admin.com', '$2a$11$..jMfFLHoPD4mkiDz6neX.80LhpUwcIwkSZtKrlB1lX4z6k4T9Jw2', 'administrador', 'Designer.jpeg', 0, '2024-04-25 11:55:52'),
(2, 'Santiago Leonel', 'Toloza', '987654321', 'leotoloza6@gmail.com', '$2a$11$IYzzl8cwybgKg7dAe/URhO9qZXGXAUtcqZXrkpHnjahqzGzJoQDuG', 'usuario', NULL, 0, '2024-04-25 11:58:13'),
(3, 'Rafael ', 'Lopez', '123456', 'lopezrafa@gmail.com', '$2a$11$zujeCmTH/ewXdFu738wpqur5i69oPqLpIam6vGtLmHRdr55zft.m6', 'administrador', NULL, 0, '2024-04-25 10:31:20');

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD CONSTRAINT `contrato_inmueble` FOREIGN KEY (`id_inmueble`) REFERENCES `inmueble` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `contrato_inquilino` FOREIGN KEY (`id_inquilino`) REFERENCES `inquilino` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD CONSTRAINT `inmueble_tipo` FOREIGN KEY (`id_tipo`) REFERENCES `tipo_inmueble` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `propietario_inmueble` FOREIGN KEY (`id_propietario`) REFERENCES `propietario` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `pago`
--
ALTER TABLE `pago`
  ADD CONSTRAINT `pago_contrato` FOREIGN KEY (`id_contrato`) REFERENCES `contrato` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `usuario_creado` FOREIGN KEY (`creado_usuario`) REFERENCES `usuario` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `usuario_editado` FOREIGN KEY (`editado_usuario`) REFERENCES `usuario` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
