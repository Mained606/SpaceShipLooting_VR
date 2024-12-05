using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class KeyPadUI : MonoBehaviour, ISignal
{
    public UnityEvent<string> OnSignal { get; } = new UnityEvent<string>();

    [SerializeField]
    private string correctCode = "1945"; // 키패드 비밀번호 값

    private string currentInput = ""; // 현재 입력된 값
    private TextMeshProUGUI displayText; // 입력된 숫자를 보여줄 텍스트

    private void Start()
    {
        // TextMeshProUGUI 컴포넌트를 자동으로 찾아서 참조
        displayText = GetComponentInChildren<TextMeshProUGUI>();

        if (displayText == null)
        {
            Debug.LogError("No TextMeshProUGUI component found in children!");
        }
    }

    public void OnButtonPressed(string number)
    {
        currentInput += number;

        // 입력값을 디스플레이에 간격 있는 형식으로 표시
        if (displayText != null)
        {
            displayText.text = FormatInputWithSpaces(currentInput);
        }

        // 입력값이 정답과 일치하면 신호 발송
        if (currentInput == correctCode)
        {
            OnSignal.Invoke("CodeMatched"); // 신호 발송
            if (displayText != null)
            {
                displayText.text = "SUCCESS!"; // 성공 메시지 출력
            }
            ClearInput(); // 입력 초기화
        }
    }

    // 입력값 초기화
    public void ClearInput()
    {
        currentInput = "";
        if (displayText != null)
        {
            displayText.text = ""; // 디스플레이 초기화
        }
        Debug.Log("Input cleared.");
    }

    // 숫자 사이에 간격 추가하는 메서드
    private string FormatInputWithSpaces(string input)
    {
        return string.Join(" ", input.ToCharArray());
    }

    public void ClearListeners()
    {
        OnSignal.RemoveAllListeners(); // 모든 이벤트 리스너 제거
    }

    public GameObject GetGameObject()
    {
        return gameObject; // 현재 오브젝트 반환
    }
}
