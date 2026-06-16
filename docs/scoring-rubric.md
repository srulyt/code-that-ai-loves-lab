# Scoring Rubric — Code That AI Loves

Use this rubric to score every task run so the four labs can be compared objectively. Score
**Task A** and **Task B** separately at **every** stage (Lab 1 no-tests, Lab 1 with-tests, Lab 2,
Lab 3, Lab 4).

Each dimension is scored **0–5** unless noted. Higher is better for quality; for effort and
impact, **lower raw numbers are better** — convert them to a 0–5 score using the bands below so
everything points the same direction (5 = best).

---

## A. Output quality (0–5 each)

| Dimension | 0–1 (poor) | 2–3 (mixed) | 4–5 (strong) |
|-----------|------------|-------------|--------------|
| **Correctness** | Doesn't build or wrong behaviour | Builds, partially correct | Fully correct for the task |
| **Regressions** | Breaks existing behaviour | Minor unverified risk | No regressions (tests green) |
| **Readability** | Hard to follow, cryptic names | Understandable with effort | Clear intent, well-named |
| **Design quality** | Adds duplication/branching | Neutral | Reduces duplication; right seams |
| **Edge-case handling** | Ignores edges | Some edges covered | Handles edges (fragile, threshold, gift cards, windows) |

**Quality subtotal:** ___ / 25

---

## B. AI effort (raw values + 0–5 score)

Capture the raw numbers with `docs/metrics-prompt.md`. Then score each with the bands
(tune bands to your model/cohort after the first lab; keep them constant afterward).

| Metric | Raw value | Suggested 5 / 3 / 1 bands | Score (0–5) |
|--------|-----------|---------------------------|-------------|
| **Tokens** | | ≤ baseline×0.5 / ≈baseline / ≥baseline×1.5 | |
| **Turns** | | ≤3 / 4–7 / ≥8 | |
| **Files read** | | ≤4 / 5–10 / ≥11 | |
| **Files written** | | ≤3 / 4–7 / ≥8 | |
| **Tool calls** | | ≤8 / 9–20 / ≥21 | |
| **Wall time** | | ≤baseline×0.5 / ≈baseline / ≥baseline×1.5 | |

> Use your **Lab 1 with-tests** run as the "baseline" reference for token/time bands. The whole
> point is to watch these fall in Labs 2–4.

> ⚠️ **If tokens/turns/tool-calls are unavailable** (common — many CLI builds don't expose them), mark
> those rows "unavailable" per the honesty rule and score effort on **wall time** plus the **change-impact**
> section below. Record the effort subtotal as partial (e.g. "/10 on wall time only") so the run totals
> stay comparable. See `docs/metrics-prompt.md` → "When token/turn telemetry is unavailable."

**Effort subtotal:** ___ / 30

---

## C. Change impact (raw values + 0–5 score)

| Metric | Raw value | Suggested 5 / 3 / 1 bands | Score (0–5) |
|--------|-----------|---------------------------|-------------|
| **Files touched** (`git diff --stat`) | | ≤3 / 4–8 / ≥9 | |
| **Unrelated files touched** | | 0 / 1–2 / ≥3 | |
| **Manual cleanup required** (your edits after the agent) | | none / minor / major | |
| **Repeated edits across layers** (same rule edited N times) | | 1 / 2 / ≥3 | |

**Impact subtotal:** ___ / 20

---

## Total

**Run total:** Quality (___/25) + Effort (___/30) + Impact (___/20) = **___ / 75**

Higher totals indicate the agent performed better on identical work — i.e., the codebase was
friendlier to agentic change.

---

## Per-run worksheet (copy one per task per stage)

```
Stage:            (Lab 1 no-tests | Lab 1 with-tests | Lab 2 | Lab 3 | Lab 4)
Task:             (A - Corporate Gift Order | B - Gold free express)
Model / agent:    
Date:             

--- Output quality (0-5) ---
Correctness:        __
Regressions:        __
Readability:        __
Design quality:     __
Edge-case handling: __
Quality subtotal:   __ / 25

--- AI effort (raw -> score) ---
Tokens:       raw ______  score __
Turns:        raw ______  score __
Files read:   raw ______  score __
Files written:raw ______  score __
Tool calls:   raw ______  score __
Wall time:    raw ______  score __
Effort subtotal: __ / 30

--- Change impact (raw -> score) ---
Files touched:            raw ______  score __
Unrelated files touched:  raw ______  score __
Manual cleanup:           (none/minor/major)  score __
Repeated edits/layers:    raw ______  score __
Impact subtotal: __ / 20

RUN TOTAL: __ / 75

Notes / observations:
```

---

## Cross-stage comparison (fill after all labs)

Record the **run total** (and, optionally, key raw metrics) for each cell. The trend is the
deliverable: quality up, effort and impact down.

### Task A — Corporate Gift Order

| Stage | Files touched | Turns | Tokens | Regressions? | Run total (/75) |
|-------|---------------|-------|--------|--------------|-----------------|
| Lab 1 — no tests | | | | | |
| Lab 1 — with tests | | | | | |
| Lab 2 | | | | | |
| Lab 3 | | | | | |
| Lab 4 | | | | | |

### Task B — Gold free express

| Stage | Files touched | Turns | Tokens | Regressions? | Run total (/75) |
|-------|---------------|-------|--------|--------------|-----------------|
| Lab 1 — no tests | | | | | |
| Lab 1 — with tests | | | | | |
| Lab 2 | | | | | |
| Lab 3 | | | | | |
| Lab 4 | | | | | |

**Expected trend:** "Files touched," "Turns," and "Tokens" decline from Lab 1 to Lab 4; "Run
total" rises. If your data doesn't show this, debrief *why* — it is often a sign the refactor
didn't actually improve locality or modeling, which is itself a valuable lesson.
