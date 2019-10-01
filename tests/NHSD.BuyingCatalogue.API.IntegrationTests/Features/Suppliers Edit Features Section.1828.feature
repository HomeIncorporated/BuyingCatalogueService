Feature: Suppliers Edit Features Section
    As a Supplier
    I want to Edit the Features Section
    So that I can make sure the information is correct

Background:
    Given Organisations exist
        | Name     |
        | GPs-R-Us |
        | Drs. Inc |
    And Solutions exist
        | SolutionID | SolutionName   | SummaryDescription             | OrganisationName | FullDescription     |
        | Sln1       | MedicOnline    | An full online medicine system | GPs-R-Us         | Online medicine 1   |
        | Sln2       | TakeTheRedPill | Eye opening experience         | Drs. Inc         | Eye opening6        |
        | Sln3       | PracticeMgr    | Fully fledged GP system        | Drs. Inc         | Fully fledged GP 12 |
    And MarketingDetail exist
        | Solution | AboutUrl | Features                                            |
        | Sln1     | UrlSln1  | { "customJson" : { "id" : 1, "name" : "feature1" }} |
        | Sln2     | UrlSln2  | { "customJson" : { "id" : 2, "name" : "feature2" }} |
        | Sln3     | UrlSln3  | { "customJson" : { "id" : 3, "name" : "feature3" }} |

@1828
Scenario: 1. Marketing Data is updated against the solution
    Given a POST request is made to update solution Sln1 features
        | Summary                | Description            | AboutUrl   | MarketingData                                                   |
        | New type of medicine 3 | A new full description | UrlSln1New | { "differentJson" : { "customId" : 6, "customName" : "thing" }} |
    Then Solutions exist
        | SolutionID | SolutionName   | SummaryDescription      | FullDescription        |
        | Sln1       | MedicOnline    | New type of medicine 3  | A new full description |
        | Sln2       | TakeTheRedPill | Eye opening experience  | Eye opening6           |
        | Sln3       | PracticeMgr    | Fully fledged GP system | Fully fledged GP 12    |
    And MarketingDetail exist
        | Solution | AboutUrl   | Features                                                        |
        | Sln1     | UrlSln1New | { "differentJson" : { "customId" : 6, "customName" : "thing" }} |
        | Sln2     | UrlSln2    | { "customJson" : { "id" : 2, "name" : "feature2" }}             |
        | Sln3     | UrlSln3    | { "customJson" : { "id" : 3, "name" : "feature3" }}             |

