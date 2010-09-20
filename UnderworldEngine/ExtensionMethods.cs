﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UnderworldEngine
{
    internal static class ExtensionMethods
    {
        public static Vector3 GetFromString(this Vector3 vec, string s)
        {
            string[] values = s.Split(' ');
            vec.X = (float)Convert.ToDouble(values[0]);
            vec.Y = (float)Convert.ToDouble(values[1]);
            vec.Z = (float)Convert.ToDouble(values[2]);
            return vec;
        }

        public static bool AlmostEquals(this Vector3 vec, Vector3 vec2, float nEpsilon)
        {
            bool bRet1 = (((vec2.X - nEpsilon) < vec.X) && (vec.X < (vec2.X + nEpsilon)));
            bool bRet2 = (((vec2.Y - nEpsilon) < vec.Y) && (vec.Y < (vec2.Y + nEpsilon)));
            bool bRet3 = (((vec2.Z - nEpsilon) < vec.Z) && (vec.Z < (vec2.Z + nEpsilon)));
            return bRet1 && bRet2 && bRet3;
        }

        public static bool AlmostEquals(this float f1, float f2, float nEpsilon)
        {
            bool bRet1 = (((f2 - nEpsilon) < f1) && (f1 < (f2 + nEpsilon)));
            return bRet1;
        }

        public static Texture2D pointTexture = new Texture2D(Game1.DefaultGraphicsDevice,
            1, 1,
            1,
            TextureUsage.None, SurfaceFormat.Color
            );
        public static void DrawLine(this SpriteBatch sb, //Texture2D blank,
              float width, Color color, Vector2 point1, Vector2 point2)
        {
            pointTexture.SetData(new[] { Color.White });

            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            sb.Draw(pointTexture, point1, null, color,
              angle, new Vector2(0, 0), new Vector2(length, width),
              SpriteEffects.None, 0);
        }
    }
}