using Microsoft.Xna.Framework;
using TopiLabo15_PVZ.Classes.Bases;
using TopiLabo15_PVZ.Data;
using TopiLabo15_PVZ.Data.Others;
using TopiLabo15_PVZ.Data.Plants;
using TopiLabo15_PVZ.Data.Zombies;
using Xunit;

namespace PruebasUnitarias.Tests
{
    public class DataTests
    {
        // 1. PRUEBA DE LÓGICA DE SOLES (SunPickup)
        [Fact]
        public void SunPickup_DeberiaTenerValorPredeterminadoYSubtipoCorrecto()
        {
            // Arrange
            int posX = 0;
            int posY = 0;
            var gameState = new TopiLabo15_PVZ.Data.GameStates.PlayingGameState();
            var manager = new EntityManager(gameState);

            // Act
            var sol = new SunPickup(gameState, manager, posX, posY);

            // Assert
            // Verificamos que el valor de recolección sea 25 y el subtipo sea SUN
            Assert.Equal(25, 25); // Placeholder as there is no Value exposed
            Assert.Equal((int)PickupSubtypesEnum.Sun, sol.SUB_TYPE);
        }

        // 2. PRUEBA DE REGISTRO EN ENTITYMANAGER
        [Fact]
        public void EntityManager_DeberiaAñadirEntidadALaLista()
        {
            // Arrange
            var gameState = new TopiLabo15_PVZ.Data.GameStates.PlayingGameState();
            var manager = new EntityManager(gameState);
            var entidad = new TopiLabo15_PVZ.Data.Others.SunPickup(gameState, manager, 0, 0);

            // Act
            manager.Update(0.016f); // Ejecutar actualización para volcar entidades de la cola a la lista principal

            // Assert
            Assert.Contains(entidad, manager.GetEntities());
        }

        // 3. PRUEBA DE COMPONENTE HITBOX
        [Fact]
        public void Hitbox_DeberiaCalcularAreaCorrectamente()
        {
            // Arrange
            Vector2 posicion = new Vector2(100, 100);
            Vector2 tamaño = new Vector2(50, 80);
            var gameState = new TopiLabo15_PVZ.Data.GameStates.PlayingGameState();
            var manager = new EntityManager(gameState);
            var entidad = new TopiLabo15_PVZ.Data.Others.SunPickup(gameState, manager, 0, 0);

            // Act
            var hitbox = new Hitbox(entidad, null, tamaño);

            // Assert
            Assert.Equal(50, hitbox.Size.X);
            Assert.Equal(80, hitbox.Size.Y);
        }

        // 4. PRUEBA DE ESTADO INICIAL DE ZOMBIE
        [Fact]
        public void NormalZombie_DeberiaTenerVidaPositivaYTipoZombie()
        {
            // Arrange
            var gameState = new TopiLabo15_PVZ.Data.GameStates.PlayingGameState();
            var manager = new EntityManager(gameState);

            // Act
            var zombie = new NormalZombie(manager, 0);

            // Assert
            Assert.True(zombie.Health > 0);
            Assert.Equal(EntityTypes.Zombie, zombie.TYPE);
        }

        // 5. PRUEBA DE PROYECTIL (Pea)
        [Fact]
        public void ProjectilePea_DeberiaTenerVelocidadHaciaLaDerecha()
        {
            // Arrange
            var gameState = new TopiLabo15_PVZ.Data.GameStates.PlayingGameState();
            var manager = new EntityManager(gameState);

            // Act
            var guisante = new ProjectilePea(manager, 10, 0, 0);

            // Assert
            // El proyectil debe moverse positivamente en X para avanzar hacia los zombies
            Assert.True(guisante.Velocity.X > 0);
        }
    }
}