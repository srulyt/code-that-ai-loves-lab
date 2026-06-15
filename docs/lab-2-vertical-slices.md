# Lab 2 — Repository & Project Structure (Vertical Slices)

> Part of the **Code That AI Loves** workshop. Builds directly on the tests added in Lab 1.

---

## 1. Title

**Code That Changes Together Should Live Together: From Horizontal Layers to Vertical Slices.**

---

## 2. Learning goals

By the end of this lab you will be able to:

- Identify which code **changes together** for a given feature.
- Refactor a horizontally-layered repo (`Controllers/`, `Services/`, `Repositories/`, …) into a
  **feature-oriented / vertical-slice** structure.
- Explain how locality of change reduces the agent's **search surface**.
- Show, with metrics, that the same task now touches **fewer, more related** files.

---

## 3. Why this lab matters for AI coding agents

An agent spends a large fraction of its effort just **finding** the code relevant to a change.
In a horizontal layout, the logic for "orders" is smeared across `Controllers/OrderEndpoints.cs`,
`Services/OrderService.cs`, `Services/PricingService.cs`, `Services/ShippingService.cs`,
`Helpers/ShippingHelper.cs`, `Repositories/OrderRepository.cs`, and more. The agent must read
broadly, hold more in context, and is more likely to **miss** a place that needs to change.

Vertical slices put everything for a feature in one folder. The agent reads less, reasons more
locally, and changes fewer unrelated files. Discoverability improves for humans too.

> Note: this lab does **not** aim for perfect architecture. The goal is **locality** — reduce
> sprawl first; deeper modeling comes in Lab 3.

---

## 4. Starting point / branch / prerequisite

- Start from `baseline-with-tests` (you need the tests as a safety net).
- Create your working branch for this lab:

```powershell
git checkout baseline-with-tests
git checkout -b after-lab-2
dotnet test CodeThatAILoves.sln    # confirm green before you start
```

---

## 5. What students should do

1. Analyze which files change together per feature.
2. Agree on a target vertical structure.
3. Refactor **incrementally**, keeping tests green after every step.
4. Re-run **Task A** and **Task B** and compare effort/quality to Lab 1.

---

## 6. Step-by-step instructions

1. **Map the slices.** Use Prompt 1 (section 10) to have Copilot list, per feature, every file
   currently involved. Confirm it matches reality.

2. **Choose a target layout.** A reasonable target:
   ```
   src/BackOffice.Api/
     Features/
       Orders/      (OrderEndpoints, OrderService, PlaceOrderRequest, Order, OrderLine)
       Pricing/     (PricingService, PricingHelper)
       Shipping/    (ShippingService, ShippingHelper)
       Returns/     (ReturnEndpoints, ReturnService, CreateReturnRequest, Return)
       Invoicing/   (InvoiceService, InvoiceHelper)
       Reporting/   (ReportEndpoints, ReportService)
     Infrastructure/ (JsonStore, repositories, AppConfig)
     Program.cs
   ```
   Shared models (`Customer`, `Product`) can live in a small `Shared/` or `Infrastructure/`
   folder for now.

3. **Move one slice at a time.** Start with Returns (smallest). After each move:
   - update namespaces/usings,
   - `dotnet build`,
   - `dotnet test` — must stay green,
   - commit (`git commit -m "Lab2: extract Returns slice"`).

4. **Preserve behaviour.** This is a structural refactor only. If a test goes red, you changed
   behaviour — undo and try again.

5. **Re-run the tasks.** After the structure settles, run Task A and Task B prompts (section 10)
   and capture metrics.

---

## 7. The exact task(s) for this lab

The refactor itself, **plus** the two recurring tasks:

- **Task A — Corporate Gift Order** (same rules as always).
- **Task B — Gold free express** (basket > threshold **and** ≥1 non-fragile item).

Run both **after** the vertical-slice refactor so you can compare to Lab 1's numbers.

---

## 8. Suggested validation steps

- `dotnet build` and `dotnet test` are green after **every** incremental move.
- The app still runs and `/reports/summary`, `/orders`, `/returns` behave as before.
- `git diff --stat baseline-with-tests..after-lab-2` shows the structural change.
- For Task A/B re-runs, record `git diff --stat` and confirm the changes are now concentrated
  in the relevant `Features/*` folder(s).

---

## 9. Discussion / reflection questions

1. Before the refactor, how many folders did a single feature span? After?
2. When you re-ran Task B, did the agent still have to hunt across the repo, or did it head
   straight for `Features/Shipping`?
3. Did vertical organization reduce the number of **unrelated** files touched?
4. The duplicated rules still exist after this lab. Did slices at least make the duplicates
   easier to *find*? (Foreshadow Lab 3/4.)
5. Where did "shared" code (Customer, Product, JsonStore) want to live, and why is that
   decision harder than it looks?

---

## 10. Suggested sample prompts

**Prompt 1 — Analyze what changes together:**
```
Analyze this repository and group the files by business feature (Orders, Pricing, Shipping,
Returns, Invoicing, Reporting). For each feature, list every file currently involved and note
any business rule that is duplicated across features. Do not change code yet — just produce the
map.
```

**Prompt 2 — Propose a vertical-slice plan:**
```
Propose a refactor from the current horizontal layout (Controllers/Services/Repositories/…) to a
vertical, feature-oriented layout under src/BackOffice.Api/Features/<Feature>. Keep behaviour
identical and keep the xUnit tests green. Give me an incremental, slice-by-slice plan with the
exact file moves and namespace changes, smallest slice first.
```

**Prompt 3 — Execute the refactor incrementally:**
```
Execute step 1 of the plan: extract the Returns slice into src/BackOffice.Api/Features/Returns.
Move the files, update namespaces and usings, then run `dotnet build` and `dotnet test`. Do not
proceed until the suite is green. Report the result and the next step.
```

**Prompt 4 — Re-run the tasks after restructure:**
```
Now implement Task B (Gold free express only when basket > configurable threshold AND at least
one non-fragile item). The shipping code now lives under Features/Shipping. Make the skipped
TaskB_GoldShippingTests pass, keep the rest of the suite green, and list the files you changed.
```

---

## 11. Expected observation about how the AI behaves

- During the refactor, the agent moves files confidently because the tests verify behaviour is
  preserved after each step.
- When re-running Task B, the agent should **navigate to `Features/Shipping` quickly** instead
  of scanning the whole repo, read less context, and propose a tighter change.
- Task A will still spread across several slices (Orders, Pricing, Invoicing, Returns,
  Reporting) — because the *modeling* is still poor. That tension sets up Lab 3.

---

## 12. How to evaluate the result

Fill the worksheet (`docs/scoring-rubric.md`) for the Lab 2 re-runs of Task A and Task B.

A "good" Lab 2 result:

- Behaviour preserved (tests green throughout).
- Same feature change now touches **fewer folders** and fewer unrelated files than in Lab 1.
- The agent's reads/turns for Task B drop noticeably versus Lab 1.

---

## 13. What should improve compared to the previous stage (Lab 1)

Compared to Lab 1's with-tests numbers, expect:

- **Lower files-read / search effort**, especially for Task B.
- **Fewer unrelated files touched** for each task.
- Equal or better correctness (tests still green).
- Roughly equal *number of duplicated edits* — the duplication is still there, just easier to
  locate. Removing it is the job of Labs 3 and 4.

Commit the final state on `after-lab-2`; you will branch `after-lab-3` from here.
