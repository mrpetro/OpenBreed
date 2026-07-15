using OpenTK.Mathematics;

namespace OpenBreed.Rendering.Abstractions.Extensions
{
    public static class Color4Extensions
    {
        public static Color4<Rgba> SetAlpha(this Color4<Rgba> color, float alpha)
        {
            return new Color4<Rgba>(
                color.X,
                color.Y,
                color.Z,
                alpha);
        }

        public static Color4<Rgba> Multiply(this Color4<Rgba> color, float value)
        {
            return new Color4<Rgba>(
                color.X * value,
                color.Y * value,
                color.Z * value,
                color.W);
        }
    }
}
