# Lab 3 — Modeling and Abstractions

> Part of the **Code That AI Loves** workshop. Builds on the vertical slices from Lab 2.

---

## 1. Title

**From Procedural Glue to Named Concepts: Domain Modeling and Clean Seams.**

---

## 2. Learning goals

By the end of this lab you will be able to:

- Spot **missing business concepts** hiding inside procedural code and magic strings.
- Introduce **domain objects, value objects, and interfaces** that name those concepts.
- Move business logic **out of orchestration** (thick services / endpoints) and into the model.
- Establish clean **seams** between input, orchestration, domain logic, and persistence.
- Show that explicit concepts let the agent change behaviour with **shorter prompts** and less
  guesswork.

---

## 3. Why this lab matters for AI coding agents

Agents reason far better about **named concepts** than about procedural glue. When the rule is
`if (customer.Type == "Gold") discount = subtotal * 0.10m;` scattered across files, the agent
has to infer intent every time and re-derive the rule. When the concept is a `CustomerTier`
with a `Discount(PricingContext)` behaviour, the agent reads the name, understands the intent,
and edits in one place.

Good modeling:

- reduces the context the agent must hold,
- removes ambiguity (one concept, one home),
- and shrinks prompts ("update the gift-order pricing policy" vs. a paragraph describing where
  the rule is copy-pasted).

This lab converts the baseline's DTO-as-domain, string-typed design into something an agent can
navigate by **meaning**, not by text search.

---

## 4. Starting point / branch / prerequisite

- Start from `after-lab-2` (vertical slices in place).
- Create your branch:

```powershell
git checkout after-lab-2
git checkout -b after-lab-3
dotnet test CodeThatAILoves.sln    # confirm green before you start
```

If you did not complete Lab 2, you may start from `baseline-with-tests`, but the slices make
this lab much smoother.

---

## 5. What students should do

1. Identify the missing domain concepts.
2. Introduce model/value objects and interfaces for them.
3. Move duplicated rule logic out of services/endpoints into those concepts (one home each).
4. Make entry points thin (endpoints → application service → domain).
5. Re-run **Task A** and **Task B**; compare to Lab 2.

---

## 6. Step-by-step instructions

1. **Find missing concepts.** Use Prompt 1 (section 10). Expect candidates such as:
   `CustomerTier`, `OrderType`, `OrderItem`, `Money`/`PricingContext`, `Shipment`,
   `ReturnRequest`, `Invoice`, and (for Task A) `GiftRecipient`.

2. **Name the types.** Introduce real types instead of magic strings. For example replace the
   string `CustomerType` with a `CustomerTier` concept (enum or small class) and centralize the
   discount behaviour there. Keep DTOs only at the API boundary; map them to domain objects.

3. **Give behaviour a home.** Move the discount rule into pricing model code; move the shipping
   rule into shipping model code; move return eligibility into a `ReturnRequest`/policy concept;
   move invoice format selection into an invoice concept. Each rule should now exist **once**.

4. **Thin the entry points.** Endpoints should: validate input, map DTO → domain, call an
   application service, map result → response. No business rules in the handler.

5. **Separate orchestration from domain.** `OrderService.PlaceOrder` becomes a thin coordinator
   that asks domain objects to price/ship/invoice themselves, then persists.

6. **Keep tests green** after each move. When you delete a duplicate, the remaining single
   implementation must still satisfy `PricingTests`, `ShippingTests`, `ReturnTests`,
   `InvoiceTests`. Commit in small steps.

> You do **not** need to eliminate all branching yet — that is Lab 4. Here, focus on naming
> concepts and giving each rule a single home.

---

## 7. The exact task(s) for this lab

The modeling refactor, **plus** the two recurring tasks:

- **Task A — Corporate Gift Order.** With proper modeling, this should be largely an exercise in
  adding a `GiftRecipient` concept and a gift-order variant, not in editing ten files.
- **Task B — Gold free express** (basket > threshold **and** ≥1 non-fragile item). With a single
  shipping rule home, this should be a one-place change plus un-skipping the tests.

> ℹ️ Reminder: `TaskA_GiftOrderTests` are **guided placeholders** — each holds commented-out pseudo-code
> describing the intended arrange/act/assert plus a failing assertion. "Enabling" them means authoring the
> real assertions against your new model, not just removing `Skip`. Task B's two shipping target tests are
> already concrete.

---

## 8. Suggested validation steps

- `dotnet build` / `dotnet test` green after each step.
- Grep for duplicated rules to confirm they are now single-homed, e.g.:
  ```powershell
  rg "0.10m" src/        # discount literal should appear in ONE place now
  rg "150m" src/         # gold threshold should appear in ONE place (ideally config)
  rg "CorporateNet30" src/   # invoice format string in ONE place
  ```
- Endpoints contain no pricing/shipping/return/invoice logic.
- Task A/B re-runs: `git diff --stat` shows changes concentrated and small.

---

## 9. Discussion / reflection questions

1. Which concepts were "hidden" in the baseline? How did naming them change the prompts you
   needed?
2. After centralizing the discount rule, how many places did Task B's pricing-adjacent edits
   touch compared to Lab 2?
3. Did the agent start **reusing** existing concepts instead of inventing new duplicates?
4. How thin did your endpoints get? Could a newcomer understand the flow from the handler alone?
5. What remained awkward? (Likely the `if`/`switch` on type/tier — the target of Lab 4.)

---

## 10. Suggested sample prompts

**Prompt 1 — Identify missing business concepts:**
```
Analyze the Orders, Pricing, Shipping, Returns, and Invoicing slices. Identify business concepts
that are currently represented as magic strings, booleans, or procedural code instead of named
types (for example customer tier, order type, money, pricing context, shipment, return request,
invoice). Propose a concrete set of domain types and value objects, and say which duplicated
rule each new type would absorb. Do not change code yet.
```

**Prompt 2 — Propose modeling/abstraction improvements:**
```
Propose a refactor that (a) introduces the domain types you identified, (b) gives each
duplicated business rule a single home on the appropriate type or policy, and (c) keeps DTOs at
the API boundary only. Keep all xUnit tests green. Give me an incremental plan with the order of
changes and where each rule will live afterward.
```

**Prompt 3 — Move orchestration into thinner application services:**
```
Refactor OrderService.PlaceOrder so it is a thin coordinator: validate, map the request DTO to
domain objects, ask the domain objects to compute pricing, shipping, and invoice details, then
persist. Remove the inlined copies of the pricing/shipping/invoice rules from this method and
from the endpoint handlers. Keep the suite green and report the files changed.
```

**Prompt 4 — Re-run the tasks after improved modeling:**
```
Implement Task A (Corporate Gift Order) using the new domain model. Add a GiftRecipient concept
and a gift-order variant that uses corporate agreement pricing, a distinct invoice format, and a
distinct return policy, persists, and appears in /reports/summary. Enable the skipped
TaskA_GiftOrderTests and make them pass without breaking other tests. List the files changed.
```

---

## 11. Expected observation about how the AI behaves

- The agent now edits **by concept**: "update `CustomerTier` discount" rather than searching for
  a literal `0.10m` in five files.
- Prompts get **shorter** and the agent asks fewer clarifying questions — the names carry intent.
- Task B becomes close to a single-file change plus test enablement.
- Task A still requires several types, but they are **additive** (new `GiftRecipient`, new order
  variant) rather than invasive edits across unrelated code.

---

## 12. How to evaluate the result

Fill the worksheet for the Lab 3 re-runs of Task A and Task B.

A "good" Lab 3 result:

- Each business rule exists in exactly **one** place (verify with `rg`).
- Endpoints and `PlaceOrder` contain no business rules.
- Task A/B touch fewer files than in Lab 2, with no regressions.
- Design-quality and readability scores rise on the rubric.

---

## 13. What should improve compared to the previous stage (Lab 2)

Compared to Lab 2 numbers, expect:

- **Fewer duplicated edits** (ideally one edit per rule).
- **Shorter prompts** and fewer agent turns for Task B.
- **Higher design-quality and readability** rubric scores.
- Task A reframed from "edit everywhere" to "add a modeled variant."

Commit the final state on `after-lab-3`; you will branch `after-lab-4` from here.
