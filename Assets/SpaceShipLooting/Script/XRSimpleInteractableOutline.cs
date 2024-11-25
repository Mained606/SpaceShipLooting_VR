using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSimpleInteractableOutline : XRSimpleInteractable
{
    [SerializeField]
    private Material outlineMaterial; // 아웃라인 머테리얼

    private Renderer objectRenderer; // 오브젝트의 Renderer
    private Material[] originalMaterials; // 원래 머테리얼 배열

    protected virtual void Start()
    {
        // 오브젝트의 Renderer 컴포넌트 가져오기
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer == null)
        {
            objectRenderer = GetComponentInChildren<Renderer>();
        }

        // 원래 머테리얼 배열 저장
        if (objectRenderer != null)
        {
            originalMaterials = objectRenderer.materials;
        }
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);

        // 호버 상태에서 아웃라인 머테리얼 추가
        if (objectRenderer != null && outlineMaterial != null)
        {
            Material[] newMaterials = new Material[originalMaterials.Length + 1];
            for (int i = 0; i < originalMaterials.Length; i++)
            {
                newMaterials[i] = originalMaterials[i];
            }
            newMaterials[newMaterials.Length - 1] = outlineMaterial;
            objectRenderer.materials = newMaterials;
        }
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);

        // 호버 상태 해제 시 원래 머테리얼로 복구
        if (objectRenderer != null && originalMaterials != null)
        {
            objectRenderer.materials = originalMaterials;
        }
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);

        // 셀렉트시 원래 머테리얼로 복구
        if (objectRenderer != null && originalMaterials != null)
        {
            objectRenderer.materials = originalMaterials;
        }
    }
}
