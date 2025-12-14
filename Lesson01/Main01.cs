namespace Playground.Lesson01;
public static class Main01
{
    public static void Entry(string[] args = null)
    {
        System.Console.WriteLine("Hello Lesson 01!");

        //FP Examples
        FpCodeExamples.RunExamples();

        //OOP Examples
        OopCodeExamples.RunExamples();

        //Imperative vs Declarative Example1
        ImperativeVsDeclarative1.RunExamples();

        //Imperative vs Declarative Example2
        ImperativeVsDeclarative2.RunExamples();
    }
}