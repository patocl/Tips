DECLARE @oldDatabaseName NVARCHAR(128) = 'database01'
DECLARE @newDatabaseName NVARCHAR(128) = 'database02'

DECLARE @sql NVARCHAR(MAX) = ''

-- Count references in Views
DECLARE @ViewCount INT
SELECT @ViewCount = COUNT(*)
FROM sys.views
WHERE OBJECT_DEFINITION(object_id) LIKE '%' + @oldDatabaseName + '%'

-- Count references in Functions
DECLARE @FunctionCount INT
SELECT @FunctionCount = COUNT(*)
FROM sys.sql_modules m
INNER JOIN sys.objects o ON m.object_id = o.object_id
WHERE m.definition LIKE '%' + @oldDatabaseName + '%'

-- Count references in Stored Procedures
DECLARE @StoredProcedureCount INT
SELECT @StoredProcedureCount = COUNT(*)
FROM sys.sql_modules m
INNER JOIN sys.objects o ON m.object_id = o.object_id
WHERE m.definition LIKE '%' + @oldDatabaseName + '%'

-- Count references in Synonyms
DECLARE @SynonymCount INT
SELECT @SynonymCount = COUNT(*)
FROM sys.synonyms
WHERE base_object_name LIKE '%' + @oldDatabaseName + '%'

-- Print the total number of references to update
PRINT '-- Total objects to update: Views(' + CAST(@ViewCount AS NVARCHAR(10)) + '), Functions(' + CAST(@FunctionCount AS NVARCHAR(10)) + '), Stored Procedures(' + CAST(@StoredProcedureCount AS NVARCHAR(10)) + '), Synonyms(' + CAST(@SynonymCount AS NVARCHAR(10)) + ')' + CHAR(13)

-- Generate alteration scripts for Views
SELECT @sql = @sql +
    'ALTER VIEW ' + QUOTENAME(OBJECT_SCHEMA_NAME(object_id)) + '.' + QUOTENAME(name) + 
    ' AS ' + REPLACE(OBJECT_DEFINITION(object_id), @oldDatabaseName, @newDatabaseName) + ';' + CHAR(13)
FROM sys.views
WHERE OBJECT_DEFINITION(object_id) LIKE '%' + @oldDatabaseName + '%'

-- Generate alteration scripts for Functions
SELECT @sql = @sql +
    'ALTER FUNCTION ' + QUOTENAME(OBJECT_SCHEMA_NAME(m.object_id)) + '.' + QUOTENAME(o.name) + 
    '(@params)' + 
    ' RETURNS ' + REPLACE(m.definition, @oldDatabaseName, @newDatabaseName) + 
    ' AS BEGIN RETURN 0 END;' + CHAR(13) + CHAR(13)
FROM sys.sql_modules m
INNER JOIN sys.objects o ON m.object_id = o.object_id
WHERE m.definition LIKE '%' + @oldDatabaseName + '%'

-- Generate alteration scripts for Stored Procedures
SELECT @sql = @sql +
    'ALTER PROCEDURE ' + QUOTENAME(OBJECT_SCHEMA_NAME(m.object_id)) + '.' + QUOTENAME(o.name) + 
    '(@params)' + 
    ' AS BEGIN ' + REPLACE(m.definition, @oldDatabaseName, @newDatabaseName) + ' END;' + CHAR(13) + CHAR(13)
FROM sys.sql_modules m
INNER JOIN sys.objects o ON m.object_id = o.object_id
WHERE m.definition LIKE '%' + @oldDatabaseName + '%'

-- Generate alteration scripts for Synonyms
SELECT @sql = @sql +
    'DROP SYNONYM ' + QUOTENAME(name) + ';' + CHAR(13) +
    'CREATE SYNONYM ' + QUOTENAME(name) + ' FOR ' +
    REPLACE(base_object_name, @oldDatabaseName, @newDatabaseName) + ';' + CHAR(13) + CHAR(13)
FROM sys.synonyms
WHERE base_object_name LIKE '%' + @oldDatabaseName + '%'

PRINT @sql -- This will print the generated scripts
