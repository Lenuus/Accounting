using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounting.Common.CustomAttribute;

namespace Accounting.Common.Constants
{
    public static class RoleClaimConstants
    {
        public const string ClaimTypeValue = "ClaimPermission";

        #region Collection Claims
        [Group("List Collection", "Collections")]
        public const string CollectionList = "Collection_List";
        [Group("Add Collection", "Collections")]
        public const string CollectionAdd = "Collection_Add";
        [Group("Update Collection", "Collections")]
        public const string CollectionUpdate = "Collection_Update";
        [Group("Delete Collection", "Collections")]
        public const string CollectionDelete = "Collection_Delete";
        #endregion

        #region Corporation Claims
        [Group("List Corporation", "Corporations")]
        public const string CorporationList = "Corporation_List";
        [Group("Add Corporation", "Corporations")]
        public const string CorporationAdd = "Corporation_Add";
        [Group("Update Corporation", "Corporations")]
        public const string CorporationUpdate = "Corporation_Update";
        [Group("Delete Corporation", "Corporations")]
        public const string CorporationDelete = "Corporation_Delete";
        #endregion

        #region Expense Claims
        [Group("List Expense", "Expenses")]
        public const string ExpenseList = "Expense_List";
        [Group("Add Expense", "Expenses")]
        public const string ExpenseAdd = "Expense_Add";
        [Group("Delete Expense", "Expenses")]
        public const string ExpenseDelete = "Expense_Delete";
        [Group("Update Expense", "Expenses")]
        public const string ExpenseUpdate = "Expense_Update";
        #endregion

        #region Order Claims
        [Group("List Order", "Orders")]
        public const string OrderList = "Order_List";
        [Group("Add Order", "Orders")]
        public const string OrderAdd = "Order_Add";
        [Group("Delete Order", "Orders")]
        public const string OrderDelete = "Order_Delete";
        [Group("Update Order", "Orders")]
        public const string OrderUpdate = "Order_Update";
        #endregion

        #region Management Claims
        [Group("List Management", "Managements")]
        public const string ManagementList = "Management_List";
        [Group("Add Management", "Managements")]
        public const string ManagementAdd = "Management_Add";
        [Group("Delete Management", "Managements")]
        public const string ManagementDelete = "Management_Delete";
        [Group("Update Management", "Managements")]
        public const string ManagementUpdate = "Management_Update";
        #endregion

        #region Product Claims
        [Group("List Product", "Products")]
        public const string ProductList = "Product_List";
        [Group("Add Product", "Products")]
        public const string ProductAdd = "Product_Add";
        [Group("Delete Product", "Products")]
        public const string ProductDelete = "Product_Delete";
        [Group("Update Product", "Products")]
        public const string ProductUpdate = "Product_Update";
        #endregion
    }
}
