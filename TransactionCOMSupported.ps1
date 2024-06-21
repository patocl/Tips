# Definir el nombre inicial del componente
$componentPrefix = "My Components.*"

# Importar el módulo de COM+ Admin
Add-Type -TypeDefinition @"
using System;
using System.EnterpriseServices;
using System.Runtime.InteropServices;

public class ComAdmin {
    [DllImport("comadmin.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr CoCreateInstance([In] ref Guid rclsid, [In] IntPtr pUnkOuter, [In] int dwClsContext, [In] ref Guid riid);
    
    public static ComAdminCatalog GetCatalog() {
        Guid CLSID_COMAdminCatalog = new Guid("F618C514-DFB8-11D1-A2CF-00805FC79235");
        Guid IID_ICOMAdminCatalog = new Guid("DD662187-DFC2-11D1-A2CF-00805FC79235");
        IntPtr pCatalog = CoCreateInstance(ref CLSID_COMAdminCatalog, IntPtr.Zero, 1, ref IID_ICOMAdminCatalog);
        return (ComAdminCatalog)Marshal.GetObjectForIUnknown(pCatalog);
    }
}
"@

# Obtener el catálogo de COM+
$catalog = [ComAdmin]::GetCatalog()

# Obtener y procesar aplicaciones y componentes COM+
$results = $catalog.GetCollection("Applications").Populate() | ForEach-Object {
    $app = $_
    $catalog.GetCollection("Components", $app.Key).Populate() | ForEach-Object {
        $component = $_
        if ($component.Name -like $componentPrefix -and $component.Transaction -gt 0) {
            [PSCustomObject]@{
                ApplicationName = $app.Name
                ComponentName = $component.Name
                TransactionSupport = $component.Transaction
            }
        }
    }
}

# Mostrar los resultados
$results
