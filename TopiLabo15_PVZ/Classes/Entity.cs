// --- Entity.cs ---
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

// Equivalente a 'Entity: Object'
public abstract class Entity
{
    // --- Propiedades Públicas (Gestión Interna) ---

    // Referencia al manager que nos creó (equivale a self:getCorrespondingEntityManager())
    public EntityManager Manager { get; private set; }

    // ID único (equivale a self:getUID())
    private readonly int UID;
    public int GetUID() => UID;

    // El que nos "disparó" o creó (ej. el jugador que dispara una bala)
    public Entity Spawner { get; private set; }

    // --- Propiedades de Estado y Física ---
    public Vector2 Position; // Equivale a self.position
    public Vector2 Velocity; // Equivale a self.velocity

    public long FrameCount { get; private set; }
    public double TimeCount { get; private set; }

    // --- Banderas de Ciclo de Vida ---
    // Equivale a self._inited (true por defecto)
    public bool IsInited { get; set; } = true;

    // Equivale a self._remove (false por defecto)
    public bool IsRemoved { get; private set; } = false;

    // --- Listas de Componentes ---
    public SpriteAnimator Sprite { get; private set; }
    public int StateIndex { get; private set; }
    public Hitbox Hitbox { get; private set; }

    // --- Constructor (Equivale a Entity:new) ---
    public Entity(EntityManager manager, int uid, Vector2 position, Vector2? velocity, Entity spawner)
    {
        this.Manager = manager;
        this.UID = uid;
        this.Position = position;
        this.Velocity = velocity ?? Vector2.Zero; // Si velocity es nulo, usa Vector2.Zero
        this.Spawner = spawner;

        this.FrameCount = 0;
        this.TimeCount = 0.0;
    }

    // --- Lógica Principal de Actualización (de entity.lua) ---

    public virtual void GenericUpdate(float dt)
    {
        this.FrameCount++;
        this.TimeCount += dt;

        this.Update(dt);
    }

    // Equivalente a 'entity:applyPhysics(dt)'
    public virtual void ApplyPhysics(float dt)
    {
        // Nota: En MonoGame, es común multiplicar por 'dt' en el 'Update' principal.
        // Pero seguimos tu lógica exacta:
        Position += Velocity * dt;
    }

    // --- Lógica de Dibujo ---
    // (Esto no estaba en entity.lua, pero es necesario en MonoGame
    // para reemplazar el obsoleto 'drawEntities' del EntityManager)
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        // Llama al callback antes de dibujar sprites
        this.PreSpriteCallback();

        this.Sprite.Draw(spriteBatch, this.Position);

        // Llama al callback después de dibujar sprites
        this.PostSpriteCallback();
    }

    // --- Gestión de Eliminación ---
    // Equivalente a 'entity:remove()'
    public void Remove()
    {
        if (IsRemoved) return; // No eliminar dos veces

        this.IsRemoved = true;
        this.OnRemove(); // Llama al trigger
    }

    // --- Callbacks Virtuales (reemplazan 'Entity:setCallback') ---
    // Las clases hijas (como 'Player') pueden hacer 'override' de estos métodos para añadir su propia lógica.
    public virtual void InitCallback() { } // Llamado una vez, en el primer frame de 'Update'

    public virtual void SpawnedCallback(Entity spawner) { } // Llamado después de Init, si 'Spawner' no es nulo. Se utilizas cuando una entidad crea otra.

    public virtual void PreUpdateCallback(float dt) { } // Llamado justo antes de 'GenericUpdate'
    public virtual void Update(float dt) { } // Llamado cada frame en 'GenericUpdate'. Aquí puede ir la lógica principal de la entidad.
    public virtual void PostUpdateCallback(float dt) { } // Llamado justo después de 'GenericUpdate'

    public virtual void PrePhysicsCallback(float dt) { } // Llamado justo antes de 'ApplyPhysics'
    public virtual void PostPhysicsCallback(float dt) { } // Llamado justo después de 'ApplyPhysics'

    public virtual void PreSpriteCallback() { } // Llamado antes de la lógica de dibujo de sprites
    public virtual void PostSpriteCallback() { } // Llamado después de la lógica de dibujo de sprites

    public virtual void OnRemove() { } // Llamado cuando la entidad se marca para eliminar (IsRemoved = true)

    public virtual void RemovedCallback() { } // Llamado por el EntityManager justo antes de ser purgado de la lista
}