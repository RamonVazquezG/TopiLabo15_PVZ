import unittest
import subprocess
import time
import pyautogui
import os

class TestPVZMouseAutomated(unittest.TestCase):
    
    @classmethod
    def setUpClass(cls):
        # Ruta EXACTA de donde se compila tu juego.
        cls.exe_path = r"C:\Users\Documents\publish\TopiLabo15_PVZ.exe" 
        
        print(f"Iniciando {cls.exe_path}...")
        try:
            cls.game_process = subprocess.Popen(cls.exe_path)
            time.sleep(5) # Damos 5 segundos para que la ventana del juego se abra completamente
        except FileNotFoundError:
            print("Error: No se encontró el ejecutable. Revisa la ruta en 'cls.exe_path'.")
            raise

    # Función auxiliar para hacer clics de forma confiable
    def hacer_clic_seguro(self, x, y, duration=0.8):
        pyautogui.moveTo(x, y, duration=duration)
        pyautogui.mouseDown()
        time.sleep(0.1)
        pyautogui.mouseUp()
        time.sleep(0.5)

    def test_01_seleccionar_y_plantar(self):
        print("Ejecutando prueba 1: Plantar Girasol...")
        
        carta_x = 700 
        carta_y = 275  
        pasto_x = 700 
        pasto_y = 470 
        
        # 1. Seleccionar Girasol
        self.hacer_clic_seguro(carta_x, carta_y)
        # 2. Plantar Girasol
        self.hacer_clic_seguro(pasto_x, pasto_y)
        
        time.sleep(1) 
        self.assertTrue(self.game_process.poll() is None, "El juego crasheó al plantar el girasol.")

    def test_02_recolectar_soles_por_imagen(self):
        print("Ejecutando prueba 2: Recolectar Sol...")
        imagen_sol = 'sol_recorte.png'
        
        if os.path.exists(imagen_sol):
            tiempo_maximo_espera = 15  
            tiempo_inicio = time.time()
            sol_encontrado = False
            
            while time.time() - tiempo_inicio < tiempo_maximo_espera:
                try:
                    ubicacion_sol = pyautogui.locateOnScreen(imagen_sol, confidence=0.8)
                    if ubicacion_sol:
                        punto_central = pyautogui.center(ubicacion_sol)
                        compensacion_caida_y = 50 
                        objetivo_y = punto_central.y + compensacion_caida_y
                        
                        self.hacer_clic_seguro(punto_central.x, objetivo_y, duration=0.15)
                        
                        sol_encontrado = True
                        break 
                except pyautogui.ImageNotFoundException:
                     pass 
                time.sleep(0.5) 
            
            if not sol_encontrado:
                print(f"Tiempo agotado: No apareció ningún sol después de {tiempo_maximo_espera} segundos.")
        else:
            print(f"OMITIDO: No creaste la imagen '{imagen_sol}' en la carpeta de pruebas.")
            
        time.sleep(1)

    def test_03_intentos_plantar_lanza_guisantes(self):
        print("Ejecutando prueba 3: Lógica de costo y colisión del Lanza Guisantes...")
        
        # --- ¡REEMPLAZA ESTAS COORDENADAS CON LAS REALES! ---
        carta_lanza_guisantes_x = 600  # Coordenada X de la carta del lanza guisantes
        carta_lanza_guisantes_y = 275  # Coordenada Y de la carta del lanza guisantes
        
        pasto_girasol_x = 700          # Casilla donde ya hay un girasol (la misma de test_01)
        pasto_girasol_y = 470 
        
        pasto_vacio_x = 600            # Casilla vacía (una casilla atrás)
        pasto_vacio_y = 470
        # ----------------------------------------------------

        # A) Intento fallido por falta de soles (actualmente tienes 50)
        print("  - Intentando plantar sin soles suficientes...")
        self.hacer_clic_seguro(carta_lanza_guisantes_x, carta_lanza_guisantes_y)
        self.hacer_clic_seguro(pasto_vacio_x, pasto_vacio_y)
        time.sleep(1) # Visualmente, no debería haber planta aquí

        # B) Esperar otro sol para llegar a 100
        print("  - Esperando otro sol para completar 100...")
        self.test_02_recolectar_soles_por_imagen() # Reutilizamos la lógica de recolección

        # C) Intento fallido por colisión (ya hay un girasol ahí)
        print("  - Intentando plantar sobre el girasol...")
        self.hacer_clic_seguro(carta_lanza_guisantes_x, carta_lanza_guisantes_y)
        self.hacer_clic_seguro(pasto_girasol_x, pasto_girasol_y)
        time.sleep(1) # Visualmente, el girasol debería seguir ahí y el ratón seguir teniendo la planta

        # D) Intento exitoso (casilla vacía con 100 soles)
        print("  - Plantando en casilla vacía...")
        # (Asumimos que la carta sigue seleccionada tras el fallo anterior. 
        # Si tu juego deselecciona la carta al fallar, descomenta la siguiente línea)
        # self.hacer_clic_seguro(carta_lanza_guisantes_x, carta_lanza_guisantes_y)
        self.hacer_clic_seguro(pasto_vacio_x, pasto_vacio_y)
        
        time.sleep(2)
        self.assertTrue(self.game_process.poll() is None, "El juego crasheó durante la prueba del lanza guisantes.")

    def test_04_prueba_pala(self):
        print("Ejecutando prueba 4: Uso de la pala...")
        
        # --- ¡REEMPLAZA ESTAS COORDENADAS CON LAS REALES! ---
        pala_x = 1050             # Coordenada X del botón de la pala
        pala_y = 275             # Coordenada Y del botón de la pala
        
        pasto_girasol_x = 700    # Casilla donde está el girasol
        pasto_girasol_y = 470 
        
        carta_girasol_x = 700    # Coordenadas de la carta del girasol
        carta_girasol_y = 275
        # ----------------------------------------------------

        # A) Seleccionar la pala y cavar el girasol
        print("  - Cavando el girasol...")
        self.hacer_clic_seguro(pala_x, pala_y)
        self.hacer_clic_seguro(pasto_girasol_x, pasto_girasol_y)
        time.sleep(1) # Visualmente, el girasol debe desaparecer

        # B) Recolectar un sol más (para tener 100 de nuevo y plantar un Lanza Guisantes ahí)
        # Nota: Dependiendo de tu economía, tal vez ya tengas los 100 soles.
        # Si es así, comenta la siguiente línea.
        print("  - Esperando sol para reponer...")
        self.test_02_recolectar_soles_por_imagen()

        # C) Plantar un nuevo Girasol donde estaba el primero
        print("  - Plantando nuevo Girasol en la casilla liberada para salvar la economía...")
        self.hacer_clic_seguro(carta_girasol_x, carta_girasol_y)
        self.hacer_clic_seguro(pasto_girasol_x, pasto_girasol_y)

        time.sleep(3)
        self.assertTrue(self.game_process.poll() is None, "El juego crasheó durante la prueba de la pala.")


    @classmethod
    def tearDownClass(cls):
        print("Pruebas terminadas. Cerrando el juego...")
        if hasattr(cls, 'game_process') and cls.game_process:
            cls.game_process.terminate()

if __name__ == "__main__":
    unittest.main()