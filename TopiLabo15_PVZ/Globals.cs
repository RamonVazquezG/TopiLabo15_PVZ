namespace TopiLabo15_PVZ
{
    public static class Globals
    {
        public const float FRAME = 1.0f / 60.0f;
        public const float HUGE = float.MaxValue;
        public const float EPSILON = 0.0001f; // Un valor muy pequeño para comparaciones de punto flotante. Se llama “epsilon” por tradición matemática: épsilon se usa para representar una cantidad positiva “muy pequeña”. En programación se adoptó para nombrar tolerancias al comparar números en coma flotante. Me lo dijo chatsito :p
        public const float PI = 3.14159265359f;
        public const float TILE_SIZE = 24.0f;
        public const float HALF_TILE_SIZE = TILE_SIZE/2f;
    }
}