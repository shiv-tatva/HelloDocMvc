using DAL_Data_Access_Layer_.DataModels;
using System.ComponentModel.DataAnnotations;

namespace DAL_Data_Access_Layer_.CustomeModel
{
    /// <summary>
    /// accessModel main CM
    /// </summary>
    public class accessModel
    {
        public List<AccountAccess> AccountAccess { get; set; }

        public AccountAccess CreateAccountAccess { get; set; }

        public List<Aspnetrole> Aspnetroles { get; set; }

        public List<Menu> Menus { get; set; }

        public List<AccountMenu> AccountMenu { get; set; }

        public List<UserAccess> UserAccess { get; set; }

        public List<Aspnetrole> AspnetUserroles { get; set; }

        public int Aspid { get; set; }

        public int? flag { get; set; }
    }

    public class AccountAccess
    {
        public int roleid { get; set; }

        public int Adminid { get; set; }

        [Required(ErrorMessage = "Role Name Is Required")]
        public string name { get; set; }

        public string accounttype { get; set; }

        [Required(ErrorMessage = "Please Select Role from dropdown list")]
        public int accounttypeid { get; set; }

    }

    public class AccountMenu
    {
        public int menuid { get; set; }

        public int roleid { get; set; }

        public string name { get; set; }

        public int accounttype { get; set; }

        public bool ExistsInTable { get; set; }

    }

    public class UserAccess
    {
        public int aspnetid { get; set; }
        public string Accounttype { get; set; }
        public string accountname { get; set; }
        public string phone { get; set; }
        public int status { get; set; }
        public int openrequest { get; set; }
        public int roleid { get; set; }

    }

}
