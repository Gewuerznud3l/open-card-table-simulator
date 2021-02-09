using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class LoadCards : MonoBehaviour
{
    public List<StackConfig> Stacks;
    public List<string> Cards;
    public List<Sprite> Backsides;
    public List<List<int>> Decks;
    IMG2Sprite i2s;

    // Start is called before the first frame update
    void Start()
    {
        i2s = new IMG2Sprite();
        Stacks = new List<StackConfig>();
        Decks = new List<List<int>>();
        string folder = Directory.GetCurrentDirectory() + "\\Cards";
        string[] paths = Directory.GetFiles(folder, "*.json");
        foreach (string path in paths)
        {
            StreamReader reader = new StreamReader(path);
            string config = reader.ReadToEnd();
            StackConfig stack = JsonUtility.FromJson<StackConfig>(config);
            Stacks.Add(stack);
            Decks.Add(new List<int>());
        }
        Cards = new List<string>();
        string[] pics = Directory.GetFiles(folder, "card_*.jpg");
        foreach (string pic in pics)
        {
            Match match = Regex.Match(pic, "[0-9]+");
            int index = int.Parse(match.Groups[0].Value.ToString());
            Cards.Add(pic);
            for (int i = 0; i < Stacks.Count; i++)
            {
                StackConfig stack = Stacks[i];
                if (index >= stack.min && index <= stack.max)
                {
                    Decks[i].Add(Cards.Count - 1);
                }
            }
        }
        Backsides = new List<Sprite>();
        for (int i = 0; i < Stacks.Count; i++)
        {
            Backsides.Add(i2s.LoadNewSprite(folder + "\\" + Stacks[i].backside));
        }
    }

    [System.Serializable]
    public class StackConfig
    {
        public string name;
        public int[] size = new int[2];
        public int min;
        public int max;
        public string backside;
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/
}
