# Pratice

Originally created for praticing sagas, it was decided to expand the area of pratice ..., while also turning the project into one that could be as a display of skills.


## Important

The folder structure differer between the People domain and the Vehicle domain.
The people domain is structured by type, where Vehicle is grouped by process and aggregate.

Regarding the command names the ...From{Value} is used to help to distint who/what the command is sent from e.g. Value could User, a System, or by an Event.


## Some Comments

All models are using int for their ids, which is fine for most models. The Person and operator models should be using GUIDs. 
The reason for this is to make it harder to guess the id of them and trying to access their information.

Regarding the trilemma of domain model purity, application performance, and domain model completeness, it was decided to focus on domain model purity by placing all external reads and writes outside the domain models.
Do note that the id generation code could be considered to break the domain model purity, but it is only there because of the mock storage and not wanting to write code, yet, that used reflection to find the id fields and fill them out when 'saving' a created entity to the context.

The mock base repository and mock contextes were quickly programmed as they were not the main focus on this project and they are fairly rough.

Regarding comments in the code, there are two types; permanents and temporals. 

A permanent follows this pattern // Sentence.

A temporal follows this pattern //sentence(.|?)

The temporal comments are just ideas and thoughts and are only present, because I work alone on this project. If working with others such comments would be placed in the task/backlogs on the devops board until they could be resolved.
The permanent comments are there to help explain why code is done in the way it is done, what it does or why it is present. 





