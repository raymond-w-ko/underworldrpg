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
                    {
                        float x = (float)Convert.ToDouble(command[2]);
                        float y = (float)Convert.ToDouble(command[3]);
                        float z = (float)Convert.ToDouble(command[4]);
                        Game1.Camera.LookAt(x, y, z);
                        break;
                    }
                case "upvector":
                    {
                        float x = (float)Convert.ToDouble(command[2]);
                        float y = (float)Convert.ToDouble(command[3]);
                        float z = (float)Convert.ToDouble(command[4]);
                        Game1.Camera.SetUpVector(x, y, z);
                        break;
                    }
                case "nplane":
                    {
                        float dist = (float)Convert.ToDouble(command[2]);
                        Game1.Camera.SetNearPlaneDistance(dist);
                        break;
                    }
                case "fplane":
                    {
                        float dist = (float)Convert.ToDouble(command[2]);
                        Game1.Camera.SetFarPlaneDistance(dist);
                        break;
                    }
                case "next":
                    {
                        Game1.Camera.nextCameraView();
                        break;
                    }
                case "prev":
                    {
                        Game1.Camera.prevCameraView();
                        break;
                    }
            }
        }

        #endregion
    }
}
