using System;
using UnityEngine;

namespace CivilianPopulation.GUI
{
    public class Grid
    {
        private string[] headers;
        private string[,] data;

        public Grid(string[] headers, string[,] data)
        {
            this.headers = headers;
            this.data = data;
        }

        public void draw()
        {
            GUILayout.BeginHorizontal();
            for (int i = 0; i < headers.Length; i++)
            {
                GUILayout.BeginVertical();
                GUILayout.Label(headers[i]);
                for (int j = 0; j < data.GetLength(0); j++)
                {
                    GUILayout.Label(data[j,i]);
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }
    }
}
