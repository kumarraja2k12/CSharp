using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSVManager
{
    class Program
    {

        private static Dictionary<string, int> columns = new Dictionary<string, int>
            {
                { "name", -1 },
                { "type", -1 },
                { "phone", -1 },
                { "state", -1 }
            };

        static void Main(string[] args)
        {

            string filePath = "../../../1sheet.csv";
            string filePath2 = "../../../2sheet.csv";

            string stateName = "Florida";

            List<UserModel> modelsInSheet1 = GetUserModels(filePath, stateName);
            List<UserModel> modelsInSheet2 = GetUserModels(filePath2, stateName);

            var result = modelsInSheet1.Where(x => modelsInSheet2.Any(y => y.Equals(x))).FirstOrDefault();

            if (result != null)
            {
                Console.WriteLine("Same Record in both sheets exists");
            }
            else
            {
                Console.WriteLine("Same Record in both sheets does not exists");
            }

            Console.ReadLine();
        }

        static List<UserModel> GetUserModels(string filePath, string stateName)
        {
            var userModels = new List<UserModel>();

            bool isColumnsIdentified = false;

            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(File.OpenRead(filePath)))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        if (!isColumnsIdentified)
                        {
                            //Header
                            for (int index = 0; index < values.Length; index++)
                            {
                                if (columns.ContainsKey(values[index]))
                                {
                                    columns[values[index]] = index;
                                }
                            }
                            isColumnsIdentified = true;
                        }
                        else
                        {
                            //Data
                            values = ManageData(values);
                            string state = values[columns["state"]];

                            if (state.Equals(stateName))
                            {
                                string name = values[columns["name"]];
                                var phone = values[columns["phone"]];
                                var type = values[columns["type"]];

                                userModels.Add(new UserModel
                                {
                                    Name = name,
                                    Phone = phone,
                                    Type = type,
                                    State = state
                                });
                            }
                            

                        }
                    }
                }
            }

            return userModels;
        }

        static string[] ManageData(string[] data)
        {
            List<string> results = new List<string>();

            int dataIndex = 0;
            int resultsIndex = 0;
            bool isInTheMiddleofAppend = false;
            foreach (string item in data)
            {
                if(item.StartsWith('"'))
                {
                    results.Add(data[dataIndex++]);
                    isInTheMiddleofAppend = true;
                    continue;
                }

                if (isInTheMiddleofAppend)
                {
                    if (item.EndsWith('"'))
                    {
                        results[resultsIndex] = $"{results[resultsIndex]}{data[dataIndex++]}";
                        isInTheMiddleofAppend = false;
                        resultsIndex++;
                        continue;
                    }
                    else
                    {
                        results[resultsIndex] = $"{results[resultsIndex]}{data[dataIndex++]}";
                        isInTheMiddleofAppend = true;
                        continue;
                    }
                }

                results.Add(data[dataIndex++]);
                resultsIndex++;
            }

            return results.ToArray();
        }
    }
}
