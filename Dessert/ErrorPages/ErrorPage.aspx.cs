using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace Dessert.ErrorPages
{
    public partial class ErrorPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Display safe error message.
            this.FriendlyErrorMsg.Text = "A problem has occurred on this web site. Please try again. " + "If this error continues, please contact support.";

            // Get the last error from the server.
            Exception ex = Server.GetLastError();

            // Show error details to only you (developer). LOCAL ACCESS ONLY.
            if (Request.IsLocal)
            {
                // Show local access details.
                DetailedErrorPanel.Visible = true;
               
                if (ex.InnerException != null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("URL : {0} <br />", Request.RawUrl);
                    sb.AppendFormat("User : {0} <br />", User.Identity.Name);
                    sb.AppendFormat("Exception Type : {0} <br />", ex.InnerException.GetType().ToString());
                    sb.AppendFormat("Exception : {0} <br />", ex.InnerException.Message);
                    sb.AppendFormat("Stack Trace : <br /> {0} ", ex.InnerException.StackTrace.Replace(Environment.NewLine, "<br />"));
                    
                    this.lblErrorDetailedMsg.Text = sb.ToString() ;
                }
            }

            Server.ClearError();
        }
    }
 }