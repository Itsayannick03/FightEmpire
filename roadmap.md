# Fight Empire Roadmap

Fight Empire is an MMA promotion management simulator where the player builds a roster, books fights, runs events, manages finances, signs fighters, and competes against rival promotions.

This roadmap describes the planned development path toward the first playable alpha.

The roadmap is intentionally flexible. Features may move between milestones as the game develops.

---

## Current Development Goal

Build a round-based MMA simulation where fictional fighters are generated with different attributes, compete for control of each round, and can win by knockout, submission, or decision.

---

# Version 0.0.1 — Fighters and Fight Simulation

## Goal

Create the foundation of the MMA simulation.

The game should be able to generate fictional fighters and simulate understandable fights between them.

## Planned features

* Generate fictional fighter names
* Generate fighter nicknames
* Generate basic fighter attributes
* Display complete fighter information
* Select two fighters for a matchup
* Determine each fighter's preferred fighting style
* Simulate fights one round at a time
* Determine the active fighting style of each round
* Calculate style-based round performance
* Determine round winners
* Allow knockouts
* Allow submissions
* Determine decision winners from rounds won
* Update fighter records
* Run repeated simulations for testing and balancing

## Initial fighter attributes

* Striking
* Wrestling
* Grappling

Additional attributes may be added later when they serve a clear purpose.

## Basic fight flow

1. Each fighter chooses a preferred fighting style.
2. The fighters compete to determine the active style of the round.
3. The active style changes which attributes are most important.
4. Both fighters receive a round-performance score.
5. The fighter with the higher performance wins the round.
6. A large performance difference may cause a finish.
7. The fight continues until there is a finish or all rounds are completed.
8. If the fight reaches the end, the fighter who won the most rounds wins by decision.

## Completion target

Version `0.0.1` is complete when the program can:

* Generate a group of fighters
* Display their attributes
* Select two fighters
* Simulate a complete fight
* Display round-by-round information
* Produce a knockout, submission, or decision
* Update the fighters' records
* Repeat the process without rebuilding the fighters manually

## Not planned yet

* Weight classes
* Events
* Promotions
* Contracts
* Finances
* Rankings
* Championships
* Injuries
* Training
* Godot interface

---

# Version 0.0.2 — Bouts and Events

## Goal

Combine individual fights into complete MMA events.

## Planned features

* Create a `Bout` model
* Create an `Event` model
* Book multiple fights on one event
* Add card positions
* Prevent fighters from appearing more than once
* Prevent fighters from fighting themselves
* Simulate every bout on an event
* Store event results
* Display a complete event summary

## Initial card positions

* Main event
* Co-main event
* Main card
* Preliminary card

## Completion target

Version `0.0.2` is complete when the player can create a four-fight card, run the entire event, and view all fight results.

---

# Version 0.0.3 — First Godot Interface

## Goal

Create the first graphical interface while keeping the simulation inside the core C# project.

Godot should display and control the game. It should not contain duplicate fight-simulation logic.

## Planned screens

### Fighter browser

* Display generated fighters
* Select a fighter
* View fighter details

### Matchup screen

* Select Fighter A
* Select Fighter B
* Confirm the matchup
* Prevent invalid selections

### Fight screen

* Start the fight simulation
* Display round results
* Display the winner
* Display the victory method
* Display updated records

## Completion target

Version `0.0.3` is complete when a user can generate fighters, select a matchup, and simulate a fight entirely through Godot.

## Visual priorities

Focus on:

* Readability
* Clear navigation
* Functional buttons
* Understandable results

Do not focus on:

* Detailed art
* Animations
* Fighter models
* Custom visual effects
* Sound design

---

# Version 0.0.4 — Promotion and Roster

## Goal

Give the player control of an MMA promotion and a roster of fighters.

## Planned features

* Create a promotion model
* Create the player's company
* Add a company name
* Add a company roster
* Restrict event booking to roster fighters
* Add fighter popularity
* Add basic company popularity
* Store completed events
* Store upcoming events

## Promotion data

A promotion may initially contain:

* Name
* Money
* Popularity
* Roster
* Upcoming events
* Completed events

Some values may remain placeholders until later milestones.

## Completion target

Version `0.0.4` is complete when the player owns a promotion, has a roster, and can create events using fighters from that roster.

---

# Version 0.0.5 — Event Finances

## Goal

Make event booking create financial risk and reward.

## Planned features

* Add venues
* Add venue capacities
* Add venue rental costs
* Add ticket prices
* Calculate event interest
* Calculate attendance
* Calculate ticket revenue
* Add fighter pay
* Add basic event expenses
* Calculate event profit or loss
* Update company money
* Display an event financial report

## Initial venues

The first version may include:

* Small hall
* Medium arena
* Large arena

Each venue should have:

* Capacity
* Rental cost

## Initial attendance factors

Attendance may depend on:

* Promotion popularity
* Main-event fighter popularity
* Supporting-card popularity
* Ticket price
* Venue capacity
* Random variation

## Completion target

Version `0.0.5` is complete when the player can run an event that makes or loses money based on their booking and venue decisions.

---

# Version 0.0.6 — Calendar and Game Loop

## Goal

Turn isolated events into a continuing management game.

## Planned features

* Add an in-game date
* Use weeks as the basic unit of time
* Schedule events for future weeks
* Advance one week at a time
* Trigger scheduled events
* Display upcoming events
* Display the next event
* Add a basic dashboard
* Allow the player to repeat the event cycle

## Basic gameplay loop

1. Review the roster
2. Create an event
3. Book matchups
4. Select a venue
5. Choose a ticket price
6. Advance time
7. Run the event
8. Review results and finances
9. Book the next event

## Completion target

Version `0.0.6` is complete when the player can schedule events, advance through time, run events, and continue the cycle indefinitely.

---

# Version 0.0.7 — Contracts and Free Agents

## Goal

Allow the player to build and change their roster.

## Planned features

* Create a free-agent pool
* Add simple fighter contracts
* Allow the player to sign fighters
* Add signing costs where appropriate
* Allow the player to release fighters
* Reduce contract fights after appearances
* Handle expired contracts
* Return unsigned fighters to free agency
* Allow contract renewals

## Initial contract data

Contracts may initially contain:

* Pay per fight
* Number of fights remaining

More advanced negotiations can be added later.

## Completion target

Version `0.0.7` is complete when the player can sign, use, renew, release, and lose fighters through simple contracts.

---

# Version 0.0.8 — Rival Promotion

## Goal

Add one computer-controlled competing promotion.

## Planned features

* Create a rival promotion
* Give the rival its own roster
* Give the rival money and popularity
* Let the rival sign free agents
* Let the rival release fighters
* Let the rival book events
* Let the rival simulate events
* Let the rival gain or lose money
* Let the rival gain popularity
* Allow competition for available fighters
* Allow rivals to approach fighters with expired contracts

## AI approach

The rival should use simple rules rather than complicated artificial intelligence.

Example rules:

* Sign fighters when the roster is too small
* Book an event when no event is scheduled
* Use smaller venues when money is low
* Place popular fighters higher on the card
* Avoid contracts the company cannot afford

The rival should follow the same basic rules as the player.

## Completion target

Version `0.0.8` is complete when one rival promotion can operate for at least one in-game year without becoming stuck.

---

# Version 0.0.9 — Save, Load, and Cleanup

## Goal

Make longer playtests possible and improve the stability of the prototype.

## Planned features

* Save the current game
* Load a saved game
* Save fighters and records
* Save rosters
* Save contracts
* Save companies
* Save finances
* Save events
* Save the current date
* Add save-version information
* Improve validation
* Improve navigation
* Fix common simulation errors
* Run long automated simulations

## Validation examples

Prevent situations such as:

* A fighter fighting themselves
* A fighter appearing twice on one event
* Invalid fighter selections
* Events with no bouts
* Invalid venue capacities
* Invalid contract values
* Missing fighters in saved events

## Long simulation tests

Test scenarios such as:

* One in-game year
* Five in-game years
* Hundreds of events
* Thousands of fights
* Repeated rival signings
* Repeated save and load cycles

## Completion target

Version `0.0.9` is complete when a campaign can be saved, loaded, and played for at least one in-game year without major errors.

---

# Version 0.1.0 — First Playable Alpha

## Goal

Create the first complete version of the main management loop.

The game does not need to be polished or ready for sale. It should be complete enough for another person to play and provide meaningful feedback.

## Planned features

* Fictional fighter generation
* Round-based fight simulation
* Knockouts, submissions, and decisions
* Fighter records
* Bouts
* Complete events
* Player-controlled promotion
* Player roster
* Fighter popularity
* Promotion popularity
* Venues
* Ticket prices
* Attendance
* Event revenue
* Event expenses
* Company money
* Calendar progression
* Contracts
* Free agents
* One rival promotion
* Saving and loading
* Basic Godot interface

## Alpha completion checklist

* [ ] A new game can be started
* [ ] A fictional fighter pool is generated
* [ ] The player has a starting roster
* [ ] Fighters can be inspected
* [ ] Fighters can be signed
* [ ] Fighters can be released
* [ ] Events can be created
* [ ] Matchups can be booked
* [ ] Invalid matchups are prevented
* [ ] Events can be scheduled
* [ ] Time can advance
* [ ] Complete events can be simulated
* [ ] Fight records update correctly
* [ ] Events sell tickets
* [ ] Events generate revenue
* [ ] Events create expenses
* [ ] Company money changes
* [ ] Contracts lose remaining fights
* [ ] Contracts can expire
* [ ] The rival promotion signs fighters
* [ ] The rival promotion runs events
* [ ] The game can be saved
* [ ] The game can be loaded
* [ ] The main game loop can be repeated
* [ ] Another person can understand the basic controls

---

# Development Principles

## Build systems in playable layers

Each milestone should add something the player can see or do.

Avoid building large hidden systems that are not connected to the game.

## Keep simulation logic outside Godot

The core simulation should remain in ordinary C# classes.

Godot should handle:

* Interface
* Input
* Display
* Navigation
* Presentation

The core project should handle:

* Fighters
* Fight simulation
* Events
* Promotions
* Contracts
* Finances
* Rival logic
* Saving

## Prefer simple systems first

The first version of a system should be understandable and easy to test.

Complexity can be added after the basic system works.

## Avoid premature realism

A realistic feature is not automatically a useful feature.

Features should create:

* Interesting decisions
* Clear consequences
* Fighter stories
* Business risks
* Rival competition

## Keep future ideas separate

Future ideas should be added to `docs/ideas.md` rather than immediately becoming development commitments.

Possible future ideas include:

* Fighter-specific peak ages
* Career development
* Training camps
* Injuries
* Reach
* Height
* Weight cutting
* Fighter morale
* Rivalries
* Championships
* Rankings
* Sponsorships
* Broadcast deals
* International markets
* Media stories
* Drug testing
* Five-round fights
* Judges and split decisions
* Specific submissions
* Fighter portraits
* Procedural visual generation
* Mod support

---

# GitHub Planning

## Milestones

Use GitHub milestones for the versions listed in this roadmap.

Each milestone should describe a playable development checkpoint.

## Issues

Use issues for medium-sized development goals.

Good examples:

* Implement round-based fight simulation
* Create event booking
* Add venue and attendance calculations
* Add simple fighter contracts
* Implement rival event booking

Avoid creating issues for every method, property, variable, or small code change.

## Project board

Use one project board with these statuses:

* Backlog
* Ready
* In Progress
* Testing
* Done

Only one or two issues should normally be in progress.

## Planning scope

Fully plan only:

* The current milestone
* The next milestone

Later milestones should remain flexible until development gets closer to them.

---

# Current Focus

## Milestone

`0.0.1 — Fighters and Fight Simulation`

## Current goal

Generate fictional fighters and simulate round-based fights where:

* Fighters choose their preferred style
* Fighters compete for control of the round
* The active style changes performance calculations
* Each round has a winner
* Large performance advantages can create finishes
* Fights can end by knockout, submission, or decision
* Fighter records update after the result

The next milestone should not begin until this complete flow works reliably.
