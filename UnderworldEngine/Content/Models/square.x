xof 0303txt 0032 
// SketchUp 6 -> DirectX (c)2008 edecadoudal, supports: faces, normals and textures 
Material Default_Material{ 
1.0;1.0;1.0;1.0;;
3.2;
0.000000;0.000000;0.000000;;
0.000000;0.000000;0.000000;;
} 
Material akb_bocote{ 
0.423529411764706;0.231372549019608;0.0470588235294118;1.0;;
3.2;
0.000000;0.000000;0.000000;;
0.000000;0.000000;0.000000;;
   TextureFilename { "square.xbocote.jpg";   } 
} 
Mesh mesh_0{
 8;
 -0.0000;7.0625;7.0625;,
 -0.0000;7.0625;7.0625;,
 -0.0000;0.0000;0.0000;,
 -0.0000;0.0000;0.0000;,
 -0.0000;0.0000;7.0625;,
 -0.0000;0.0000;7.0625;,
 -0.0000;7.0625;0.0000;,
 -0.0000;7.0625;0.0000;;
 4;
 3;4,2,0,
 3;1,3,5,
 3;6,0,2,
 3;3,1,7;;
  MeshMaterialList {
  2;
  4;
  1,
  0,
  1,
  0;
  { Default_Material }
  { akb_bocote }
  }
  MeshTextureCoords {
  8;
  2.5393,-0.5885;
  8.0625,-7.0625;
  1.0000,-0.0000;
  1.0000,-0.0000;
  2.5393,-0.0000;
  8.0625,-0.0000;
  1.0000,-0.5885;
  1.0000,-7.0625;;
  }
  MeshNormals {
  8;
    1.0000;0.0000;0.0000;
-1.0000;-0.0000;-0.0000;
1.0000;0.0000;0.0000;
-1.0000;-0.0000;-0.0000;
1.0000;0.0000;0.0000;
-1.0000;-0.0000;-0.0000;
1.0000;0.0000;0.0000;
-1.0000;-0.0000;-0.0000;;
  4;
  3;4,2,0;
  3;1,3,5;
  3;6,0,2;
  3;3,1,7;;
  }
 }