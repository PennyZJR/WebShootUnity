using System.Diagnostics;
using System.IO;
using UnityEditor;

public class GenerateProtobufTool
{
    private static string PROTO_PATH =
        "C:\\Users\\DELL\\Desktop\\Unity\\WebShoot\\Assets\\Protobuf";

    private static string PROTOTOOL_PATH =
        "C:\\Users\\DELL\\Downloads\\APP\\Edge\\protoc-32.1-win64\\bin\\protoc.exe";

    private static string CSHARP_OUT_PATH =
        "C:\\Users\\DELL\\Desktop\\Unity\\WebShoot\\Assets\\Scripts\\Protobuf\\ProtobufMessage";

    [MenuItem("ProtobufTool/GenerateCSharpCode")]
    public static void GenerateCSharp()
    {
        //遍历文件夹
        DirectoryInfo directoryInfo = Directory.CreateDirectory(PROTO_PATH);
        FileInfo[] files = directoryInfo.GetFiles("*.proto");
        for (int i = 0; i < files.Length; i++)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = PROTOTOOL_PATH;
            cmd.StartInfo.Arguments = $" -I={PROTO_PATH} --csharp_out={CSHARP_OUT_PATH} {files[i]}";
            cmd.Start();
            UnityEngine.Debug.Log($"{files[i]}代码生成成功");
        }
    }
}