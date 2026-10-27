# 💖 Magic Heart — Survival Defense (Unity URP)

🌐 **Language / Idioma:** [English](#-english) | [Español](#-español)

---

## 🇬🇧 English

[![WebGL Demo](https://img.shields.io/badge/WebGL-Play%20Demo-brightgreen?style=for-the-badge&logo=unity)](https://itch.io/embed-upload/18917167?color=000000)
[![Unity](https://img.shields.io/badge/Unity-2022.3%20LTS-black?style=for-the-badge&logo=unity)](https://unity.com/)
[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)

Action-arcade survival defense game developed in **Unity 3D** utilizing the **Universal Render Pipeline (URP)**, optimized for seamless execution on WebGL browsers and mobile touch devices.

### 🛠️ Key Systems & Technical Architecture

#### 1. Dynamic Wave Generation System
* Spawning algorithm based on mathematical progressive difficulty scaling over time.
* Active enemy density control to maintain stable framerates (FPS) during peak wave cycles.

#### 2. Shader Graph & Procedural Game Feel
* **Ice Slowdown Shader:** Custom visual shader applying freezing and speed reduction visual feedback.
* **Magma Damage-over-Time (DOT):** Custom thermal pulse shaders for continuous damage areas.
* **Juice & Game Feel:** Procedural squash and stretch transform scaling calculated in code for hit feedback without animation clip overhead.

#### 3. Audio Architecture & Input Handling
* **Multi-channel AudioMixer:** Centralized bus management (Music, SFX, UI) with dynamic ducking and pause filters.
* **Hybrid Input System:** Optimized Raycast projection unifying mobile touch interfaces and desktop mouse pointers.

### 🎮 Controls & Instructions

* **Mouse / Touch Screen:** Tap or drag on screen targets to deploy defensive skills and eliminate enemies.
* **Pause:** Top UI menu button to pause the game loop and adjust master volume.

### 📦 Local Setup & Build

1. Open **Unity 2022.3 LTS** or higher.
2. Clone this repository:
   ```bash
   git clone [https://github.com/MarketablePlushiee/Magic-Heart-Tower-Defense.git](https://github.com/MarketablePlushiee/Magic-Heart-Tower-Defense.git)
   ```
3. Open the main scene at `Assets/Scenes/MainGame.unity`.
4. To build for browser, select `File > Build Settings > WebGL`.

### 👨‍💻 Developer Contact

* **Ivan Latrille Brito** — [ivan.latrille1@gmail.com](mailto:ivan.latrille1@gmail.com)
* **Portfolio:** [marketableplushiee.github.io/Portafolio](https://marketableplushiee.github.io/Portafolio/)

---

## 🇪🇸 Español

[![Demo WebGL](https://img.shields.io/badge/WebGL-Jugar%20Demo-brightgreen?style=for-the-badge&logo=unity)](https://itch.io/embed-upload/18917167?color=000000)
[![Unity](https://img.shields.io/badge/Unity-2022.3%20LTS-black?style=for-the-badge&logo=unity)](https://unity.com/)
[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/es-es/dotnet/csharp/)

Juego de defensa y supervivencia arcade desarrollado en **Unity 3D** utilizando el **Universal Render Pipeline (URP)**, diseñado y optimizado para ejecución fluida en navegadores (WebGL) y dispositivos táctiles/móviles.

### 🛠️ Sistemas Clave y Arquitectura Técnica

#### 1. Sistema de Generación Dinámica de Oleadas
* Algoritmo de *spawning* basado en curvas de dificultad matemática progresivas.
* Control de densidad de enemigos por segundo para optimizar la tasa de cuadros por segundo (FPS).

#### 2. Shader Graph y Game Feel Procedimental
* **Ice Slowdown Shader:** Shader personalizado para ralentización visual y congelamiento de unidades.
* **Magma Damage-over-Time (DOT):** Shaders de pulso térmico para áreas de daño continuo.
* **Juice & Game Feel:** Animación procedimental de compresión y estiramiento (*squash and stretch*) calculada por código para feedback de impacto.

#### 3. Arquitectura de Audio e Inputs
* **AudioMixer Multicanal:** Control centralizado de buses de sonido (Música, SFX, UI) con atenuación y filtros de pausa dinámicos.
* **Sistema de Entrada Híbrido:** Proyección de Raycast optimizada para respuesta unificada en pantallas táctiles y mouse.

### 🎮 Controles e Instrucciones

* **Mouse / Pantalla Táctil:** Presionar o arrastrar sobre los objetivos en pantalla para desplegar habilidades defensivas.
* **Pausa:** Botón en interfaz superior para detener el ciclo del juego y ajustar volumen.

### 📦 Ejecución y Despliegue Local

1. Abrir **Unity 2022.3 LTS** o superior.
2. Clonar el repositorio:
   ```bash
   git clone [https://github.com/MarketablePlushiee/Magic-Heart-Tower-Defense.git](https://github.com/MarketablePlushiee/Magic-Heart-Tower-Defense.git)
   ```
3. Cargar la escena principal en `Assets/Scenes/MainGame.unity`.
4. Para compilar a WebGL, seleccionar `File > Build Settings > WebGL`.

### 👨‍💻 Desarrollador

* **Ivan Latrille Brito** — [ivan.latrille1@gmail.com](mailto:ivan.latrille1@gmail.com)
* **Portafolio:** [marketableplushiee.github.io/Portafolio](https://marketableplushiee.github.io/Portafolio/)
