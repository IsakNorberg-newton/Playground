using Models.Music;
using Newtonsoft.Json;
using PlayGround.Generics;

namespace PlayGround.Extensions;

public static partial class FunctionalExtensions
{
    public static Maybe<T> ToMaybe<T>(this T @this) => new Something<T>(@this);

    // The Bind operation is crucial for chaining operations that might fail, ensuring proper error propagation and null safety.
    public static Maybe<TOut> Bind<TIn, TOut>(
        this Maybe<TIn> @this,
        Func<TIn, TOut> f)
    {
        try
        {
            Maybe<TOut> updatedValue = @this switch
            {
                Something<TIn> s when !EqualityComparer<TIn>.Default.Equals(s.Value, default) =>
                    new Something<TOut>(f(s.Value)),
                Something<TIn> s when s.GetType().GetGenericArguments()[0].IsPrimitive =>
                    new Something<TOut>(f(s.Value)),

                Something<TIn> _ => new Nothing<TOut>(),
                Nothing<TIn> _ => new Nothing<TOut>(),
                
                Error<TIn> e => new Error<TOut>(e.CapturedError),
                _ => new Error<TOut>(new Exception("Unknown state: " + @this.GetType()))
            };
            return updatedValue;
        }
        catch (Exception e)
        {
            return new Error<TOut>(e);
        }
    }

    // The Tap operation allows performing side effects (logging, debugging, etc.) without modifying the value
    public static Maybe<T> Tap<T>(
        this Maybe<T> @this,
        Action<T> action)
    {
        try
        {
            switch (@this)
            {
                case Something<T> s when !EqualityComparer<T>.Default.Equals(s.Value, default):
                    action(s.Value);
                    break;
                case Something<T> s when s.GetType().GetGenericArguments()[0].IsPrimitive:
                    action(s.Value);
                    break;
                case Nothing<T>:
                case Error<T>:
                    // Do nothing for Nothing or Error cases
                    break;
            }
            return @this;
        }
        catch (Exception e)
        {
            return new Error<T>(e);
        }
    }

    
}