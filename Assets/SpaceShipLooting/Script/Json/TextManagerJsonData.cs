using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

public class TextManagerJsonData
{
    private static TextManagerJsonData instance;
    public Dictionary<string, String_Table> dicString_Table;

    public static TextManagerJsonData GetInstance()
    {
        if (instance == null)
        {
            instance = new TextManagerJsonData();
        }
        return instance;
    }

    public void LoadDatas()
    {
        var stringTableText = Resources.Load<TextAsset>("Json/String_Table")?.text;
        if (stringTableText == null)
        {
            Debug.LogError("String_Table.json 파일을 찾을 수 없습니다.");
            return;
        }

        var settings = new JsonSerializerSettings
        {
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            ContractResolver = new DefaultContractResolver
            {
                DefaultMembersSearchFlags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
            },
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore // IL2CPP 호환성
        };

        try
        {
            var arrStringDataTable = JsonConvert.DeserializeObject<String_Table[]>(stringTableText, settings);
            dicString_Table = new Dictionary<string, String_Table>();

            foreach (var entry in arrStringDataTable)
            {
                dicString_Table[entry.string_index] = entry;
            }
            Debug.Log("JSON 데이터 로드 성공!");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"JSON 파싱 오류: {ex.Message}");
        }
    }
}
