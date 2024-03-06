DECLARE @oldDatabaseName NVARCHAR(128) = 'database01'
DECLARE @newDatabaseName NVARCHAR(128) = 'database02'

DECLARE @sql NVARCHAR(MAX)

-- Actualizar referencias en Vistas
SELECT @sql = ISNULL(@sql, '') +
    'ALTER VIEW ' + QUOTENAME(OBJECT_SCHEMA_NAME(object_id)) + '.' + QUOTENAME(name) + 
    ' AS ' + REPLACE(OBJECT_DEFINITION(object_id), @oldDatabaseName, @newDatabaseName) + ';' + CHAR(13)
FROM sys.views
WHERE OBJECT_DEFINITION(object_id) LIKE '%' + @oldDatabaseName + '%'

-- Actualizar referencias en Funciones
SELECT @sql = @sql + ISNULL(@sql, '') +
    'ALTER FUNCTION ' + QUOTENAME(OBJECT_SCHEMA_NAME(object_id)) + '.' + QUOTENAME(name) + 
    '(@params)' + 
    ' RETURNS ' + REPLACE(definition, @oldDatabaseName, @newDatabaseName) + 
    ' AS BEGIN RETURN 0 END;' + CHAR(13)
FROM sys.sql_modules
WHERE definition LIKE '%' + @oldDatabaseName + '%'

-- Actualizar referencias en Procedimientos Almacenados
SELECT @sql = @sql + ISNULL(@sql, '') +
    'ALTER PROCEDURE ' + QUOTENAME(OBJECT_SCHEMA_NAME(object_id)) + '.' + QUOTENAME(name) + 
    '(@params)' + 
    ' AS BEGIN ' + REPLACE(definition, @oldDatabaseName, @newDatabaseName) + ' END;' + CHAR(13)
FROM sys.sql_modules
WHERE definition LIKE '%' + @oldDatabaseName + '%'

-- Actualizar referencias en Sinónimos
SELECT @sql = @sql + ISNULL(@sql, '') +
    'DROP SYNONYM ' + QUOTENAME(name) + ';' + CHAR(13) +
    'CREATE SYNONYM ' + QUOTENAME(name) + ' FOR ' +
    REPLACE(base_object_name, @oldDatabaseName, @newDatabaseName) + ';' + CHAR(13)
FROM sys.synonyms
WHERE base_object_name LIKE '%' + @oldDatabaseName + '%'

PRINT @sql -- Esto imprimirá los scripts generados
