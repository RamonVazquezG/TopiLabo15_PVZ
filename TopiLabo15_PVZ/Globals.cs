using System;

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

        public static float Lerp(float a, float b, float t)
        {
            t = MathF.Max(MathF.Min(t, 1.0f), 0.0f);
            // Esta es la fórmula de Lerp
            return a + (b - a) * t;
        }

        //Una verison mas estable de lerp que funciona bien con cualquier framerate (en especial si hay muhcos picos de lag).
        public static float ExpDec(float a, float b, float decay, float dt, float lim = 0.0f)
        {
            // 1. if a == b then return b end
            // (Optimizacion para no hacer cálculos si ya estamos en el objetivo)
            if (a == b)
            {
                return b;
            }

            // 2. local result = b+(a-b)*exp(-decay*dt)
            // (La fórmula de decaimiento exponencial)
            // Nota: 'exp' en Lua es 'MathF.Exp' en C# (para floats)
            float result = b + (a - b) * MathF.Exp(-decay * dt);

            // 3. return abs(result-b) < (lim or 0) and b or result
            // (El 'and/or' de Lua se traduce al operador ternario '?:' en C#)
            // 'abs' en Lua es 'MathF.Abs' en C#
            // 'lim or 0' se maneja con el valor por defecto del parámetro 'lim = 0.0f'
            return MathF.Abs(result - b) < lim ? b : result;
        }
    }
}