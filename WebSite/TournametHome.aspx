<%@ Page Language="C#" MasterPageFile="~/masterMain.Master" AutoEventWireup="true" Codefile="TournametHome.aspx.cs" Inherits="DistrictOne.TournamentHome" Title="District Tournament Home" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" cellpadding="5" cellspacing="5">
        <tr>
            <td style="text-align: center">
                <br />
                <asp:Label ID="lblAnnouncement" runat="server" Text="Due to the imending weather, all games today will be postponed."></asp:Label><br />
                <br />
                <asp:Label ID="lblUpdateTime" runat="server" ForeColor="Red"></asp:Label></td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlAdminAnnouncement" runat="server" Width="100%">
                <table width="100%" cellpadding="5" cellspacing="5">
                    <tr>
                        <td>
                            <asp:TextBox ID="txtAnnouncement" runat="server" TextMode="MultiLine" Width="100%" Rows="6"></asp:TextBox>
							<script type="text/javascript">
					            make_wyzz("ctl00_ContentPlaceHolder1_txtAnnouncement");
							</script>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center">
                            <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" /></td>
                    </tr>
                </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
