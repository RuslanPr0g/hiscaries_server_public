# This repository is dedicated to one of my projects. This project (further referenced as application) will be refactored using every best practice known to mankind.
## This & FRONTEND https://github.com/RuslanPr0g/Hiscary-WebsiteClient apps would be refactored to align with the latest coding standards, such as DDD, clean architecture, CQS & CQRS, etc.

# Refactoring plan
- refactor domain to include domain methods
- refactor API controllers to use only mediator LOGIC
- refactor mediator commands and queries to use service interfaces properly
- refactor application layer, separate it into read and write, where read would be dapper with read models (views in the db), and write as EF CORE with change tracker and strict transactional bound within one aggregate root.
- implement the application read and write layers for all the interface contacts
- refactor auth logic (add oauth, etc.)
- refactor API level to use all the best practices regarding REST, etc.
- refactor persistence level to use ef core for write and dapper for read logic
- add view sql generation into the migrations feature of ef core
- .... todo: other points
- add docker to have it all in one place
- add optimistic concurrency
- populate this readme with how to run information
- add unit tests
- add integration tests