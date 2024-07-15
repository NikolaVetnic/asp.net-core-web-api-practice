@Integration
Feature: DELETE TodoItem

Scenario: Deleting an existing todo item
    Given a todo item t1
    When a DELETE request is sent to the /todo/{id} endpoint with t1.Id
    Then the response status code is 204 (No Content)
    And the response body is empty
    When a GET request is sent to the /todo/{id} endpoint with t1.Id
    Then the response status code is 404 (Not Found)

Scenario: Deleting a nonexistent todo item
    When a DELETE request is sent to the /todo/{id} endpoint with a nonexistent id
    Then the response status code is 404 (Not Found)