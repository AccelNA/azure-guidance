using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.ServiceBus;
using System.Text;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        var nameSpace = txtNamespace.Text;
        var eventHub = txtEventHub.Text;
        var policyName = txtPolicyName.Text;
        var policyKey = txtPolicyKey.Text;
        var ttl = TimeSpan.FromMinutes( Convert.ToDouble(txtTTL.Text));

        var resource = ServiceBusEnvironment.CreateServiceUri("https", nameSpace, eventHub).ToString().Trim('/');
        lblResult.Text = SharedAccessSignatureTokenProvider.GetSharedAccessSignature(policyName, policyKey, resource, ttl);

    }
}