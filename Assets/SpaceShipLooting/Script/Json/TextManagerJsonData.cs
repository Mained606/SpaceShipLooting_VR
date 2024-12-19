
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

public class TextManagerJsonData
{
    private static TextManagerJsonData instance;
    public Dictionary<string, String_Table> dicString_Table;
    
    public static TextManagerJsonData GetInstance()
    {
        if(TextManagerJsonData.instance == null)
            TextManagerJsonData.instance = new TextManagerJsonData();
        return TextManagerJsonData.instance;
    }
    public void LoadDatas()
    {
        var String_Table = Resources.Load<TextAsset>("Json/String_Table").text;

        
        
        var arr_String_DataTable = JsonConvert.DeserializeObject<String_Table[]>(String_Table);
  
        /* foreach(var data in arrSkull_DataTable)
        {
            Debug.LogFormat("{0} ",data.skull_index);
        } */
        this.dicString_Table = arr_String_DataTable.ToDictionary(x => x.string_index);
 
       

    }
}


