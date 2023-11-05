using ImGuiNET;
using Lunacy.Components;
using Lunacy.Core;
using Lunacy.Renderer;
using Lunacy.Utils;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace SampleGame;

public class ObjectSettingsEditor : Component
{
    private System.Numerics.Vector3 color = System.Numerics.Vector3.One;
    private System.Numerics.Vector3 location;
    private System.Numerics.Vector3 rotation;
    private System.Numerics.Vector3 scale;
    public override void OnUpdate()
    {
        location = new System.Numerics.Vector3(gameObject.location.X, gameObject.location.Y, gameObject.location.Z);
        rotation = new System.Numerics.Vector3(gameObject.rotation.X * 180 / MathF.PI, gameObject.rotation.Y * 180 / MathF.PI, gameObject.rotation.Z * 180 / MathF.PI);
        scale = new System.Numerics.Vector3(gameObject.scale.X, gameObject.scale.Y, gameObject.scale.Z);

        
        ImGui.Begin($"Object settings editor ({gameObject.GetName()})");
        ImGui.SetWindowFontScale(2);
        ImGui.ColorEdit3("Albedo", ref color);
        gameObject.GetComponent<MeshRenderer2D>()!.GetShader().SetAlbedo(new Vector4(color.X, color.Y, color.Z, 1));
        ImGui.DragFloat3("Location", ref location, .01f);
        gameObject.location = new Vector3(location.X, location.Y, location.Z);
        ImGui.DragFloat3("Rotation", ref rotation);
        gameObject.rotation = new Vector3(rotation.X * MathF.PI / 180, rotation.Y * MathF.PI / 180, rotation.Z * MathF.PI / 180);
        ImGui.DragFloat3("Scale", ref scale, .01f);
        gameObject.scale = new Vector3(scale.X, scale.Y, scale.Z);
        ImGui.End();
    }
}