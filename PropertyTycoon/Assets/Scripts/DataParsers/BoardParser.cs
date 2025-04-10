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

    /// <summary>
    /// Loads the xml file via the path into an XML document and then stores the string into a dataset
    /// </summary>
    /// <param name="path"></param>
    /// <returns= "dataSet.Tables"> Table form of a dataset </returns>
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

    
    /// <summary>
    /// Sets the size of the array and stores the datatable into the premade 2d array Data
    /// </summary>
    private static void Parse()
    {
      DataTable propertyList = LoadDataTableFromXml(Application.streamingAssetsPath + "/Property Table.xml");
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

    /// <summary>
    /// Reads the 3rd Column on the 2d Array and creates an object and stores it within a list
    /// and the it will return the list to the gameboard
    /// </summary>
    /// <returns = "tiles"> Tile list which is used by the gameboard</returns>
    public static List<SquareData> TileCreator()
    {
       //Create list of squaredata to store each board spot
      Parse();
      List<SquareData> tiles = new List<SquareData>();

      //Loop with switch statement within. Dictates what type of square should be created and stored within the list.
      for (int i = 0; i < 40; i++)
      {
       //Convert String and other data type to int
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
            //Loop to grab rentlist from XML
            int[] rentlist = new int[6];
            rentlist[0] = rent;
            for (int x = 1; x < 6; x++)
            {
              int.TryParse(data[i, x + 4], out temp);
              rentlist[x] = temp;
            }
            //Store colour as a colour
            Colour colour = (Data.Colour)typeof(Data.Colour).GetField(data[i, 2].Replace(" ", ""), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)?.GetValue(Colour.Black);

            tiles.Add(new PropertyData(name, cost, rentlist, colour, housecost));
            break;

          case ("Utilities"):
            tiles.Add(new UtilityData(name, cost));
            break;

          case ("Go to jail"):
            tiles.Add(new SquareData(name));
            break;

          case ("Take card"):
            tiles.Add(new CardSquareData(name));
            break;

          case ("Station"):
            tiles.Add(new StationData(name, cost, rent));
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
