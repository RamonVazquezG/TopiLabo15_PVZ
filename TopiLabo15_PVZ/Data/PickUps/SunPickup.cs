using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopiLabo15_PVZ.Classes.Bases;
using TopiLabo15_PVZ.Data.GameStates;
using TopiLabo15_PVZ.Data.Zombies;

namespace TopiLabo15_PVZ.Data.Others
{
    public class SunPickup : BoardEntity
    {
        public bool Bounce = false;

        private bool Grabbed = false;
        private const float LIFE_TIME = 25f;
        private SpriteAnimator ShineSprite;
        private PlayingGameState playingGameState;
        public SunPickup(PlayingGameState playingGameState, EntityManager manager, int boardX, int boardY, Entity? spawner = null)
            : base(manager, EntityTypes.Pickup, (int?)PickupSubtypesEnum.Sun, Vector2.Zero, Vector2.Zero, spawner)
        {
            this.playingGameState = playingGameState;

            SetPositionFromBoard(boardX, boardY);

            if (spawner != null) { return; }

            int RANDOM_OFFSER = (int)Globals.TILE_SIZE/2;
            Position.X += Random.Shared.Next(-RANDOM_OFFSER, RANDOM_OFFSER);
            Position.Y += Random.Shared.Next(-RANDOM_OFFSER, RANDOM_OFFSER);

            this.SetZ(-11f * Globals.TILE_SIZE);
            this.ZVelocity = -Globals.TILE_SIZE * 2f;
        }

        public override void InitCallback()
        {
            base.InitCallback();

            Sprite = new SpriteAnimator("sunPickup", "default");
            ShineSprite = new SpriteAnimator("sunPickup", "shine");

            this.Hitbox = new Hitbox(this, null, new Vector2(31f, 31f));

        }

        public override void SpawnedCallback(Entity spawner)
        {
            Bounce = true;
            SetZ(Globals.TILE_SIZE / 2);
            SimpleJump(90f, 400f);
            Velocity.X = Random.Shared.NextSingle() * Globals.TILE_SIZE;
            Velocity.Rotate(Random.Shared.NextSingle() * MathF.PI * 2f);
        }

        public override void UpdateCallback(float dt)
        {
            if (!Grabbed)
            {
                if (Bounce & IsOnGround())
                {
                    ZVelocity =  -ZVelocity * 0.6f;
                    Velocity *= 0.6f;
                }

                ShineSprite.Visible = !ShineSprite.Visible; //Para generar un efecto de transparencia retro. JC: Me gustan este tipo de efectos epilepticos jiji.
                ShineSprite.Update(dt); //Como solo Sprite se actualiza en GenericUpdate(), necesitamos actualizar ShineSprite manualmente.
        
                if (TimeCount >= LIFE_TIME - 5f)
                {
                    Sprite.Visible = FrameCount % 2 == 0; //Aplicando el mismo efecto de "transparencia" pero solo cuando esta a 5 segundos de desaparecer.
                    if (TimeCount >= LIFE_TIME) { Remove(); }
                }
            }
            else
            {
                Sprite.Visible = true;
                ShineSprite.Visible = false;

                ClearHitboxes();

                float CORNER = Globals.TILE_SIZE / 2;
                Position.X = Globals.ExpDec(Position.X, CORNER, 16, dt);
                Position.Y = Globals.ExpDec(Position.Y, CORNER, 11, dt);

                SetZ( Globals.ExpDec(GetZ(), 0f, 13, dt) );
                ZVelocity = 0f;
                Gravity = 0f;

                if (TimeCount >= 0.5f) 
                {
                    playingGameState.IncrementSunCount(25);
                    Remove(); 
                }
            }
        }

        public override void PostPhysicsCallback(float dt)
        {
            if (Hitbox == null) { return; }
            Hitbox.Offset.Y = -GetZ() - Hitbox.Size.Y/2;
        }

        public override void HitboxCallback(Entity other, Hitbox otherHitbox, string tag, string otherTag)
        {
            if (other.TYPE == EntityTypes.Mouse)
            {
                if (!Grabbed) { TimeCount = 0f; }
                Grabbed = true;
            }
        }

        public override void PreSpriteCallback(SpriteBatch spriteBatch)
        {
            base.PreSpriteCallback(spriteBatch);
            this.Sprite.LayerDepth += 0.3f; // Asegura que los soles se dibujen sobre los zombies y plantas
            this.ShineSprite.LayerDepth = this.Sprite.LayerDepth - 0.05f; // Asegura que los soles se dibujen sobre los zombies y plantas
        }
        public override void DrawSpriteCallback(SpriteBatch spriteBatch)
        {
            Vector2 PositionWithZ = new Vector2(Position.X, Position.Y - GetZ());
            ShineSprite.Draw(spriteBatch, PositionWithZ);
            Sprite.Draw(spriteBatch, PositionWithZ);
        }
    }
}
