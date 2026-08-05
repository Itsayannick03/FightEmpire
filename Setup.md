# Fight Empire — GitHub Project Setup

## Repository structure

```text
FightEmpire/
├── FightEmpire.Core/
├── FightEmpire.Console/
├── FightEmpire.Tests/
└── FightEmpire.sln
```

Later:

```text
FightEmpire.Godot/
```

---

# Labels

Create these labels first.

## Type labels

| Label                 | Purpose                                       |
| --------------------- | --------------------------------------------- |
| `type: feature`       | New gameplay or technical functionality       |
| `type: bug`           | Something behaves incorrectly                 |
| `type: test`          | Automated testing work                        |
| `type: refactor`      | Improving code without changing behavior      |
| `type: documentation` | README or design documentation                |
| `type: balancing`     | Adjusting probabilities and game values       |
| `type: research`      | A decision or system that needs investigation |

## Area labels

| Label                  | Purpose                            |
| ---------------------- | ---------------------------------- |
| `area: fighters`       | Fighter models and generation      |
| `area: simulation`     | Fight simulation                   |
| `area: events`         | Fight cards and event management   |
| `area: contracts`      | Signings and negotiations          |
| `area: finances`       | Revenue, expenses and ticket sales |
| `area: ai`             | Rival companies                    |
| `area: persistence`    | Saving and loading                 |
| `area: ui`             | Console or Godot interface         |
| `area: infrastructure` | Project setup, builds and tooling  |

## Priority labels

| Label                | Purpose                         |
| -------------------- | ------------------------------- |
| `priority: critical` | Blocks the current milestone    |
| `priority: high`     | Important for the milestone     |
| `priority: normal`   | Standard work                   |
| `priority: low`      | Nice improvement, not essential |

## Effort labels

| Label            | Meaning                          |
| ---------------- | -------------------------------- |
| `effort: small`  | One short development session    |
| `effort: medium` | Several sessions                 |
| `effort: large`  | Should probably be split further |

Do not create separate labels for every possible concept. Labels should help filter work, not become another project to manage.

---

# Project board

Create a GitHub Project named:

```text
Fight Empire Development
```

Use these columns or statuses:

```text
Backlog
Ready
In Progress
Testing
Done
```

Optional additional status:

```text
Blocked
```

## Suggested project fields

### Status

```text
Backlog
Ready
In Progress
Testing
Done
Blocked
```

### Priority

```text
Critical
High
Normal
Low
```

### Area

```text
Infrastructure
Fighters
Simulation
Events
Contracts
Finances
AI
Persistence
UI
```

### Effort

```text
Small
Medium
Large
```

### Target version

```text
0.0.1
0.0.2
0.0.3
0.0.4
0.0.5
0.1.0
Future
```

---

# Milestones

## Milestone 0.0.1 — Project Foundation

The solution exists, builds correctly and contains the first basic fighter model.

### Completion requirements

* Repository initialized
* C# solution created
* Core project created
* Console project created
* Test project created
* Project references configured
* Fighter class created
* Two fighters can be displayed in the console

---

## Milestone 0.0.2 — First Fight

Two fighters can be passed into a simulator and a basic winner is returned.

### Completion requirements

* Fighters have a single skill rating
* Fight simulator exists
* Fighter performance contains basic randomness
* Higher performance wins
* Draws are handled
* Invalid fights are rejected
* Simulator returns a result object
* Basic automated tests exist

---

## Milestone 0.0.3 — Fight Records

Fight results have lasting consequences.

### Completion requirements

* Fighters track wins and losses
* Results update fighter records
* Result data includes winner and loser
* Repeated simulations can be run
* Statistics can be printed for many fights

---

## Milestone 0.0.4 — Basic MMA Simulation

The single skill rating is replaced by a small set of MMA attributes.

### Completion requirements

* Striking exists
* Wrestling exists
* Grappling exists
* Cardio exists
* Attributes affect fight results
* Victory method can be knockout, submission or decision
* Basic balancing tests exist

---

## Milestone 0.0.5 — Basic Fight Cards

Multiple fights can be booked and simulated as one event.

### Completion requirements

* Bout model exists
* Event model exists
* Fighters can be assigned to bouts
* Invalid matchups are prevented
* Full cards can be simulated
* Event results are displayed

---

## Milestone 0.1.0 — First Playable Alpha

The complete basic management loop is playable.

### Completion requirements

* Roster management
* Free agents
* Contracts
* Event booking
* Venues
* Ticket sales
* Event finances
* Weekly advancement
* One rival company
* Saving and loading
* Basic Godot interface

---

# Milestone 0.0.1 Issues

## Issue: Set up the Fight Empire solution

**Labels**

```text
type: feature
area: infrastructure
priority: critical
effort: small
```

**Description**

Create the initial .NET solution and projects for Fight Empire.

**Acceptance criteria**

* `FightEmpire.sln` exists
* `FightEmpire.Core` exists
* `FightEmpire.Console` exists
* `FightEmpire.Tests` exists
* All projects are included in the solution
* The console project references the core project
* The test project references the core project
* `dotnet build` completes successfully
* `dotnet test` completes successfully

### Sub-issues

#### Create the .NET solution

```bash
dotnet new sln -n FightEmpire
```

Acceptance criteria:

* `FightEmpire.sln` exists

#### Create the core project

```bash
dotnet new classlib -n FightEmpire.Core
```

Acceptance criteria:

* The project builds
* Default placeholder classes are removed

#### Create the console project

```bash
dotnet new console -n FightEmpire.Console
```

Acceptance criteria:

* The console application runs
* It prints a temporary startup message

#### Create the test project

```bash
dotnet new xunit -n FightEmpire.Tests
```

Acceptance criteria:

* Tests can be executed
* The default test passes or is replaced

#### Configure project references

Acceptance criteria:

* Console references Core
* Tests reference Core
* The complete solution builds

---

## Issue: Create the initial fighter model

**Labels**

```text
type: feature
area: fighters
priority: critical
effort: small
```

**Description**

Create the smallest possible model representing an MMA fighter.

**Acceptance criteria**

* A `Fighter` class exists
* A fighter has a name
* The class belongs to the core project
* A fighter can be instantiated from the console project

### Sub-issues

#### Create the Models folder

Create:

```text
FightEmpire.Core/Models/
```

#### Create Fighter.cs

Initial implementation:

```csharp
namespace FightEmpire.Core.Models;

public class Fighter
{
    public string Name { get; set; } = string.Empty;
}
```

Acceptance criteria:

* The project builds
* Fighter names can be assigned

#### Display one fighter in the console

Acceptance criteria:

* One fighter is instantiated
* The fighter's name appears in the console

#### Display two fighters in the console

Acceptance criteria:

* Two separate fighters are instantiated
* Both names appear correctly

---

## Issue: Add repository documentation

**Labels**

```text
type: documentation
area: infrastructure
priority: normal
effort: small
```

**Description**

Add enough documentation to explain what Fight Empire is and how to run the project.

**Acceptance criteria**

* README contains a short game description
* README explains the current development stage
* README contains build instructions
* README contains run instructions
* README states that the game is currently an early prototype

### Suggested README description

```text
Fight Empire is an MMA promotion management simulator.

The player builds a roster, signs fighters, creates fight cards, runs events,
sells tickets and competes against rival promotions.

The project is currently in early prototype development.
```

---

## Issue: Configure Git repository basics

**Labels**

```text
type: feature
area: infrastructure
priority: high
effort: small
```

**Acceptance criteria**

* Git repository is initialized
* A suitable `.gitignore` exists
* Build artifacts are ignored
* Initial project state is committed
* Main branch is pushed to GitHub

### Sub-issues

#### Create the .gitignore

Use the standard Visual Studio or .NET `.gitignore`.

#### Create the initial commit

Suggested commit message:

```text
Initialize Fight Empire solution
```

#### Push the repository to GitHub

Acceptance criteria:

* Repository is visible on GitHub
* Main branch contains the solution

---

# Milestone 0.0.2 Issues

## Issue: Add a basic skill rating to fighters

**Labels**

```text
type: feature
area: fighters
priority: critical
effort: small
```

**Description**

Give each fighter one temporary skill rating representing their overall fighting ability.

**Acceptance criteria**

* Fighter has a `Skill` property
* Skill uses an integer
* Two fighters can have different skill values
* Skill values appear in the console

### Sub-issues

#### Add the Skill property

```csharp
public int Skill { get; set; }
```

#### Assign skill ratings to test fighters

Example:

```text
Marcus Silva: 75
Erik Nilsson: 68
```

#### Display fighter ratings

Acceptance criteria:

* Fighter names and skills are printed clearly

---

## Issue: Create the first deterministic fight simulator

**Labels**

```text
type: feature
area: simulation
priority: critical
effort: small
```

**Description**

Create a simulator that compares two fighter skill ratings and returns the stronger fighter.

**Acceptance criteria**

* `FightSimulator` exists
* The simulator accepts two fighters
* Fighter with higher skill wins
* Equal skill produces a draw
* Simulation logic is not located in `Program.cs`

### Sub-issues

#### Create the Simulation folder

Create:

```text
FightEmpire.Core/Simulation/
```

#### Create FightSimulator.cs

Initial method:

```csharp
public Fighter? DetermineWinner(Fighter fighterA, Fighter fighterB)
```

#### Return fighter A when fighter A has higher skill

#### Return fighter B when fighter B has higher skill

#### Return null when skills are equal

#### Display the result in Program.cs

---

## Issue: Add randomized fighter performance

**Labels**

```text
type: feature
area: simulation
priority: high
effort: small
```

**Description**

Add limited randomness so that weaker fighters can occasionally win.

**Acceptance criteria**

* Each fighter receives a random performance modifier
* Performance is based on fighter skill
* Stronger fighters usually win
* Weaker fighters can occasionally win
* Randomness is limited enough that skill still matters

### Sub-issues

#### Generate performance values

Initial formula:

```csharp
int performance = fighter.Skill + random.Next(-10, 11);
```

#### Compare performance instead of raw skill

#### Verify that upsets are possible

#### Verify that stronger fighters still win more often

---

## Issue: Create a FightResult model

**Labels**

```text
type: feature
area: simulation
priority: critical
effort: small
```

**Description**

Return structured fight information instead of returning only a fighter.

**Acceptance criteria**

* `FightResult` exists
* Result contains the winner
* Result indicates whether the fight was a draw
* Result contains both performance values
* FightSimulator does not print directly to the console

### Sub-issues

#### Create FightResult.cs

Properties:

```text
Winner
IsDraw
FighterAPerformance
FighterBPerformance
```

#### Change FightSimulator to return FightResult

#### Move result display into Program.cs

---

## Issue: Add fight validation

**Labels**

```text
type: feature
area: simulation
priority: high
effort: small
```

**Description**

Reject invalid input before attempting to simulate a fight.

**Acceptance criteria**

* Fighter A cannot be null
* Fighter B cannot be null
* A fighter cannot fight themselves
* Invalid input produces a clear exception

### Sub-issues

#### Reject null fighter A

#### Reject null fighter B

#### Reject the same fighter instance

---

## Issue: Add tests for the basic fight simulator

**Labels**

```text
type: test
area: simulation
priority: high
effort: medium
```

**Description**

Add automated tests for the initial simulator behavior.

**Acceptance criteria**

* Higher performance wins
* Lower performance loses
* Equal performance produces a draw
* Same-fighter matchups are rejected
* Null inputs are rejected
* Tests can use deterministic randomness

### Suggested tests

```text
Simulate_WhenFighterAPerformsHigher_ReturnsFighterA
Simulate_WhenFighterBPerformsHigher_ReturnsFighterB
Simulate_WhenPerformancesAreEqual_ReturnsDraw
Simulate_WhenSameFighterIsUsedTwice_ThrowsException
Simulate_WhenFighterAIsNull_ThrowsException
Simulate_WhenFighterBIsNull_ThrowsException
```

---

## Issue: Support seeded randomness

**Labels**

```text
type: feature
area: simulation
priority: normal
effort: small
```

**Description**

Allow the simulator to receive a random seed so results can be reproduced.

**Acceptance criteria**

* FightSimulator can be initialized with a seed
* The same seed and fighters produce the same result
* The simulator can still use normal randomness when no seed is provided
* Automated tests do not depend on unpredictable random values

---

# Recommended issue hierarchy

Use one parent issue for each larger feature.

Example:

```text
Parent issue:
Create the first basic fight simulator

Sub-issues:
- Create the Simulation folder
- Create FightSimulator
- Compare fighter skills
- Handle equal skill
- Display result in console
```

Avoid creating sub-issues for extremely tiny actions such as:

```text
Open Visual Studio Code
Create one variable
Press run
```

A sub-issue should normally produce a small but verifiable result.

---

# Recommended first sprint

Move only these issues into `Ready`:

```text
Set up the Fight Empire solution
Create the initial fighter model
Configure Git repository basics
Add repository documentation
```

Everything from Milestone `0.0.2` should remain in `Backlog` until `0.0.1` is finished.

Your first board should look approximately like this:

## Ready

```text
Set up the Fight Empire solution
Create the initial fighter model
Configure Git repository basics
```

## In Progress

```text
Only one issue at a time
```

## Testing

```text
Issues that are implemented but not yet manually verified
```

## Done

```text
Completed and committed work
```

---

# Definition of done

Use this checklist for feature issues:

```markdown
## Definition of done

- [ ] Implementation is complete
- [ ] Project builds without errors
- [ ] Relevant tests pass
- [ ] Feature has been manually tested
- [ ] No debug code remains
- [ ] Changes have been committed
- [ ] Acceptance criteria are satisfied
```

For very small issues, not every item will apply.

---

# Commit naming

Use simple, descriptive commit messages.

Examples:

```text
Initialize Fight Empire solution
Add initial fighter model
Display fighters in console
Add basic fight simulator
Add randomized fighter performance
Add fight result model
Validate fight participants
Add fight simulator tests
```

Avoid vague messages such as:

```text
stuff
changes
update
fixed code
more work
```

---

# Branch naming

For a solo project, working directly on `main` is acceptable during the earliest prototype.

When you start using branches:

```text
feature/initial-fighter-model
feature/basic-fight-simulator
feature/random-performance
test/fight-simulator
fix/draw-result
```

Do not create a complicated Git workflow before the project needs one.

---

# Immediate first issue

Start with:

```text
Set up the Fight Empire solution
```

Its sub-issues should be:

```text
Create the .NET solution
Create FightEmpire.Core
Create FightEmpire.Console
Create FightEmpire.Tests
Add projects to the solution
Configure project references
Verify build and tests
```

After that, begin:

```text
Create the initial fighter model
```
