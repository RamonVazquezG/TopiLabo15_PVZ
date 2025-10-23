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
    public int UID { get; private set; }

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
    // (Usamos el SpriteAnimator que creamos en el paso anterior)
    public List<SpriteAnimator> Sprites { get; private set; }
    public List<IState> States { get; private set; }
    public List<Hitbox> Hitboxes { get; private set; }

    // --- Constructor (Equivale a Entity:new) ---
    public Entity(EntityManager manager, int uid, Vector2 position, Vector2? velocity, Entity spawner)
    {
        this.Manager = manager;
        this.UID = uid;
        this.Position = position;
        this.Velocity = velocity ?? Vector2.Zero; // Si velocity es nulo, usa Vector2.Zero
        this.Spawner = spawner;

        // Inicializar las listas de componentes
        this.Sprites = new List<SpriteAnimator>();
        this.States = new List<IState>();
        this.Hitboxes = new List<Hitbox>();

        this.FrameCount = 0;
        this.TimeCount = 0.0;
    }

    // --- Lógica Principal de Actualización (de entity.lua) ---

    // Equivalente a 'entity:statesUpdate(dt)'
    public virtual void StatesUpdate(float dt)
    {
        this.FrameCount++;
        this.TimeCount += dt;

        foreach (var state in States)
        {
            state.Update(dt);
        }
    }

    // Equivalente a 'entity:applyPhysics(dt)'
    public virtual void ApplyPhysics(float dt)
    {
        // Nota: En MonoGame, es común multiplicar por 'dt' en el 'Update' principal.
        // Pero seguimos tu lógica exacta:
        Position += Velocity * dt;
        // Aquí iría la lógica de colisión con muros
    }

    // --- Lógica de Dibujo ---
    // (Esto no estaba en entity.lua, pero es necesario en MonoGame
    // para reemplazar el obsoleto 'drawEntities' del EntityManager)
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        // Llama al callback antes de dibujar sprites
        this.PreSpriteCallback();

        foreach (var sprite in Sprites)
        {
            // Dibuja el sprite en la posición de esta entidad
            sprite.Draw(spriteBatch, this.Position);
        }

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
    // Las clases hijas (como 'Player') pueden hacer 'override' de estos
    // métodos para añadir su propia lógica.

    // Llamado una vez, en el primer frame de 'Update'
    public virtual void InitCallback() { }

    // Llamado después de Init, si 'Spawner' no es nulo
    public virtual void SpawnedCallback(Entity spawner) { }

    // Llamado justo antes de 'StatesUpdate'
    public virtual void PreStateCallback(float dt) { }

    // Llamado justo después de 'StatesUpdate'
    public virtual void PostStateCallback(float dt) { }

    // Llamado justo antes de 'ApplyPhysics'
    public virtual void PrePhysicsCallback(float dt) { }

    // Llamado justo después de 'ApplyPhysics'
    public virtual void PostPhysicsCallback(float dt) { }

    // Llamado antes de la lógica de dibujo de sprites
    public virtual void PreSpriteCallback() { }

    // Llamado después de la lógica de dibujo de sprites
    public virtual void PostSpriteCallback() { }

    // Llamado cuando la entidad se marca para eliminar (IsRemoved = true)
    public virtual void OnRemove() { }

    // Llamado por el EntityManager justo antes de ser purgado de la lista
    public virtual void RemovedCallback() { }
}