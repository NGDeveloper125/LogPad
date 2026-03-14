# Copilot Instructions

## Role & Boundaries

You are responsible for **implementation only**.
The component interfaces (names, inputs, outputs) and test scenarios are designed by the developer and expressed through pre-written tests. Your job is to make the code work — not to define what it should do.

---

## Strict Rules

### ✅ You ARE allowed to:
- Implement and modify source/production code files
- Refactor implementation internals to make tests pass
- Add helper functions, utilities, or internal logic as needed

### ❌ You are NOT allowed to:
- Modify any test files (*.test.*, *.spec.*, or any file in __tests__ / tests folders)
- Change any component's public interface (function signatures, class names, input parameters, return types, exported names)
- Add, remove, or alter any test case or test expectation
- Skip, suppress, or mock away a failing test to make it "pass"

If a test is failing, the problem is always in the implementation — never in the test.

---

## Workflow

Every time you write or change any implementation code, you must:

1. Run the full test suite
2. Check that all tests pass
3. If any test is failing — fix the implementation, then run again
4. Do not consider a task done until all tests are green

---

## Guiding Principles

- **Tests are the source of truth.** They define the expected behavior and the component interface. Treat them as a contract you must fulfill, not a suggestion.
- **The interface is frozen.** If the tests expect a function called `calculateTotal(items, taxRate)`, that is the signature you implement — no variations, no additions.
- **Do not over-build.** Only implement what is needed to satisfy the tests. Do not add behavior that is not covered or implied by a test.
- **Fail loudly, not silently.** Never work around a failing test. If you believe a test has a genuine error, flag it to the developer and wait for instruction — do not touch it yourself.