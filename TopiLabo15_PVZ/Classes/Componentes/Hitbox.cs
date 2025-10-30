using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
public class Hitbox
{
    public Entity Parent;

    public string Tag; //Puede servir para identificar dos hitboxes de una misma entidad en un HitboxCallback.

    public int? LaneHash = null; //Si es null, colisiona con todas las lanes. Si no es null, solo colisiona con hitboxes del mismo laneHash.

    public Vector2 Size;
    public Vector2 Offset;

    /// <summary>
    /// <param name="parentEntity">Un hitbox debe tener una referencia a la entidad que la esta creando. Una entidad solo tienen un hitbox y visceversa.</param>
    /// <param name="laneHash">Se usa para que el hitbox solo cheque intersecciones si el otro hitbox esta en la misma fila que este. Si es nulo, checara todas las filas (seria util, por ejemplo, para hacer la explosion de la petacereza).</param>
    /// <param name="Size">Se explica solo :v. El tamaño es en pixeles.</param>
    /// <param name="Offset">Que tanto se debe de desplazar en pixeles el hitbox de su entidad padre. Si es nulo, se centrara perfectamente hacia la entidad.</param>
    public Hitbox(Entity parentEntity, int? laneHash, Vector2 Size, Vector2? Offset = null, string tag = "")
    {
        this.Parent = parentEntity;

        this.LaneHash = laneHash;

        this.Size = Size;
        this.Offset = Offset ?? -Size/2f;

        this.Tag = tag;
    }

    public Vector2 GetParentPosition()
    {
        return this.Parent.Position;
    }

    public void CheckHitboxHash(Dictionary<int, Entity> entitiesFromManager)
    {
        //JC: Si no tiene entidad padre, no hacemos nada.
        //    En teoría esto no debería pasar nunca pero bueno por si acaso :v
        if (this.Parent == null) { return; } 

        //JC: Recorremos todas las entidades del EntityManager y comprobamos colisiones.
        //    Una forma muy sucia, cochina y marrana de checar collisiones porque la complejidad es O(n^2), pero para esta tarea (solo una tarea :,D) ta bien.
        foreach (var pair in entitiesFromManager) {
            Entity otherEntity = pair.Value;

            if (otherEntity == this.Parent) { continue; }
            if (otherEntity.IsRemoved) { continue; }

            // Soporte: si la otra entidad no tiene hitboxes, saltar
            if (otherEntity.Hitboxes == null || otherEntity.Hitboxes.Count ==0) { continue; }

            foreach (var otherHb in otherEntity.Hitboxes)
            {
                if (otherHb == null) continue;
                if(otherEntity.IsRemoved) continue;
                if (this.LaneHash != null && otherHb.LaneHash != this.LaneHash) { continue; }

                if (this.Intersects(otherHb))
                {
                    this.Parent.HitboxCallback(otherEntity, otherHb, this.Tag, otherHb.Tag);
                }
            }
        }
    }

    public bool Intersects(Hitbox other)
    {
        Vector2 thisPos = this.GetParentPosition() + this.Offset;
        Vector2 otherPos = other.GetParentPosition() + other.Offset;
        return (
            thisPos.X < otherPos.X + other.Size.X &&
            thisPos.X + this.Size.X > otherPos.X &&
            thisPos.Y < otherPos.Y + other.Size.Y &&
            thisPos.Y + this.Size.Y > otherPos.Y
        );
    }

    public bool PointIntersects(Vector2 point)
    {
        Vector2 thisPos = this.GetParentPosition() + this.Offset;
        return (
            thisPos.X < point.X && thisPos.X + this.Size.X > point.X &&
            thisPos.Y < point.Y && thisPos.Y + this.Size.Y > point.Y
        );
    }
}
