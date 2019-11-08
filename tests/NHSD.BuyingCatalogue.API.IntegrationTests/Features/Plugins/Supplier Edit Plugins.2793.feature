Feature:  Supplier Edit Plugins
    As a Supplier
    I want to Edit the Plugins Section
    So that I can ensure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
        | Drs. Inc |
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | OrganisationName | FullDescription     | SupplierStatusId |
        | Sln1       | MedicOnline    | An full online medicine system | GPs-R-Us         | Online medicine 1   | 1                |

@2786
Scenario: 1. Plugins is updated
    Given MarketingDetail exist
        | Solution | ClientApplication                                                                                                                                                                             |
        | Sln1     | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins" : {"Required" : false, "AdditionalInformation": "orem ipsum" } } |
    When a PUT request is to update solution Sln1 plug-ins section
        | Required | AdditionalInformation     |
        | yes      | This is extra information |
    Then a successful response is returned
    And MarketingDetail exist
        | Solution | ClientApplication                                                                                                                                                                                            |
        | Sln1     | { "ClientApplicationTypes": ["browser-based"],"BrowsersSupported" : [ "IE8", "Opera" ], "MobileResponsive": false, "Plugins" : { "Required" : true , "AdditionalInformation": "This is extra information"} } |

                                                                                                                                                                             
@2786
Scenario: 2. Solution is not found
    Given a Solution Sln4 does not exist
    When a PUT request is to update solution Sln4 plug-ins section
        | Required | AdditionalInformation     |
        | no       | This is extra information |
    Then a response status of 404 is returned 

@2786
Scenario: 3. Service Failure
    Given the call to the database to set the field will fail
    When a PUT request is to update solution Sln1 plug-ins section
        | Required | AdditionalInformation     |
        | no       | This is extra information |
    Then a response status of 500 is returned

@2786
Scenario: 4. Solution id is not present in the request
    When a PUT request is made to update plug-ins section with no solution id
         | Required | AdditionalInformation     |
         | no       | This is extra information |
    Then a response status of 400 is returned