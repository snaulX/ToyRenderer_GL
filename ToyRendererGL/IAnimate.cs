using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyRendererGL
{
    public interface IAnimate
    {
        void ExecuteAnimation(double deltaTime);
    }
}
