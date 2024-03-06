DECLARE @oldDatabaseName NVARCHAR(128) = 'database01'
DECLARE @newDatabaseName NVARCHAR(128) = 'database02'

DECLARE @sql NVARCHAR(MAX) = ''

-- Update references in Views
SELECT @sql = @sql +
    '-- Update ' + CAST(COUNT(*) AS NVARCHAR(10)) + ' references in Views' + CHAR(13) +
    'ALTER VIEW ' + QUOTENAME(OBJECT_SCHEMA_NAME(v.object_id)) + '.' + QUOTENAME(v.name) + 
    ' AS ' + REPLACE(OBJECT_DEFINITION(v.object_id), @oldDatabaseName, @newDatabaseName) + ';' + CHAR(13)
FROM sys.views v
WHERE OBJECT_DEFINITION(v.object_id) LIKE '%' + @oldDatabaseName + '%'
GROUP BY OBJECT_SCHEMA_NAME(v.object_id), v.name

-- Update references in Functions
SELECT @sql = @sql +
    '-- Update ' + CAST(COUNT(*) AS NVARCHAR(10)) + ' references in Functions' + CHAR(13) +
    'ALTER FUNCTION ' + QUOTENAME(OBJECT_SCHEMA_NAME(m.object_id)) + '.' + QUOTENAME(o.name) + 
    '(@params)' + 
    ' RETURNS ' + REPLACE(m.definition, @oldDatabaseName, @newDatabaseName) + 
    ' AS BEGIN RETURN 0 END;' + CHAR(13) + CHAR(13)
FROM sys.sql_modules m
INNER JOIN sys.objects o ON m.object_id = o.object_id
WHERE m.definition LIKE '%' + @oldDatabaseName + '%'
GROUP BY OBJECT_SCHEMA_NAME(m.object_id), o.name

-- Update references in Stored Procedures
SELECT @sql = @sql +
    '-- Update ' + CAST(COUNT(*) AS NVARCHAR(10)) + ' references in Stored Procedures' + CHAR(13) +
    'ALTER PROCEDURE ' + QUOTENAME(OBJECT_SCHEMA_NAME(m.object_id)) + '.' + QUOTENAME(o.name) + 
    '(@params)' + 
    ' AS BEGIN ' + REPLACE(m.definition, @oldDatabaseName, @newDatabaseName) + ' END;' + CHAR(13) + CHAR(13)
FROM sys.sql_modules m
INNER JOIN sys.objects o ON m.object_id = o.object_id
WHERE m.definition LIKE '%' + @oldDatabaseName + '%'
GROUP BY OBJECT_SCHEMA_NAME(m.object_id), o.name

-- Update references in Synonyms
SELECT @sql = @sql +
    '-- Update ' + CAST(COUNT(*) AS NVARCHAR(10)) + ' references in Synonyms' + CHAR(13) +
    'DROP SYNONYM ' + QUOTENAME(s.name) + ';' + CHAR(13) +
    'CREATE SYNONYM ' + QUOTENAME(s.name) + ' FOR ' +
    REPLACE(s.base_object_name, @oldDatabaseName, @newDatabaseName) + ';' + CHAR(13) + CHAR(13)
FROM sys.synonyms s
WHERE s.base_object_name LIKE '%' + @oldDatabaseName + '%'
GROUP BY s.name, s.base_object_name

PRINT @sql -- This will print the generated scripts
