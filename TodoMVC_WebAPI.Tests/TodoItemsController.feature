Feature: Retrieving TodoItems
    test TodoItemsController for http status code and response

@Get
Scenario: Retrieving an existing TodoItem
	Given id equals 1 for existing description
	When it is retrieved
	Then Http status 200 should be returned
	Then Response context contains "test description 1"

Scenario: Retrieving TodoItem that does not exist
	Given id equals 100 for non-existing description
	When it is retrieved
	Then Http status 404 should be returned

	
