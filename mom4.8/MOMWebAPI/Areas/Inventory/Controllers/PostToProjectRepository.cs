using BusinessEntity;
using BusinessEntity.InventoryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOMWebAPI.Areas.Inventory.Controllers
{
    public class PostToProjectRepository : IPostToProjectRepository
    {
        /// <summary>
        /// For PostToProject Screen : PostToProject.aspx / PostToProject.aspx.cs
        /// </summary>
        /// API's Naming Conventions : PostToProject_Method Name(Parameter)
        /// 

        public ListPostInventoryItemsToProject PostToProject_PostInventoryItemsToProject(PostInventoryItemsToProjectParam _PostInventoryItemsToProjectParam, string ConnectionString)
        {
            return new BusinessLayer.BL_MapData().PostInventoryItemsToProject(_PostInventoryItemsToProjectParam, ConnectionString);
        }

        public List<GetEMPViewModel> PostToProject_GetEMP(GetEMPParam _GetEMP, string ConnectionString)
        {
            return new BusinessLayer.BL_User().getEMP(_GetEMP, ConnectionString);
        }
    }
}
