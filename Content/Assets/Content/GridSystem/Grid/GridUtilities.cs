﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GridUtilities
{
    namespace GridReader
    {
        public static class GridReader
        {
            public static string ReadGrid(this BlockGrid blockGrid)
            {
                var output = "";
                output += blockGrid.shipGrid.transform.name;
                blockGrid.blockList.ForEach(block => output += ReadBlock(block));
                return output;
            }

            private static string ReadBlock(BlockGrid.Block block)
            {
                var orientation = Extensions.GetOrientation(block.block.blockBaseClass.parentClass.transform.localRotation.eulerAngles.z);
                var pos = block.transform.localPosition;
                return $@";{block.block.blockBaseClass.blockID},{pos.x},{pos.y},{Extensions.GetOrientationIndex(orientation)}";
            }
        }
    }
    namespace GridWriter
    {
        public static class GridWriter
        {
            public static List<GameObject> ReadString(string value, out string name, out bool valid)
            {
                List<string> lines = value.Split(';').ToList();
                List<GameObject> blocks = new List<GameObject>();
                var index = 1;
                name = lines.First();
                lines.RemoveAt(0);
                lines.ForEach(line => blocks.Add(ReadLine(line, ref index)));
                var succesful = true;
                blocks.ForEach(block => succesful = succesful ? block != null : false);
                if (PlayerPrefs.Instance.debug10)
                    Debug.Log(succesful ? "write operation succesful" : "write operation unsuccesful");
                valid = succesful;
                return blocks;
            }

            private static GameObject ReadLine(string line, ref int index)
            {
                string[] args = line.Split(',');
                var go = BlockDictionary.Instance[int.Parse(args[0])];
                if (go != null)
                {
                    go = GameObject.Instantiate(go);
                    go.SetActive(false);
                }
                go.name += $@"({index})";
                go.transform.localPosition = new Vector2(float.Parse(args[1]), float.Parse(args[2]));
                var rot = new Quaternion();
                rot.eulerAngles = new Vector3(0f, 0f, Extensions.GetRotation(Extensions.GetOrientationByIndex(int.Parse(args[3]))));
                go.transform.localRotation = rot;
                return go;
            }
        }
    }
    public static class Utilities
    {
        public static string IntToHex(int value)
        {
            return value.ToString("x4");
        }

        public static int HexToInt(string value)
        {
            return int.Parse(value, System.Globalization.NumberStyles.HexNumber);
        }
    }
    public struct Blueprint
    {
        private List<GameObject> _blocks;
        public List<GameObject> blocks => _blocks;

        public string _name;
        public string name => _name;

        public bool _valid;
        public bool valid => _valid;

        public Blueprint(string pureBluePrintString)
        {
            _valid = true;
            _name = "";
            _blocks = new List<GameObject>();
            _blocks = GridWriter.GridWriter.ReadString(pureBluePrintString, out _name, out _valid);
        }
    }
}

public static class StringUtil
{
    public static int key = 22;

    public static int randomRange = 58;


    /// <summary>
    /// Scrambles using an internal key
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static void Scramble(ref string value) => value = PNRGCeasar(value, false);

    /// <summary>
    /// Scrambles back using the internal key
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static void ScrambleBack(ref string value) => value = PNRGCeasar(value, true);

    /// <summary>
    /// Ceasar cipher method using a key and a pseudo random number as shift index
    /// </summary>
    /// <param name="value">String to scramble</param>
    /// <param name="back">Wether to scramble back or not</param>
    /// <returns></returns>
    private static string PNRGCeasar(string value, bool back)
    {
        var buffer = value.ToCharArray();
        for (int i = 0; i < buffer.Length; i++)
        {
            //generates pseudorandom char index
            var random = new System.Random(buffer.Length + i + key).Next() % randomRange;
            //gets letter char index
            var letter = (int)buffer[i];
            //applies or removes the random value based on scrambling mode
            letter += (back ? (-random) : (random));
            //return char index as char
            buffer[i] = (char)letter;
        }
        return new string(buffer);
    }

    public static void CopyToClipboard(this string s) => GUIUtility.systemCopyBuffer = s;

    public static string ReadClipboard() => GUIUtility.systemCopyBuffer;

    public static void ClearClipboard() => "".CopyToClipboard();
}
