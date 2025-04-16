# Rope Physics Simulattion

This project demonstrates a few foundational mechanics for a Unity-based system, including:
- A custom rope physics system that simulates basic rope behavior and constraints.
- Simulates a dynamic rope using Verlet integration and constraint relaxation. Each rope particle is affected by physics, but the system could benefit from further tuning and optimizations.

<p align="center">
    <img src="https://github.com/bugrahanakbulut/rope_physics/blob/main/Resources/rope_movement.gif" alt="animated" />
</p>

---

## How to Use

1. Clone the project repository to your local machine.
2. Open the project in Unity (Unity **6000.0.32f1** built-in render pipeline is used for development).
3. Experiment with the rope physics behavior in the **RopePhysics.Rope** class. The `UpdatePhysics` function applies simulation updates to particles.

---

### Current Progress
This project is in an **early development phase**. The following features are functional:
1. Basic `Transform` movement with keyboard input.
2. Fully functional rope physics system with simple constraints and collision handling.

### Planned Improvements
- **3D Visualization**: Adding visual representations for the rope (line renderer, procedural mesh, etc.).
- **Physics Updates**: The current physics implementation for the rope works but needs refined constraints, collision handling, and stability improvements.
- **Cloth Simulation Mechanics**: Enhance the existing rope physics system to support cloth simulation mechanics. Add physics for two-dimensional surfaces (like flags, curtains, or clothing), with proper collision detection, wind response, and realistic cloth tearing behavior.
---

## Contribution

If you're interested in helping shape this project, feel free to fork the repository and contribute. All contributions and PRs will be reviewed and welcomed!
For feedback, suggestions, or inquiries, feel free to reach out to me.

---

## References
[1] Müller, Matthias, et al. "Position based dynamics." Journal of Visual Communication and Image Representation 18.2 (2007): 109-118.

