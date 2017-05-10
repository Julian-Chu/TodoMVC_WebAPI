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

Scenario: Retrieving all existing TodoItems
	Given existing issues
	When all items are retrieved
	Then Http status 200 should be returned
	Then all items are returned


@Post
Scenario: Create a new TodoItem
	Given a new TodoItem with description "test description Post"
	When a Post request is made
	Then Http status 201 should be returned
	Then Response context contains "test description Post"
	Then The response location header will be set to the resource location
	
Scenario: Create a new TodoItem with invalidated model
	Given a new TodoItem with description "test description Post tttttttttttttttttttttttttttttttttttttttttt"
	When a Post request is made
	Then Http status 400 should be returned
