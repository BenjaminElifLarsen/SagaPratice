# Pratice

Originally created for praticing sagas, it was decided to expand the area of pratice to cover event storage and event sourcing, command handlers, command and event busses, and more, while also turning the project into one that could be as a display of skills.


## Important

The folder structure differer between the People domain and the Vehicle domain.
The people domain is structured by type, where Vehicle is grouped by process and aggregate.

Regarding the command names the ...From{Value} is used to help to distint who/what the command is sent from e.g. Value could User, a System, or by an Event (This naming policy will change).

Most of the things praticed in this project are the first time experiencing and should thus be considered to be rough versions of what will come later.

## Refactoring Notes

As I learn more about the different learning subjects I start refactoring code. Most of the time the refactoring will happen in the People domain first as this domain is, for the most part, more simple than the Vehicle domain.

## Git

This project contains a GIT submodule (the module 'Common'). It is very important to ensure the correct version of Common is used as Common is designed for use in multiple projects. 
If pulling this project down it should pull the correct version of Common.

Not that it has an impact on the software, but the Git CLI was used.

## Notes

Regarding the trilemma of domain model purity, application performance, and domain model completeness, it was decided to focus on domain model purity by placing all external reads and writes outside the domain models.

Given the use of GUIDs for ids each domain model calculates their own Id. This is, from reading, considered a common pratice for DDD and events.

The mock base repository and mock contextes were quickly programmed as they were not the main focus on this project and they are fairly rough. This means e.g. that if a model operation fails that the model is not reversed to the state it was in before the operation started.

Regarding expection handling, expection throwing, and such, these are not really been implemented yet as the focus is on learning new competencies.

### Repository Design

It is important to notice that all repositories in the different domains are used as abstractions over the repository in the BaseRepository project. 
The mock repositories in that project are fully generic (as is their contracts) and designed to fulfill any requirements that might be needed by the abstraction layer repositories.
This also means that if changing data context, like going from inline-memory to a SQL-database, all that need to be changed are the concreate implementations and maybe the builder.Services over in the API project. 


### Security

Currently the software contains no security on the endpoints, but it is planned to add security policy on the endspoints that will require the request to contain a valid JWT. 

### Comment Design

Regarding comments in the code, there are two types; permanents and temporals. 

#### Permanent

A permanent follows this pattern // Sentence.

The permanent comments are there to help explain why code is done in the way it is done, what it does or why it is present. 

#### Temporal

A temporal follows this pattern //sentence( |.|?)

The temporal comments are just ideas and thoughts and are only present because I work alone on this project. If working with others such comments would be placed in the task/backlogs on the devops board until they could be resolved.


### Enum Naming Pattern

The names of enum types either ends in singular or pural,e.g. Status/Statuses.
The reason behind this is to indicate by name alone (and by C Sharp standard) whether an enum is a binary flag or not.
Binary flag enum names are plural, while non binary flag enum names are singular.


