using System;
using Silk.NET.Input;
using Silk.NET.Windowing;

namespace ToyRendererGL
{
    public class Input : IDisposable
    {
        private readonly IInputContext input;

        public event Action<IKeyboard, Key, int> OnKeyDown
        {
            add
            {
                for (int i = 0; i < input.Keyboards.Count; i++)
                    input.Keyboards[i].KeyDown += value;
            }
            remove
            {
                for (int i = 0; i < input.Keyboards.Count; i++)
                    input.Keyboards[i].KeyDown -= value;
            }
        }
        public event Action<IKeyboard, Key, int> OnKeyUp
        {
            add
            {
                for (int i = 0; i < input.Keyboards.Count; i++)
                    input.Keyboards[i].KeyUp += value;
            }
            remove
            {
                for (int i = 0; i < input.Keyboards.Count; i++)
                    input.Keyboards[i].KeyUp -= value;
            }
        }

        public Input(IWindow window)
        {
            input = window.CreateInput();
        }

        public bool IsKeyPressed(Key key)
        {
            for (int i = 0; i < input.Keyboards.Count; i++)
            {
                if (input.Keyboards[i].IsKeyPressed(key))
                    return true;
            }
            return false;
        }

        public void Dispose()
        {
            input.Dispose();
        }
    }
}
