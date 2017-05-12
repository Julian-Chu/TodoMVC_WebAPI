Feature: CRUD API
	test TodoItemsController for http status code and response

@Get
Scenario: Retrieving an existing TodoItem
	Given a existing TodoItem with Id 1
	When it is retrieved
	Then Http status 200 should be returned
	Then Response context contains "test description 1"

Scenario: Retrieving TodoItem that does not exist
	Given a non-existing TodoItem with Id 100
	When it is retrieved
	Then Http status 404 should be returned

Scenario: Retrieving all existing TodoItems
	Given existing TodoItems
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

@Delete
Scenario: Deleting a existing TodoItem
	Given a existing TodoItem with Id 1
	When a Delete request is made
	Then Http status 200 should be returned
	Then the TodoItem should be removed

Scenario: Deleting a non-existing TodoItem
	Given a non-existing TodoItem with Id 100
	When a Delete request is made
	Then Http status 404 should be returned


@Put
Scenario: Updating a existing TodoItem
	Given a existing TodoItem with Id 1
	When a Put request is made
	Then Http status 204 should be returned

Scenario: Updating a non-existing TodoItem
	Given a non-existing TodoItem with Id 100
	When a Put request is made
	Then Http status 404 should be returned




