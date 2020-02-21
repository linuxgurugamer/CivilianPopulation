using UnityEngine;

namespace CivilianPopulation.GUI
{
    public class Grid
    {
        private string filter;

        private string[] headers;
        private string[,] data;

        public Grid()
        {
            this.filter = "";
        }

        public void setHeaders(string[] headers)
        {
            this.headers = headers;
        }

        public void setData(string[,] data)
        {
            this.data = data;
        }

        public void draw()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Filter");
            this.filter = GUILayout.TextField(filter, 20);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            for (int i = 0; i < headers.Length; i++)
            {
                GUILayout.BeginVertical();
                GUILayout.Label(headers[i]);
                for (int j = 0; j < data.GetLength(0); j++)
                {
                    if (matchFilter(j))
                    {
                        GUILayout.Label(data[j, i]);
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }

        private bool matchFilter(int j)
        {
            bool res = false;
            for (int i = 0; i < headers.Length; i++)
            {
                if (data[j, i] != null && data[j, i].ToLower().Contains(filter.ToLower()))
                {
                    res = true;
                }
            }
            return res;
        }

        private void log(string message)
        {
            Debug.Log(this.GetType().Name + " - " + message);
        }
    }
}
