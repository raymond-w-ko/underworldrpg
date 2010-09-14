using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnderworldEngine.Scripting
{
    class SetC : IInterpretable
    {
        /// <summary>
        /// represents the setc command for setting the camera
        /// </summary>


        #region IInterpretable Members

        public void run(string function)
        {
            string[] command = function.Split(new Char[] { ' ' });
            command[1] = command[1].ToLower();
            switch (command[1])
            {
                case "lookat":
                case "l":
                    {
                        float x = (float)Convert.ToDouble(command[2]);
                        float y = (float)Convert.ToDouble(command[3]);
                        float z = (float)Convert.ToDouble(command[4]);
                        Game1.Camera.LookAt(x, y, z);
                        break;
                    }
                case "upvector":
                case "u":
                    {
                        float x = (float)Convert.ToDouble(command[2]);
                        float y = (float)Convert.ToDouble(command[3]);
                        float z = (float)Convert.ToDouble(command[4]);
                        Game1.Camera.SetUpVector(x, y, z);
                        break;
                    }
                case "nplane":
                case "z":
                    {
                        float dist = (float)Convert.ToDouble(command[2]);
                        Game1.Camera.SetNearPlaneDistance(dist);
                        break;
                    }
                case "fplane":
                case "f":
                    {
                        float dist = (float)Convert.ToDouble(command[2]);
                        Game1.Camera.SetFarPlaneDistance(dist);
                        break;
                    }
                case "next":
                case "n":
                    {
                        Game1.Camera.NextCameraView();
                        break;
                    }
                case "prev":
                case "p":
                    {
                        Game1.Camera.PrevCameraView();
                        break;
                    }
                default:
                    throw new ArgumentException("invalid option", command[1]);
            }
        }

        #endregion
    }
}
