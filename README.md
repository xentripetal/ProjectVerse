# Project Verse

### Project Verse: A fully moddable Farming and Trading game

[About](#about)  

[Copyright & Licensing](#copyright--licensing)

[Development](#Development)

[Contributing](#contributing)  

## About

Project Verse is being designed as a fully moddable Unity game in
reference to Stardew Valley. All objects, events, worlds, cutscenes, and
scripted actions will be implemented using the API. 

The game itself will be taking place in the far future with heavy influences from the T.V. series Firefly.
The player will crash into a small town, making friends with local villagers and while working on earning money. Eventually they will gain enough wealth to fix their ship
and explore other worlds with different themes and quests all while promoting their budding enterprise of cargo
trading, production, and crime.


### Planned Features
* Crop Growing - plantable crops that grow over a given lifecycle and produce a sellable item
* Home Building - Add objects into home, decorative and functional
* Modular Ship Design - Build/Purchase upgrades to ship that reflect in its physical design
* Quests - Players will given quests to complete.
* Relationships - Players will be able to build friendships and and enemies with the NPCs.
* Morality - Players will be given choices over time that will change a players morality score and affect the lives of NPCs.
* Management - Players will be able to hire certain NPCS to do tasks for them, from handling crops, chopping trees, running cargo, or robbing ships.
* Automation - Players will slowly be able to add automation to their enterprise. Taking inspiration from Factorio with transportation and processing. With this, I want controllers to manipulate these machines to be fully programmable, presumably via LUA or possibly a Zachtronics inspired psuedo-assembly. However I'm uncertain how feasible this will be.
* Trading - There will be an influenceable economy that will affect what and how the player trades items and moves cargo.
* A lot more - I am planning on updating this readme over time. 


## Development

### Currently Implemented Features
* Movable Player
* Basic objects that can be scripted and have their data set via json.
* Storing and loading rooms from json files to allow editing/modability.
* Loading different rooms


### Design Goals
* Reduce development dependency on Unity as much as possible. While Unity is a great dev environment it isn't designed to allow mods/patching. Unity Editor should be used for as few systems as possible by making a runtime level/scene editor that supports patching and having a custom lifecycle system for registering classes. Unity Engine will still be heavily used and most things will still use Monobehaviors.
* Pick a specific code style and reformat all code
* Add unit tests and a build tracker


### Stages

Currently, the game is in stage INDEV. The expected stages are listed below.
Note that when I first started this project I had more free time than I do now. I abandonded this project for a year and am now picking it back up in my spare time.

Stage    | Description 
-------- | -------------
INDEV    | No contributions allowed, everything is variable to change. Goal is to have all basic mechanics plausible using the current API.
ALPHA    | No contributions allowed, some marketing will be done. basic story will be planned/implemented and custom art will become a priority. Advanced functionality will be added.
BETA     | Contributions allowed, game will be heavily marketed and sold. Forums/Website will be setup. API will be under heavy scrutiny and API feature requests will be heavily promoted. All art must be custom. Maybe networking/online?
OFFICIAL | API will be finalized, only bug fixes will be done to the API. More story will be added. Steam release with workshop support.

The development progress is currently tracked through hacknplan, however if this game picks up any popularity I will move
the progress tracker to a publicly accessible website. If you're reading this and want to see the progress, then it has
clearly reached enough popularity to merit it. So just put up an issue tracker in reference to this and ill go ahead and do it.


## Copyright & Licensing

The license is currently undecided. I intend to have some restrictions that would prevent this from being a truly FOSS game. 
However, until such a date, feel free to use whatever is here without restriction. Once a license is added, any changes made
to the resources above from that point on will no longer be without restriction but under the new license.

Note that most of the art assets are from [Hyptosis](https://opengameart.org/content/lots-of-free-2d-tiles-and-sprites-by-hyptosis) at OpenGameArt.org and thereby fall under CC BY 3.0. 


Additionally JSON.NET library is licensed under Apache License Version 2.0.

## Installing
Project requires git-lfs for the pre-existing images. If nothing is showing when running the demo scenes make sure to run 'git lfs pull'.


## Contributing
This project is currently not accepting contributions. If you would like a feature or notice a bug (which there will be a lot of for the next few months) please create an issue for it.



