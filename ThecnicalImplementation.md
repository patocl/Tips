# Technical Document: Enhancements in Design and Development - Introduction of WaitRetryPolicyOnSqlDeadlock and Extension Classes

## Table of Contents
1. Introduction
2. Background
3. WaitRetryPolicyOnSqlDeadlock Class
   - 3.1 Purpose
   - 3.2 Constructor Parameters
   - 3.3 Benefits
4. Extension Classes
   - 4.1 CheckExtensions
     - 4.1.1 Purpose
     - 4.1.2 Benefits
   - 4.2 ConvertExtensions
     - 4.2.1 Purpose
     - 4.2.2 Benefits
   - 4.3 GuardExtensions
     - 4.3.1 Purpose
     - 4.3.2 Benefits
5. Conclusion

## 1. Introduction

In the ever-evolving landscape of software development, it is imperative to continuously enhance and improve our codebase to meet the demands of performance, reliability, and maintainability. This technical document outlines the need for introducing a new class, WaitRetryPolicyOnSqlDeadlock, and the addition of extension classes - CheckExtensions, ConvertExtensions, and GuardExtensions - into our software development framework.

## 2. Background

As our software application grows in complexity and handles a greater volume of data and transactions, it is crucial to address specific challenges that may arise. One of these challenges is handling SQL deadlocks gracefully. SQL deadlocks occur when multiple database transactions compete for the same resources and can lead to performance degradation or application failures.

The code project must be built on the VB.NET programming language, and we have chosen to stick with VB.NET to avoid the complexity and overhead associated with creating a new project in C#. This decision aligns with our goal of keeping our current solution as straightforward as possible.


## 3. Folder Structure for the Project

To maintain a well-organized and structured codebase, our project follows a logical folder structure. This structure ensures that different components and modules of the application are neatly organized for easy navigation and maintenance. Below is an overview of our project's folder structure:

### 3.1 Config

- **IConfigurationManager.vb:** This file defines an interface for configuration management within our application.
- **ConfigurationManager.vb:** The ConfigurationManager class implements the IConfigurationManager interface and provides methods to manage application configurations.

### 3.2 Data

- **IDbClient.vb:** This file contains the IDbClient interface, which defines the contract for interacting with the database.
- **SqlClient.vb:** The SqlClient class implements the IDbClient interface and provides functionality for communicating with SQL databases.

### 3.3 Extensions

- **CheckExtensions.vb:** This file hosts functions responsible for performing various checks and returning true or false based on the evaluation.
- **ConvertExtensions.vb:** The ConvertExtensions class contains functions for data type conversion and object transformation.
- **GuardExtensions.vb:** GuardExtensions houses functions that raise exceptions when specific conditions fail.

### 3.4 Logging

- **CompanyLogger.vb:** CompanyLogger is responsible for application-specific logging and implements the ICompanyLogger interface.
- **ICompanyLogger.vb:** The ICompanyLogger interface defines the contract for application-specific logging.
- **ISqlLogger.vb:** ISqlLogger is an interface that specifies methods for SQL-related logging.
- **SqlLogger.vb:** SqlLogger implements ISqlLogger and handles SQL-specific logging.

### 3.5 Polly

- **ContextInfo.vb:** ContextInfo contains classes and structures related to context information during retries.
- **Parameters.vb:** Parameters class encapsulates parameters used in Polly policies.
- **IWaitRetryPolicyOnSqlDeadlock.vb:** This file defines the interface for a wait and retry policy specifically tailored for handling SQL deadlocks.
- **WaitRetryPolicyOnSqlDeadlock.vb:** The WaitRetryPolicyOnSqlDeadlock class implements the IWaitRetryPolicyOnSqlDeadlock interface and manages retry logic in SQL deadlock scenarios.

This organized folder structure aids in code maintenance, promotes code separation, and allows team members to easily locate and work on specific parts of the application. It also aligns with best practices for software development and ensures that our project remains manageable as it continues to evolve.

Please refer to the respective folders and files for detailed documentation on each component and its functionality.

## 4. WaitRetryPolicyOnSqlDeadlock Class

### 4.1 Purpose

The WaitRetryPolicyOnSqlDeadlock class serves a critical role in addressing SQL deadlocks within our software application. When a deadlock occurs during a database transaction, this class harnesses the power of Polly, a resilience and transient-fault-handling library, to automatically manage the retry logic and wait periods, granting the transaction an opportunity to complete successfully.

### 4.2 Constructor Parameters

The WaitRetryPolicyOnSqlDeadlock class features a constructor with three essential parameters:

1. **RetryCount:** This parameter defines the number of retry attempts that will be made before marking the operation as failed. For example, if you set RetryCount to 3, the system will attempt the original transaction and, in the event of a deadlock, will retry it up to three times, allowing for multiple opportunities to succeed.

2. **RetryDelay:** The RetryDelay parameter determines the duration of the pause or wait time between each retry attempt. It is specified in milliseconds. For instance, if you set RetryDelay to 1000 milliseconds, the system will wait for one second between each retry.

3. **ILogger:** This parameter accepts an instance of an ILogger, which is a logging interface commonly used in .NET applications. The ILogger allows you to capture and record detailed information about retry attempts, making it invaluable for troubleshooting and performance monitoring.

### 4.3 Benefits

By utilizing the WaitRetryPolicyOnSqlDeadlock class with the specified constructor parameters, our application can reap several key advantages:

- **Enhanced Reliability:** The automatic retry mechanism increases the likelihood of successful transaction completion in the presence of SQL deadlocks, ultimately boosting the reliability of our application.

- **Improved User Experience:** Users will encounter fewer disruptions caused by database-related issues, resulting in a more seamless and enjoyable experience when interacting with our software.

- **Streamlined Error Handling:** The class encapsulates the retry and wait logic, simplifying error handling and making our codebase more robust and maintainable.

The integration of WaitRetryPolicyOnSqlDeadlock empowers our application to gracefully handle SQL deadlocks, ensuring its resilience even in challenging database contention scenarios.

## 5. Extension Classes

### 5.1 CheckExtensions

#### 5.1.1 Purpose

The CheckExtensions class is responsible for hosting functions that perform various checks and return true or false based on the evaluation. These functions are designed to enhance the readability and reusability of our code by encapsulating common validation logic.

#### 5.1.2 Benefits

- **Code readability:** By centralizing validation logic in the CheckExtensions class, our code becomes more concise and easier to understand.

- **Reusability:** The functions in this class can be reused across different parts of the application, reducing code duplication.

- **Consistency:** Centralized checks promote consistency in our validation processes.

### 5.2 ConvertExtensions

#### 5.2.1 Purpose

The ConvertExtensions class hosts functions responsible for converting between different object types. These functions facilitate data transformations and type conversions, enhancing the flexibility of our codebase.

#### 5.2.2 Benefits

- **Data transformation:** ConvertExtensions simplifies the process of converting data from one format to another, improving data compatibility.

- **Type safety:** By encapsulating type conversion logic, we reduce the risk of type-related errors.

- **Code modularity:** The ConvertExtensions class promotes modular coding practices by isolating conversion functionality.

### 5.3 GuardExtensions

#### 5.3.1 Purpose

The GuardExtensions class hosts functions that raise exceptions when specific checks fail. These functions serve as a defensive coding mechanism to detect and handle exceptional scenarios early in the code execution flow.

#### 5.3.2 Benefits

- **Early error detection:** GuardExtensions helps identify and address issues at the earliest stage of execution, improving the robustness of our code.

- **Exception handling:** By centralizing exception-raising logic, we maintain a consistent approach to handling exceptional situations.

- **Code maintainability:** Isolating error-checking code in GuardExtensions improves code maintainability and readability.


## 6. Conclusion

The introduction of the WaitRetryPolicyOnSqlDeadlock class and the extension classes (CheckExtensions, ConvertExtensions, and GuardExtensions) represents a significant enhancement to our design and development framework. These additions aim to improve the reliability, maintainability, and flexibility of our software application, ensuring that it continues to meet the evolving needs of our users and stakeholders.

By embracing these changes, we are poised to create a more robust and responsive software solution, capable of handling the challenges and demands of modern software development.


## 7. Unit Testing with xUnit, Moq, and FluentAssertions

To ensure the robustness and reliability of our software, we embrace a comprehensive unit testing approach. In this section, we will outline our unit testing strategy and the tools and libraries we use.

### 7.1 Testing Frameworks

We employ the following tools and frameworks for unit testing:

- **xUnit:** xUnit is our chosen unit testing framework. It provides a simple and extensible architecture for writing and executing tests. We utilize xUnit to define and run our unit tests. 

- **Moq:** Moq is a mocking framework that aids in creating mock objects and setting up behavior for dependencies. It allows us to isolate the code being tested by replacing real dependencies with mock implementations.

- **FluentAssertions:** FluentAssertions is a library that enhances the readability and expressiveness of assertions in our unit tests. It provides a fluent and natural syntax for writing assertions, making our tests more understandable and maintainable.

### 7.2 Unit Testing Goals

Our primary goal in unit testing is to ensure that individual components and classes of our application work as intended in isolation. This includes testing various scenarios, inputs, and edge cases to validate the correctness of our code.

### 7.3 Code Coverage

We strive for achieving a code coverage of 100% in our unit tests. Code coverage measures the percentage of our codebase that is exercised by our tests. A 100% code coverage means that every line of code and every branch is tested at least once. This rigorous approach helps us identify untested or under-tested code, reducing the risk of undiscovered issues in our application.

### 7.4 Test Structure

Our unit tests are organized into the following categories:

- **Configuration Tests:** These tests ensure that the configuration management functions provided by ConfigurationManager are working correctly. We use Moq to create mock configurations for testing different scenarios.

- **Database Access Tests:** The IDbClient interface and its implementations in SqlClient are thoroughly tested to guarantee that database interactions are handled correctly. We use Moq to isolate the database access code.

- **Extension Method Tests:** CheckExtensions, ConvertExtensions, and GuardExtensions classes are subjected to extensive testing to validate the correctness of their methods.

- **Logging Tests:** We test the logging functionality, including CompanyLogger and SqlLogger, to verify that logs are generated as expected.

- **Retry Policy Tests:** The WaitRetryPolicyOnSqlDeadlock class is tested to ensure that it behaves correctly in various SQL deadlock scenarios. We utilize xUnit, Moq, and FluentAssertions to create and execute these tests.

### 7.5 Benefits of Unit Testing

Unit testing offers numerous advantages, including:

- Early issue detection: Problems are identified during development, reducing the cost and effort required to fix them.

- Improved code quality: Testing encourages clean, modular, and maintainable code.

- Confidence in changes: Tests provide a safety net when making modifications, preventing regressions.

- Documentation: Tests serve as living documentation, demonstrating how code should be used and what it should do.

By adhering to rigorous unit testing practices, we ensure that our codebase remains reliable and that new features and modifications do not introduce unintended consequences.

## 8. Conclusion

Incorporating unit testing with xUnit, Moq, and FluentAssertions into our development process is essential for maintaining the quality and reliability of our software. With a focus on achieving 100% code coverage, we minimize the risk of defects and regressions, allowing us to deliver a robust and stable application to our users.

For specific test cases and examples, please refer to the corresponding unit test projects within our solution.


## Organization of the Solution and Test Projects

**Organizing the Solution and Test Projects:**

1. **Remove the existing "Tests" folder:**
   - Open your project solution within your integrated development environment (IDE).

   - In the Solution Explorer, locate and delete the "Tests" folder or any other folder where unit tests are currently stored. You can perform this action by selecting and deleting the folder from the IDE's user interface.

2. **Create separate unit test projects:**
   - For each component requiring validation, create new individual unit test projects. Utilize a naming convention such as "[ProjectName].Test" for these test projects.

3. **Physically organize test projects in the "Test" folder:**
   - Create a new folder named "Test" in the solution directory.

   - Relocate the newly created unit test projects (e.g., "Helper.Test") into the "Test" folder within the solution directory.

4. **Adjust references and configurations:**
   - Update project references as needed to ensure that the primary projects (e.g., "Helper") correctly reference the corresponding unit test projects (e.g., "Helper.Test").

By following these steps, the developer can effectively organize the solution and test projects as required, as detailed in the technical document's "Organization of the Solution and Test Projects" section.