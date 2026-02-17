# Aggregate Exercises 1 - State Threading in Nested Loops

## Overview
These exercises focus on understanding how to thread state across nested `Aggregate` operations, which is a crucial concept in functional programming.

## Key Concepts

### State Threading
When working with nested aggregates, you often need to:
1. **Pass state down** from outer loops to inner loops
2. **Pass state up** from inner loops to outer loops
3. **Transform state** at different levels

### The Challenge
The original `Example_NestedAggregate` keeps state local to each loop level:
- Loop1 has state: `("Loop1", ImmutableList<string>.Empty)`
- Loop2 has state: `("Loop2", ImmutableList<string>.Empty)`
- Loop3 has state: `("Loop3", ImmutableList<string>.Empty)`

Each loop only passes its **accumulated list** to its parent, but the **identifier strings** stay isolated.

## Exercise 1: Threading State Downward
**Goal**: Access "Loop1" identifier inside the Loop3 action.

**Key Learning**: 
- Expand state tuples to include data from outer scopes
- Initial state of inner loops can include data from outer loops
- State structure: `(outerData, currentData, accumulator)`

**Hint**: Change Loop2's initial state from a 2-tuple to a 3-tuple that includes Loop1's identifier.

## Exercise 2: Threading State Upward
**Goal**: Make "Loop3" identifier appear in `resultLoop1.Item1`.

**Key Learning**:
- Return values from inner loops can replace outer loop's data
- You choose what to propagate upward when building return states
- Each level chooses which part of the inner result to extract

**Hint**: When building `newStateLoop2`, use `resultLoop3.Item1` instead of `stateLoop2.Item1`.

## Exercise 3: Transform and Thread Through All Levels
**Goal**: Start with "Loop1", modify it to "Loop3 Modified" in the innermost loop, and have this modified value end up in the final result.

**Key Learning**:
- Combines downward threading (Exercise 1) and upward threading (Exercise 2)
- Demonstrates data transformation at a specific level
- Shows complete control over state flow

**Steps**:
1. Thread "Loop1" down to Loop3 (like Exercise 1)
2. In Loop3, modify the value to "Loop3 Modified"
3. Thread the modified value back up to resultLoop1 (like Exercise 2)

## Real-World Applications

These patterns are essential for:
- **Parsing nested structures** (XML, JSON) where context from outer levels is needed
- **State machines** that need to track hierarchical state
- **Game loops** where outer game state affects inner AI decisions
- **Report generation** where summary data flows from detailed calculations

## Tips for Success

1. **Draw the data flow**: Sketch arrows showing where data needs to go
2. **Mind the tuple positions**: When you add items to tuples, indexes shift
3. **Trace one iteration**: Walk through manually with i=1, j=1, k=1
4. **Type matching**: Ensure initial state type matches return state type

## Running the Exercises

```csharp
// In Main10.cs or Program.cs
Playground.Lesson10.Exercises.AggregateExercises.RunExercises();

// Check your work against
Playground.Lesson10.Exercises.AggregateExercisesAnswers.RunAnswers();
```
