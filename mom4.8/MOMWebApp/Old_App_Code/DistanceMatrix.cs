using System.Collections.Generic;

public class Distance
{
    public string text { get; set; }
    public double value { get; set; }
}

public class Duration
{
    public string text { get; set; }
    public int value { get; set; }
}

public class Element
{
    public Distance distance { get; set; }
    public Duration duration { get; set; }
    public string status { get; set; }
}

public class Row
{
    public List<Element> elements { get; set; }
}

public class RootObject
{
    public List<Row> rows { get; set; }
    public string[] originAddresses { get; set; }
    public string[] destinationAddresses { get; set; }
}

