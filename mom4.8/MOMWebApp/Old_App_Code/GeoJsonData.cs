/// <summary>
/// Summary description for GeoJsonData
/// </summary>
public class GeoJsonData
{
    public GeoJsonData()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region Properties  ///////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Gets or sets the status.
    /// </summary>
    /// <value>The status.</value>
    
    /// <summary>
    /// Gets or sets the results.
    /// </summary>
    /// <value>The results.</value>
    public JsonResult[] results { get; set; }

    #endregion

    #region Nested Types //////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 
    /// </summary>
    public class JsonAddress
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        
    }

    /// <summary>
    /// 
    /// </summary>
    public class JsonBounds
    {
        public JsonLocation northeast;
        public JsonLocation southwest;
    }

    /// <summary>
    /// 
    /// </summary>
    public class JsonGeometry
    {
        public JsonBounds bounds { get; set; }
        public JsonLocation location { get; set; }
        
        public JsonBounds viewport { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class JsonLocation
    {
        public double lat;
        public double lng;
    }

    /// <summary>
    /// 
    /// </summary>
    public class JsonResult
    {
        public JsonAddress[] address_components { get; set; }
        public string formatted_address { get; set; }
        public JsonGeometry geometry { get; set; }
        public string partial_match { get; set; }
        
    }
    #endregion
}
