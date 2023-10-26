using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LoadSceneJSON : MonoBehaviour
{

    string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/FactoryFrenzy/";
    [SerializeField] Transform LevelContainer;


    // Start is called before the first frame update
    void Start()
    {
        this.LoadScene("Level.json");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadScene(string levelName)
    {
        string FilePath = path + levelName + ".json";
        if (File.Exists(FilePath))
        {
            string file = File.ReadAllText(path + levelName + ".json");
            Objectwrapper wrapper = JsonUtility.FromJson<Objectwrapper>(file);

            List<PlateformStruct> plateformStructs = wrapper.datas;
            if (plateformStructs == null)
            {
                return;
            }
            foreach (var plateform in plateformStructs)
            {
                StartCoroutine(InstantiatePlateform(plateform));
            }
        }
        else
        {
            return;
        }
    }

    private IEnumerator InstantiatePlateform(PlateformStruct plateform)
    {
        Vector3 pos = new Vector3(plateform.Position.PosX, plateform.Position.PosY, plateform.Position.PosZ);
        Quaternion rot = new Quaternion(plateform.Rotation.RotX, plateform.Rotation.RotY, plateform.Rotation.RotZ, plateform.Rotation.RotW);
        var plateHandle = Addressables.LoadAssetAsync<GameObject>(plateform.PrefabName);
        Debug.Log(plateform.PrefabName);
        yield return new WaitUntil(() => plateHandle.IsDone);
        if (plateHandle.Result != null)
        {
            Instantiate(plateHandle.Result, pos, rot, LevelContainer);
            Debug.Log($"Successfully instantiated GameObject with name '{plateHandle.Result.name}'.");
        }
        else { Debug.Log("No addressable"); }
    }
}

public class Objectwrapper
{
    public List<PlateformStruct> datas;
}
