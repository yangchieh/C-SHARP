using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;

namespace XmlSchemaParser
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. 檢查參數
            if (args.Length == 0)
            {
                Console.WriteLine("請提供 XML 檔案路徑。範例：dotnet run schema.xml");
                return;
            }
            string filePath = args[0];
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"找不到檔案: {filePath}");
                return;
            }
            try
            {
                // 2. 載入 XML
                XDocument doc = XDocument.Load(filePath);
                // 3. 取得系統名稱 (根節點 SYSTEMS 的 NAME 屬性)
                string rootSystemName = doc.Root?.Attribute("NAME")?.Value;
                Console.WriteLine($"主系統名稱: {rootSystemName}");
                // 4. 解析每一個 SYSTEM 節點
                var systems = doc.Descendants("SYSTEM");
                foreach (var system in systems)
                {
                    string sysName = system.Attribute("NAME")?.Value;
                    Console.WriteLine($"\n[系統模組: {sysName}]");
                    // 5. 解析 CLASS 節點
                    var classes = system.Descendants("CLASS");
                    foreach (var cls in classes)
                    {
                        string className = cls.Attribute("NAME")?.Value;
                        string classTitle = cls.Attribute("TITLE")?.Value;
                        Console.WriteLine($"\n  ? 類別: {className} ({classTitle})");
                        // 6. 解析 FIELD 節點
                        var fields = cls.Elements("FIELD");
                        foreach (var field in fields)
                        {
                            string fieldName = field.Attribute("NAME")?.Value;
                            string fieldTitle = field.Attribute("TITLE")?.Value;
                            string fieldType = field.Attribute("TYPE")?.Value;
                            string fieldSize = field.Attribute("SIZE")?.Value;
                            Console.WriteLine($"     - 欄位: {fieldName,-15} | {fieldTitle,-20} | 型別: {fieldType}({fieldSize})");
                            // 7. 檢查是否有列舉值 (VALUE 節點)
                            var values = field.Elements("VALUE");
                            foreach (var val in values)
                            {
                                string vName = val.Attribute("NAME")?.Value;
                                string vTitle = val.Attribute("TITLE")?.Value;
                                Console.WriteLine($"        └ 設定值: {vName} = {vTitle}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"解析錯誤: {ex.Message}");
            }
        }
    }
}