using System;
using System.Data;
using System.Xml;

public class xmlparser
{ 

  public static DataTable LoadDataTableFromXml(string Path)
  {
    //Creates object xml and loads the path passed through the initial call
    XmlDocument xml = new XmlDocument();
    xml.Load(Path);
    //Creates a dataset and store the Xml contents into dataSet
    DataSet dataSet = new DataSet();
    dataSet.ReadXml(new StringReader(xml.InnerXml));
    //This is returned to propertyList
    return dataSet.Tables[0];
  }

  static void Main()
  {
    DataTable propertyList = LoadDataTableFromXml("Property Table.xml");
    //Sets the length of the array depending on the attributes loaded
    string [,] data = new string[propertyList.Rows.Count,propertyList.Columns.Count];

    int row = 0;
    //Loop to store the data into the 2d array
    foreach(DataRow dataRow in propertyList.Rows)
    {
      for(int col = 0; col < dataRow.ItemArray.Count(); col++)
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
}
