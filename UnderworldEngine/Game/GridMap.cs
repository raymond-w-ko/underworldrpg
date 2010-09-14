using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using UnderworldEngine.Graphics;
/*
namespace UnderworldEngine.Game
{
    public class GridMap : GameObject
    {
        private uint xSize;
        private uint ySize;
        private Vector3 origin;

        private Random rand;

        private List<GridColumn> gridQuadList;

        public GridMap(uint x, uint y) : this(x, y, Vector3.Zero) { }

        public GridMap(uint x, uint y, Vector3 orig) :
            base()
        {
            this.xSize = x;
            this.ySize = y;
            this.Position = this.origin = orig;

            gridQuadList = new List<GridColumn>();

            rand = new Random(9001);
            GenerateQuads(true);
        }

        private void GenerateQuads(bool UseRandomY)
        {
            for (int xx = 0; xx < this.xSize; xx++) {
                for (int zz = 0; zz < this.ySize; zz++) {
                    int yValue = UseRandomY ? (int)Math.Ceiling((rand.NextDouble() * 2)) : 0;
                    GridColumn grid = new GridColumn(
                        new Vector3(xx + 0.5f, yValue, zz + 0.5f),
                        yValue
                        );
                    gridQuadList.Add(grid);
                }
            }
        }

        public override void Draw()
        {
            if (!this.IsVisible) {
                return;
            }
            CompileTransformations();

            foreach (GridColumn g in gridQuadList) {
                g.Draw();
            }
        }
    }
}
*/