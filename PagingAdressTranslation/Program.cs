using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace PagingAdressTranslation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the file name");
            string fileName = Console.ReadLine();
            StreamReader sr = new StreamReader(fileName);
            ulong[] temp = sr.ReadLine().Split().Select(ulong.Parse).ToArray();
            int numberOfStrings = (int)temp[0];
            int numberOfQueries = (int)temp[1];
            ulong rootAddress = temp[2];
            Dictionary<ulong, ulong> map = new Dictionary<ulong, ulong>();
            ulong[] queries = new ulong[numberOfQueries];
            string[] answers = new string[numberOfQueries];
            for (int i = 0; i < numberOfStrings; i++)
            {
                ulong[] arr = sr.ReadLine().Split().Select(ulong.Parse).ToArray();
                map.Add(arr[0], arr[1]);
            }
            Translator translator = new Translator(map, rootAddress);
            for (int i = 0; i < numberOfQueries; i++)
            {
                queries[i] = ulong.Parse(sr.ReadLine());
                answers[i] = translator.GetPhisycalAddress(queries[i]);
            }
            sr.Close();
            StreamWriter sw = new StreamWriter("answers.txt");
            string answer = string.Join(Environment.NewLine, answers);
            sw.Write(answer);
            sw.Close();
        }
    }
}
