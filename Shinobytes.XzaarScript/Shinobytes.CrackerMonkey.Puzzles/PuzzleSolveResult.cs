using System.Linq;
using Shinobytes.XzaarScript.Assembly.Models;

namespace Shinobytes.Bytecode.Puzzles
{
    public class PuzzleSolveResult
    {
        public PuzzleSolveResult()
        {
            Results = new Collection<PuzzleResult>();
        }

        public Collection<PuzzleResult> Results { get; }

        public bool Success { get { return Results.All(x => x.Success); } }
    }
}