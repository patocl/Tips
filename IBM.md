### Instructions: Modifying the IBM MQ Client Configuration to Use Installed GSKit
### Step 1: Locate the mqclient.ini file 
1. **Navigate to the IBM MQ client installation directory** : 
- Typically, this directory is something like `C:\Program Files\IBM\MQ\bin` or similar. 
2. **Find the mqclient.ini file** : 
- Within this directory, locate the file named `mqclient.ini`. If it's not present, you may need to create it.
### Step 2: Edit the mqclient.ini file 
1. **Open the mqclient.ini file in a text editor** :
- Right-click on the file and select "Open with", then choose your preferred text editor such as Notepad or Visual Studio Code. 
2. ** section** : 
- In the mqclient.ini file, find or add a section called `[SSL]`. This section contains settings related to SSL/TLS, where you'll specify the location of the GSKit libraries. 
3. **Add the GSKit configuration** : 
- Within the `[SSL]` section, add the following line to specify the path to the GSKit libraries:

```ini
SSLKeyRepository=C:\Program Files\IBM\gsk8
``` 
- Make sure to replace `C:\Program Files\IBM\gsk8` with the actual location where you installed GSKit. 
4. **Save the changes** :
- Save the mqclient.ini file after making the modifications.
### Step 3: Verify the configuration 
1. **Review the mqclient.ini file to ensure the changes were saved correctly** .
- Open the file again in your text editor and verify that the line you specified in the previous step is present and correct.
### Step 4: Restart the IBM MQ client 
1. **Restart any application or service using the IBM MQ client** .
- This will ensure that the configuration changes take effect properly.
### Step 5: Test the configuration 
1. **Test connecting to the IBM MQ server** .
- Use your IBM MQ client application or tool to connect to the server and perform connection tests.
- Verify that there are no SSL/TLS or GSKit-related errors during the connection process.

Following these steps, you should be able to correctly modify the IBM MQ client configuration file to point to the specific instance of GSKit that you have installed. This will ensure that the MQ client uses the appropriate SSL/TLS security configuration when communicating with the MQ server.


### Step 3: Configure IBM Data Server Driver 
1. **Configure GSKit in IBM Data Server Driver** :
- Open a command prompt with administrator privileges.
- Set the specific environment variable for IBM Data Server Driver to use the GSKit instance. 
2. **Modify the db2cli.ini file** :
- Navigate to the directory where IBM Data Server Driver is installed. 
- Open or create the `db2cli.ini` file. 
- Add or modify the GSKit configuration to point to the installed GSKit instance:

```ini
[COMMON]
GSKIT_PATH=C:\Program Files\IBM\gsk8
``` 
3. **Save the changes** : 
- Save the `db2cli.ini` file after making the modifications.
### Step 4: Verify the Configuration 
1. **Check the db2cli.ini file** : 
- Open the `db2cli.ini` file again to ensure the changes were saved correctly.
### Step 5: Restart IBM Data Server Driver 
1. **Restart the applications using IBM Data Server Driver** :
- Restart any applications or services using IBM Data Server Driver to apply the configuration changes.
### Step 6: Test the Configuration 
1. **Test the connection to the DB2 database** :
- Use your DB2 client application or tool to connect to the database.
- Verify that there are no errors related to SSL/TLS or GSKit during the connection process.

Following these steps should allow you to configure IBM Data Server Driver to use the installed instance of GSKit successfully. This ensures that the driver uses the appropriate SSL/TLS security configuration when connecting to the DB2 database.

