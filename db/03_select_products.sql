-- =====================================================================
-- 03_select_products.sql
-- Consultas de ejemplo sobre la tabla products.
-- Ejecutar con psql, DBeaver o pgAdmin.
-- =====================================================================

-- 1. Listado completo (equivalente a GET /v1/api/products)
SELECT id, name, description, price, stock, created_at, updated_at
FROM public.products
ORDER BY id;

-- 2. Producto por id (equivalente a GET /v1/api/products/{id})
SELECT id, name, description, price, stock, created_at, updated_at
FROM public.products
WHERE id = 1;

-- 3. Busqueda por nombre (sin distinguir mayusculas/minusculas)
SELECT id, name, price, stock
FROM public.products
WHERE name ILIKE '%notebook%'
ORDER BY price DESC;

-- 4. Productos en un rango de precios
SELECT id, name, price
FROM public.products
WHERE price BETWEEN 100 AND 500
ORDER BY price;

-- 5. Productos sin stock o con stock bajo (reposicion)
SELECT id, name, stock
FROM public.products
WHERE stock < 10
ORDER BY stock, name;

-- 6. Top 5 productos mas caros
SELECT id, name, price
FROM public.products
ORDER BY price DESC
LIMIT 5;

-- 7. Resumen del inventario
SELECT COUNT(*)                         AS total_productos,
       SUM(stock)                       AS unidades_en_stock,
       ROUND(AVG(price), 2)             AS precio_promedio,
       SUM(price * stock)               AS valor_total_inventario,
       COUNT(*) FILTER (WHERE stock = 0) AS productos_sin_stock
FROM public.products;

-- 8. Valor de inventario por producto (precio x stock)
SELECT id, name, price, stock, price * stock AS valor_inventario
FROM public.products
ORDER BY valor_inventario DESC;

-- 9. Productos modificados por la API en los ultimos 7 dias
SELECT id, name, updated_at
FROM public.products
WHERE updated_at >= now() - INTERVAL '7 days'
ORDER BY updated_at DESC;



SELECT
    current_database() AS database_name,
    current_user AS username;


SELECT table_schema, table_name
FROM information_schema.tables
WHERE table_schema = 'public'
  AND table_name = 'products';


GRANT USAGE ON SCHEMA public TO bootcamp_user;

GRANT SELECT, INSERT, UPDATE, DELETE
ON TABLE public.products
TO bootcamp_user;