using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Numerics;

namespace MonoGameFractals
{
    class Fractal
    {
        /// <summary>
        /// Number of iteration in the escaping computing
        /// </summary>
        const short iteration = 100;

        /// <summary>
        /// Allows to see all fractal
        /// </summary>
        const float zoomDivider = 200f;

        /// <summary>
        /// Move the fractal on the right to watch it better
        /// </summary>
        const int xOffset = 100;

        /// <summary>
        /// Fractral texture
        /// </summary>
        public Texture2D FractalTexture { get; set; }

        /// <summary>
        /// Texture position
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// True if fractal has been calculated and drawn
        /// </summary>
        bool hasDrawn;

        /// <summary>
        /// Initialize, what else ?
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        public void Initialize(Texture2D texture, Vector2 position)
        {
            FractalTexture = texture;
            Position = position;

            Color[] pixels = new Color[FractalTexture.Width * FractalTexture.Height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i].R = 0;
                pixels[i].G = 0;
                pixels[i].B = 0;
            }
            FractalTexture.SetData(pixels);
            hasDrawn = false;
        }

        /// <summary>
        /// Update and draw fractal
        /// </summary>
        public void Update()
        {
            if(!hasDrawn)
            {
                Color[] pixels = new Color[FractalTexture.Width * FractalTexture.Height];
                for (int i = 0; i < pixels.Length; i++)
                {
                    int escapeSpeed = EscapingIteration(PixelToCoordinate(i));
                    // Non-escaping complexes in white
                    if (escapeSpeed >= iteration)
                    {
                        pixels[i].R = 255;
                        pixels[i].G = 255;
                        pixels[i].B = 255;
                    }
                    // Color in function of escaping speed
                    else
                        pixels[i] = GetEscapingColor(escapeSpeed);
                }
                FractalTexture.SetData(pixels);
                hasDrawn = true;
            }
        }

        /// <summary>
        /// Get a color in function of escaping speed
        /// </summary>
        /// <param name="escapeSpeed"></param>
        /// <returns></returns>
        private Color GetEscapingColor(int escapeSpeed)
        {
            byte r = 0;
            byte g = 0;
            byte b = 0;
            if (escapeSpeed <= 10)
            {
                b = (byte)Math.Round((100 - escapeSpeed) * 0.20);
            }
            else if (escapeSpeed > 10 && escapeSpeed <= 25)
            {
                b = (byte)Math.Round((100 - escapeSpeed) * 0.55);
                g = (byte)Math.Round((100 - escapeSpeed) * 0.40);
            }
            else if (escapeSpeed > 25)
            {
                b = (byte)Math.Round((100 - escapeSpeed) * 0.75);
                g = (byte)Math.Round((100 - escapeSpeed) * 2.55);
                r = (byte)Math.Round((100 - escapeSpeed) * 0.55);
            }
            return new Color(r, g, b);
        }

        /// <summary>
        /// True is point escapes with the fractal algorithm
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private int EscapingIteration(Vector2 number)
        {
            Complex c = new Complex(number.X, number.Y);
            Complex z = new Complex(0, 0);
            for(int i = 0 ; i < iteration ; i++)
            {
                z = Iterate(z, c);
                if (Math.Sqrt(Math.Pow(z.Real, 2) + Math.Pow(z.Imaginary, 2)) > 2)
                    return i;
            }
            return iteration;
        }

        /// <summary>
        /// Convert a texture pixel into his coordinate on the texture
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private Vector2 PixelToCoordinate(int i)
        {
            float x = i % (float)FractalTexture.Width - FractalTexture.Width / 2 - xOffset;
            float y = (float)Math.Floor(i / (float)FractalTexture.Width) - FractalTexture.Width / 2;
            return new Vector2(x / zoomDivider, y / zoomDivider);
        }

        /// <summary>
        /// Iterative fonction, used to compute if a point is escaping or not
        /// </summary>
        /// <param name="zn">Iterated value (n th iteration)</param>
        /// <param name="c">Additive seed</param>
        /// <returns>Iterated value (n+1 th iteration)</returns>
        private Complex Iterate(Complex zn, Complex c)
        {
            return Complex.Pow(zn, 2) + c;
        }

        /// <summary>
        /// Drawing the fractal
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(FractalTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f); 
        } 
    }
}
