using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Replica
{
    class Drawable    
    {
        public float existenceTime;
        public float ExistenceTime { get { return existenceTime; } }

        public virtual void Initialize()
        {
            existenceTime = 0;
        }

        public virtual void Update(GameTime gameTime)
        { 
        }

        public virtual void Draw()
        {
        }
    }
}
