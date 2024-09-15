using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.ViewModel.RoleVM;
using Intl.Realty.Firm.Model.ViewModel.RoleVM;
using Microsoft.AspNetCore.Identity;

namespace Intl.Realty.Firm.Utility.Mapper
{
    public static class RoleMapper
    {
        public static List<RoleExport> ToRoleListExport(this List<ApplicationRole> modelList)
        {
            List<RoleExport> exportList = new List<RoleExport>();

            if (modelList != null)
            {
                exportList = modelList.Select(x => new RoleExport
                {
                    Name = x.Name!,
                }).ToList();
            }
            return exportList;
        }
    }
}