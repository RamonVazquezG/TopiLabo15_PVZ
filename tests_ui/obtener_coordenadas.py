import pyautogui
import time

print("Presiona Ctrl+C en la terminal para salir.")
print("Mueve el ratón sobre tu juego para ver las coordenadas...\n")

try:
    while True:
        # Obtiene la posición actual del ratón
        x, y = pyautogui.position()
        # Imprime la posición en la misma línea
        print(f"Posición actual del ratón -> X: {x} | Y: {y}", end="\r")
        time.sleep(0.1)
except KeyboardInterrupt:
    print("\n¡Listo! Anota los números que necesites.")