using System;
using Silk.NET.Input;
using Silk.NET.Windowing;

namespace ToyRendererGL
{
    public class Input
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

        public Input(IWindow window)
        {
            input = window.CreateInput();
        }
    }
}
