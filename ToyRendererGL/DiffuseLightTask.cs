using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace ToyRendererGL
{
    public class DiffuseLightTask : IRenderTask
    {
        private const PrimitiveType Primitive = PrimitiveType.Triangles;
        private const string VertexShaderPath = "Shaders\\diffuse_lighting.vert";
        private const string FragShaderPath = "Shaders\\diffuse_lighting.frag";

        public GL Gl { get; set; }

        private Pipeline pipeline;

        private readonly string vertCode, fragCode;

        public DiffuseLightTask(GL gl)
        {
            Gl = gl;

            vertCode = File.ReadAllText(VertexShaderPath);
            fragCode = File.ReadAllText(FragShaderPath);
        }

        public void Init()
        {
            pipeline = new Pipeline(Gl, vertCode, fragCode);
        }

        public void Render(Scene scene, double deltaTime)
        {
            pipeline.Use();
            Camera cam = scene.Camera;
            pipeline.SetUniform("projection", cam.PerspectiveMatrix);
            pipeline.SetUniform("view", cam.ViewMatrix);
            pipeline.SetUniform("dirLightsCount", scene.DirectionLights.Length);
            for (int i = 0; i < scene.DirectionLights.Length; i++)
            {
                DirectionLight dirLight = scene.DirectionLights[i];
                pipeline.SetUniform($"dirLights[{i}].direction", dirLight.Direction);
                pipeline.SetUniform($"dirLights[{i}].ambient", dirLight.Ambient);
                pipeline.SetUniform($"dirLights[{i}].diffuse", dirLight.Diffuse);
                pipeline.SetUniform($"dirLights[{i}].specular", dirLight.Specular);
            }
            pipeline.SetUniform("pointLightsCount", scene.PointLights.Length);
            for (int i = 0; i < scene.PointLights.Length; i++)
            {
                PointLight pointLight = scene.PointLights[i];
                pipeline.SetUniform($"pointLights[{i}].position", pointLight.Position);
                pipeline.SetUniform($"pointLights[{i}].ambient", pointLight.Ambient);
                pipeline.SetUniform($"pointLights[{i}].diffuse", pointLight.Diffuse);
                pipeline.SetUniform($"pointLights[{i}].specular", pointLight.Specular);
                pipeline.SetUniform($"pointLights[{i}].constant", pointLight.Constant);
                pipeline.SetUniform($"pointLights[{i}].linear", pointLight.Linear);
                pipeline.SetUniform($"pointLights[{i}].quadratic", pointLight.Quadratic);
            }
            pipeline.SetUniform("spotLightsCount", scene.SpotLights.Length);
            for (int i = 0; i < scene.SpotLights.Length; i++)
            {
                SpotLight spotLight = scene.SpotLights[i];
                pipeline.SetUniform($"spotLights[{i}].position", spotLight.Position);
                pipeline.SetUniform($"spotLights[{i}].direction", spotLight.Direction);
                pipeline.SetUniform($"spotLights[{i}].ambient", spotLight.Ambient);
                pipeline.SetUniform($"spotLights[{i}].diffuse", spotLight.Diffuse);
                pipeline.SetUniform($"spotLights[{i}].specular", spotLight.Specular);
                pipeline.SetUniform($"spotLights[{i}].constant", spotLight.Constant);
                pipeline.SetUniform($"spotLights[{i}].linear", spotLight.Linear);
                pipeline.SetUniform($"spotLights[{i}].quadratic", spotLight.Quadratic);
                pipeline.SetUniform($"spotLights[{i}].cutOff", spotLight.CutOff);
                pipeline.SetUniform($"spotLights[{i}].outerCutOff", spotLight.OuterCutOff);
            }
            foreach (var mesh in scene.Meshes)
            {
                mesh.VertexArray.Bind();
                mesh.Material.DiffuseTexture.Bind(TextureUnit.Texture0);
                mesh.Material.SpecularTexture.Bind(TextureUnit.Texture1);
                pipeline.SetUniform("material.shininess", mesh.Material.Shiness);
                mesh.ExecuteAnimation(deltaTime);
                Matrix4x4 model = mesh.Transform.ViewMatrix;
                pipeline.SetUniform("model", model);
                Gl.DrawArrays(Primitive, 0, mesh.Count);
            }
        }
    }
}
