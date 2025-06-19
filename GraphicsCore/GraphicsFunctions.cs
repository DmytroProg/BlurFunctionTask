namespace GraphicsCore;

public static class GraphicsFunctions
{

    /// <summary>
    /// Uses matrix manipulations to apply blur effect on a picture matrix
    /// </summary>
    /// <param name="picture">picture's colors matrix</param>
    /// <param name="width">picture's width</param>
    /// <param name="height">pictures' height</param>
    /// <returns>a matrix with blur effect</returns>
    /// <exception cref="ArgumentException">throws if <param name="width"> or 
    /// <param name="height"> is less or equal to 0
    /// or if <param name="blurRange"> does not fall within the range [0-10]</exception>
    public static ColorData[,] BlurPicture(ColorData[,] picture, int width, int height)
    {
        throw new NotImplementedException();
    }
}
