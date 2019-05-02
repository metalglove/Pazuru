using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pazuru.Domain;
using System.Text;

namespace Pazuru.Hitori.Tests
{
    [TestClass]
    public class HitoriPuzzleTests
    {
        private HitoriPuzzle HitoriPuzzle { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            byte[] grid = Encoding.Default.GetBytes(
                "2G7G1G5G4G7G2G5G9G" +
                "2G8G5G6G3G4G9G5G3G" +
                "4G4G8G2G9G5G5G7G2G" +
                "6G9G9G7G7G1G3G1G5G" +
                "9G9G5G8G6G7G7G3G2G" +
                "5G5G9G8G8G9G6G4G4G" +
                "5G4G2G1G7G9G1G2G6G" +
                "8G7G2G4G3G5G7G1G4G" +
                "8G2G6G4G5G4G1G2G8G");
            PuzzleState puzzleState = new PuzzleState(grid);
            HitoriPuzzle = new HitoriPuzzle(puzzleState, 9);
        }

        [TestMethod]
        public void GetChar_Should_Return_N_With_Row_2_And_Column_2()
        {
            // Arrange
            HitoriMoveColorKey expectedCharAt2_2 = HitoriMoveColorKey.None;

            // Act
            HitoriMoveColorKey charAt2_2 = HitoriPuzzle.GetColorKey(2, 2);

            // Asserts
            Assert.AreEqual(expectedCharAt2_2, charAt2_2);
        }

        [TestMethod]
        public void Solve_Hitori()
        {
            HitoriGenerator hitoriGenerator = new HitoriGenerator();
            HitoriPuzzle hitoriPuzzle = hitoriGenerator.Generate();
            HitoriSolver hitoriSolver = new HitoriSolver(hitoriPuzzle);
            bool isSolved = hitoriSolver.Solve();
            Assert.IsTrue(isSolved);
        }

        //[TestMethod]
        //public void SetChar_Should_Set_G_To_B_With_Row_2_And_Column_2_And_HitoriMoveColorKey_Black()
        //{
        //    // Arrange
        //    const char expectedCharAt2_2 = 'B';

        //    // Act
        //    HitoriPuzzle.SetChar(2, 2, HitoriMoveColorKey.Black);
        //    char charAt2_2 = HitoriPuzzle.GetChar(2, 2);

        //    // Assert
        //    Assert.AreEqual(expectedCharAt2_2, charAt2_2);
        //}
    }
}
