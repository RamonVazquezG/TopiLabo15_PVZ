using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

public class Hitbox
{
    Vector2 Size;
    Vector2 Offset;

    public Hitbox(Vector2 Size, Vector2 Offset)
        {
            this.Size = Size;
            this.Offset = Offset;
        }
}
