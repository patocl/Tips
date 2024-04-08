DECLARE @oldDatabaseName NVARCHAR(128) = 'database01'
DECLARE @newDatabaseName NVARCHAR(128) = 'database02'

DECLARE @sql NVARCHAR(MAX) = ''

-- Generate alteration scripts for Views
SELECT @sql = @sql +
    'GO' + CHAR(13) +
    'IF OBJECT_ID(''[' + SCHEMA_NAME(schema_id) + '].[' + name + ']'') IS NOT NULL' + CHAR(13) +
    '    DROP VIEW ' + QUOTENAME(SCHEMA_NAME(schema_id)) + '.' + QUOTENAME(name) + ';' + CHAR(13) +
    'GO' + CHAR(13) +
    REPLACE(OBJECT_DEFINITION(object_id), @oldDatabaseName, @newDatabaseName) + CHAR(13)
FROM sys.views
WHERE OBJECT_DEFINITION(object_id) LIKE '%' + @oldDatabaseName + '%';

-- Generate alteration scripts for Functions
SELECT @sql = @sql +
    'GO' + CHAR(13) +
    'IF OBJECT_ID(''[' + SCHEMA_NAME(o.schema_id) + '].[' + o.name + ']'') IS NOT NULL' + CHAR(13) +
    '    DROP FUNCTION ' + QUOTENAME(SCHEMA_NAME(o.schema_id)) + '.' + QUOTENAME(o.name) + ';' + CHAR(13) +
    'GO' + CHAR(13) +
    REPLACE(m.definition, @oldDatabaseName, @newDatabaseName) + CHAR(13)
FROM sys.sql_modules m
INNER JOIN sys.objects o ON m.object_id = o.object_id
WHERE m.definition LIKE '%' + @oldDatabaseName + '%'
AND o.type_desc IN ('SQL_SCALAR_FUNCTION', 'SQL_TABLE_VALUED_FUNCTION', 'SQL_INLINE_TABLE_VALUED_FUNCTION');

-- Generate alteration scripts for Stored Procedures
SELECT @sql = @sql +
    'GO' + CHAR(13) +
    'IF OBJECT_ID(''[' + SCHEMA_NAME(o.schema_id) + '].[' + o.name + ']'') IS NOT NULL' + CHAR(13) +
    '    DROP PROCEDURE ' + QUOTENAME(SCHEMA_NAME(o.schema_id)) + '.' + QUOTENAME(o.name) + ';' + CHAR(13) +
    'GO' + CHAR(13) +
    REPLACE(m.definition, @oldDatabaseName, @newDatabaseName) + CHAR(13)
FROM sys.sql_modules m
INNER JOIN sys.objects o ON m.object_id = o.object_id
WHERE m.definition LIKE '%' + @oldDatabaseName + '%'
AND o.type_desc = 'SQL_STORED_PROCEDURE';

PRINT @sql -- This will print the generated scripts
