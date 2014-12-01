<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Marketo_SOAP_API_Sample_Project.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <!-- Settings -->
    <table>
        <tr>
            <td colspan="2"><h2>Marketo API Connection Configuration</h2></td>
        </tr>
        <tr>
            <td>User ID
            </td>
            <td>EncryptionID
            </td>
        </tr>
        <tr>
            <td><asp:TextBox ID="txtUserID" runat="server" Width="321px"></asp:TextBox> 
            </td>
            <td><asp:TextBox ID="txtEncryptionID" runat="server" Width="381px"></asp:TextBox> 
            </td>
        </tr>
    </table>

    <!-- Get Lead -->
    <table>
        <tr>
            <td colspan="2"><h2>Make a Get Lead Call</h2></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Button ID="btnGetLeads" runat="server" Text="Execute" onclick="btnGetLeads_Click" />
            </td>
            <td valign="top">
                <asp:TextBox ID="txtLeadID" runat="server" Text="1155301"></asp:TextBox> 
            </td>
            <td valign="top">
                <asp:Label ID="lblResult" runat="server"></asp:Label>
            </td>
            <td valign="top">
                <asp:Label ID="lblResultGetLead" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <!-- Sync Lead -->
    <table>
        <tr>
            <td colspan="2"><h2>Make a Sync Lead Call</h2></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Button ID="btnSyncLead" runat="server" Text="Execute" 
                    onclick="btnSyncLead_Click" />
            </td>
            <td valign="top">
                <asp:TextBox ID="txtEmail" runat="server" Text="angelo@marketo.com"></asp:TextBox> 
            </td>
            <td valign="top">
                <asp:Label ID="lblResultSyncLead" runat="server"></asp:Label>
            </td>
            <td valign="top">
                <asp:Label ID="lblResultSyncLeadData" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <!-- Request Campaign-->
    <table>
        <tr>
            <td colspan="2"><h2>Make a Request Campaign Call</h2></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Button ID="btnRequestCampaign" runat="server" Text="Execute" 
                    onclick="btnRequestCampaign_Click" />
            </td>
            <td valign="top">
                <asp:TextBox ID="txtReqMarketoLead" runat="server" Text="1155301"></asp:TextBox> 
                <asp:TextBox ID="txtReqCampaignID" runat="server" Text="1118"></asp:TextBox> 
            </td>
            <td valign="top">
                <asp:Label ID="lblResultRequestCampaign" runat="server"></asp:Label>
            </td>
            <td valign="top">
                <asp:Label ID="lblResultRequestCampaignData" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <!-- Get Multiple Leads-->
    <table>
        <tr>
            <td colspan="2"><h2>Make a getMultipleLeads Call to Return Marketo Leads changed 
                since lastUpdatedDate</h2></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Button ID="btnGetMultipleLeadsDate" runat="server" Text="Execute" 
                    onclick="btnGetMultipleLeadsDate_Click" />
            </td>
            <td valign="top">
                <asp:TextBox ID="txtGetMultipleLeadsDate" runat="server" 
                    Text="2012-09-26T07:33:51-06:00"></asp:TextBox> 
            </td>
            <td valign="top">
                <asp:Label ID="lblResultGMLDate" runat="server"></asp:Label>
            </td>
            <td valign="top">
                <asp:Label ID="lblResultGMLDateData" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <!-- Get Multiple Leads-->
    <table>
        <tr>
            <td colspan="2"><h2>Make a getMultipleLeads Call to Return Marketo Leads in existing 
                Static List</h2>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Button ID="btnGetMultipleLeadsList" runat="server" Text="Execute" 
                    onclick="btnGetMultipleLeadsList_Click" />
            </td>
            <td valign="top">
                <asp:TextBox ID="txtGetMultipleLeadsList" runat="server" 
                    Text="Test_Static_List"></asp:TextBox> 
            </td>
            <td valign="top">
                <asp:Label ID="lblResultGMLList" runat="server"></asp:Label>
            </td>
            <td valign="top">
                <asp:Label ID="lblResultGMLListData" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
        <!-- Get Multiple Leads-->
    <table>
        <tr>
            <td colspan="2"><h2>Make a getMultipleLeads Call to Return Marketo Leads - Pass in 
                LeadKey (i.e. IDs)</h2></td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Button ID="btnGetMultipleLeadsLeadList" runat="server" Text="Execute" 
                    onclick="btnGetMultipleLeadsLeadList_Click" />
            </td>
            <td valign="top">
                <asp:TextBox ID="txtGetMultipleLeadsLeadsList" runat="server" 
                    Text="318212,318213"></asp:TextBox> 
            </td>
            <td valign="top">
                <asp:Label ID="lblResultGMLListID" runat="server"></asp:Label>
            </td>
            <td valign="top">
                <asp:Label ID="lblResultGMLListIDData" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
