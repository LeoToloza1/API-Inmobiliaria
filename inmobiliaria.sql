-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 24-05-2024 a las 01:00:54
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
CREATE TABLE `contrato` (
  `id` int(11) NOT NULL,
  `id_inquilino` int(11) NOT NULL,
  `id_inmueble` int(11) NOT NULL,
  `fecha_inicio` date DEFAULT NULL,
  `fecha_fin` date DEFAULT NULL,
  `fecha_efectiva` date DEFAULT NULL,
  `monto` decimal(9,2) DEFAULT NULL,
  `borrado` tinyint(1) DEFAULT 0,
  `creado_fecha` datetime NOT NULL DEFAULT current_timestamp(),
  `cancelado_fecha` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`id`, `id_inquilino`, `id_inmueble`, `fecha_inicio`, `fecha_fin`, `fecha_efectiva`, `monto`, `borrado`, `creado_fecha`, `cancelado_fecha`) VALUES
(1, 3, 1, '2022-04-23', '2023-04-30', '0001-01-01', '1500.00', 0, '2024-04-23 01:47:34', '2024-04-24 16:31:44'),
(2, 3, 1, '2023-04-23', '2025-04-23', '2025-04-23', '23425.00', 0, '2024-04-23 01:49:19', '2024-04-23 01:53:57'),
(3, 6, 9, '2024-04-26', '2025-04-25', '2025-04-25', '2500.00', 0, '2024-04-23 02:04:46', '2024-04-23 02:04:46'),
(4, 4, 3, '2024-04-25', '2024-04-30', '2024-04-30', '1000.00', 0, '2024-04-24 22:10:10', NULL),
(5, 4, 10, '2024-05-01', '2024-05-01', '2024-05-01', '5000.00', 1, '2024-04-25 12:15:10', NULL),
(6, 6, 11, '2024-05-01', '2025-05-01', '2025-05-01', '6000.00', 0, '2024-05-01 17:31:34', NULL),
(7, 4, 4, '2024-05-23', '2025-05-31', NULL, '150000.00', 0, '2024-05-23 19:53:51', NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmueble`
--

DROP TABLE IF EXISTS `inmueble`;
CREATE TABLE `inmueble` (
  `id` int(11) NOT NULL,
  `direccion` varchar(100) NOT NULL COMMENT 'calle y altura',
  `uso` enum('Comercial','Residencial') NOT NULL DEFAULT 'Comercial',
  `id_tipo` int(11) NOT NULL,
  `ambientes` tinyint(2) NOT NULL DEFAULT 1,
  `coordenadas` varchar(100) DEFAULT NULL,
  `precio` decimal(11,2) DEFAULT NULL,
  `id_propietario` int(11) NOT NULL,
  `estado` enum('Disponible','Retirado') DEFAULT 'Retirado',
  `borrado` tinyint(1) NOT NULL DEFAULT 0,
  `descripcion` text DEFAULT NULL,
  `avatarUrl` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`id`, `direccion`, `uso`, `id_tipo`, `ambientes`, `coordenadas`, `precio`, `id_propietario`, `estado`, `borrado`, `descripcion`, `avatarUrl`) VALUES
(1, 'san martin 45', 'Residencial', 1, 9, '-32.414566613131946, -65.00877119828924', '10000.00', 11, 'Disponible', 0, 'Casa  de 2 ambientes', 'casa1.jpeg'),
(2, 'av Los Mandarinos 566', 'Comercial', 4, 0, '-32.4239588393048, -65.01171109578154', '20000.00', 11, 'Disponible', 0, 'Oficina de 250m2', 'casa2.jpg'),
(3, 'Av. Illia 1234', 'Residencial', 2, 0, '-32.35045498578529, -65.01366813627159', '3.00', 1, 'Disponible', 0, 'Local con dependencias, oficina de 12m2', NULL),
(4, 'Av San Pedro 345 - Villa Mercedes', 'Comercial', 4, 0, '-32.35045498578529, -65.01366813627159', '40000.00', 11, 'Disponible', 0, 'Local 232 metros, con entrepiso  2 baños', 'local1.jpg'),
(5, 'Junin 345', 'Comercial', 1, 5, '-33.25, -66.36', '55.00', 3, 'Disponible', 0, 'Casa 2 plantas, techo tecja, con cochera 3 autos', NULL),
(6, 'Junin 345', 'Comercial', 2, 9, '-33.25, -66.36', '2635.52', 3, 'Disponible', 0, 'Dpto 5º piso,  3 dormitorios, uno en suite. ', NULL),
(7, 'Junin 345', 'Comercial', 1, 0, '-33.25, -66.36', '5555.00', 3, 'Disponible', 0, '55dthysh', NULL),
(8, 'nose', 'Comercial', 2, 0, '-33.25, -66.36', '150000.00', 3, 'Disponible', 0, '6666666666666', NULL),
(9, 'Junin 345', 'Comercial', 1, 5, '-33.25, -66.36', '5625.00', 3, 'Disponible', 0, 'csa de 2 planta, 3 dormitorios, uno en suite\r\n', NULL),
(10, 'Junin 345', 'Comercial', 1, 4, '-33.25, -66.36', '666.00', 1, 'Disponible', 0, 'ssaf', NULL),
(11, 'Av SiempreViva', 'Residencial', 1, 3, '-33.25, -66.36', '5500.00', 11, 'Disponible', 0, 'Casa de 3 ambientes', 'casa3.jpg'),
(13, 'el poleo 1430', 'Residencial', 1, 3, '-33,52 -60,27', '1500.00', 11, 'Disponible', 0, 'casa de 3 ambientes', 'casa4.jpg'),
(15, 'el poleo 1234', 'Comercial', 1, 1, NULL, '10000.00', 11, 'Disponible', 0, 'venta de birra', 'IMG-20240511-WA0029.jpeg'),
(16, 'Córdoba 123', 'Residencial', 2, 2, NULL, '120000.00', 11, 'Retirado', 0, 'Departamento en el centro', 'IMG-20240519-WA0017.jpg');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilino`
--

DROP TABLE IF EXISTS `inquilino`;
CREATE TABLE `inquilino` (
  `id` int(11) NOT NULL,
  `nombre` varchar(45) NOT NULL,
  `apellido` varchar(45) NOT NULL,
  `dni` varchar(11) NOT NULL,
  `email` varchar(45) NOT NULL,
  `telefono` varchar(45) NOT NULL,
  `borrado` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `inquilino`
--

INSERT INTO `inquilino` (`id`, `nombre`, `apellido`, `dni`, `email`, `telefono`, `borrado`) VALUES
(1, 'Marcelo', 'Jofre', '1256555', 'marcelo@gmail.com', '2664271471', 0),
(2, 'Jorge', 'Mendez', '1256555', 'jorge@gmail.com', '2664702741', 0),
(3, 'Natalia', 'Gomez', '1256555', 'nataly@gmail.com', '265712345', 0),
(4, 'Maria Florencia', 'Fernandez', '1256555', 'mari_flor@gmail.com', '11339987', 0),
(6, 'Eduardo', 'Maldonado', '1234567890', 'mail_raro@saas.com', '12345678', 0),
(7, 'Milton', 'Hersheys', '123456789', 'milton@mail.com', '2664112233', 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

DROP TABLE IF EXISTS `pago`;
CREATE TABLE `pago` (
  `id` int(11) NOT NULL,
  `id_contrato` int(11) NOT NULL,
  `fecha_pago` date NOT NULL DEFAULT current_timestamp(),
  `importe` decimal(11,2) NOT NULL COMMENT 'si es negativo es una nota de credito',
  `estado` tinyint(1) DEFAULT 0,
  `numero_pago` int(10) UNSIGNED NOT NULL DEFAULT 1,
  `detalle` varchar(150) NOT NULL COMMENT 'aca van los detalles de cada abono -> paga el mes x - abono mes x'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `pago`
--

INSERT INTO `pago` (`id`, `id_contrato`, `fecha_pago`, `importe`, `estado`, `numero_pago`, `detalle`) VALUES
(2, 1, '2024-04-24', '1500.00', 0, 1, 'pago de mayo'),
(3, 1, '2024-04-24', '1500.00', 0, 2, 'pago adelantado de junio'),
(4, 1, '2024-04-24', '-500.00', 0, 3, 'Nota de crédito: reposición de un caño '),
(7, 1, '2024-05-01', '-1500.00', 0, 4, 'Nota de credito por rotura de puerta'),
(8, 6, '2024-05-19', '6000.00', 0, 1, 'Pago correspondiente al mes de mayo'),
(9, 2, '2024-05-23', '23425.00', 0, 1, 'Pago correspondiente al mes en curso');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietario`
--

DROP TABLE IF EXISTS `propietario`;
CREATE TABLE `propietario` (
  `id` int(11) NOT NULL,
  `nombre` varchar(45) NOT NULL,
  `apellido` varchar(45) NOT NULL,
  `dni` varchar(45) NOT NULL,
  `email` varchar(100) NOT NULL,
  `password` varchar(200) DEFAULT NULL,
  `telefono` varchar(45) NOT NULL,
  `borrado` tinyint(1) NOT NULL DEFAULT 0,
  `avatarUrl` varchar(200) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `propietario`
--

INSERT INTO `propietario` (`id`, `nombre`, `apellido`, `dni`, `email`, `password`, `telefono`, `borrado`, `avatarUrl`) VALUES
(1, 'Jose ', 'Perez', '123456789', 'jose@gmail.com', '$2a$11$SQMNFnL3HbnJtQPrj4gp5uiqjWBrAy7huhQ9/qWLYwNu4hHMddqQq', '64771473-ff83-11ee-a424-b8aeedb3ac9e', 0, NULL),
(3, 'Marcelo', 'JOFE', '12345678', '372d0744-ff9d-11ee-a424-b8aeedb3ac9e', NULL, '372d0751-ff9d-11ee-a424-b8aeedb3ac9e', 0, NULL),
(8, 'Jose', 'Perez', '12345678', 'aaa@aa.coms', NULL, '1234', 0, NULL),
(9, 'Marcelo', 'JOFE', '12345678', 'cbd8df41-faa2-11ee-9c9d-b8aeedb3', NULL, 'cbd8df4b-faa2-11ee-9c9d-b8aeedb3', 0, NULL),
(10, 'Pedro', 'Blanco', '12569865', '4bd9ea81-7f90-4358-8b67-2bc6e78f7f16', NULL, 'b2944aa0-0053-11ef-a424-b8aeedb3ac9e', 0, NULL),
(11, 'Santiago Leonel', 'Toloza', '38860057', 'leotoloza6@gmail.com', '$2a$11$RFB2rvycLfV0dIhw4ZbS9O91WQ0YZePXLCWTam78N.o8UjNT3sr9m', '1133466839', 0, 'designer.jpeg');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipo_inmueble`
--

DROP TABLE IF EXISTS `tipo_inmueble`;
CREATE TABLE `tipo_inmueble` (
  `id` int(11) NOT NULL,
  `tipo` varchar(200) NOT NULL,
  `borrado` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

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
(8, 'CAMPING', 0),
(11, 'Hotel', 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

DROP TABLE IF EXISTS `usuario`;
CREATE TABLE `usuario` (
  `id` int(11) NOT NULL,
  `nombre` varchar(45) NOT NULL,
  `apellido` varchar(45) NOT NULL,
  `dni` varchar(45) NOT NULL,
  `email` varchar(45) NOT NULL,
  `password` varchar(250) NOT NULL,
  `rol` enum('usuario','administrador') NOT NULL DEFAULT 'usuario' COMMENT 'solo vamos a usar 2 tipos de usuarios.\\n- usuario normal de la plataforma\\n- un  administrador',
  `avatarUrl` varchar(100) DEFAULT NULL,
  `borrado` tinyint(1) DEFAULT 0 COMMENT '0 para activo, 1 para inactivo',
  `update_at` datetime NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='tabla para usuarios internos del sistema';

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`id`, `nombre`, `apellido`, `dni`, `email`, `password`, `rol`, `avatarUrl`, `borrado`, `update_at`) VALUES
(1, 'Leonel', 'Toloza', '123456789', 'admin@admin.com', '$2a$11$IYzzl8cwybgKg7dAe/URhO9qZXGXAUtcqZXrkpHnjahqzGzJoQDuG', 'administrador', 'Designer.jpeg', 0, '2024-05-04 14:19:44'),
(2, 'Santiago Leonel', 'Toloza', '987654321', 'leotoloza6@gmail.com', '$2a$11$IYzzl8cwybgKg7dAe/URhO9qZXGXAUtcqZXrkpHnjahqzGzJoQDuG', 'administrador', NULL, 0, '2024-05-04 14:17:18'),
(3, 'Rafael ', 'Lopez', '123456', 'lopezrafa@gmail.com', '$2a$11$zujeCmTH/ewXdFu738wpqur5i69oPqLpIam6vGtLmHRdr55zft.m6', 'administrador', NULL, 0, '2024-04-25 10:31:20');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD PRIMARY KEY (`id`),
  ADD KEY `contrato_inmueble_idx` (`id_inmueble`),
  ADD KEY `contrato_inquilino_idx` (`id_inquilino`),
  ADD KEY `fecha_inicio` (`fecha_inicio`,`fecha_fin`);

--
-- Indices de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD PRIMARY KEY (`id`),
  ADD KEY `inmueble_tipo_idx` (`id_tipo`),
  ADD KEY `propietario_inmueble_idx` (`id_propietario`);

--
-- Indices de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `email_UNIQUE` (`email`);

--
-- Indices de la tabla `pago`
--
ALTER TABLE `pago`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `id_contrato` (`id_contrato`,`numero_pago`),
  ADD KEY `pago_contrato_idx` (`id_contrato`);

--
-- Indices de la tabla `propietario`
--
ALTER TABLE `propietario`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `email_UNIQUE` (`email`);

--
-- Indices de la tabla `tipo_inmueble`
--
ALTER TABLE `tipo_inmueble`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `tipo` (`tipo`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contrato`
--
ALTER TABLE `contrato`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT de la tabla `tipo_inmueble`
--
ALTER TABLE `tipo_inmueble`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

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
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
