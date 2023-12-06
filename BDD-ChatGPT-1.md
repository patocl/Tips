# Prompt 1:

Act as a quality analyst who is highly experienced in behavioural driven development and developing well-constructed Gherkin Scenarios from supplied requirements. When I supply a requirement, I want you to create full coverage in the following way: 

1. Use Gherkin BDD language and output as one entire code snippet for easy copying. 
2. Provide positive and negative scenarios.
3. Ensure all common steps you create are added as a Gherkin 'Background' 
4. Ensure 'Background' Is provided only once and Is placed after the user story and before the scenarios. 
5. Ensure all variables used are created as a Gherkin 'Scenario Outline' 
6. Ensure variables are added to a Gherkin 'Examples' table appropriately. 
7. Include feature-level tags and scenario-level tags e.g., @valid, @invalid, @feature-example, @smoke-test, @regresslon-test 
8. Provide feature and user story 
9. Afterwards, suggest an appropriate name for the *.feature file and explain your work. 
10. Do not assume any output like error messages or variables not part of the requirements. 
11. The background should be on the top

Before you answer, I want you to do the following: If you have any questions about my task or uncertainty about delivering the best expert scenarios possible, always ask bullet point questions for clarification before generating your answer. Is that understood and are you ready for the requirements? 

# Prompt 2:

Here is a list of requirements that may be used for building a "Query eligible assets (daily data)" page: 

1. Basic Search by ISIN code

2. Provide Advanced Search by the following criteria:

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

3. Before click seach you can select the result set be Sort by
  - "ISIN code"
  - "Haircut category"
  - "Asset type"
  - "Reference market"
  - "Denomination"
  - "Issuer CSD"
  - "Issuer name"
  - "Guarantor name"
  - "Coupon definition"

4. When the search button is clicked, it should perform a search and return results

5. When clear filters are clicked should clean filters and results

6. If no results are found, it should be shown a "There are no EA records which meet your search criteria. Please refine your query."

7. If the search found records they should be presented on a table with the following header
- ISIN code	
- Asset details	
- Haircut category
- Reference market
- Issuer

8. Create each individual advanced search criterion, providing comprehensive test coverage for all possible critearia valid and invalid. Valid should return results and invalid shoult return not results.

9. And Due please include another scenario to cover all possible combinations of the advanced search criteria, provide a full set of examples per each criteria.


# Prompt 3:
use the answer to Create Page Object Model 'queryEligibleAssets' in typescript for the following feature:
* Use 'page' from '@playwright/queryEligibleAssets' as constructor parameter.
* Use Given, When, Then from 'playwright-bdd/decorators' as BDD decorators, for example, @Given('pattern {string}').
* Replace all verify methods with a single method that verifies the list of visible items
* make all methods async — sometimes ChatGPT generates synchronous methods
* use {string} for string pattern parameters — to stick to Cucumber Expression syntax for parameters
* create todo items inside scenario "xxx" — to fix a scenario that uses data from another scenario, tests should be isolated
* Don't start method names with given/when/then — for better method names
* "generate all required steps in typscript."

--------

Gerkhin Generated:

```gerkhin
@feature-example
Feature: Query Eligible Assets (Daily Data)

  As a user
  I want to query eligible assets based on various criteria
  So that I can find the relevant records efficiently

  Background:
    Given the system has eligible assets data

  @valid @basic-search
  Scenario Outline: Basic Search by ISIN code
    When I search with ISIN code "<isin_code>"
    Then the system should display relevant results

    Examples:
      | isin_code      |
      | ABC123          |
      | XYZ456          |

  @valid @advanced-search
  Scenario Outline: Advanced Search by Asset Type
    When I perform an advanced search with Asset Type "<asset_type>"
    Then the system should display relevant results

    Examples:
      | asset_type  |
      | Equity      |
      | Fixed Income |

  @valid @advanced-search
  Scenario Outline: Advanced Search by Reference Market
    When I perform an advanced search with Reference Market "<ref_market>"
    Then the system should display relevant results

    Examples:
      | ref_market     |
      | Stock Exchange |
      | Bond Market    |

  @valid @advanced-search
  Scenario Outline: Advanced Search by Denomination
    When I perform an advanced search with Denomination "<denomination>"
    Then the system should display relevant results

    Examples:
      | denomination |
      | USD           |
      | EUR           |

  @valid @advanced-search
  Scenario Outline: Advanced Search by Issuance Date Range
    When I perform an advanced search with Issuance Date Range from "<issuance_from>" to "<issuance_to>"
    Then the system should display relevant results

    Examples:
      | issuance_from | issuance_to   |
      | 2023-01-01    | 2023-12-31    |
      | 2022-05-01    | 2022-12-31    |

  @valid @advanced-search
  Scenario Outline: Advanced Search by Maturity Date Range
    When I perform an advanced search with Maturity Date Range from "<maturity_from>" to "<maturity_to>"
    Then the system should display relevant results

    Examples:
      | maturity_from | maturity_to   |
      | 2025-01-01    | 2025-12-31    |
      | 2024-01-01    | 2024-06-30    |

  @valid @advanced-search
  Scenario Outline: Advanced Search by Issuer CSD
    When I perform an advanced search with Issuer CSD "<issuer_csd>"
    Then the system should display relevant results

    Examples:
      | issuer_csd |
      | CSD1       |
      | CSD2       |

  @valid @advanced-search
  Scenario Outline: Advanced Search by Issuer Name
    When I perform an advanced search with Issuer Name "<issuer_name>"
    Then the system should display relevant results

    Examples:
      | issuer_name |
      | ABC Corp     |
      | DEF Corp     |

  @valid @advanced-search
  Scenario Outline: Advanced Search by Issuer Residence
    When I perform an advanced search with Issuer Residence "<issuer_residence>"
    Then the system should display relevant results

    Examples:
      | issuer_residence |
      | USA               |
      | Germany           |

  @valid @advanced-search
  Scenario Outline: Advanced Search by Issuer Group
    When I perform an advanced search with Issuer Group "<issuer_group>"
    Then the system should display relevant results

    Examples:
      | issuer_group |
      | Group1       |
      | Group2       |

  @valid @advanced-search
  Scenario Outline: Advanced Search by Guarantor Name
    When I perform an advanced search with Guarantor Name "<guarantor_name>"
    Then the system should display relevant results

    Examples:
      | guarantor_name |
      | XYZ Corp        |
      | null            |

  @valid @advanced-search
  Scenario Outline: Advanced Search by Guarantor Residence
    When I perform an advanced search with Guarantor Residence "<guarantor_residence>"
    Then the system should display relevant results

    Examples:
      | guarantor_residence |
      | Europe               |
      | null                 |

  @valid @advanced-search
  Scenario Outline: Advanced Search by Guarantor Group
    When I perform an advanced search with Guarantor Group "<guarantor_group>"
    Then the system should display relevant results

    Examples:
      | guarantor_group |
      | Group2          |
      | null            |

  @valid @advanced-search
  Scenario Outline: Advanced Search by Coupon Definition
    When I perform an advanced search with Coupon Definition "<coupon_definition>"
    Then the system should display relevant results

    Examples:
      | coupon_definition |
      | Fixed             |
      | Floating          |

  @valid @advanced-search
  Scenario Outline: Advanced Search by Non-Own Use Haircut
    When I perform an advanced search with Non-Own Use Haircut "<non_own_haircut>"
    Then the system should display relevant results

    Examples:
      | non_own_haircut |
      | 15.5            |
      | null            |

  @valid @advanced-search
  Scenario Outline: Advanced Search by Own Use Haircut
    When I perform an advanced search with Own Use Haircut "<own_haircut>"
    Then the system should display relevant results

    Examples:
      | own_haircut |
      | 10.0        |
      | null        |

  @valid @sort-results
  Scenario Outline: Sort Search Results
    Given the system displays search results
    When I sort the results by "<sort_option>"
    Then the results should be sorted accordingly

    Examples:
      | sort_option          |
      | ISIN code             |
      | Haircut category      |
      | Asset type            |
      | Reference market      |
      | Denomination          |
      | Issuer CSD            |
      | Issuer name           |
      | Guarantor name        |
      | Coupon definition     |

  @valid @clear-filters
  Scenario: Clear Filters and Results
    Given the system displays search results with applied filters
    When I click on "Clear Filters"
    Then the filters and results should be cleared

  @invalid @no-results
  Scenario: No Results Found
    Given the system displays no search results
    Then a message should be shown stating "There are no EA records which meet your search criteria. Please refine your query."
```
```
File Name Suggestion:
query_eligible_assets.feature

Explanation:
This feature file covers various scenarios for both basic and advanced searches, including positive and negative cases. Each scenario focuses on a specific search criterion, ensuring comprehensive test coverage for all possible combinations. The file is named query_eligible_assets.feature for clarity and consistency.
```





@feature-example
Feature: Query eligible assets (daily data)

  As a user
  I want to query eligible assets
  So that I can view the details of the assets

  Background:
    Given I am on the "Query eligible assets" page
    And I have entered "<ISIN_code>" in the "Basic Search" field

  @valid
  Scenario Outline: Example positive scenario
    When I enter "<asset_type>" in the "Asset type" field
    And I enter "<reference_market>" in the "Reference market" field
    And I enter "<denomination>" in the "Denomination" field
    And I enter "<issuance_date_from>" in the "Issuance date from" field
    And I enter "<issuance_date_to>" in the "Issuance date to" field
    And I enter "<maturity_date_from>" in the "Maturity date from" field
    And I enter "<maturity_date_to>" in the "Maturity date to" field
    And I enter "<issuer_csd>" in the "Issuer CSD" field
    And I enter "<issuer_name>" in the "Issuer name" field
    And I enter "<issuer_residence>" in the "Issuer residence" field
    And I enter "<issuer_group>" in the "Issuer group" field
    And I enter "<guarantor_name>" in the "Guarantor name" field
    And I enter "<guarantor_residence>" in the "Guarantor residence" field
    And I enter "<guarantor_group>" in the "Guarantor group" field
    And I enter "<coupon_definition>" in the "Coupon definition" field
    And I enter "<non_own_use_haircut>" in the "Non own use haircut" field
    And I enter "<own_use_haircut>" in the "Own use haircut" field
    And I select "<sort_by>" from the "Sort by" dropdown
    And I click the "Search" button
    Then I should see the following table:
      | ISIN code | Asset details | Haircut category | Reference market | Issuer |
      | value1    | value2        | value3           | value4           | value5 |
      | value6    | value7        | value8           | value9           | value10|

    Examples:
      | ISIN_code | asset_type | reference_market | denomination | issuance_date_from | issuance_date_to | maturity_date_from | maturity_date_to | issuer_csd | issuer_name | issuer_residence | issuer_group | guarantor_name | guarantor_residence | guarantor_group | coupon_definition | non_own_use_haircut | own_use_haircut | sort_by |
      | value11   | value12    | value13          | value14      | value15           | value16         | value17           | value18         | value19    | value20    | value21         | value22     | value23        | value24             | value25         | value26          | value27            | value28         | value29 |
      
  @invalid
  Scenario Outline: Example negative scenario
    When I enter "<asset_type>" in the "Asset type" field
    And I enter "<reference_market>" in the "Reference market" field
    And I enter "<denomination>" in the "Denomination" field
    And I enter "<issuance_date_from>" in the "Issuance date from" field
    And I enter "<issuance_date_to>" in the "Issuance date to" field
    And I enter "<maturity_date_from>" in the "Maturity date from" field
    And I enter "<maturity_date_to>" in the "Maturity date to" field
    And I enter "<issuer_csd>" in the "Issuer CSD" field
    And I enter "<issuer_name>" in the "Issuer name" field
    And I enter "<issuer_residence>" in the "Issuer residence" field
    And I enter "<issuer_group>" in the "Issuer group" field
    And I enter "<guarantor_name>" in the "Guarantor name" field
    And I enter "<guarantor_residence>" in the "Guarantor residence" field
    And I enter "<guarantor_group>" in the "Guarantor group" field
    And I enter "<coupon_definition>" in the "Coupon definition" field
    And I enter "<non_own_use_haircut>" in the "Non own use haircut" field
    And I enter "<own_use_haircut>" in the "Own use haircut" field
    And I select "<sort_by>" from the "Sort by" dropdown