// --- EntityManager.cs ---
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq; // Necesario para .ToList()

public class EntityManager
{
    // Equivale a 'self.list' (una tabla UID -> Entidad)
    private Dictionary<int, Entity> entities;

    // Listas temporales para gestionar el ciclo de vida de forma segura
    private List<Entity> spawnedThisFrame;
    private List<int> removedThisFrame;

    // Equivale a 'self.nextUID'
    private int nextUID;

    // (No necesitamos 'iDidSpawn' debido a cómo manejamos la lista 'spawnedThisFrame')

    // --- Constructor (Equivale a EntityManager:new) ---
    public EntityManager()
    {
        entities = new Dictionary<int, Entity>();
        spawnedThisFrame = new List<Entity>();
        removedThisFrame = new List<int>();
        nextUID = 0;
    }

    // Equivale a 'pollNextUID' (asumiendo incremento simple)
    private int PollNextUID()
    {
        nextUID++;
        return nextUID;
    }

    // Equivale a 'getEntity'
    public Entity GetEntity(int uid)
    {
        if (entities.TryGetValue(uid, out Entity entity))
        {
            return entity;
        }
        return null;
    }

    // Devuelve todas las entidades, para el bucle de Dibujo (Draw)
    public IEnumerable<Entity> GetEntities()
    {
        return entities.Values;
    }

    // --- Equivalente a 'spawn' (¡Usando Genéricos!) ---
    // 'T' es el tipo de entidad a crear (ej. Player, Bullet)
    // 'where T : Entity' significa que 'T' DEBE ser una clase que hereda de 'Entity'.
    [System.Obsolete("Este metodo está obsoleto. Usa SpawnInstance().")]
    public T Spawn<T>(Vector2? position, Vector2? velocity = null, Entity spawner = null) where T : Entity
    {
        int uid = PollNextUID();

        // Esto busca un constructor en la clase 'T' que coincida con los argumentos
        // (manager, uid, position, velocity, spawner)
        object[] args = { this, uid, position, velocity, spawner };
        T instance = (T)Activator.CreateInstance(typeof(T), args);

        // En lugar de añadir directamente a 'entities', lo añadimos a la cola
        // para evitar errores si esto se llama *durante* un bucle de 'Update'.
        spawnedThisFrame.Add(instance);

        return instance;
    }

    public void SpawnInstance(Entity entity)
    {
        entity.SetUIDOnce(PollNextUID());
        spawnedThisFrame.Add(entity);
    }


    // --- Equivalente a 'updateEntityLogic' ---
    // Se llama una vez por frame desde tu clase principal del juego.
    public void Update(float dt)
    {
        // NOTA: Usamos .ToList() para crear una copia temporal de la lista
        // de entidades. Esto nos permite añadir/quitar entidades de la
        // lista 'entities' de forma segura *durante* el bucle (ej. una
        // entidad 'spawnea' a otra).
        foreach (var entity in entities.Values.ToList())
        {
            // 1. Manejar eliminación
            if (entity.IsRemoved)
            {
                removedThisFrame.Add(entity.GetUID());
                continue; // Saltar el resto de la lógica
            }

            // 2. Manejar inicialización (equivale a 'initEntity(entity)')
            if (entity.IsInited)
            {
                entity.InitCallback();
                if (entity.Spawner != null)
                {
                    entity.SpawnedCallback(entity.Spawner);
                }
                entity.IsInited = false;
            }

            // 3. Ejecutar lógica (equivale a 'updateSingleEntityLogic(dt, entity)')
            entity.PreUpdateCallback(dt);
            entity.GenericUpdate(dt);
            entity.PostUpdateCallback(dt);

            entity.PrePhysicsCallback(dt);
            entity.ApplyPhysics(dt);
            entity.PostPhysicsCallback(dt);
        }

        // --- Limpieza Post-Actualización ---
        // Ahora que el bucle terminó, hacemos la limpieza de forma segura.

        // 1. Purgar entidades eliminadas
        foreach (int uid in removedThisFrame)
        {
            if (entities.TryGetValue(uid, out Entity entity))
            {
                entity.RemovedCallback(); // Callback final de limpieza
                entities.Remove(uid);
            }
        }
        removedThisFrame.Clear();

        // 2. Añadir nuevas entidades
        foreach (Entity newEntity in spawnedThisFrame)
        {
            entities[newEntity.GetUID()] = newEntity;
        }
        spawnedThisFrame.Clear();
    }

    public void HitboxUpdate()
    {
        foreach (var pair in entities)
        {
            Entity entity = pair.Value;
            if (entity.Hitbox != null)
            {
                //Debug.WriteLine("Checking hitbox for entity UID: " + entity.GetUID());
                entity.Hitbox.CheckHitboxHash(entities);
            }
        }
    }
}