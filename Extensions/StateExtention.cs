using Models.Employees;
using Models.Music;
using Newtonsoft.Json;
using PlayGround.Generics;
using Seido.Utilities.SeedGenerator;
using System.Collections.Immutable;

namespace PlayGround.Extensions;

public static partial class FunctionalExtensions
{
    public static State<TS, TV> ToState<TS, TV>(this TS @this, TV value) =>
        new(@this, value);

    /// <summary>
    /// Creates a State with the given state, where the extension method is called on the value.
    /// Usage: newValue.WithState(state) - more intuitive when computing new values in Bind operations.
    /// </summary>
    public static State<TS, TV> WithState<TS, TV>(this TV @this, TS state) =>
        new(state, @this);

    public static State<TS, TV> UpdateState<TS, TV>(
            this State<TS, TV> @this,
            Func<TS, TS> f
        ) => new(f(@this.CurrentState), @this.CurrentValue);

    public static State<TS, TVOut> Bind<TS, TVIn, TVOut>(
        this State<TS, TVIn> @this,
        Func<TS, TVIn, State<TS, TVOut>> f)
    {        
        var newState = f(@this.CurrentState, @this.CurrentValue);
        return new State<TS, TVOut>(
            newState.CurrentState,
            newState.CurrentValue);
    }
}

