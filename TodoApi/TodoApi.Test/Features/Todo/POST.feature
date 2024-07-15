@Integration
Feature: POST TodoItem

Scenario: POST TodoItem
	Given a TodoItem with the following properties
		| Name         | IsCompleted |
		| My ToDo Item | false       |
	When a POST request is sent to the Todo endpoint
	Then the response status code is 201 (Created)
	And the response body is a TodoItem with the following properties
		| Name         | IsCompleted |
		| My ToDo Item | false       |