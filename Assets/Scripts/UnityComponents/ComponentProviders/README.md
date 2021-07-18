# Component providers

This folder contains MonoBehaviour based classes which can be attached to element on schene (or prefab).
When game started they add corresponding component to ECS world.
Such components should not be used directly in systems.
Instead a separate system should create proper set of components for this entity and remove initial one.

**IPORTANT**

When adding provider to object or prefab ensure all fields in "Value" section are set correctly.