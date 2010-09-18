using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnderworldEngine.GameState
{
    interface IScreen
    {
        bool IsFocused
        {
            get;
            set;
        }

        void Unload();
        void Update();
        void Draw();
    }
}
