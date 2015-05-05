<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            font-size: large;
        }
        .auto-style3 {
            width: 492px;
        }
        .auto-style4 {
            height: 26px;
            width: 492px;
        }
        .auto-style5 {
            width: 207px;
        }
        .auto-style6 {
            height: 26px;
            width: 207px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <table style="width:70%; margin-left:100px;" >
            <tr>
                <td colspan="2">
                     <span class="auto-style1"><strong>Azure Event Hub Shared Access Signature Generator</strong></span><hr />
                </td>
            </tr>
            <tr>
                <td class="auto-style5">Namespace</td>
                <td class="auto-style3">
                    <asp:TextBox ID="txtNamespace" runat="server" Width="506px"></asp:TextBox>
                </td>

            </tr>
            <tr>
                <td class="auto-style5">Event Hub name</td>
                <td class="auto-style3">
                    <asp:TextBox ID="txtEventHub" runat="server" Width="505px"></asp:TextBox>
                </td>
     
            </tr>
            <tr>
                <td class="auto-style5">Policy Name (Sender)</td>
                <td class="auto-style3">
                    <asp:TextBox ID="txtPolicyName" runat="server" Width="505px"></asp:TextBox>
                </td>
  
            </tr>
            <tr>
                <td class="auto-style6">Policy Key</td>
                <td class="auto-style4">
                    <asp:TextBox ID="txtPolicyKey" runat="server" Width="505px"></asp:TextBox>
                </td>

            </tr>
            <tr>
                <td class="auto-style5">Token TTL (Minutes) </td>
                <td class="auto-style3">
                    <asp:TextBox ID="txtTTL" runat="server" Width="505px" TextMode="Number">60</asp:TextBox>
                </td>

            </tr>
            <tr>
                <td class="auto-style5">&nbsp;</td>
                <td class="auto-style3">
                    <asp:Button ID="btnGenerate" runat="server" Text="Generate Signature" OnClick="btnGenerate_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    
                </td>
     
            </tr>
             <tr>

                <td colspan="2">
                    <asp:Label ID="lblResult" runat="server"></asp:Label>
                </td>
     
            </tr>
        </table>
       
    </form>
</body>
</html>
