using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Jared_Spring_2016_42.Models;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Configuration;

namespace Jared_Spring_2016_42.Account
{
    public partial class Register : Page
    {
        protected void CreateUser_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            var user = new ApplicationUser() { UserName = Email.Text, Email = Email.Text };
            IdentityResult result = manager.Create(user, Password.Text);
            if (result.Succeeded)
            {
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                //string code = manager.GenerateEmailConfirmationToken(user.Id);
                //string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                //manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");

                signInManager.SignIn( user, isPersistent: false, rememberBrowser: false);
                string cs = ConfigurationManager.ConnectionStrings["csUserData"].ToString();
                SqlConnection conUsers = new SqlConnection(cs);
                string sql = "Insert INTO [UserData] ([userId], [firstName], [lastName], [phone]) Values (@userID, @firstName, @lastName, @phone)";
                SqlCommand comNewUse = new SqlCommand(sql, conUsers);
                comNewUse.Parameters.Add("@userID", System.Data.SqlDbType.NVarChar, 50).Value = Email.Text;
                comNewUse.Parameters.Add("@lastName", System.Data.SqlDbType.NVarChar, 50).Value = txtLast.Text;
                comNewUse.Parameters.Add("@firstName", System.Data.SqlDbType.NVarChar, 50).Value = txtFirst.Text;
                comNewUse.Parameters.Add("@phone", System.Data.SqlDbType.NVarChar, 50).Value = txtPhone.Text;

                try
                {
                    conUsers.Open();
                    comNewUse.ExecuteNonQuery();
                }

                catch (Exception ex)
                {
                    lblOutput.Text = "Sorry, unable to store user data.  Standard exception message is: " + ex.Message;
                    return;
                }
                finally {
                    conUsers.Close();
                }

                IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
            }
            else 
            {
                ErrorMessage.Text = result.Errors.FirstOrDefault();
            }
        }
    }
}