# Lab 1 — Baseline With and Without Tests

> Part of the **Code That AI Loves** workshop. This is the first lab; it establishes the
> baseline measurements every later lab is compared against.

---

## 1. Title

**Tests as a Definition of Done: Running the Recurring Tasks With and Without a Safety Net.**

---

## 2. Learning goals

By the end of this lab you will be able to:

- Explain why tests are critical for **agentic** coding, not just for humans.
- Show that without tests an AI agent has a **weak feedback loop** and cannot prove success.
- Demonstrate that tests give the agent a **definition of done** it can iterate against.
- Capture your first set of **baseline metrics** (quality, effort, change impact) for both
  recurring tasks.

---

## 3. Why this lab matters for AI coding agents

An AI coding agent works in a loop: read context → make a change → check the result → repeat.
The "check the result" step is where tests matter.

- **Without tests**, the agent's only feedback is "does it compile?" and its own reasoning.
  It cannot tell whether it preserved existing behaviour, so it either stops too early
  (under-confident) or makes sweeping, risky edits (over-confident). Regressions slip through.
- **With tests**, the agent gets a precise, executable specification. It can run the suite,
  see red/green, and keep iterating until done. Failing tests become a to-do list; passing
  tests become proof.

This lab makes that difference visible and measurable using the messy baseline.

---

## 4. Starting point / branch / prerequisite

Prerequisites:

- .NET 8 SDK, Git, and GitHub Copilot CLI installed.
- You have read the root `README.md` and can run the API.

Branches:

- **First half (no tests):** `git checkout baseline-no-tests`
- **Second half (with tests):** `git checkout baseline-with-tests`

Confirm the baseline runs before you start:

```powershell
dotnet run --project src/BackOffice.Api/BackOffice.Api.csproj
curl http://localhost:5080/reports/summary
```

---

## 5. What students should do

1. On `baseline-no-tests`, attempt **Task A** and **Task B** with Copilot. Record metrics.
2. Switch to `baseline-with-tests`, where a characterization suite already exists.
3. Re-attempt the **same** tasks. Record metrics again.
4. Compare the two runs and discuss.

> Tip: create a throwaway branch for each attempt so you can `git diff` and `git reset` freely:
> `git checkout -b attempt/lab1-taskA-notests`

---

## 6. Step-by-step instructions

### Part A — No tests (`baseline-no-tests`)

1. `git checkout baseline-no-tests`
2. `git checkout -b attempt/lab1-notests`
3. Start a Copilot CLI session in the repo root.
4. Run the **Task A** prompt (section 10). Let the agent work. Do **not** hand-hold.
5. When the agent says it is done:
   - Build: `dotnet build CodeThatAILoves.sln`
   - Run the app and try to exercise the new behaviour manually with `curl`.
   - Note what you had to clean up by hand.
6. Capture metrics using `docs/metrics-prompt.md`.
7. `git reset --hard` to discard, then repeat steps 4–6 for **Task B**.

### Part B — With tests (`baseline-with-tests`)

1. `git checkout baseline-with-tests`
2. Run the suite once to see the starting point: `dotnet test CodeThatAILoves.sln`
   (expect green with several **skipped** tests — those are your targets).
3. `git checkout -b attempt/lab1-withtests`
4. Run the **Task A "with tests"** prompt (section 10). The agent should un-skip and satisfy
   the `TaskA_GiftOrderTests`.
5. Let the agent run `dotnet test` itself and iterate until green.
6. Capture metrics.
7. `git reset --hard`, then repeat for **Task B** using the `TaskB_GoldShippingTests` targets.

> The point is not to finish the feature perfectly — it is to **observe how differently the
> agent behaves** when a definition of done exists.

---

## 7. The exact task(s) for this lab

You will run **both** recurring tasks, in both modes (no-tests, with-tests).

### Task A — Corporate Gift Order
- Ships to multiple recipients; carries a gift message.
- Uses corporate agreement pricing (not retail promotions).
- Distinct invoice format; distinct return policy.
- Persisted and visible in `/reports/summary`.

### Task B — Gold free express shipping
- Gold customers get free express **only when** basket total > configurable threshold **and**
  the order has at least one non-fragile item. Otherwise normal shipping applies.

The skipped tests on `baseline-with-tests` (`TaskA_GiftOrderTests`, `TaskB_GoldShippingTests`)
encode these rules precisely.

---

## 8. Suggested validation steps

For **every** attempt, record:

- **Builds?** `dotnet build` — yes/no.
- **Behaves?** Manual `curl` checks for the new/changed behaviour.
- **Regressions?** (With tests) `dotnet test` — are previously-green tests still green?
  (No tests) you must reason about regressions manually — note how confident you feel.
- **Files touched:** `git diff --stat`.
- **Manual cleanup:** count edits you had to make after the agent stopped.

Concrete Task B check:

```powershell
# Gold, Express, basket 177 (>150), has non-fragile item -> expect free express (shippingCost 0)
curl -X POST http://localhost:5080/orders -H "Content-Type: application/json" `
  -d '{"customerId":"C-1002","shippingMethod":"Express","lines":[{"productId":"P-5","quantity":3}]}'

# Gold, Express, all-fragile basket over threshold -> after Task B expect NOT free
curl -X POST http://localhost:5080/orders -H "Content-Type: application/json" `
  -d '{"customerId":"C-1002","shippingMethod":"Express","lines":[{"productId":"P-6","quantity":3}]}'
```

---

## 9. Discussion / reflection questions

1. In the no-tests run, how did you *know* the agent succeeded? Could you be sure it didn't
   break existing orders, invoices, or returns?
2. How many places did the agent have to change for each task? Why so many?
3. In the with-tests run, did the agent use the failing tests as a guide? Did it run them?
4. Which run felt safer to ship? Which run was faster overall (including your cleanup)?
5. Did the agent introduce a *new* duplicate of a rule rather than reusing an existing one?
   Why might the messy structure encourage that?

---

## 10. Suggested sample prompts

Paste these into GitHub Copilot. Adjust paths if needed.

**Prompt 1 — Task A, no tests:**
```
Implement a new "Corporate Gift Order" order type in this .NET 8 Minimal API.
Rules: it can ship to multiple recipients, carries a gift message, uses corporate
agreement pricing instead of retail promotions, uses a distinct invoice format, has a
different return policy than standard retail, is persisted, and appears in
/reports/summary. There are no tests in this repo. Implement it end to end and tell me
exactly which files you changed and why.
```

**Prompt 2 — Task B, no tests:**
```
Change shipping so that Gold customers get free express shipping ONLY when the basket
total is above a configurable threshold AND the order contains at least one non-fragile
item. Otherwise apply normal shipping. There are no tests. Find every place this rule is
implemented and update all of them consistently. List the files you changed.
```

**Prompt 3 — Create tests (if your cohort adds tests by hand instead of switching branches):**
```
This repo has no tests. Add an xUnit test project that characterizes the CURRENT behaviour
of pricing, shipping (including Gold express and the fragile surcharge), return eligibility,
and invoice format selection. Then add skipped tests describing the TARGET behaviour for
"Corporate Gift Order" (Task A) and the new "Gold free express" rule (Task B). Keep the suite
green by skipping the unimplemented targets.
```

**Prompt 4 — Retry with tests present:**
```
The test project tests/BackOffice.Tests defines skipped tests for Task B in
TaskB_GoldShippingTests. Implement the rule so those tests pass: remove the Skip markers,
run `dotnet test`, and iterate until the whole suite is green without breaking any existing
test. Report which files you changed and the final test counts.
```

---

## 11. Expected observation about how the AI behaves

- **No tests:** The agent makes broad, somewhat speculative edits, often **adds yet another
  copy** of a rule, stops with a confident-sounding summary you cannot easily verify, and may
  silently regress invoices or returns. Your manual cleanup is high.
- **With tests:** The agent runs the suite, treats failing/skipped tests as a checklist,
  iterates to green, and is far less likely to regress. It stops with **evidence** ("33/33
  green") instead of a claim.

You should also notice that even *with* tests, the messy structure forces the agent to touch
many files — setting up the motivation for Lab 2.

---

## 12. How to evaluate the result

Use `docs/scoring-rubric.md` and fill the worksheet for **four** data points:
(Task A no-tests, Task A with-tests, Task B no-tests, Task B with-tests).

A result is "good" when, in the with-tests run, the agent:

- ends green with no regressions,
- changed roughly the minimum set of files the messy code allows,
- and produced output you would be willing to ship after light review.

Record raw effort (tokens, turns, tool calls, wall time) for each — these are your **baseline**
numbers for the rest of the workshop.

---

## 13. What should improve compared to the previous stage

This is the first lab, so the comparison is **within** the lab: no-tests vs with-tests.

Expect the **with-tests** run to show:

- Fewer regressions (ideally zero).
- Higher confidence / verifiable "done."
- Similar or **lower** total effort once your manual cleanup is included.
- A clearer, test-driven change list.

Carry your baseline metrics forward — Labs 2, 3, and 4 must beat them.
