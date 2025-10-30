// --- Entity.cs ---
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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

    public long FrameCount { get;  set; }
    public double TimeCount { get;  set; }

    // --- Banderas de Ciclo de Vida ---
    // Marca si una entidad ha sido inicializada y por ende se le ha llamado su initCallback().
    public bool IsInited { get; set; } = true; //Si es true, todavia no ha sido iniciado, si no pues no :v (aunque debio haber sido al revez (^^;).

    // Equivale a self._remove (false por defecto)
    public bool IsRemoved { get; private set; } = false;

    // --- Listas de Componentes ---
    public SpriteAnimator Sprite { get; set; }
    public int StateIndex { get; private set; }
    public List<Hitbox> Hitboxes { get; } = new List<Hitbox>();

    // Compatibilidad: referencia al primer hitbox de la lista
    [Obsolete("Usa 'Hitboxes' en su lugar. Esta propiedad referencia el primer hitbox de la lista.")]
    public Hitbox Hitbox
    {
        get => Hitboxes.Count > 0 ? Hitboxes[0] : null;
        set
        {
            if (value == null)
            {
                if (Hitboxes.Count > 0) Hitboxes.RemoveAt(0);
                return;
            }

            // Asegurar la relación padre
            if (value.Parent != this) value.Parent = this;

            if (Hitboxes.Count == 0) Hitboxes.Add(value);
            else Hitboxes[0] = value;
        }
    }

    // Utilidades para gestionar múltiples hitboxes
    public Hitbox AddHitbox(Hitbox hitbox)
    {
        if (hitbox == null) return null;
        if (hitbox.Parent != this) hitbox.Parent = this;
        Hitboxes.Add(hitbox);
        return hitbox;
    }

    public bool RemoveHitbox(Hitbox hitbox)
    {
        if (hitbox == null) return false;
        return Hitboxes.Remove(hitbox);
    }

    public void ClearHitboxes()
    {
        Hitboxes.Clear();
    }

    public void SetUIDOnce(int uid) {
        if (this.UID != -1) { return; }
        this.UID = uid;
    }

    // --- Constructor (Equivale a Entity:new) ---
    //Una entidad no puede crearse sin un EntityManager que la gestione.
    //Por lo que solo se pueden instanciar entidades desde un EntityManager.
    public Entity(EntityManager manager, EntityTypes type, int? subtype, Vector2? position, Vector2? velocity, Entity? spawner)
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
        if (this.Sprite != null) { 
            // Llama al callback antes de dibujar sprites
            this.PreSpriteCallback(spriteBatch);

            this.DrawSpriteCallback(spriteBatch); 

            // Llama al callback después de dibujar sprites
            this.PostSpriteCallback(spriteBatch);
        }
    }

    // --- Gestión de Eliminación ---
    // Equivalente a 'entity:remove()'
    public void Remove()
    {
        if (IsRemoved) return; // No eliminar dos veces

        this.ClearHitboxes();

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

    public virtual void PreSpriteCallback(SpriteBatch spriteBatch) { } // Llamado antes de la lógica de dibujo de sprites
    public virtual void DrawSpriteCallback(SpriteBatch spriteBatch) { this.Sprite.Draw(spriteBatch, this.Position); }
    public virtual void PostSpriteCallback(SpriteBatch spriteBatch) { } // Llamado después de la lógica de dibujo de sprites

    public virtual void OnRemove() { } // Llamado cuando la entidad se marca para eliminar (IsRemoved = true)

    public virtual void RemovedCallback() { } // Llamado por el EntityManager justo antes de ser purgado de la lista

    public virtual void HitboxCallback(Entity other, Hitbox otherHitbox, string tag, string otherTag) { } // Llamado cuando esta entidad colisiona con otra
}