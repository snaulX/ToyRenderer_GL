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
        public PointLight[] PointLights => _pointLights.ToArray();
        public DirectionLight[] DirectionLights => _directionLights.ToArray();
        public SpotLight[] SpotLights => _spotLights.ToArray();

        //private const int MAX_COUNT = 128;

        private readonly List<PointLight> _pointLights = new List<PointLight>(/*MAX_COUNT*/);
        private readonly List<DirectionLight> _directionLights = new List<DirectionLight>(/*MAX_COUNT*/);
        private readonly List<SpotLight> _spotLights = new List<SpotLight>(/*MAX_COUNT*/);
        private readonly List<TexturedMesh<float, uint>> _meshes = new List<TexturedMesh<float, uint>>();

        public Scene(Camera camera, params TexturedMesh<float, uint>[] meshes)
        {
            Camera = camera;
            _meshes = meshes.ToList();
        }

        public void AddLight(PointLight light)
        {
            _pointLights.Add(light);
        }
        public void AddLight(DirectionLight light)
        {
            _directionLights.Add(light);
        }
        public void AddLight(SpotLight light)
        {
            _spotLights.Add(light);
        }

        public void AddMesh(TexturedMesh<float, uint> mesh)
        {
            _meshes.Add(mesh);
        }
    }
}
