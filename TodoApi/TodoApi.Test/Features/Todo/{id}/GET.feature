@Integration
Feature: GET TodoItem

Scenario: Retrieving a todo item
	Given a todo item t1
	When a GET request is sent to the /todo/{id} endpoint with t1.Id
	Then the response status code is 200 (OK)
	And the response contains todo item t1

Scenario: Requesting a nonexistent TodoItem
	When a GET request is sent to the /todo/{id} endpoint with a nonexistent id
	Then the response status code is 404 (Not Found)