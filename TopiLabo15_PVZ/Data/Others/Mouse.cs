using Microsoft.Xna.Framework;


namespace TopiLabo15_PVZ.Data.Others
{
    //Esta entidad utiliza la posicion de MouseInput para colisionar con los SunPickups y la barra de plantas.
    public class Mouse : Entity
    {
        public Mouse(EntityManager manager)
            : base(manager, EntityTypes.Mouse, null, MouseInput.Position, null, null)
        {
            this.Hitbox = new Hitbox(this, null, Vector2.Zero, Vector2.Zero); //Si es posible que un hitbox colisione con un tamaño de 0.
        }

        public override void UpdateCallback(float dt)
        {
            this.Position = MouseInput.Position;
        }
    }
}
