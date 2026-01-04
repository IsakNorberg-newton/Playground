using Models.Music;
using Newtonsoft.Json;

namespace PlayGround.Generics;

// State is a Monad because it provides:
// 1. A type constructor: State<S, A> that represents a stateful computation
// 2. A return/unit operation: creates State from a value, leaving state unchanged
// 3. A bind operation: chains stateful computations, passing the updated state along
// This enables modeling computations that carry state through a sequence of operations

public class State<TS, TV>
{
    public TS CurrentState { get; init; }
    public TV CurrentValue { get; init; }
    public State(TS s, TV v)
    {
        CurrentValue = v;
        CurrentState = s;
    }
}
