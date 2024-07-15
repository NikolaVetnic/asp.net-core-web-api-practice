@Integration
Feature: PUT TodoItem

Scenario: Updating an existing todo item
	Given a todo item t1
	When a PUT request is sent to the /todo/{id} endpoint with t1.Id and the following data
		| Name         | IsCompleted |
		| Updated Todo | true        |
	Then the response status code is 204 (No Content)
	And the response body is empty
	When a GET request is sent to the /todo/{id} endpoint with t1.Id
	Then the response status code is 200 (OK)
	And the response contains the updated todo item t1 with the following properties
		| Name         | IsCompleted |
		| Updated Todo | true        |