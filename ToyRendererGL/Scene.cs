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
        public TexturedMesh<float, uint>[] Meshes => _meshes.ToArray();
        public Light[] Lights => _lights.ToArray();

        private readonly List<Light> _lights = new List<Light>();
        private readonly List<TexturedMesh<float, uint>> _meshes = new List<TexturedMesh<float, uint>>();

        public Scene(Camera camera, params TexturedMesh<float, uint>[] meshes)
        {
            Camera = camera;
            _meshes = meshes.ToList();
        }

        public void AddLight(Light light)
        {
            _lights.Add(light);
        }

        public void AddMesh(TexturedMesh<float, uint> mesh)
        {
            _meshes.Add(mesh);
        }
    }
}
