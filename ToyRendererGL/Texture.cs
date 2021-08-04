using Silk.NET.OpenGL;
using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Runtime.InteropServices;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Processing;

namespace ToyRendererGL
{
    public class Texture : IDisposable
    {
        private readonly GL gl;
        private readonly uint code;

        public unsafe Texture(GL gl, string texturePath)
        {
            this.gl = gl;
            code = gl.GenTexture();

            Image<Rgba32> img = (Image<Rgba32>)Image.Load(texturePath);
            img.Mutate(x => x.Flip(FlipMode.Vertical));

            fixed (void* data = &MemoryMarshal.GetReference(img.GetPixelRowSpan(0)))
            {
                Load(gl, data, (uint)img.Width, (uint)img.Height);
            }

            img.Dispose();
        }

        public unsafe Texture(GL gl, Span<byte> data, uint width, uint height)
        {
            this.gl = gl;
            code = gl.GenTexture();

            fixed (void* d = &data[0])
            {
                Load(gl, d, width, height);
            }
        }

        private unsafe void Load(GL gl, void* data, uint width, uint height)
        {
            Bind();

            gl.TexImage2D(TextureTarget.Texture2D, 0, (int)InternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);
            gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.ClampToEdge);
            gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.ClampToEdge);
            gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.Linear);
            gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);
            gl.GenerateMipmap(TextureTarget.Texture2D);
        }

        public void Bind(TextureUnit textureSlot = TextureUnit.Texture0)
        {
            gl.ActiveTexture(textureSlot);
            gl.BindTexture(TextureTarget.Texture2D, code);
        }

        public void Dispose()
        {
            gl.DeleteTexture(code);
        }
    }
}
