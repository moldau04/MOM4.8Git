using BusinessEntity;
using BusinessEntity.InventoryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOMWebAPI.Areas.Inventory.Controllers
{
    public interface IPostToProjectRepository
    {
        /// <summary>
        /// For PostToProject Screen : PostToProject.aspx / PostToProject.aspx.cs
        /// </summary>
        /// API's Naming Conventions : PostToProject_Method Name(Parameter)
        /// 

        ListPostInventoryItemsToProject PostToProject_PostInventoryItemsToProject(PostInventoryItemsToProjectParam _PostInventoryItemsToProjectParam, string ConnectionString);
        List<GetEMPViewModel> PostToProject_GetEMP(GetEMPParam _GetEMP, string ConnectionString);
    }
}
