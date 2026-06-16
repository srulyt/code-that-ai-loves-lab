# Metrics Prompt — Extracting AI Effort from a Copilot CLI Session

After each task run, use this prompt to have **GitHub Copilot CLI** summarize how hard the agent
worked. The goal is an honest, side-by-side effort table you paste into the worksheet in
`docs/scoring-rubric.md`.

> ⚠️ **Honesty rule:** the metrics must come from real session state. If a value cannot be
> determined, the summary must say **"unavailable"** — never estimate or invent a number.

---

## How to use

1. Finish a task run (e.g., Task B on Lab 3).
2. In the **same** Copilot CLI session (so the metrics describe that work), paste the prompt
   below.
3. Copy the resulting table into the scoring worksheet for that stage/task.
4. Reset your attempt branch and move on.

If your CLI starts a fresh session per invocation, run the prompt as the **last step** of the
same session that did the work, or point it at the specific session you want to measure.

---

## The prompt (copy/paste into Copilot CLI)

```
Summarize how much effort YOU (the agent) spent on the work in THIS session. Inspect whatever
internal session state, logs, or telemetry you can actually access for the current (or the most
recently completed) session — for example a local session store, transcript, or event log.

Produce a single Markdown table with exactly these rows:

| Metric         | Value |
|----------------|-------|
| Tokens used    |       |
| Turns          |       |
| Files read     |       |
| Files written  |       |
| Total tool calls |     |
| Total wall time |      |

Rules:
- Use ONLY values you can derive from real session state. Do not estimate, guess, or fabricate.
- If a metric is not available to you, write "unavailable" in its Value cell and, in one line
  under the table, say which metrics were unavailable and why (e.g. "tokens not exposed in the
  local session store").
- "Files read" and "files written" should count DISTINCT file paths touched via tools in this
  session. If you can only get a non-distinct count, say so.
- "Total wall time" should be the elapsed time between the session's first and last activity if
  timestamps are available; otherwise "unavailable".
- After the table, add a 1-2 sentence plain-language note on where the data came from (the source
  of truth you inspected) so the numbers are auditable.
- Do not include any other commentary or speculation.
```

---

## Expected output shape

```markdown
| Metric           | Value      |
|------------------|------------|
| Tokens used      | 42,318     |
| Turns            | 6          |
| Files read       | 9          |
| Files written    | 4          |
| Total tool calls | 17         |
| Total wall time  | 4m 12s     |

Unavailable: none.
Source: local Copilot CLI session store for session <id>, events table.
```

…or, when some values cannot be obtained:

```markdown
| Metric           | Value       |
|------------------|-------------|
| Tokens used      | unavailable |
| Turns            | 6           |
| Files read       | 9           |
| Files written    | 4           |
| Total tool calls | 17          |
| Total wall time  | 4m 12s      |

Unavailable: tokens — not exposed by the local session store on this CLI build.
Source: session transcript + tool-call log for the current session.
```

---

## Tips for clean measurement

- **One session per attempt.** Don't mix Task A and Task B work in the same session, or the
  metrics blend together.
- **Capture before reset.** Record the table *before* `git reset --hard` or switching branches.
- **Keep prompts constant** across stages so differences reflect the **code**, not the wording.
- **Trends over absolutes.** Token and time figures vary by model and machine; what matters is
  that they fall from Lab 1 to Lab 4 for the same task.
- **Cross-check change impact** with Git, which is always available regardless of CLI telemetry:
  ```powershell
  git diff --stat        # files touched + insertions/deletions for this attempt
  git status --short     # untracked/added files (e.g., new policy classes in Lab 4)
  ```
  Use these as a reliable fallback for "files written / files touched" if the session store
  cannot provide them.

---

## When token/turn telemetry is unavailable (read this)

On many CLI builds the per-run **tokens / turns / tool-call** counts are **not exposed** by the local
session store. When that happens, the honesty rule applies: mark those cells **"unavailable"** — never
estimate. You can still measure effort with two always-available signals:

- **Wall time** — elapsed between the first and last activity of the run.
- **Change impact (git)** — `git diff --stat` (files touched, insertions/deletions) and the count of
  repeated edits of the same rule. This is environment-independent and is the most trustworthy signal of
  how "AI-friendly" each codebase stage is.

**Graceful scoring path:** if tokens/turns/tool-calls are unavailable, score the AI-effort section on
**wall time alone** and lean on the **change-impact** section for the comparison. Note in the worksheet
that the effort subtotal is partial so totals stay comparable across stages.

> ⚠️ **Shared-session contamination.** If you run several stages (or both tasks) inside **one continuous
> agent session**, later runs benefit from the agent's memory of earlier runs, so raw wall-time/token/turn
> figures **understate** a cold agent's cost and the lab-to-lab *effort* trend is partly learning, not code
> quality. The **change-impact** metrics are immune to this. Prefer one fresh session per attempt; when that
> is not possible, treat change-impact as the primary evidence and annotate the effort numbers accordingly.
