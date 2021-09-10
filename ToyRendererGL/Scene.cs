using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyRendererGL
{
    public class Scene
    {
        public Camera Camera { get; private set; }
        public TexturedMesh<float, uint>[] Meshes { get; private set; }

        public Scene(Camera camera, params TexturedMesh<float, uint>[] meshes)
        {
            Camera = camera;
            Meshes = meshes;
        }
    }
}
