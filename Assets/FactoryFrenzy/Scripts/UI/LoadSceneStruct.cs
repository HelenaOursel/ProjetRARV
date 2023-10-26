using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PositionStruct
{
    public float PosX, PosY, PosZ;
    public PositionStruct(float PosX, float PosY, float PosZ)
    {
        this.PosX = PosX;
        this.PosY = PosY;
        this.PosZ = PosZ;
    }
}

[Serializable]
public struct RotationStruct
{
    public float RotX, RotY, RotZ, RotW;
    public RotationStruct(float RotX, float RotY, float RotZ, float RotW)
    {
        this.RotX = RotX;
        this.RotY = RotY;
        this.RotZ = RotZ;
        this.RotW = RotW;
    }
}

[Serializable]
public struct PlateformStruct
{
    public int ID;
    public PositionStruct Position;
    public RotationStruct Rotation;
    public string Name;
    public string PrefabName;

    public PlateformStruct(int ID, PositionStruct position, RotationStruct rotation, string Name, string prefabName)
    {
        this.ID = ID;
        this.Position = position;
        this.Rotation = rotation;
        this.Name = Name;
        this.PrefabName = prefabName;
    }
}

[Serializable]
public class Plateform : MonoBehaviour
{
    private PlateformStruct _plateformStruct;
    public PlateformStruct plateformStruct
    {
        get { return _plateformStruct; }
        set
        {
            _plateformStruct = value;
            SetPlateformPosition();
        }
    }

    //public PlateformStruct InitializePlateform(int id, GameObject plateform, string addrName)
    //{
    //    PositionStruct pos = new PositionStruct(plateform.transform.position.x, plateform.transform.position.y, plateform.transform.position.z);
    //    RotationStruct rot = new RotationStruct(plateform.transform.rotation.x, plateform.transform.rotation.y, plateform.transform.rotation.z, plateform.transform.rotation.w);
    //    plateformStruct = new PlateformStruct(id, pos, rot, plateform.name, addrName);
    //    return plateformStruct;
    //}
    public void SetPlateformPosition()
    {
        var position = _plateformStruct.Position;
        var rotation = _plateformStruct.Rotation;
        this.transform.position = new Vector3(position.PosX, position.PosY, position.PosZ);
        this.transform.rotation = new Quaternion(rotation.RotX, rotation.RotY, rotation.RotZ, rotation.RotW);
    }
}