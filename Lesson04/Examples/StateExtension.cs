using Models.Employees;
using Models.Music;
using PlayGround.Extensions;
using PlayGround.Generics;
using Seido.Utilities.SeedGenerator;
using System.Collections.Immutable;

namespace Playground.Lesson04;

/// <summary>
/// StateExtension Examples demonstrating stateful computations with Employee and MusicGroup data.
/// The State monad manages both state transformation and value computation in a single operation.
/// State is now represented as tuples or records instead of primitives, converted to strings at presentation.
/// </summary>
public static class StateExtensionExamples
{
    // Record types for structured state representation
    public record ComputationState(int Step, int Multiplier, DateTime Timestamp);
    public record CounterState(int Current, int Previous, int Operations);
    
    public static void RunExamples()
    {
        Console.WriteLine("\n=== State Extension Method Examples (with structured state) ===\n");
        
        // Simple state example
         var result = 10.ToState(10)           // s=10, x=10
        .Tap(state => 
            Console.WriteLine($"Initial: State={state.CurrentState}, Value={state.CurrentValue}"))
        .Bind((s, x) => (s * x).WithState(s))  // s=10, x=100
        .Tap(state => 
            Console.WriteLine($"After first Bind: State={state.CurrentState}, Value={state.CurrentValue}"))
        .Bind((s, x) => (x - s).WithState(s))  // s=10, x = 90
        .Tap(state => 
            Console.WriteLine($"After second Bind: State={state.CurrentState}, Value={state.CurrentValue}"))
        .UpdateState(s => s - 5)               // s=5, x = 90
        .Tap(state => 
            Console.WriteLine($"After Update: State={state.CurrentState}, Value={state.CurrentValue}"))
        .Bind((s, x) => (x / 5).WithState(s))  // s=5, x = 18
        .Tap(state => 
            Console.WriteLine($"Final: State={state.CurrentState}, Value={state.CurrentValue}"));

        Console.WriteLine();

        // Simple tuple-based state example
        var tupleResult = (count: 0, total: 0)
            .ToState("Starting")
            .Tap(state => Console.WriteLine($"Initial: {state}"))
            .Bind((s, msg) => $"{msg} -> Added 5".WithState((s.count + 1, s.total + 5)))
            .Tap(state => Console.WriteLine($"After add: {state}"))
            .Bind((s, msg) => $"{msg} -> Added 3".WithState((s.count + 1, s.total + 3)))
            .Tap(state => Console.WriteLine($"Final: {state}"));
        
        EmployeeStateExamples();
        MusicGroupStateExamples();

        Console.WriteLine("\n=== End of State Extension Method Examples ===\n");
    }
    
    #region Employee State Examples
    
    // Record types for employee-related state management
    public record BudgetState(decimal Amount, int ProcessedCount, DateTime LastUpdate);
    public record CapacityState(int MaxCapacity, int Current, int Available);
    public record CoverageState(int RequiredHours, int AssignedHours, List<string> Schedule);
    public record ComplianceState(int MaxViolations, int CurrentViolations, List<string> Issues);
    
    static void EmployeeStateExamples()
    {
        Console.WriteLine("--- Employee State Examples ---");
        
        var seeder = new SeedGenerator();
        var employees = seeder.ItemsToList<Employee>(50).ToImmutableList();
        
        Console.WriteLine($"Working with {employees.Count} employees");
        
        // Example 1: Payroll processing with structured budget state tracking
        var initialBudget = new BudgetState(2_000_000m, 0, DateTime.Now);
        var payrollState = initialBudget
            .ToState(employees)
            .Bind((budget, emps) => {
                var totalSalaries = emps.Sum(e => GetBaseSalary(e.Role));
                var updatedBudget = budget with { 
                    Amount = budget.Amount - totalSalaries,
                    ProcessedCount = emps.Count,
                    LastUpdate = DateTime.Now
                };
                var payrollSummary = $"Processed {emps.Count} employees, Total: ${totalSalaries:N0}";
                return payrollSummary.WithState(updatedBudget);
            })
            .UpdateState(budget => budget with { Amount = Math.Max(0, budget.Amount) }); // Ensure budget doesn't go negative
            
        Console.WriteLine($"Payroll Processing: {payrollState.CurrentValue}");
        Console.WriteLine($"Budget State: {BudgetStateToString(payrollState.CurrentState)}\n");
        
        // Example 2: Employee onboarding with capacity tracking
        var initialCapacity = new CapacityState(200, employees.Count, 200 - employees.Count);
        var onboardingState = initialCapacity
            .ToState(employees)
            .Bind((capacity, emps) => {
                var newHires = emps.Where(e => DateTime.Now.Year - e.HireDate.Year < 1).Count();
                var canHire = Math.Min(capacity.Available, 25); // Plan to hire up to 25 more
                
                var updatedCapacity = capacity with { 
                    Available = capacity.Available - canHire,
                    Current = capacity.Current + canHire
                };
                
                var onboardingPlan = $"Current: {capacity.Current}, New hires this year: {newHires}, " +
                                   $"Can hire: {canHire} more";
                return onboardingPlan.WithState(updatedCapacity);
            });
            
        Console.WriteLine($"Onboarding Planning: {onboardingState.CurrentValue}");
        Console.WriteLine($"Capacity State: {CapacityStateToString(onboardingState.CurrentState)}\n");
                
        // Example 3: Credit card policy compliance with violation tracking
        var initialCompliance = new ComplianceState(5, 0, new List<string>());
        var complianceState = initialCompliance
            .ToState(employees)
            .Bind((compliance, emps) => {
                var highRiskEmployees = emps.Where(e => e.CreditCards.Count > 3).ToList();
                var newViolations = Math.Min(compliance.MaxViolations - compliance.CurrentViolations, highRiskEmployees.Count);
                var issues = highRiskEmployees.Take(newViolations).Select(e => $"{e.FirstName} {e.LastName}").ToList();
                
                var updatedCompliance = compliance with { 
                    CurrentViolations = compliance.CurrentViolations + newViolations,
                    Issues = compliance.Issues.Concat(issues).ToList()
                };
                
                var complianceReport = $"High-risk employees: {highRiskEmployees.Count}, " +
                                     $"Policy violations: {newViolations}";
                
                return complianceReport.WithState(updatedCompliance);
            });
            
        Console.WriteLine($"Compliance Check: {complianceState.CurrentValue}");
        Console.WriteLine($"Compliance State: {ComplianceStateToString(complianceState.CurrentState)}\n");
                
        // Example 4: Processing employees sequentially with accumulating state
        SequentialEmployeeProcessing(employees);
    }
    
    #region Sequential Processing Examples
    
    // Record types for sequential processing state
    public record HiringMetrics(int TotalHired, decimal CurrentBudget, List<string> HiredEmployees);
    
    static void SequentialEmployeeProcessing(ImmutableList<Employee> employees)
    {
        Console.WriteLine("--- Sequential Employee Processing ---");
        
        // Simple hiring decisions based on budget
        var initialMetrics = new HiringMetrics(0, 800_000m, new List<string>());
        
        var finalState = employees.Take(8).Aggregate(
            initialMetrics.ToState("Starting hiring process"),
            (currentState, employee) => currentState.Bind((metrics, report) => {
                var salary = GetBaseSalary(employee.Role);
                var hiringCost = salary + (salary * 0.15m); // 15% benefits overhead
                
                if (metrics.CurrentBudget >= hiringCost)
                {
                    var newMetrics = metrics with { 
                        TotalHired = metrics.TotalHired + 1, 
                        CurrentBudget = metrics.CurrentBudget - hiringCost,
                        HiredEmployees = metrics.HiredEmployees.Append($"{employee.FirstName} {employee.LastName}").ToList()
                    };
                    
                    var newReport = $"{report}\n  ✓ Hired {employee.FirstName} {employee.LastName} - Cost: ${hiringCost:N0}";
                    return newReport.WithState(newMetrics);
                }
                else
                {
                    var newReport = $"{report}\n  ✗ Rejected {employee.FirstName} {employee.LastName} - Budget insufficient";
                    return newReport.WithState(metrics);
                }
            })
        );
        
        Console.WriteLine($"{finalState.CurrentValue}");
        Console.WriteLine($"\nSimple Hiring State: {HiringMetricsToString(finalState.CurrentState)}\n");
    }
    #endregion
    
    #endregion
    
    #region MusicGroup State Examples
    
    // Record types for music-related state management
    public record LabelBudgetState(decimal Budget, decimal Revenue, decimal Reserve, int GroupsProcessed);
    public record StageTimeState(int TotalMinutes, int UsedMinutes, List<string> Lineup);
    public record PlaylistDiversityState(int Target, int CurrentScore, Dictionary<string, int> GenreBalance);
    public record TourCapacityState(int MaxCapacity, int UsedCapacity, List<string> Bookings);
    public record InvestmentMetrics(int GroupsSigned, decimal BudgetRemaining, List<string> SignedGroups);
    
    static void MusicGroupStateExamples()
    {
        Console.WriteLine("--- MusicGroup State Examples ---");
        
        var seeder = new SeedGenerator();
        var musicGroups = seeder.ItemsToList<MusicGroup>(30).ToImmutableList();
        
        Console.WriteLine($"Working with {musicGroups.Count} music groups");
        
        // Example 1: Record label budget allocation with structured revenue tracking
        var initialLabelState = new LabelBudgetState(5_000_000m, 0m, 0m, 0);
        var budgetState = initialLabelState
            .ToState(musicGroups)
            .Bind((labelState, groups) => {
                var totalRevenue = groups.Sum(g => g.Albums.Sum(a => a.CopiesSold * 15.99m));
                var investmentPerGroup = labelState.Budget / Math.Max(1, groups.Count);
                var roi = totalRevenue / Math.Max(1, labelState.Budget - (labelState.Budget * 0.2m)); // 20% overhead
                
                var updatedLabelState = labelState with { 
                    Revenue = totalRevenue,
                    Reserve = labelState.Budget * 0.2m,
                    GroupsProcessed = groups.Count
                };
                
                var allocation = $"Allocated ${investmentPerGroup:N0} per group, " +
                               $"Generated ${totalRevenue:N0} revenue, ROI: {roi:P1}";
                
                return allocation.WithState(updatedLabelState);
            });
            
        Console.WriteLine($"Label Budget: {budgetState.CurrentValue}");
        Console.WriteLine($"Label State: {LabelBudgetStateToString(budgetState.CurrentState)}\n");
        
        // Example 2: Festival lineup curation with structured stage time allocation
        var initialStageState = new StageTimeState(480, 0, new List<string>()); // 8 hours in minutes
        var lineupState = initialStageState
            .ToState(musicGroups.OrderByDescending(g => g.Albums.Sum(a => a.CopiesSold)).Take(12).ToList())
            .Bind((stageState, topGroups) => {
                var timePerGroup = (stageState.TotalMinutes - stageState.UsedMinutes) / Math.Max(1, topGroups.Count);
                var lineup = topGroups.Select((group, index) => {
                    var slotTime = index < 3 ? timePerGroup + 15 : timePerGroup; // Headliners get extra time
                    var sales = group.Albums.Sum(a => a.CopiesSold);
                    var lineupEntry = $"{group.Name} ({group.Genre}): {slotTime}min, {sales:N0} sales";
                    return new {
                        Group = group,
                        TimeSlot = slotTime,
                        Description = lineupEntry
                    };
                }).ToList();
                
                var usedTime = lineup.Sum(l => l.TimeSlot);
                var lineupList = lineup.Select(l => l.Description).ToList();
                
                var updatedStageState = stageState with { 
                    UsedMinutes = stageState.UsedMinutes + usedTime,
                    Lineup = stageState.Lineup.Concat(lineupList).ToList()
                };
                
                var lineupDetails = $"Festival Lineup ({lineup.Count} groups):\n  " + 
                                  string.Join("\n  ", lineup.Select(l => l.Description));
                
                return lineupDetails.WithState(updatedStageState);
            });
            
        Console.WriteLine($"{lineupState.CurrentValue}");
        Console.WriteLine($"Stage State: {StageTimeStateToString(lineupState.CurrentState)}\n");
        
        // Example 3: Sequential music group signing with investment tracking
        SequentialMusicGroupProcessing(musicGroups);
    }
    
    static void SequentialMusicGroupProcessing(ImmutableList<MusicGroup> musicGroups)
    {
        Console.WriteLine("--- Sequential Music Group Processing ---");
        
        // Simple record label signing decisions based on budget
        var initialMetrics = new InvestmentMetrics(0, 500_000m, new List<string>());
        
        var finalState = musicGroups.Take(8).Aggregate(
            initialMetrics.ToState("Starting group evaluations"),
            (currentState, group) => currentState.Bind((metrics, report) => {
                var totalSales = group.Albums.Sum(a => a.CopiesSold);
                var signingCost = totalSales > 100_000 ? 80_000m : 40_000m;
                
                if (metrics.BudgetRemaining >= signingCost)
                {
                    var newMetrics = metrics with {
                        GroupsSigned = metrics.GroupsSigned + 1,
                        BudgetRemaining = metrics.BudgetRemaining - signingCost,
                        SignedGroups = metrics.SignedGroups.Append(group.Name).ToList()
                    };
                    
                    var newReport = $"{report}\n  ✓ Signed {group.Name} - Cost: ${signingCost:N0}";
                    return newReport.WithState(newMetrics);
                }
                else
                {
                    var newReport = $"{report}\n  ✗ Passed on {group.Name} - Budget insufficient";
                    return newReport.WithState(metrics);
                }
            })
        );
        
        Console.WriteLine($"{finalState.CurrentValue}");
        Console.WriteLine($"\nSimple Investment State: {InvestmentMetricsToString(finalState.CurrentState)}\n");
    }
    
    #endregion
    
    #region Helper Methods
    
    // Helper methods for converting structured state to string representation at presentation time
    private static string BudgetStateToString(BudgetState state) =>
        $"Amount=${state.Amount:N0}, Processed={state.ProcessedCount}, Updated={state.LastUpdate:HH:mm:ss}";
    
    private static string CapacityStateToString(CapacityState state) =>
        $"Max={state.MaxCapacity}, Current={state.Current}, Available={state.Available}";
        
    private static string ComplianceStateToString(ComplianceState state) =>
        $"Violations={state.CurrentViolations}/{state.MaxViolations}, Issues=[{string.Join(", ", state.Issues.Take(2))}]";
    
    private static string HiringMetricsToString(HiringMetrics state) =>
        $"Hired={state.TotalHired}, Budget=${state.CurrentBudget:N0}, Employees=[{string.Join(", ", state.HiredEmployees.Take(3))}]";
    
    private static string LabelBudgetStateToString(LabelBudgetState state) =>
        $"Budget=${state.Budget:N0}, Revenue=${state.Revenue:N0}, Reserve=${state.Reserve:N0}, Groups={state.GroupsProcessed}";
    
    private static string StageTimeStateToString(StageTimeState state) =>
        $"Total={state.TotalMinutes}min, Used={state.UsedMinutes}min, Remaining={state.TotalMinutes - state.UsedMinutes}min, Acts={state.Lineup.Count}";
            
    private static string InvestmentMetricsToString(InvestmentMetrics state) =>
        $"Signed={state.GroupsSigned}, Budget=${state.BudgetRemaining:N0}, Groups=[{string.Join(", ", state.SignedGroups.Take(3))}]";

    // Original helper methods for salary calculations
    private static decimal GetBaseSalary(WorkRole role) => role switch
    {
        WorkRole.Management => 120_000m,
        WorkRole.Veterinarian => 95_000m,
        WorkRole.ProgramCoordinator => 65_000m,
        WorkRole.AnimalCare => 45_000m,
        WorkRole.Maintenance => 50_000m,
        _ => 40_000m
    };    
    #endregion
}