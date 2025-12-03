using FsCheck;
using FsCheck.Fluent;
using FsCheck.Xunit;
using static CSTest.Session09.DiamondTests.DiamondProperties;
using static CSTest.Session09.DiamondTests.Generators;
using static FsCheck.Fluent.Prop;

namespace CSTest.Session09.DiamondTests;

public class DiamondTest
{
    [Property]
    Property diamond_is_a_square() =>
        ForAll(Diamonds.ToArbitrary(), IsASquare);

    [Property]
    Property diamond_is_horizontally_symmetric() =>
        ForAll(Diamonds.ToArbitrary(), diamond => diamond.IsHorizontallySymmetric());

    [Property]
    Property diamond_is_vertically_symmetric() =>
        ForAll(Diamonds.ToArbitrary(), diamond => diamond.IsVerticallySymmetric());

    [Property]
    Property contains_all_letters() =>
        ForAll(
            DiamondsAndTargets.ToArbitrary(),
            useCase =>
                useCase.Diamond
                    .ContainsAllLettersUpTo(useCase.Target));

    [Property]
    Property each_quadrant_row_contains_one_letter_only() =>
        ForAll(
            Quadrants.ToArbitrary(),
            quadrant => quadrant.ContainsOnlyOneLetter());

    [Property]
    Property each_quadrant_has_letters_only_on_diagonal() =>
        ForAll(
            Quadrants.ToArbitrary(),
            quadrant => quadrant.ContainsLettersOnDiagonal());


    [Fact]
    void is_horizontally_symmetric()
    {
        List<string> s = ["abcba"];
        Assert.True(s.IsHorizontallySymmetric());
    }

    [Fact]
    void is_vertically_symmetric()
    {
        List<string> s =
        [
            "zbcXbaQ",
            "abcXbaX",
            "-------",
            "abcXbaX",
            "zbcXbaQ"
        ];
        Assert.True(s.IsVerticallySymmetric());
    }
}
