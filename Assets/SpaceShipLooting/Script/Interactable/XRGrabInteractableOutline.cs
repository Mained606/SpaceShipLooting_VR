using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableOutline : XRGrabInteractable
{
    // 아웃라인 설정
    [Header("Outline Settings")]
    [Tooltip("아웃라인 사용 여부")]
    [SerializeField] private bool outlineUse = true;
    [Tooltip("오브젝트가 호버 상태일 때 적용할 아웃라인 머티리얼.")]
    [SerializeField] private Material outlineMaterial; // 아웃라인 머테리얼

    // 아웃라인 렌더링
    private Renderer objectRenderer; // 오브젝트의 Renderer
    private Material[] originalMaterials; // 원래 머테리얼 배열

    // 그랩 Attach 포지션
    [Header("Grab Attach Point Settings")]
    [Tooltip("왼손 컨트롤러가 오브젝트를 잡을 때 사용할 위치.")]
    [SerializeField] private Transform leftAttachTransform;
    [Tooltip("오른손 컨트롤러가 오브젝트를 잡을 때 사용할 위치.")]
    [SerializeField] private Transform rightAttachTransform;

    // 레이어
    private int originalLayer;

    private bool isGrabbed = false;

    protected virtual void Start()
    {
        if(outlineUse)
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
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);

        if(!isGrabbed && outlineUse)
        {
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
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);

        if(!isGrabbed && outlineUse)
        {
            // 호버 상태 해제 시 원래 머테리얼로 복구
            if (objectRenderer != null && originalMaterials != null)
            {
                objectRenderer.materials = originalMaterials;
            }
        }
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        isGrabbed = true;

        // Attach Point 셋팅
        if (args.interactorObject.transform.CompareTag("LeftHand"))
        {
            attachTransform = leftAttachTransform;
        }
        else if (args.interactorObject.transform.CompareTag("RightHand"))
        {
            attachTransform = rightAttachTransform;
        }

        // 오브젝트를 잡을 때 레이어를 GrabbedObject로 변경하여 손 레이와 충돌하지 않게 설정
        originalLayer = gameObject.layer; // 원래 레이어 저장
        gameObject.layer = LayerMask.NameToLayer("GrabbedObject");
        
        if(outlineUse)
        {
            // 셀렉트시 원래 머테리얼로 복구
            if (objectRenderer != null && originalMaterials != null)
            {
                objectRenderer.materials = originalMaterials;
            }
        }
        
        base.OnSelectEntering(args);
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        isGrabbed = false;

        // 오브젝트를 놓을 때 원래 레이어로 복구
        gameObject.layer = originalLayer;

        base.OnSelectExiting(args);
    }
}