CREATE PROCEDURE [dbo].[GetJobExpenseByJob]
	@Job INT,
	@PageIndex INT = 1,
    @PageSize INT = 10
AS
BEGIN
	 SELECT ROW_NUMBER() OVER
      (
            ORDER BY Line ASC
      )AS RowNumber, 
	  isnull(Code,'') as Code,  	                
          Line as Phase,                              
          fDesc,                                      
          (CASE Type WHEN 0 THEN 'Revenues' ELSE 'Costs' END) AS JobType,                        
          Type,                                       
          ISNULL(Actual,0)    As MatAct,              
          ISNULL(Budget,0)    As MatBgt,              
          ISNULL(Actual,0) - (ISNULL(Budget,0) + ISNULL(Modifier,0)) As MatDiff,                  
          ISNULL(Modifier,0)  As MatMod,              
          ISNULL(THours,0)    As HourAct,             
          ISNULL(BHours,0)    As HourBgt,             
          ISNULL(ETC,0)       As LaborBgt,            
          ISNULL(ETCMod,0)    As LaborMod,            
          ISNULL(Labor,0)     As LaborAct,            
          ISNULL(Labor,0) - (ISNULL(ETC,0) + ISNULL(ETCMod,0)) As LabDiff 
	  INTO #Results                        
      FROM JobTItem WHERE Type=1 AND Job= @Job                     
      ORDER BY Line   

	  
           
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1

	  SELECT COUNT(*) as RecordCount FROM #Results
     
      DROP TABLE #Results
END
GO
