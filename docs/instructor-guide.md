# Instructor Guide — Code That AI Loves

A practical, ready-to-use guide for running the workshop. It assumes a one-day (≈7-hour)
format with four labs, but the pacing scales down to a half-day "highlights" version.

---

## 1. Overall teaching narrative

The workshop tells **one story with one codebase**:

> The tasks never change. The code gets cleaner. Watch the AI get better.

Everything hangs on that controlled experiment. Because Task A and Task B are fixed, every
improvement in the agent's behaviour is attributable to the **code**, not the prompt or luck.
Keep returning to this framing after every lab: "same task, cleaner code — what changed in how
the AI behaved?"

The four labs map to four levers of code quality:

1. **Tests** — give the agent a definition of done and a feedback loop.
2. **Structure (vertical slices)** — reduce the agent's search surface.
3. **Modeling & abstractions** — let the agent reason by named concepts, not text search.
4. **De-branching (policies/IOC)** — make the axis of variation explicit, so change is additive.

End state: a feature that was "edit ten files and pray" in Lab 1 becomes "add a class and
register it" in Lab 4 — for the **same** business requirement.

---

## 2. How to frame the baseline

Open by demoing the baseline honestly:

- Show `OrderService.PlaceOrder` and `ReportService.BuildSummary` — the two large methods.
- Grep the duplicated rules live so students *feel* the problem:
  ```powershell
  rg "0.10m" src/        # discount rule duplicated
  rg "150m" src/         # gold threshold duplicated
  rg "CorporateNet30" src/   # invoice format duplicated
  ```
- Stress: **this is realistic, not a strawman.** Most production code degrades exactly this way —
  one reasonable change at a time. The baseline compiles and runs; it is just hostile to change.

Set the rule: **nobody "fixes" the baseline early.** The mess is the experimental control.

---

## 3. When students should measure effort

Measure **every time** a task is run, using `docs/metrics-prompt.md` and the worksheet in
`docs/scoring-rubric.md`. The minimum data points:

| Stage | Task A | Task B |
|-------|--------|--------|
| Lab 1 — no tests | ✅ | ✅ |
| Lab 1 — with tests | ✅ | ✅ |
| Lab 2 | ✅ | ✅ |
| Lab 3 | ✅ | ✅ |
| Lab 4 | ✅ | ✅ |

Tell students to capture metrics **immediately** after each run, before they forget or reset.
Have them paste the metrics table into a running notes file or the shared worksheet.

---

## 4. Suggested pacing for a 7-hour workshop

| Time | Segment | Notes |
|------|---------|-------|
| 0:00–0:30 | Intro & framing | The core claim; the controlled experiment; tour the baseline. |
| 0:30–0:45 | Setup check | Everyone runs the API and hits `/reports/summary`. Fix environments now. |
| 0:45–2:00 | **Lab 1** | No-tests vs with-tests for both tasks. This is the emotional anchor. |
| 2:00–2:15 | Break + debrief | Compare no-tests vs with-tests numbers aloud. |
| 2:15–3:30 | **Lab 2** | Vertical slices. Emphasize incremental, test-green moves. |
| 3:30–4:15 | Lunch | — |
| 4:15–5:30 | **Lab 3** | Modeling & abstractions. The biggest "aha" for prompt length. |
| 5:30–5:40 | Break | — |
| 5:40–6:40 | **Lab 4** | Policies/strategies/IOC. Task A becomes "add a class." |
| 6:40–7:00 | Final debrief | Cross-stage comparison; the trend line; takeaways. |

If you are short on time, cut Lab 2 or Lab 4 to a demo and keep Labs 1 and 3 hands-on — they
produce the strongest reactions.

---

## 5. What instructors should point out after each rerun

- **Lab 1 (no tests):** the agent's confident-but-unverifiable summary; the new duplicate rule it
  added; the regression nobody caught. Then with tests: it runs `dotnet test` and iterates to
  green. "Notice it stopped claiming success and started **proving** it."
- **Lab 2:** the agent navigating straight to `Features/Shipping` instead of scanning the repo;
  fewer unrelated files in `git diff --stat`.
- **Lab 3:** prompts got **shorter**; the agent edits "the `CustomerTier` discount" by name; Task
  B is nearly a one-file change.
- **Lab 4:** Task A is mostly **new files** plus one registration line; the `git diff` of existing
  code is tiny. Pull up the metrics trend across all four labs.

---

## 6. Common student mistakes

- **Fixing the baseline early.** Tempting for strong engineers. Stop it — it destroys the control.
- **Hand-holding the agent.** Let it struggle in Lab 1; the struggle is the lesson.
- **Not resetting between attempts.** Use a throwaway branch per attempt and `git reset --hard`.
- **Forgetting to capture metrics** before resetting. Capture first, reset second.
- **Refactoring behaviour, not just structure, in Lab 2.** If a test goes red in a "move," they
  changed behaviour. Undo and redo.
- **Over-abstracting in Lab 4.** Policies only where the business actually varies. No
  `IEverythingFactory`.
- **Changing the prompt wording between stages.** Keep prompts as constant as possible so the
  comparison stays clean.
- **Letting the JSON data drift.** Use `scripts/reset-data.ps1` if the seed gets mutated.

---

## 7. What "good" observations sound like

Encourage students to articulate findings like:

- "Without tests I couldn't tell if returns still worked; with tests the agent caught its own
  regression and fixed it."
- "In Lab 1 Task B touched five files with the same rule; in Lab 3 it touched one."
- "My Lab 3 prompt was one sentence because the concept already had a name."
- "Task A went from editing ten files to adding two classes and one registration."
- "Token and turn counts dropped at every stage even though the task was identical."

Weak observations to push on: "it felt easier" (quantify it), "the code is nicer" (tie it to an
agent metric), "the AI is smarter now" (no — the *code* changed, not the model).

---

## 8. How to debrief the comparison across stages

1. Put the cross-stage table (from `docs/scoring-rubric.md`) on screen with everyone's medians.
2. Plot two simple trend lines: **files modified** and **agent turns** per task, Lab 1 → Lab 4.
   Both should slope down.
3. Plot **design-quality** and **correctness** — they should slope up.
4. Ask: *which lever moved the needle most for your cohort?* (Often tests for safety, modeling
   for effort.)
5. Land the thesis: **AI coding agents are amplifiers of code quality.** Cleaner code is not a
   nicety — it is a direct, measurable productivity multiplier for agentic development.
6. Bridge to their work: "Where is your real codebase on this curve, and which lever is cheapest
   to pull first?"

---

## 9. Facilitation notes & answer key

- **Why are Gold and Corporate both 10%?** Deliberate trap: it makes the duplication look
  harmless until someone changes one and forgets the others. Great discussion seed.
- **The seed order O-5001** has free express at a sub-threshold total — a pre-existing data
  inconsistency. Use it to discuss how messy code hides bad data.
- **Skipped tests** are the definition of done for Task A/B. If a student asks "is the feature
  done?", answer: "are the target tests un-skipped and green?"
- **Expected metric direction**, not absolute values, is what matters. Models and machines vary;
  the *trend* across stages is the result.

Keep the energy on the experiment. The repo is just the apparatus — the insight is the trend.
