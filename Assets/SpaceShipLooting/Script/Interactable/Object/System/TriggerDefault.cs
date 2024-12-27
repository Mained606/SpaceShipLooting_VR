using UnityEngine;

public class TriggerDefault : MonoBehaviour
{
    [SerializeField] private string selectedStringIndex; // Inspector에서 입력할 JSON 키값

    void Start()
    {
        // JSON 데이터 로드
        var dataManager = TextManagerJsonData.GetInstance();
        dataManager.LoadDatas();

        // Inspector에서 입력한 키값이 유효하지 않으면 경고 출력
        if (!string.IsNullOrEmpty(selectedStringIndex) && !dataManager.dicString_Table.ContainsKey(selectedStringIndex))
        {
            Debug.LogWarning($"'{selectedStringIndex}'는 JSON에 없는 키에용");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // JSON에서 해당 키의 대사를 출력
        if (!string.IsNullOrEmpty(selectedStringIndex) && TextManagerJsonData.GetInstance().dicString_Table.ContainsKey(selectedStringIndex))
        {
            JsonTextManager.instance.OnDialogue(selectedStringIndex);
        }
        else
        {
            Debug.LogError($"키 '{selectedStringIndex}'는 JSON에 없는데용.");
        }
        this.gameObject.SetActive(false);
    }

}
