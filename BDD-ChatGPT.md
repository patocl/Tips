# Prompt 1:

Act as a quality analyst who is highly experienced in behavioural driven development and developing well-constructed Gherkin Scenarios from supplied requirements. When I supply a requirement, I want you to create full coverage in the following way: 

1. Use Gherkin BDD language and output as one entire code snippet for easy copying. 
2. Provide positive and negative scenarios.
3. Ensure all common steps you create are added as a Gherkin 'Background' 
4. Ensure 'Background' Is provided only once and Is placed after the user story and before the scenarios. 
5. Ensure all variables used are created as a Gherkin 'Scenario Outline' 
6. Ensure variables added to a Gherkin 'Examples' table appropriately. 
7. Include feature level tags and scenario level tags e.g., @valid, @invalid, @feature-example, @smoke-test, @regresslon-test 
8. Provide feature and user story 
9. Afterwards, suggest an appropriate name for the *.feature file and explain your working. 
10. Do not assume any output like error messages or variables not part of the requirements. 
11. The background should be on the top

Before you answer, I want you to do the following: If you have any questions about my task or uncertainty about delivering the best expert scenarios possible, always ask bullet point questions for clarification before generating your answer. Is that understood and are you ready for the requirements? 

# Prompt 2:

Here is a list of requirements that may be used for building a "Query eligible assets (daily data)" page: 

1. Basic Search by ISIN code
2. Provide Advanced Search by the following criterias:
- Asset type
- Reference market
- Denomination
- Issuance date
  - from
  - to
- Maturity date
  - from
  - to
- Issuer CSD
- Issuer name
- Issuer residence
- Issuer group
- Guarantor name
- Guarantor residence
- Guarantor group
- Coupon definition
- Non own use haircut with Format: 99.9
- Own use haircut with Format: 99.9
 
- Sort by
  - "ISIN code"
  - "Haircut category"
  - "Asset type"
  - "Reference market"
  - "Denomination"
  - "Issuer CSD"
  - "Issuer name"
  - "Guarantor name"
  - "Coupon definition"
4. When search button is clicked should perform a seach and returns a results
5. When clear filters is click should clean filters and results
6. If no results found should be shown a "There are no EA records which meet your search criteria. Please refine your query."
7. If search found records it should be presented on a table with the following header
- ISIN code	
- Asset details	
- Haircut category
- Reference market
- Issuer

# Prompt 3:
use the answer to Create Page Object Model 'Login' in typescript for the following feature:
* Use 'page' from '@playwright/login' as constructor parameter.
* Use Given, When, Then from 'playwright-bdd/decorators' as BDD decorators, for example: @Given('pattern {string}').
* Don't fill methods body.
* Replace all verify methods with single method that verifies list of visible items

* make all methods async — sometimes ChatGPT generates synchronous methods
* use {string} for string pattern parameters — to stick to Cucumber Expression syntax for parameters
* create todo items inside scenario "xxx" — to fix scenario that uses data from another scenario, tests should be isolated
* don't start method names with given/when/then — for better method names
