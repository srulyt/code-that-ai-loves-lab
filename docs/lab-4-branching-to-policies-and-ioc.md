# Lab 4 — Reduce Branching with Policies, Strategies, Factories, and IOC

> Part of the **Code That AI Loves** workshop. The final refactoring lab; builds on Lab 3.

---

## 1. Title

**Pushing Variation Into Design: From `if`/`switch` Sprawl to Polymorphism and Composition.**

---

## 2. Learning goals

By the end of this lab you will be able to:

- Locate the worst **branching hot spots** (`if customer.Type == …`, `switch shippingMethod`).
- Replace repeated branching with **strategy / policy objects, polymorphism, and factories**.
- Wire dependencies with **simple IOC / dependency injection** and a clear **composition root**.
- Explain why type- and policy-based designs reduce the agent's **search surface** and make
  targeted changes reliable.

---

## 3. Why this lab matters for AI coding agents

Repeated branching is one of the most expensive smells for an agent. Every `switch` on
`OrderType` or `if` on `CustomerTier` is a place that must be found and updated when a new
variant appears — and the agent can never be sure it found them all. Adding "Corporate Gift
Order" to a code base full of `switch (order.Type)` means hunting every switch in the repo.

When variation lives in **types and policies** selected by a **factory** and injected via **IOC**:

- adding a variant means adding **one class**, not editing N switches,
- the composition root is the single, obvious place to register it,
- and the agent's change is local, mechanical, and verifiable.

This is where "code that AI loves" pays off most: the dimension along which the business varies
is now explicit in the design.

---

## 4. Starting point / branch / prerequisite

- Start from `after-lab-3` (named concepts, single-homed rules).
- Create your branch:

```powershell
git checkout after-lab-3
git checkout -b after-lab-4
dotnet test CodeThatAILoves.sln    # confirm green before you start
```

---

## 5. What students should do

1. Inventory the branching hot spots.
2. Introduce policy/strategy interfaces for the axes of variation.
3. Add a factory to select the right policy per order/customer/shipping type.
4. Wire it with the built-in .NET DI container (composition root in `Program.cs`).
5. Re-run **Task A** and **Task B**; compare to all prior stages.

---

## 6. Step-by-step instructions

1. **Find the hot spots.** Use Prompt 1 (section 10). Expect `switch (order.Type)` in reporting
   and order handling, `if (customerType == "Gold"/"Corporate")` in pricing/shipping, and the
   shipping-method `switch`.

2. **Introduce policy interfaces** for each axis of variation, for example:
   - `IDiscountPolicy` (per `CustomerTier` / agreement)
   - `IShippingPolicy` (per shipping method, incl. the Gold free-express rule)
   - `IReturnPolicy` (per order type / customer tier)
   - `IInvoiceFormatter` (per order type / customer tier)
   - `IOrderHandler` or `IOrderFactory` (per `OrderType`, incl. the gift-order variant)

   Move the body of each branch into a small class implementing the interface.

3. **Add a factory / resolver** that maps a key (tier, order type, shipping method) to the right
   policy. Prefer a dictionary or DI-keyed lookup over a `switch` — the lookup is data, not code.

4. **Wire IOC.** Register policies and the factory in `Program.cs` (the composition root) using
   `builder.Services.AddSingleton<...>()`. Replace the `new XService()` calls scattered through
   the endpoints with constructor-injected or resolved dependencies.

5. **Delete the branches.** As each policy takes over, remove the corresponding `if`/`switch`.
   Keep tests green after every removal; commit in small steps.

6. **Un-skip the target tests.** Enable `TaskB_GoldShippingTests` (including the configurable
   threshold) and the relevant `TaskA_GiftOrderTests`. The threshold should now come from
   configuration/policy, not a literal.

   > ℹ️ `TaskA_GiftOrderTests` (and `Target_threshold_is_configurable`) are **guided placeholders** holding
   > commented-out pseudo-code, not ready-to-pass assertions. Enabling them means turning that pseudo-code
   > into real assertions against your policies/config — removing `Skip` alone will fail.

> Don't force an abstraction where there is genuinely no variation. Apply policies where the
> business actually varies (tier, order type, shipping method, return/invoice rules).

---

## 7. The exact task(s) for this lab

The de-branching refactor, **plus** the two recurring tasks:

- **Task A — Corporate Gift Order.** Now ideally implemented as a **new policy/handler class**
  registered in the composition root — minimal edits to existing code.
- **Task B — Gold free express** (basket > **configurable** threshold **and** ≥1 non-fragile
  item). Now a change inside one `IShippingPolicy` implementation plus configuration.

---

## 8. Suggested validation steps

- `dotnet build` / `dotnet test` green after each step; finish with the previously-skipped
  Task A/B targets **enabled and passing**.
- Branch count drops — verify hot spots are gone:
  ```powershell
  rg "switch .*Type" src/
  rg 'Type == "Gold"' src/
  rg "== \"Corporate\"" src/
  ```
- `Program.cs` shows the registrations (the composition root).
- Task A/B re-runs: `git diff --stat` shows Task A is largely **new files**, not edits to old
  ones.

---

## 9. Discussion / reflection questions

1. How many `if`/`switch` sites did Task A force you to edit in Lab 1 vs. now?
2. Did implementing Task A become "add one class and register it"? If not, what variation is
   still hard-coded?
3. How did the composition root help the agent find where to wire a new policy?
4. Are there places you *resisted* abstracting because there was no real variation? Why is that
   the right call?
5. Compare the agent's effort metrics across all four labs. Which refactor moved the needle most?

---

## 10. Suggested sample prompts

**Prompt 1 — Locate duplicated branching:**
```
Find every branching hot spot related to business variation in this repo: switches on order type
or shipping method, and conditionals on customer tier (Gold/Corporate). For each, tell me the
file, what axis of variation it represents, and how many distinct sites implement the same axis.
Do not change code yet.
```

**Prompt 2 — Propose a strategy/policy/factory refactor:**
```
Propose a refactor that replaces these branches with policy/strategy interfaces (e.g.
IDiscountPolicy, IShippingPolicy, IReturnPolicy, IInvoiceFormatter, IOrderHandler) plus a factory
that selects the right implementation by key. Keep all xUnit tests green. Give an incremental
plan: which interface to introduce first, which branches it absorbs, and how the factory selects.
```

**Prompt 3 — Implement IOC / composition-root wiring:**
```
Register the policies and the factory in Program.cs using the built-in .NET DI container as the
composition root. Replace the scattered `new SomeService()` calls in the endpoint handlers with
injected/resolved dependencies. Keep behaviour identical and the suite green. Show the final
Program.cs registrations.
```

**Prompt 4 — Re-run the tasks after the refactor:**
```
Implement Task A (Corporate Gift Order) by adding a new IOrderHandler/IInvoiceFormatter/
IReturnPolicy implementation and registering it in the composition root, editing as little
existing code as possible. Then implement Task B by changing only the relevant IShippingPolicy
and reading the threshold from configuration. Enable the skipped TaskA/TaskB tests and make the
whole suite green. List exactly which files you added vs. modified.
```

---

## 11. Expected observation about how the AI behaves

- The agent implements Task A mostly by **adding classes** and a registration line, touching
  little existing code — the opposite of the Lab 1 experience.
- Task B is a precise, single-policy change plus a config value.
- The agent stops inventing duplicates because the design tells it exactly where variation goes.
- Effort metrics (turns, tokens, files read/written) should be at their **lowest** of the
  workshop, with quality at its highest.

---

## 12. How to evaluate the result

Fill the worksheet for the Lab 4 re-runs of Task A and Task B, then complete the
**cross-stage comparison** table in `docs/scoring-rubric.md`.

A "good" Lab 4 result:

- Business branching hot spots are gone (verified with `rg`).
- Task A = "add and register a class"; Task B = "edit one policy + config."
- All tests green, including the previously-skipped targets.
- Best design-quality, readability, and lowest-effort scores of the four labs.

---

## 13. What should improve compared to the previous stage (Lab 3)

Compared to Lab 3 numbers, expect:

- **Fewest files modified** for Task A (mostly additions, not edits).
- **Lowest agent effort** (turns, tokens, tool calls, wall time) across the whole workshop.
- **Highest design-quality** score; intent is explicit in the type/policy structure.
- A clean composition root that makes future variants trivial to add.

### Closing the loop

Lay the four stages side by side (Lab 1 → Lab 4) for both tasks and present the trend. The story
should be unmistakable: **as the code got cleaner, the AI did less work and produced better,
safer results — for the exact same tasks.**
