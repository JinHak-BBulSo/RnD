using UnityEngine;
using Google.Protobuf.Protocol;
using Google.Protobuf;
using System;
using TMPro;

public class GoogleProtoTester : MonoBehaviour
{
    private TestData testData_;
    byte[] data_;
    [SerializeField] TextMeshProUGUI text_;
    void Start()
    {
        Init();
    }
    public void Init()
    {
        testData_ = new TestData
        {
            Id = 1,
            Hp = 100,
            Level = 999,
            Speed = 10
        };
    }

    public void TestSerialize()
    {
        data_ = testData_.ToByteArray();
        text_.text = BitConverter.ToString(data_);
    }
    public void Deserialize()
    {
        TestDeserialize(data_);
    }
    public void TestDeserialize(byte[] data)
    {
        TestData data_ = TestData.Parser.ParseFrom(data);
        text_.text = $"ID : {data_.Id}, HP : {data_.Hp}, Level : {data_.Level}, Speed : {data_.Speed}";
    }
}

//public class TestData
//{
//    int id;
//    int level;
//    int hp;
//    int speed;
//    public TestData(int id_, int level_, int hp_, int speed_)
//    {
//        id = id_;
//        level = level_;
//        hp = hp_;
//        speed = speed_;
//    }
//}
