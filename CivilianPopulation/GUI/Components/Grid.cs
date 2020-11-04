using UnityEngine;

namespace CivilianPopulation.GUI
{
    public class Grid
    {
        private string filter;

        private string[] headers;
        private string[,] data;
        private int[] headerWidths;
        private GUIStyle[] styles;
        private string currentSort = "";
        private bool reverseSort = false;
        public Grid()
        {
            this.filter = "";
        }

        public void setHeaders(string[] headers, int[] widths, GUIStyle[] styles)
        {
            this.headers = headers;
            this.headerWidths = widths;
            this.styles = styles;
        }

        public string GetCurrentSort { get { return currentSort; } }
        public bool GetReverseSort { get { return reverseSort; } }
        public void setData(string[,] data)
        {
            this.data = data;
        }
        Vector2 scrollPosition;

        public void draw()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Filter");
            this.filter = GUILayout.TextField(filter, 20);
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            for (int i = 0; i < headers.Length; i++)
            {
                if (GUILayout.Button(headers[i], GUILayout.Width(headerWidths[i])))
                {
                    if (currentSort == headers[i])
                        reverseSort = !reverseSort;
                    else
                    {
                        currentSort = headers[i];
                        reverseSort = false;
                    }
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true, GUIStyle.none, RegisterToolbar.DefGuiSkin.verticalScrollbar, GUILayout.Width(620), GUILayout.Height(260));
            GUILayout.BeginHorizontal();
            for (int i = 0; i < headers.Length; i++)
            {
                GUILayout.BeginVertical(GUILayout.Width(headerWidths[i]));
                for (int j = 0; j < data.GetLength(0); j++)
                {
                    if (matchFilter(j))
                    {
                        GUILayout.Label(data[j, i], styles[i],GUILayout.Width(headerWidths[i]) );
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
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

#if false
        private void log(string message)
        {
            Debug.Log(this.GetType().Name + " - " + message);
        }
#endif
    }
}
