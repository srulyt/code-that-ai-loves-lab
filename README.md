# Code That AI Loves — Lab Repository

A hands-on training repository for the workshop **"Code That AI Loves."** It demonstrates,
with a single evolving codebase, how **code quality changes the performance of AI coding
agents** such as GitHub Copilot.

You start with an intentionally messy back-office system and refactor it across four labs.
After each stage you re-run the **same two business tasks** and measure how the AI behaved:
output quality, correctness, files touched, manual cleanup, and raw effort (tokens, turns,
tool calls, wall time).

---

## Course context

The central claim of the course:

> AI coding agents are **codebase-quality amplifiers**. The cleaner the code, the better
> the agent performs — and the messier the code, the more the agent struggles, hallucinates,
> and regresses.

The labs prove this empirically. The architecture improves while the tasks stay fixed, so any
change in AI behaviour is attributable to the code, not the prompt.

| Agents struggle when the code has…        | Agents thrive when the code has…              |
|-------------------------------------------|-----------------------------------------------|
| Leaky abstractions                        | Clean abstractions and clear interfaces       |
| Horizontal sprawl                         | Vertical, feature-oriented slices             |
| Poor domain modeling                      | Explicit, well-named business concepts        |
| Orchestration mixed with business logic   | Thin entry points; separated orchestration    |
| Rules duplicated across layers            | One rule, one home                            |
| `if`/`switch` sprawl                      | Policies, strategies, factories, IOC          |
| No tests                                  | Tests as a definition of done                 |

---

## The sample system

A **Retail Order Processing & Returns Back Office** — a small commerce backend exposed as a
**.NET 8 Minimal API**. It handles customers, products, orders, invoices, shipping, returns,
discounts, and reporting summaries.

Persistence is simulated with **local JSON files** under `src/BackOffice.Api/Data/`
(`customers.json`, `products.json`, `orders.json`, `returns.json`). No database, no Docker,
no external services.

> ⚠️ **The baseline is intentionally messy.** Horizontal folders, DTOs used as domain objects,
> business rules encoded as magic strings, the same rule duplicated in three or four places,
> and two large orchestration methods. It compiles and runs, but it is painful to change.
> Do **not** "fix" it ahead of the labs — the mess is the point.

### Project layout (baseline)

```
src/BackOffice.Api/
  Program.cs            # Minimal API wiring + a couple of inline endpoints
  Controllers/          # Endpoint handlers with validation/business logic mixed in
  Services/             # OrderService (huge), PricingService, ShippingService,
                        # ReturnService, InvoiceService, ReportService (huge)
  Repositories/         # File-based repositories (copy-pasted load/save)
  Models/               # Anemic POCOs
  Dtos/                 # Request DTOs, used as working domain objects
  Helpers/              # PricingHelper, ShippingHelper, InvoiceHelper, JsonStore
  Config/               # AppConfig constants (some duplicated as literals elsewhere)
  Data/                 # JSON "database"
tests/BackOffice.Tests/ # xUnit suite (only on baseline-with-tests and later)
docs/                   # Labs, instructor guide, rubric, metrics prompt
scripts/                # run.ps1, test.ps1, reset-data.ps1
```

---

## Setup

Prerequisites:

- **.NET 8 SDK** (`dotnet --version` should report `8.x`; the repo targets `net8.0`)
- **GitHub Copilot CLI** (for the labs and the metrics prompt)
- Git

Clone and restore:

```powershell
git clone <your-fork-url> code-that-ai-loves-lab
cd code-that-ai-loves-lab
dotnet restore CodeThatAILoves.sln
```

### Run the baseline

```powershell
./scripts/run.ps1
# or:
dotnet run --project src/BackOffice.Api/BackOffice.Api.csproj
```

The API listens on `http://localhost:5080`. Try it:

```powershell
curl http://localhost:5080/                       # endpoint index
curl http://localhost:5080/reports/summary        # reporting summary
curl http://localhost:5080/orders                 # list seeded orders

# Place an order (Gold customer, Express, basket > 150 -> free express)
curl -X POST http://localhost:5080/orders -H "Content-Type: application/json" `
  -d '{"customerId":"C-1002","shippingMethod":"Express","lines":[{"productId":"P-5","quantity":3}]}'
```

### Run the tests

Tests live on `baseline-with-tests` (and later stages), **not** on the `main` / `baseline-no-tests`
baseline:

```powershell
git checkout baseline-with-tests
./scripts/test.ps1
# or:
dotnet test CodeThatAILoves.sln
```

Expect the suite to pass with several **skipped** tests — those skipped tests are the
ready-made "definition of done" for Task A and Task B.

### Reset the JSON data

The running app writes to the copy of `Data/` next to the binary (`bin/...`). The source seed
files are never modified. To reset runtime data:

```powershell
./scripts/reset-data.ps1
```

---

## The two recurring business tasks

These **exact** tasks are repeated in every lab so you can compare apples to apples.

### Task A — Add a new order scenario: **Corporate Gift Order**

- Can ship to multiple recipients
- Allows a gift message
- Uses corporate agreement pricing instead of retail promotions
- Invoice format differs from standard consumer orders
- Return policy differs from standard retail orders
- Is persisted and appears in summaries/reports

### Task B — Change a cross-cutting rule: **Gold free express shipping**

Gold customers get **free express shipping only when**:

- the basket total is above a configurable threshold, **and**
- the order contains at least one **non-fragile** item

Otherwise, normal shipping rules apply.

Both tasks are deliberately cross-cutting. In the baseline they force edits across many files;
after the refactors they should become localized, low-risk changes.

---

## The four labs

| Lab | Theme | You will… | Doc |
|-----|-------|-----------|-----|
| 1 | Tests & baseline | Run both tasks without tests, then with tests; compare confidence and regressions | [docs/lab-1-tests-and-baseline.md](docs/lab-1-tests-and-baseline.md) |
| 2 | Vertical slices | Refactor horizontal layers into feature folders; re-run both tasks | [docs/lab-2-vertical-slices.md](docs/lab-2-vertical-slices.md) |
| 3 | Modeling & abstractions | Introduce real domain concepts, thin entry points, clean seams; re-run both tasks | [docs/lab-3-modeling-and-abstractions.md](docs/lab-3-modeling-and-abstractions.md) |
| 4 | Branching → policies & IOC | Replace `if`/`switch` sprawl with policies, strategies, factories, DI; re-run both tasks | [docs/lab-4-branching-to-policies-and-ioc.md](docs/lab-4-branching-to-policies-and-ioc.md) |

Supporting material:

- [docs/instructor-guide.md](docs/instructor-guide.md) — teaching narrative and pacing
- [docs/scoring-rubric.md](docs/scoring-rubric.md) — how to score quality, effort, and impact
- [docs/metrics-prompt.md](docs/metrics-prompt.md) — Copilot CLI prompt to extract effort metrics

---

## Suggested branch progression

The repo ships with three real branches; the rest are produced **by you** during the labs.

| Branch | State | Provided? |
|--------|-------|-----------|
| `main` | Messy baseline (no tests) **plus all docs** — clone this | ✅ |
| `baseline-no-tests` | Snapshot of the messy baseline code, no tests | ✅ |
| `baseline-with-tests` | Baseline + xUnit characterization tests | ✅ |
| `after-lab-2` | After the vertical-slice refactor | 🛠️ you create |
| `after-lab-3` | After modeling & abstractions | 🛠️ you create |
| `after-lab-4` | After policies/strategies/IOC | 🛠️ you create |

Recommended flow:

```powershell
# Lab 1 first half (no tests)
git checkout baseline-no-tests

# Lab 1 second half + Labs 2-4 (tests present)
git checkout baseline-with-tests
git checkout -b after-lab-2     # at the start of Lab 2
# ...refactor, commit...
git checkout -b after-lab-3     # at the start of Lab 3
git checkout -b after-lab-4     # at the start of Lab 4
```

Commit after each lab so you can diff stages and quantify "files touched."

---

## Expected learning outcomes

By the end of the workshop you should be able to:

- Explain **why** AI coding agents perform better on clean code, with evidence you gathered.
- Use **tests as a definition of done** that lets an agent self-verify and avoid regressions.
- Apply **vertical-slice** organization to improve change locality and discoverability.
- Model business concepts as **domain objects, value objects, and interfaces** instead of
  procedural glue and magic strings.
- Replace duplicated **`if`/`switch`** branching with **policies, strategies, factories, and IOC**.
- Measure agent **effort and change impact**, and connect those metrics back to code quality.
