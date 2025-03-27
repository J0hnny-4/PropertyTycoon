using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Xml;
using Data;


public class Parser
{
  private static string[,] data;
  private static DataTable LoadDataTableFromXml(string path)
  {
    //Creates object xml and loads the path passed through the initial call
    XmlDocument xml = new XmlDocument();
    xml.Load(path);
    //Creates a dataset and store the Xml contents into dataSet
    DataSet dataSet = new DataSet();
    dataSet.ReadXml(new StringReader(xml.InnerXml));
    return dataSet.Tables[0];
  }

  public static void Parse()
  {
    DataTable propertyList = LoadDataTableFromXml("Assets/Resources/Data/PropertyTable.xml");
    //Sets the length of the array depending on the attributes loaded
    data = new string[propertyList.Rows.Count, propertyList.Columns.Count];
    int row = 0;
    //Loop to store the data into the 2d array
    foreach (DataRow dataRow in propertyList.Rows)
    {
      for (int col = 0; col < dataRow.ItemArray.Count(); col++)
      {
        data[row, col] = dataRow[col].ToString();
        //Test
        Console.Write("{0} ", data[row, col]);
      }
      row++;
      //Test
      Console.Write(Environment.NewLine + Environment.NewLine);
    }

  }

  public static List<SquareData> TileCreator()
  {
    Parse();
    List<SquareData> tiles = new List<SquareData>();
    int[] rentlist = new int[2];

    for (int i = 0; i < 39; i++)
    {
      int cost;
      int rent;
      string name = data[i, 1];
      int housecost;
      int temp;

      int.TryParse(data[i, 5], out cost);
      int.TryParse(data[i, 6], out rent);
      int.TryParse(data[i, 6], out housecost);

      int x = 0;
      switch (data[i, 2])
      {
        case ("Action"):
          tiles.Add(new OwnableData(name, cost));
          break;

        case ("Brown" or "Blue" or "Green" or "Red" or "Yellow" or "Purple" or "Deep Blue" or "Orange"):
          x = 0;
          int y = 6;
          while (x < 3)
          {
            int.TryParse(data[i, y], out temp);
            rentlist[x] = temp;
            x++;
            y++;
          }
          Colour colour = (Data.Colour)typeof(Data.Colour).GetField(data[i, 2].Replace(" ", ""), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)?.GetValue(null);

          tiles.Add(new PropertyData(name, cost, rentlist, colour, housecost));
          break;

        case ("Utilities"):
          x = 0;
          y = 6;
          while (x < 3)
          {
            int.TryParse(data[i, y], out temp);
            rentlist[x] = temp;
            x++;
            y++;
          }
          break;

        case ("Go to Jail"):
          tiles.Add(new OwnableData(name, cost));
          break;

        case ("Take card"):
          tiles.Add(new OwnableData(name, cost));
          break;

        case ("Station"):
          tiles.Add(new StationData(name, cost, rent));
          break;
      }
    }

    return tiles;
  }

}
