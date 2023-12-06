# Introduction

After a COM+ server application is installed on a computer, it may be necessary to access the application remotely by using DCOM or Message Queuing.

If an application is to be accessed remotely, you can use the Component Services administrative tool to export a COM+ application proxy. The application proxy file (.msi) contains the information needed to remotely access classes in a COM+ server application from a client computer through DCOM. Using this file with the Component Services administrative tool, you can install the COM+ application proxy on the site’s client computers.

It is possible to create an application proxy that accesses a different server than the computer from which it was exported. This is useful when the application proxy for a production computer needs to be exported from a staging computer. To change the server name, click the Options tab of the computer properties page in the Component Services administrative tool and enter the correct application proxy remote server name (RSN).

# Exporting COM+ Application Proxies

You can use the Component Services administrative tool to export a COM+ application proxy. When the proxy is installed on client computers, client applications can use the proxy information to find and access, via DCOM, a COM+ server application running on a production computer.

**Important** If a component’s class ID (CLSID), type library identifier (TypeLibId), or interface identifier (IID) changes after you have exported an application, you must export the application proxy again and reinstall it on the client computers.

## To export a COM+ application proxy

1. In the Component Services administrative tool console tree, right-click the application and then click **Export**.
1. In the initial dialog box for the **COM+ Application Export Wizard**, click Next.
1. In the **Application Export Information** dialog box, enter the full path of the application proxy file to be created or click **Browse** to locate an existing file or directory.
1. Under Export as, click **Application proxy – Install on other machines to enable access to this machine** and then click Next.
1. In the closing dialog box, click **Finish**.

By default, the application proxy file includes remote server information that points to the server computer from which the application proxy was exported. If you export an application proxy from a virtual server, by default the application proxy uses the physical node the virtual server was on when exporting as the remote server name. You can use the Component Services administrative tool to change the default remote server name.

The default remote server name in an application proxy file can also be overridden at installation time.

## To specify an alternate remote server used by application proxies

1. In the Component Services administrative tool console tree, right-click the computer from which you are exporting applications and click **Properties**.
1. In the computer properties dialog box, click the **Options** tab.
1. Under **Export**, in the **Application Proxy RSN** box, enter the name of the remote server computer to be used by application proxies.
1. Click **OK**.

# Installing COM+ Application Proxies

After the application proxy is created as a Windows Installer (.msi) file, it is easy to install it on a client computer. Installing the application proxy provides the registration information needed for client applications to access the COM+ application remotely from the client computer.

**Important** You can install a COM+ application proxy on any computer that’s running the Windows Installer. The Windows Installer is automatically installed on all computers running Microsoft® Windows® 2000 or later and is available for computers running Microsoft Windows 95, Microsoft Windows 98, and Microsoft Windows NT® 4.0. However, if an application proxy requires the COM+ Queued Components service, this proxy can be installed only on a computer running Windows 2000 or later. The Queued Components service is available only on platforms that support Component Services.

## To install a COM+ application proxy using the Component Services administrative tool

1. In the console tree of the Component Services administrative tool, under **Component Services**, locate the **COM+ Applications** folder associated with the computer on which you want to install the application.

**Note** If you have the COM+ Partitions service enabled, you can locate the **COM+ Applications** folder in the **COM+ Partitions** folder for the computer on which you want to install the application proxy. For more information about the COM+ Partitions service and instructions for enabling it on your system.

1. Right-click the COM+ Applications folder, point to New, and then click Application.

1. In the initial dialog box for the COM+ Application Install Wizard, click Next.

1. In the Install or Create a New Application dialog box, click Install pre-built application(s).

1. In the Install from application file dialog box, browse to select the .msi file for the application proxy you are installing and then click Open.

1. In the Select Application Files dialog box, click Add to browse for any additional application proxies you want to install and then click Next.

1. In the Application Installation Options dialog box, specify the directory in which to install the COM+ application proxy and then click Next.

The application’s component files are copied from the .msi file to the installation directory. The default installation directory is:

```sh
   %Program Files%\COMPlus Applications\<appid>
```

You can either accept the default directory or click Specific directory and Browse to search for another location.

**Note** To perform this procedure, you must have write access to the installation directory.

1. Click Finish.

The hierarchy in the console tree now shows the new application in the detail pane. If you install multiple applications, the directory you choose in step 8 is used for all the applications.

## To install a COM+ application proxy using Windows Explorer

1. On the client computer, locate the application proxy file (.msi) by using Windows Explorer. This file can reside locally or on a network share.

1. Double-click the application proxy file. You can also right-click the filename and then click Install. The Windows Installer automatically installs the application proxy on the client computer.

## To install a COM+ application proxy using the command prompt

At the command prompt, type the following:

```sh
msiexec -i [<property overrides>] <application_name>.msi
```
In addition to the standard Windows Installer property overrides, you can specify the installer options described in the following table.

Installer option and Description

1. **REMOTESERVERNAME = <new target server>**: Changes the server to which the client computer connects, from the exported default to the specified new target server.

1. **ALLUSERS = 0**: Installs the application proxy only for the current user. By default, the application proxy is installed machine-wide.

1. **TARGETDIR = <target installation directory>**: Changes the installation directory for the COM+ application from the default to the specified directory.