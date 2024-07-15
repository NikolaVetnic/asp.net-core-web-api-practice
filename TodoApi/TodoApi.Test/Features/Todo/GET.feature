@Integration
Feature: GET TodoItem

Scenario: Retrieving all todo items
	Given the todo list is not empty
	When a GET request is sent to the /todo endpoint
	Then the response status code is 200 (OK)
	And the response contains a list of todo items