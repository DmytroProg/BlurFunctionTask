using GraphicsCore;

namespace BlutFunction.Tests;

public class BlurPictureFunctionTests
{
    [Fact]
    public void BlurPicture_ThrowsException_WhenWidthOrHeightIsInvalid()
    {
        var picture = new ColorData[1, 1];

        Assert.Throws<ArgumentException>(() => BlurPicture(picture, 0, 1));
        Assert.Throws<ArgumentException>(() => BlurPicture(picture, 1, 0));
        Assert.Throws<ArgumentException>(() => BlurPicture(picture, -1, 1));
        Assert.Throws<ArgumentException>(() => BlurPicture(picture, 1, -1));
    }

    [Fact]
    public void BlurPicture_ReturnsMatrixWithSameDimensions()
    {
        int width = 3, height = 3;
        var picture = new ColorData[width, height];

        SetPictureMatrixWithWhiteColors(picture);
        var result = BlurPicture(picture, width, height);

        Assert.Equal(width, result.GetLength(0));
        Assert.Equal(height, result.GetLength(1));
    }

    [Fact]
    public void BlurPicture_BlursCorrectlyOnUniformColor()
    {
        var picture = new ColorData[3, 3];
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                picture[i, j] = new ColorData(100, 150, 200);

        var result = BlurPicture(picture, 3, 3);

        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                Assert.Equal(new ColorData(100, 150, 200), result[i, j]);
    }

    [Fact]
    public void BlurPicture_BlursMiddlePixelCorrectly()
    {
        var picture = new ColorData[3, 3]
        {
            { new ColorData(0,0,0), new ColorData(0,0,0), new ColorData(0,0,0) },
            { new ColorData(0,0,0), new ColorData(255,255,255), new ColorData(0,0,0) },
            { new ColorData(0,0,0), new ColorData(0,0,0), new ColorData(0,0,0) }
        };

        var result = BlurPicture(picture, 3, 3);
        var expectedGray = (byte)(255 * 4 / 16); // kernel center weight is 4, all other weights are 0 because pixels are black

        Assert.Equal(new ColorData(expectedGray, expectedGray, expectedGray), result[1, 1]);
    }

    [Theory]
    [MemberData(nameof(GetPictureMiddleColorMatrixes))]
    public void BlurPicture_MiddleColorPaint_CorrectPictureMatrix(ColorData[,] picture, ColorData[,] bluredPicture)
    {
        var result = BlurPicture(picture, picture.GetLength(0), picture.GetLength(1));

        AssertPicture(bluredPicture, result);
    }

    [Theory]
    [MemberData(nameof(GetPictureMiddleColorMatrixes))]
    public void BlurPicture_RandomColorPaint_CorrectPictureMatrix(ColorData[,] picture, ColorData[,] bluredPicture)
    {
        var result = BlurPicture(picture, picture.GetLength(0), picture.GetLength(1));

        AssertPicture(bluredPicture, result);
    }

    // Helper method to call the original static method (assuming it's in some class)
    private static ColorData[,] BlurPicture(ColorData[,] picture, int width, int height)
    {
        if (width <= 0 || height <= 0)
            throw new ArgumentException("Width and height must be positive");

        return GraphicsFunctions.BlurPicture(picture, width, height);
    }

    private static void SetPictureMatrixWithWhiteColors(ColorData[,] picture)
    {
        for(int i = 0; i < picture.GetLength(0); i++)
        {
            for(int j = 0; j < picture.GetLength(1); j++)
            {
                picture[i, j] = StaticColors.White;
            }
        }
    }

    private static void AssertPicture(ColorData[,] expected, ColorData[,] actual)
    {
        for (int i = 0; i < expected.GetLength(0); i++)
        {
            for (int j = 0; j < expected.GetLength(1); j++)
            {
                if (expected[i, j] != actual[i, j])
                {
                    Assert.Fail($"Cell [{i};{j}] is incorrect. Expected: {expected}. But was {actual}");
                }
            }
        }
    }

    public static IEnumerable<object[]> GetPictureMiddleColorMatrixes()
    {
        yield return [
            new ColorData[,]
            {
                { StaticColors.White, StaticColors.White, StaticColors.White },
                { StaticColors.White, StaticColors.Red, StaticColors.White },
                { StaticColors.White, StaticColors.White, StaticColors.White },
            },
            new ColorData[,]
            {
                { new(255, 226, 226), new(255, 212, 212), new(255, 226, 226) },
                { new(255, 212, 212), new(255, 191, 191), new(255, 212, 212) },
                { new(255, 226, 226), new(255, 212, 212), new(255, 226, 226) },
            }
        ];
        yield return [
            new ColorData[,]
            {
                { StaticColors.White, StaticColors.White, StaticColors.White },
                { StaticColors.White, StaticColors.Green, StaticColors.White },
                { StaticColors.White, StaticColors.White, StaticColors.White },
            },
            new ColorData[,]
            {
                { new(226, 255, 226), new(212, 255, 212), new(226, 255, 226) },
                { new(212, 255, 212), new(191, 255, 191), new(212, 255, 212) },
                { new(226, 255, 226), new(212, 255, 212), new(226, 255, 226) },
            }
        ];
        yield return [
            new ColorData[,]
            {
                { StaticColors.White, StaticColors.White, StaticColors.White },
                { StaticColors.White, StaticColors.Blue, StaticColors.White },
                { StaticColors.White, StaticColors.White, StaticColors.White },
            },
            new ColorData[,]
            {
                { new(226, 226, 255), new(212, 212, 255), new(226, 226, 255) },
                { new(212, 212, 255), new(191, 191, 255), new(212, 212, 255) },
                { new(226, 226, 255), new(212, 212, 255), new(226, 226, 255) },
            }
        ];
        yield return [
            new ColorData[,]
            {
                { StaticColors.White, StaticColors.White, StaticColors.White },
                { StaticColors.White, StaticColors.Black, StaticColors.White },
                { StaticColors.White, StaticColors.White, StaticColors.White },
            },
            new ColorData[,]
            {
                { new(226, 226, 226), new(212, 212, 212), new(226, 226, 226) },
                { new(212, 212, 212), new(191, 191, 191), new(212, 212, 212) },
                { new(226, 226, 226), new(212, 212, 212), new(226, 226, 226) },
            }
        ];
    }

    public static IEnumerable<object[]> GetRandomPictureMatrixes()
    {
        yield return [
            new ColorData[,]
            {
                { StaticColors.Red, StaticColors.White, StaticColors.Red },
                { StaticColors.White, StaticColors.White, StaticColors.White },
                { StaticColors.Red, StaticColors.White, StaticColors.Red },
            },
            new ColorData[,]
            {
                { new(255, 141, 141), new(255, 170, 170), new(255, 141, 141) },
                { new(255, 170, 170), new(255, 191, 191), new(255, 170, 170) },
                { new(255, 141, 141), new(255, 170, 170), new(255, 141, 141) },
            }
        ];
        yield return [
            new ColorData[,]
            {
                { StaticColors.Blue, StaticColors.White, StaticColors.White },
                { StaticColors.White, StaticColors.Blue, StaticColors.White },
                { StaticColors.White, StaticColors.White, StaticColors.Blue },
            },
            new ColorData[,]
            {
                { new(113, 113, 255), new(170, 170, 255), new(226, 226, 255) },
                { new(170, 170, 255), new(159, 159, 255), new(170, 170, 255) },
                { new(226, 226, 255), new(170, 170, 255), new(113, 113, 255) },
            }
        ];
        yield return [
            new ColorData[,]
            {
                { StaticColors.White, StaticColors.Green, StaticColors.White },
                { StaticColors.Green, StaticColors.Green, StaticColors.White },
                { StaticColors.White, StaticColors.Green, StaticColors.White },
            },
            new ColorData[,]
            {
                { new(113, 255, 113), new(85, 255, 85), new(113, 85, 113) },
                { new(106, 255, 106), new(96, 255, 96), new(106, 255, 106) },
                { new(170, 255, 170), new(170, 255, 170), new(170, 255, 170) },
            }
        ];
        yield return [
            new ColorData[,]
            {
                { StaticColors.Red, StaticColors.Black, StaticColors.White },
                { StaticColors.White, StaticColors.White, StaticColors.Blue },
                { StaticColors.Green, StaticColors.White, StaticColors.White },
            },
            new ColorData[,]
            {
                { new(198, 85, 85), new(191, 191, 148), new(141, 255, 141) },
                { new(148, 106, 127), new(175, 175, 191), new(191, 233, 212) },
                { new(141, 141, 198), new(148, 148, 233), new(198, 198, 255) },
            }
        ];
    }
}