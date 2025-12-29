using Models.Employees;
using Seido.Utilities.SeedGenerator;

namespace Playground.Lesson02;

public static class Main02
{
    public static void Entry(string[] args = null)
    {      
        System.Console.WriteLine("Hello Lesson 02!");

        //Records Examples
        Records.RunExamples();

        //ImmutableList Examples
        ImmutableLists.RunExamples();

        //Tuples Examples
        Tuples.RunExamples();

        //Pattern Matching Examples
        PatternMatching.RunExamples();

        //Generics Examples
        Generics.RunExamples();

        //Enumerable Examples
        Enumerables.RunExamples();

        //Enumerable Examples
        EnumerablesImplementation.RunExamples();
        
        //Extension Method Examples
        Extensions.RunExamples();

        //Linq Examples
        LinqOverview.RunExamples();

        //Excercise 02.01
        HomeExcericee02Answers.Entry();
    }
}