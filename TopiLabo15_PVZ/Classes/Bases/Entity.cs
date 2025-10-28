// --- Entity.cs ---
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using TopiLabo15_PVZ.Data;

// Equivalente a 'Entity: Object'
public abstract class Entity
{
    // --- Propiedades Públicas (Gestión Interna) ---

    // Referencia al manager que lo creó.
    public EntityManager Manager { get; private set; }

    // ID único en la lista de entidades del EntityManager.
    private int UID = -1;
    public int GetUID() => UID;

    // Tipo de entidad (ej. "Plant", "Zombie", "Pickup", "UI", etc.)
    public readonly EntityTypes TYPE;
    public readonly int? SUB_TYPE;

    // El que nos "disparó" o creó (ej. el jugador que dispara una bala)
    public Entity Spawner { get; private set; }

    // --- Propiedades de Estado y Física ---
    public Vector2 Position; // Equivale a self.position
    public Vector2 Velocity; // Equivale a self.velocity

    public long FrameCount { get; private set; }
    public double TimeCount { get; private set; }

    // --- Banderas de Ciclo de Vida ---
    // Marca si una entidad ha sido inicializada y por ende se le ha llamado su initCallback().
    public bool IsInited { get; set; } = true;

    // Equivale a self._remove (false por defecto)
    public bool IsRemoved { get; private set; } = false;

    // --- Listas de Componentes ---
    public SpriteAnimator Sprite { get; set; }
    public int StateIndex { get; private set; }
    public Hitbox Hitbox { get; set; }

    public void SetUIDOnce(int uid) {
        if (this.UID != -1) { return; }
        this.UID = uid;
    }

    // --- Constructor (Equivale a Entity:new) ---
    //Una entidad no puede crearse sin un EntityManager que la gestione.
    //Por lo que solo se pueden instanciar entidades desde un EntityManager.
    public Entity(EntityManager manager, EntityTypes type, int? subtype, Vector2? position, Vector2? velocity, Entity spawner)
    {
        this.Manager = manager;
        this.TYPE = type;
        this.SUB_TYPE = subtype;
        this.Position = position ?? Vector2.Zero; // Si position es nulo, usa Vector2.Zero
        this.Velocity = velocity ?? Vector2.Zero; // x2
        this.Spawner = spawner;

        this.FrameCount = 0;
        this.TimeCount = 0.0;

        manager.SpawnInstance(this);
    }

    // --- Lógica Principal de Actualización ---

    public virtual void GenericUpdate(float dt)
    {
        this.FrameCount++;
        this.TimeCount += dt;

        this.UpdateCallback(dt);

        this.Sprite?.Update(dt);
    }

    public virtual void ApplyPhysics(float dt)
    {
        Position += Velocity * dt;
        //Debug.WriteLine(this.FrameCount); // Para probar que se está llamando solo una vez por frame.
    }

    // --- Lógica de Dibujo ---
    // (Esto no estaba en entity.lua, pero es necesario en MonoGame
    // para reemplazar el obsoleto 'drawEntities' del EntityManager)
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        // Llama al callback antes de dibujar sprites
        this.PreSpriteCallback();

        if (this.Sprite != null) { this.DrawSpriteCallback(spriteBatch); }

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
    public virtual void InitCallback() { } // Llamado una vez al inicializarce, en el primer frame de 'Update'. Aquí puede ir la lógica de configuración inicial, pero algunos componentes como el SpriteAnimator deben inicializarce aqui y no en el constructor.

    public virtual void SpawnedCallback(Entity spawner) { } // Llamado después de Init, si 'Spawner' no es nulo. Se utilizas cuando una entidad crea otra.

    public virtual void PreUpdateCallback(float dt) { } // Llamado justo antes de 'GenericUpdate'
    public virtual void UpdateCallback(float dt) { } // Llamado cada frame en 'GenericUpdate'. Aquí puede ir la lógica principal de la entidad.
    public virtual void PostUpdateCallback(float dt) { } // Llamado justo después de 'GenericUpdate'

    public virtual void PrePhysicsCallback(float dt) { } // Llamado justo antes de 'ApplyPhysics'
    public virtual void PostPhysicsCallback(float dt) { } // Llamado justo después de 'ApplyPhysics'

    public virtual void PreSpriteCallback() { } // Llamado antes de la lógica de dibujo de sprites
    public virtual void DrawSpriteCallback(SpriteBatch spriteBatch) { this.Sprite.Draw(spriteBatch, this.Position); }
    public virtual void PostSpriteCallback() { } // Llamado después de la lógica de dibujo de sprites

    public virtual void OnRemove() { } // Llamado cuando la entidad se marca para eliminar (IsRemoved = true)

    public virtual void RemovedCallback() { } // Llamado por el EntityManager justo antes de ser purgado de la lista

    public virtual void HitboxCallback(Entity other) { } // Llamado cuando esta entidad colisiona con otra
}