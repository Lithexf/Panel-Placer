using System;
using Google.OrTools.LinearSolver;

namespace panel_placer
{
    public class PanelPlacement
    {
        public static void Solution()
        {
            
            int N = 5; 
            int M = 5;
            double w = 1.0; 
            double h = 1.0; 
            int P = 9;

           
            Solver solver = Solver.CreateSolver("SCIP");
            if (solver is null)
            {
                Console.WriteLine("Nem sikerült létrehozni a solver-t.");
                return;
            }

            
            Variable[,] x = new Variable[N, M];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    x[i, j] = solver.MakeIntVar(0, 1, $"x_{i}_{j}");
                }
            }

            
            for (int i = 0; i < N; i++)
            {
                for (int j = 1; j < M; j++)
                {
                    solver.Add(x[i, j] - x[i, j - 1] <= 0);
                }
            }

            for (int j = 0; j < M; j++)
            {
                for (int i = 1; i < N; i++)
                {
                    solver.Add(x[i, j] - x[i - 1, j] <= 0);
                }
            }

            Constraint panelConstraint = solver.MakeConstraint(P, P, "panelConstraint");
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    panelConstraint.SetCoefficient(x[i, j], 1);
                }
            }

            
            Objective objective = solver.Objective();
            for (int i = 0; i < N; i++)
            {
                objective.SetCoefficient(x[i, 0], w);
            }
            for (int j = 0; j < M; j++)
            {
                objective.SetCoefficient(x[0, j], h);
            }
            objective.SetMinimization();

            
            solver.Solve();

          
            Console.WriteLine("Megoldás mátrix formában:");
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    Console.Write(x[i, j].SolutionValue() + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
