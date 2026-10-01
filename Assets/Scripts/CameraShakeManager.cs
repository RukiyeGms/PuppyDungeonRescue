using Unity.Cinemachine;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance;

    private void Awake()
    {
        instance = null;
        instance = this;
    }

    public void CameraShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulse();
    }
}
