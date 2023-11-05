using Lunacy.Core;
using OpenTK.Mathematics;

namespace Lunacy.Components;

public class Camera : Component
{
    public float fov;
    public float nearClipDistance = 0.01f;
    public float farClipDistance = 1000f;

    public Camera(float fov = 70)
    {
        this.fov = fov;
    }
    
    public Matrix4 GetProjectionMatrix()
    {
        float hFov = fov * MathF.PI / 180;
        float vFov = 2 * MathF.Atan(MathF.Tan(hFov / 2) / LunacyEngine.GetAspectRatio());
        Matrix4 result;
        Matrix4.CreatePerspectiveFieldOfView(vFov, LunacyEngine.GetAspectRatio(), nearClipDistance, farClipDistance, out result);
        return result;
    }

    public Matrix4 GetViewMatrix()
    {
        float pitch = gameObject.rotation.X;
        float yaw = gameObject.rotation.Y;
        float roll = gameObject.rotation.Z;

        Vector3 front;
        front.Z = (float)Math.Cos(pitch) * (float)Math.Cos(yaw);
        front.Y = (float)Math.Sin(pitch);
        front.X = (float)Math.Cos(pitch) * (float)Math.Sin(yaw);

        return Matrix4.LookAt(gameObject.location, gameObject.location + front, Quaternion.FromAxisAngle(front, roll) * Vector3.UnitY);
    }
}