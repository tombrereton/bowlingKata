using System;
using System.Globalization;
using NUnit.Framework;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace BowlingGameKata
{
    public class BowlingGame
    {
        public static int CalculateScore(int[] throws)
        {
            var totalScore = 0;
            const int totalThrows = 20;
            for (var index = 0; index < totalThrows; index += 2)
            {
                var firstThrow = throws[index];
                var secondThrow = throws[index + 1];

                int frameScore = firstThrow + secondThrow;

                totalScore += frameScore;

                if (IsStrike(firstThrow))
                {
                    if (IsLastFrame(index))
                    {
                        totalScore += CalculateLastFrameScoreWhenStrike(throws, index);
                    }
                    else
                    {
                        totalScore += CalculateStrikeBonus(throws, index);
                    }
                }
                else if (IsSpare(frameScore))
                {
                    totalScore += CalculateSpareBonus(throws, index);
                }
            }

            return totalScore;
        }

        private static int CalculateLastFrameScoreWhenStrike(int[] throws, int index)
        {
            return throws[index + 2];
        }

        private static bool IsLastFrame(int index)
        {
            return index >= 18;
        }

        private static int CalculateSpareBonus(int[] throws, int index)
        {
            return throws[index + 2];
        }

        private static int CalculateStrikeBonus(int[] throws, int index)
        {
            //if (index == 16 && throws[index + 2] == 10)
            //{
            //    return throws[index + 2] + throws[index + 3] + throws[index + 4];
            //}

            return throws[index + 2] + throws[index + 3];
        }

        private static bool IsStrike(int firstThrow)
        {
            return firstThrow == 10;
        }

        private static bool IsSpare(int frameScore)
        {
            return frameScore == 10;
        }
    }

    [TestFixture]
    public class BowlingGameTests
    {
        [Test]
        [TestCase(new[] {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 1, TestName = "Given Single Throw Then Expect Score of 1")]
        [TestCase(new[] {1, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 5, TestName = "Given First Frame Then Expect Score of 5")]
        [TestCase(new[] {1, 4, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 7, TestName = "Given Two Frames Then Expect Score of 7")]
        [TestCase(new[] {1, 4, 4, 5, 6, 4, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 34, TestName = "Given Spare On Third Frame Then Expect Bonus Points Added")]
        [TestCase(new[] {1, 4, 4, 5, 6, 4, 5, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 36, TestName = "Given Spare On Third Frame Then Expect Bonus Points Added")]
        [TestCase(new[] {1, 4, 4, 5, 6, 4, 5, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 39, TestName = "Given Spare On Third Frame Then Expect Bonus Points Added")]
        [TestCase(new[] {1, 4, 4, 5, 6, 4, 5, 5, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 43, TestName = "Given Two Spares In A Row Then Expect Bonus Points Added")]
        [TestCase(new[] {10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 10, TestName = "Given Single Strike Then Expected Score Is Correct")]
        [TestCase(new[] {10, 0, 2, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, 20, TestName = "Given Single Strike Then Bonus Calculated Correctly")]
        [TestCase(new[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 5, 1}, 11, TestName = "Given Spare in Last Frame Then Bonus Calculated Correctly")]
        [TestCase(new[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 1, 1}, 12, TestName = "Given Strike in Last Frame Then Bonus Calculated Correctly")]
        [TestCase(new[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 10, 10, 10}, 60, TestName = "Given Three Strikes in Last Frame Then Bonus Calculated Correctly")]
        //[TestCase(new[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 0, 10, 0, 10, 10, 10}, 90, TestName = "Given Five Strikes Then Bonus Calculated Correctly")]
        [TestCase(new[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 10, 10}, 30, TestName = "Given Three Strikes in Last Frame Then Bonus Calculated Correctly")]
        [TestCase(new[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 5, 10}, 20, TestName = "Given Strike in Last Frame Then Bonus Calculated Correctly")]
        //[TestCase(new[] {10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 10, 10}, 300, TestName = "Given Strike in Last Frame Then Bonus Calculated Correctly")]
        public void GivenGameIsPlayed_ThenReturnExpectedScore(int[] throws, int expectedScore)
        {
            var actualScore = BowlingGame.CalculateScore(throws);

            Assert.That(actualScore, Is.EqualTo(expectedScore));
        }
    }
}