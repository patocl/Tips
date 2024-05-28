/* Definir las macros para los valores de conexión */
%let dsn = MiConexion;
%let user = tu_usuario;
%let password = tu_contraseña;
%let server = gateway_server;

/* Definir la biblioteca SAS que apunta a la base de datos SQL Server usando ODBC */
libname mydblib odbc 
    dsn="&dsn" 
    user="&user" 
    password="&password" 
    server="&server";

/* Opcional: Verificar la conexión listando las tablas disponibles en la base de datos */
proc datasets lib=mydblib;
run;

/* Ejemplo de consulta a la base de datos SQL Server */
proc sql;
   connect to odbc(dsn="&dsn" user="&user" password="&password" server="&server");
   create table work.mi_tabla as
   select * from connection to odbc
   (
      select * from nombre_de_tu_tabla
   );
   disconnect from odbc;
quit;

/* Desconectar la biblioteca SAS */
libname mydblib clear;
