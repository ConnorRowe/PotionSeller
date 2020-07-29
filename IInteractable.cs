using Godot;
using System;

public interface IInteractable
{
    // If returns true, free node
    bool Interact(Player player);
    bool HasInteractParticle();
    Particles2D GetInteractParticles();
}
