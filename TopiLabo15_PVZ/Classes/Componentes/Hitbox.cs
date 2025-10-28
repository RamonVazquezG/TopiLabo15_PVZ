using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
public class Hitbox
{
    public Entity Parent;

    public int? LaneHash = null; //Si es null, colisiona con todas las lanes. Si no es null, solo colisiona con hitboxes del mismo laneHash.

    public Vector2 Size;
    public Vector2 Offset;

    public Hitbox(Entity parentEntity, int? laneHash, Vector2 Size, Vector2 Offset)
    {
        this.Parent = parentEntity;

        this.LaneHash = laneHash;

        this.Size = Size;
        this.Offset = Offset;
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
            if (otherEntity.Hitbox == null) { continue; }
            if (this.LaneHash != null && otherEntity.Hitbox.LaneHash != this.LaneHash) { continue; }

            if (this.Intersects(otherEntity.Hitbox))
            {
                this.Parent.HitboxCallback(otherEntity);
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
