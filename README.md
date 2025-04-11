# 🔗 Dot Connect Puzzle Game

## 📁 Overview of Code Structure


- **Dot.cs**: Handles individual dot behavior including color assignment, drag interactions, connection animations, and special tile state.
- **DotConnector.cs**: Manages the user’s drag path, stores valid connections, and processes dot destruction.
- **GameManager.cs**: Handles the grid creation, checks for empty spaces, and refills dots correctly from the top.
- **LineRenderer**: Used to visualize the connections between dots during drag interactions.

---

## 🧠 Design Choices & Assumptions

- **Same-color connections only**: Players can only connect dots of the same color.
- **Adjacency required**: Only directly adjacent dots (up, down, left, right) can be connected — diagonals are not allowed.
- **Minimum 3 connections**: At least three connected dots are required to trigger destruction.
- **refill logic**: New dots fall from the top to fill empty spaces after connections are cleared.
- **Drag-based interaction**: The entire game is playable via dragging from one dot to the next.
- **Clean code principles**: Components are separated by responsibility to ensure readability and maintainability.

---

## ✅ Completed Features

- Procedural grid generation with random colors.
- Tap-to-cycle color (debug/testing).
- Drag-based connection system with visual line feedback.
- Real-time connection validation based on color and position.
- Destruction of connected dots (if count ≥ 3).
- Dot connect animations and scale feedback.
- Grid refill system that ensures all spaces are filled from the top.
