using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml;
using Data;
using UnityEngine;

namespace DataParsers
{
  public class BoardParser
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

    private static void Parse()
    {
      DataTable propertyList = LoadDataTableFromXml(Application.streamingAssetsPath + "/Data/Property Table.xml");
      //Sets the length of the array depending on the attributes loaded
      data = new string[propertyList.Rows.Count, propertyList.Columns.Count];
      int row = 0;
      //Loop to store the data into the 2d array
      foreach (DataRow dataRow in propertyList.Rows)
      {
        for (int col = 0; col < dataRow.ItemArray.Count(); col++)
        {
          data[row, col] = dataRow[col].ToString();
        }
        row++;
      }
    }

    public static List<SquareData> TileCreator()
    {
      Parse();
      List<SquareData> tiles = new List<SquareData>();

      for (int i = 0; i < 40; i++)
      {
        string name = data[i, 0];
        int temp;

        int.TryParse(data[i, 3], out var cost);
        int.TryParse(data[i, 4], out var rent);
        
        switch (data[i, 2])
        {
          case ("Action"):
            if (name == "Income Tax")
            {
              tiles.Add(new TaxData(name, Cons.IncomeTax));
            }
            else if (name == "Super Tax")
            {
              tiles.Add(new TaxData(name, Cons.SuperTax));
            }
            else
            {
              tiles.Add(new SquareData(name));
            }
            break;

          case ("Brown" or "Blue" or "Green" or "Red" or "Yellow" or "Purple" or "Deep blue" or "Orange"):
            int.TryParse(data[i, 10], out var housecost);

            int[] rentlist = new int[6];
            rentlist[0] = rent;
            for (int x = 1; x < 6; x++)
            {
              int.TryParse(data[i, x + 4], out temp);
              rentlist[x] = temp;
            }
            Colour colour = (Data.Colour)typeof(Data.Colour).GetField(data[i, 2].Replace(" ", ""), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)?.GetValue(Colour.Black);

            tiles.Add(new PropertyData(name, cost, rentlist, colour, housecost));
            break;

          case ("Utilities"):
            tiles.Add(new UtilityData(name, cost));
            break;

          case ("Go to jail"):
            tiles.Add(new SquareData(name)); //todo: Update to suitable type
            break;

          case ("Take card"):
            tiles.Add(new OwnableData(name, cost)); //todo: Update to suitable type
            break;

          case ("Station"):
            tiles.Add(new StationData(name, cost, rent));
            break;
          
          case ("Pot Luck"):
            tiles.Add(new CardSquareData(name));
            break;
          
          case ("Opportunity Knocks"):
            tiles.Add(new CardSquareData(name));
            break;

          default:
            Debug.LogError($"Unknown tile type {data[i, 2]}");
            break;
        }
      }

      return tiles;
    }
  }
}
