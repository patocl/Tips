# Configuring IBM MQ with AD Accounts and TLS/SSL

## Introduction

This guide provides step-by-step instructions to configure IBM MQ for seamless integration with Active Directory (AD) accounts using VAS (Vendor Authentication Service), mapping the "NETWORK SERVICE" account for MSDTC (Microsoft Distributed Transaction Coordinator) on Windows, and implementing TLS/SSL authentication for secure communication between clients and servers. Follow these instructions to ensure a smooth setup and deployment of IBM MQ in your environment.

Shortcuts to emulate CTRL+ALT+DEL -> 
C:\Windows\explorer.exe shell:::{2559a1f2-21d7-11d4-bdaf-00c04f60b9f0}

## Part 1: Configuring the MQ Server on Linux  

1. **Configure VAS on Linux**  

- Verify VAS Configuration:

    ```sh
    id ad_user@DOMAIN
    ``` 

2. **Configure IBM MQ** 

- Create the Queue Manager:

    ```sh
    crtmqm QM1
    strmqm QM1
    ``` 

- Create and Configure a Listener:

    ```sh
    runmqlsr -t TCP -m QM1 -p 1414 &
    ``` 

- Create the Server Connection Channel:

    ```sh
    runmqsc QM1
    DEFINE CHANNEL(CHANNEL_NAME) CHLTYPE(SVRCONN) TRPTYPE(TCP) MCAUSER('mqm_user')
    ``` 

3. **Assign Permissions to AD Users**  

- Permissions for Specific AD Users:

    ```sh
    setmqaut -m QM1 -t qmgr -p ad_user@DOMAIN +connect +inq +setid
    setmqaut -m QM1 -n QUEUE_NAME -t queue -p ad_user@DOMAIN +all
    ``` 

- Permissions for AD Groups:

    ```sh
    setmqaut -m QM1 -t qmgr -g ad_group +connect +inq +setid
    setmqaut -m QM1 -n QUEUE_NAME -t queue -g ad_group +all
    ``` 

4. ** Rules**  

- Configure User-Specific Rules:

    ```sh
    runmqsc QM1
    SET CHLAUTH(CHANNEL_NAME) TYPE(USERMAP) CLNTUSER('ad_user@DOMAIN') USERSRC(MAP) MCAUSER('mqm_user')
    ``` 
- Configure General Rules for Domain Users:

    ```sh
    runmqsc QM1
    SET CHLAUTH(CHANNEL_NAME) TYPE(USERMAP) CLNTUSER('*@DOMAIN') USERSRC(MAP) MCAUSER('mqm_user')
    ``` 

5. **Configure TLS/SSL in IBM MQ**  

- Create Certificates and Keystores:

    First, create a Keystore and a *Truststore* on the MQ server.

    ```sh
    # Create directories for the stores
    mkdir /var/mqm/qmgrs/QM1/ssl

    # Create the keystore
    runmqckm -keydb -create -db /var/mqm/qmgrs/QM1/ssl/key.kdb -pw password -type cms -stash

    # Create the certificate
    runmqckm -cert -create -db /var/mqm/qmgrs/QM1/ssl/key.kdb -pw password -label ibmwebspheremq1 -dn "CN=yourserver.yourdomain.com, OU=YourOU, O=YourOrg, L=YourCity, ST=YourState, C=YourCountry"

    # Extract the certificate
    runmqckm -cert -extract -db /var/mqm/qmgrs/QM1/ssl/key.kdb -pw password -label ibmwebspheremq1 -file /var/mqm/qmgrs/QM1/ssl/ibmwebspheremq1.arm -format ascii
    ``` 

- Configure the Queue Manager to Use TLS/SSL:

    ```sh
    runmqsc QM1
    ALTER QMGR SSLKEYR('/var/mqm/qmgrs/QM1/ssl/key')
    ``` 

- Configure the Channel to Use TLS/SSL:

    ```sh
    runmqsc QM1
    ALTER CHANNEL(CHANNEL_NAME) CHLTYPE(SVRCONN) SSLCIPH(TLS_RSA_WITH_AES_256_CBC_SHA256)
    ALTER CHANNEL(CHANNEL_NAME) CHLTYPE(SVRCONN) SSLCAUTH(REQUIRED)
    ```
---

## Part 2: Configuring the MQ Client on Windows

1. **Install the IBM MQ Client on Windows**  

- Install the IBM MQ Client:

    Ensure you have the IBM MQ Client installed on Windows. 

- Set Environment Variables:

    ```sh
    set MQSERVER=CHANNEL_NAME/TCP/hostname(port)
    ``` 

2. **Configure MSDTC to Use "NETWORK SERVICE"**  

- Configure MSDTC to Run as "NETWORK SERVICE":

    In the Windows Services Manager, configure the MSDTC service to run under the "Network Service" account.

    1. Open the Services Manager (services.msc).
    1. Find the "Distributed Transaction Coordinator" service.
    1. Right-click and select "Properties".
    1. Go to the "Log On" tab.
    1. Select "Local System account" and ensure "Allow service to interact with desktop" is checked. 

3. **Configure TLS/SSL in the MQ Client**  

- Import Server Certificate to the Client Truststore:

    On the client, import the server's certificate into the Truststore.

    ```sh
    runmqckm -cert -add -db truststore.kdb -pw password -label ibmwebspheremq1 -file C:\path\to\ibmwebspheremq1.arm -format ascii
    ``` 

- Set Keystore and Truststore in the Client:

    ```sh
    SET MQSSLKEYR=C:\path\to\key
    ``` 
- Configure the Client Application to Use TLS/SSL:

    ```java
    MQEnvironment.sslCipherSuite = "TLS_RSA_WITH_AES_256_CBC_SHA256";
    MQEnvironment.sslKeyRepository = "C:\\path\\to\\key";
    ``` 
4. **Configure the Application to Use MSDTC**  

- Application Configuration (Example in .NET):

    ```csharp
    var factory = new MQXAResourceFactory();
    factory.HostName = "hostname";
    factory.Channel = "CHANNEL_NAME";
    factory.Port = 1414;
    factory.UserID = "ad_user@DOMAIN";
    factory.Password = "ad_password";
    factory.SSLCipherSpec = "TLS_RSA_WITH_AES_256_CBC_SHA256";
    factory.SSLKeyRepository = "C:\\path\\to\\key";

    var xares = factory.CreateXAResource();
    ```

---

## Part 3: Validation of the Configuration  

1. **Validation on the MQ Server (Linux)**  

- Verify Users and Groups:

    ```sh
    id ad_user@DOMAIN
    ``` 

- Verify Assigned Permissions:

    ```sh
    dspmqaut -m QM1 -t qmgr -p ad_user@DOMAIN
    dspmqaut -m QM1 -n QUEUE_NAME -t queue -p ad_user@DOMAIN
    ``` 

- Rules:

    ```sh
    runmqsc QM1
    DISPLAY CHLAUTH(CHANNEL_NAME)
    ``` 

- Verify TLS/SSL Configuration :

    ```sh
    runmqsc QM1
    DISPLAY CHANNEL(CHANNEL_NAME) SSLCAUTH
    DISPLAY QMGR SSLKEYR
    ``` 

2. **Validation on the MQ Client (Windows)**

- Test Connection from the Client:

    Use a test application to connect to the MQ server.

    ```java
    MQQueueManager qMgr = new MQQueueManager("QM1");
    ``` 

- Verify Active Connections on the Server:

    ```sh
    runmqsc QM1
    DISPLAY CONN(*) TYPE(USERID)
    ```

---

## Documentation of Problems and Solutions

- **User Not Recognized** :

    Verify the VAS configuration and proper integration with AD. 

- **Insufficient Permissions** :

    Check the permissions assigned with `setmqaut` and adjust as needed. 

- **Connection Errors** :

    Ensure network and firewall configurations allow communication between the client and the MQ server. 

- **TLS/SSL Errors** :

    Verify the correct configuration of certificates and keystores. Ensure the certificates are valid and trusted. 

- **MSDTC Errors** :

    Ensure MSDTC is properly configured and network settings allow distributed transactions.

Following this guide, you should be able to configure IBM MQ to use AD accounts via VAS, map the "NETWORK SERVICE" account for MSDTC, and secure the communication with TLS/SSL.
