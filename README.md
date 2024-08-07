# This repository is ...
dedicated to one of my projects. This project (further referenced as application) will be refactored using every best practice known to mankind.

# Refactoring plan for the server
- refactor domain to include domain methods
- refactor API controllers to use only mediator LOGIC
- add entity configuration for persistence layer EF core
- refactor mediator commands and queries to use service interfaces properly
- simplify read dto as much as possible, it carries too much info
- refactor application layer, separate it into read and write, where read would be dapper with read models (views in the db), and write as EF CORE with change tracker and strict transactional bound within one aggregate root.
- implement the application read and write layers for all the interface contacts
- refactor auth logic (add oauth, etc.)
- refactor API level to use all the best practices regarding REST, etc.
- refactor persistence level to use ef core for write and dapper for read logic
- add view sql generation into the migrations feature of ef core
- add global exception handling
- add logging for the system
- .... todo: other points
- add auth checks for user roles, etc, such as user should be able to delete only its own comment, story, etc. only admin can delete all ,etc.
- add docker to have it all in one place
- add optimistic concurrency
- add audit (created at, updated at, etc) for Entity<>
- populate this readme with how to run information
- add unit tests
- add integration tests
- add specification pattern to the repositories
- add domain events, etc.
- fix TODOs

### In the future
Create a separate module/system/microservice that will be using an ML/AI to generate stories, not only that, but also
AI will simulate user activity in the entire system, such that we can improve upon logging, metrics, etc.

# Refactoring plan for the client
- separate into different components
- add store https://ngrx.io/guide/store
