using System;
using System.Numerics;
using Silk.NET.OpenGL;

namespace ToyRendererGL
{
    public class Pipeline : IDisposable
    {
        private readonly GL gl;
        private readonly uint code;

        public Pipeline(GL gl, string vertexCode, string fragmentCode)
        {
            this.gl = gl;
            uint vertexShader = LoadShader(ShaderType.VertexShader, vertexCode);
            uint fragmentShader = LoadShader(ShaderType.FragmentShader, fragmentCode);
            code = gl.CreateProgram();
            gl.AttachShader(code, vertexShader);
            gl.AttachShader(code, fragmentShader);
            gl.LinkProgram(code);
            gl.GetProgram(code, GLEnum.LinkStatus, out int status);
            if (status == 0)
                throw new Exception($"Program failed to link with error: {gl.GetProgramInfoLog(code)}");

            // delete shaders
            gl.DetachShader(code, vertexShader);
            gl.DetachShader(code, fragmentShader);
            gl.DeleteShader(vertexShader);
            gl.DeleteShader(fragmentShader);
        }

        private int GetLocation(string name)
        {
            int location = gl.GetUniformLocation(code, name);
            if (location == -1)
                throw new Exception($"{name} uniform not found in shader.");
            return location;
        }

        public uint LoadShader(ShaderType type, string shaderCode)
        {
            uint code = gl.CreateShader(type);
            gl.ShaderSource(code, shaderCode);
            gl.CompileShader(code);
            string infoLog = gl.GetShaderInfoLog(code);
            if (!string.IsNullOrWhiteSpace(infoLog))
                throw new Exception($"Error compiling shader of type {type}, failed with error {infoLog}");
            return code;
        }

        public void Use() => gl.UseProgram(code);

        #region Unifroms
        public void SetUniform(string name, bool value) => gl.Uniform1(GetLocation(name), value ? 1 : 0);
        public void SetUniform(string name, float value) => gl.Uniform1(GetLocation(name), value);
        public void SetUniform(string name, int value) => gl.Uniform1(GetLocation(name), value);
        public void SetUniform(string name, double value) => gl.Uniform1(GetLocation(name), value);
        public void SetUniform(string name, Vector2 value) => gl.Uniform2(GetLocation(name), value);
        public void SetUniform(string name, Vector3 value) => gl.Uniform3(GetLocation(name), value);
        public void SetUniform(string name, Vector4 value) => gl.Uniform4(GetLocation(name), value);
        public void SetUniform(string name, Quaternion value) => gl.Uniform4(GetLocation(name), value);
        public unsafe void SetUniform(string name, Matrix4x4 matrix)
            => gl.UniformMatrix4(GetLocation(name), 1, false, (float*)&matrix);
        #endregion

        public void Dispose()
        {
            gl.DeleteProgram(code);
        }
    }
}
