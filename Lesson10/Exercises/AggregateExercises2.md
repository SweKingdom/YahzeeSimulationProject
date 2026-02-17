# Aggregate Exercises 2 - Mutable State Threading

## Overview
These exercises focus on threading **mutable state** (ImmutableLists that change as we iterate) through nested aggregates. This is a more advanced pattern that simulates real-world scenarios where data is consumed and transformed across nested operations.

## Key Concepts

### Immutable State Modification
While `ImmutableList` is immutable, we can create **new versions** with modifications:
- `list.RemoveAt(index)` - returns a new list without the element
- `list.Add(item)` - returns a new list with the item added
- `list.SetItem(index, newValue)` - returns a new list with the item replaced

### State Threading Patterns
Building on the previous exercises, we now:
1. **Thread data downward** - pass collections from outer to inner loops
2. **Modify data** - transform the collections in the innermost loop
3. **Thread modified data upward** - return the transformed collections to the final result

### Multi-State Management
Exercise 2 introduces managing **multiple independent state objects** simultaneously:
- Numbers list (being consumed)
- Players list (being updated)
- Counter (for distribution logic)
- Results accumulator (for tracking iterations)

## Exercise 1: Consuming a Shared Resource

**Scenario**: Imagine drawing cards from a deck through nested game rounds.

**Goal**: 
- Start with 27 numbers (representing a deck)
- Pass the deck down to the innermost loop (Loop3)
- In each Loop3 iteration, remove the last number (draw a card)
- Carry the modified deck back up through all loops
- End with an empty deck

**Key Learning**:
- How to maintain shared mutable state across nested iterations
- Understanding that each inner loop sees the state modified by previous iterations
- State flows: Down (read-only) → Modified in Loop3 → Up (updated)

**Verification**:
- Starting with 27 numbers
- 3 × 3 × 3 = 27 iterations of Loop3
- Each iteration removes 1 number
- Final result: empty list

## Exercise 2: Distributing Resources

**Scenario**: Imagine dealing cards from a deck to multiple players.

**Goal**:
- Start with 27 numbers (deck) and 3 players (empty hands)
- Pass both the deck and players down to Loop3
- In each Loop3 iteration:
  - Remove the last number from the deck
  - Give it to a player using round-robin distribution
- Carry both modified structures back to the final result
- End with an empty deck and players holding cards

**Key Learning**:
- Threading multiple mutable states simultaneously
- Using a counter for distribution logic
- Updating nested immutable structures (players with their number lists)
- Coordinating changes across multiple data structures

**Verification**:
- Starting with 27 numbers and 3 players with empty lists
- Each Loop3 iteration: deck shrinks by 1, one player gains 1 number
- Final result: 
  - Empty deck
  - Each player has 9 numbers (27 ÷ 3)
  - Numbers distributed in reverse order (27, 26, 25, ...)

## Real-World Applications

### Card Games
```csharp
// Deal cards from a deck to players
var (emptyDeck, playersWithCards) = DealCards(deck, players, cardsPerPlayer);
```

### Resource Allocation
```csharp
// Distribute tasks from a queue to workers
var (remainingTasks, workersWithTasks) = AllocateTasks(taskQueue, workers);
```

### Inventory Management
```csharp
// Fulfill orders from inventory
var (updatedInventory, fulfilledOrders) = ProcessOrders(inventory, orders);
```

### Data Processing Pipelines
```csharp
// Process batches of data, moving items from input to output queues
var (inputQueue, outputQueue) = ProcessBatches(inputData, batchSize);
```

## Implementation Patterns

### Pattern 1: Single Mutable State
```csharp
(numbers, results) =>
{
    // Modify numbers
    var updatedNumbers = numbers.RemoveAt(numbers.Count - 1);
    
    // Return modified state
    return (updatedNumbers, results.Add(item));
}
```

### Pattern 2: Multiple Mutable States
```csharp
(numbers, players, counter, results) =>
{
    // Modify multiple states
    var updatedNumbers = numbers.RemoveAt(...);
    var updatedPlayers = players.SetItem(...);
    var updatedCounter = counter + 1;
    
    // Return all updated states
    return (updatedNumbers, updatedPlayers, updatedCounter, results.Add(item));
}
```

### Pattern 3: Conditional Modification
```csharp
if (numbers.Count > 0)
{
    // Safe to modify
    var number = numbers[numbers.Count - 1];
    var updated = numbers.RemoveAt(numbers.Count - 1);
    return (updated, ...);
}
else
{
    // No modification needed
    return (numbers, ...);
}
```

## Tips for Success

1. **Trace the flow**: Numbers start at 27, after each Loop3 iteration count decreases
2. **State structure**: `(mutableData1, mutableData2, ..., accumulator)` - accumulator usually last
3. **Immutable updates**: Always create new instances, never mutate
4. **Index safety**: Check `.Count > 0` before accessing `[Count - 1]`
5. **Distribution logic**: Use `counter % playerCount` for round-robin
6. **Tuple items**: Remember `.Item1`, `.Item2`, etc. - track what each position means

## Running the Exercises

```csharp
// In Main10.cs or Program.cs
Playground.Lesson10.Exercises.AggregateExercises2.RunExercises();

// Check your work
Playground.Lesson10.Exercises.AggregateExercises2Answers.RunAnswers();
```

## Expected Outputs

### Exercise 1
```
Initial numbers: [1, 2, 3, ..., 27]
...
Loop3 iteration: 1, Removed: 27, Remaining: 26
Loop3 iteration: 2, Removed: 26, Remaining: 25
...
Final numbers remaining: []
Numbers removed: 27
```

### Exercise 2
```
Initial numbers: [1, 2, 3, ..., 27]
Initial players: Alice: [], Bob: [], Charlie: []
...
Loop3 iteration: 1 - Alice receives 27
Loop3 iteration: 2 - Bob receives 26
Loop3 iteration: 3 - Charlie receives 25
...
Final player states:
  Alice: [27, 24, 21, 18, 15, 12, 9, 6, 3] (Count: 9)
  Bob: [26, 23, 20, 17, 14, 11, 8, 5, 2] (Count: 9)
  Charlie: [25, 22, 19, 16, 13, 10, 7, 4, 1] (Count: 9)
```
