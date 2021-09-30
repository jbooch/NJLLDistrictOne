<%@ Page Language="C#" MasterPageFile="~/masterMain.master" AutoEventWireup="true" CodeFile="InterleagueStandings.aspx.cs" Inherits="DistrictOne.InterleagueStandings" Title="District One Interleague Standings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="container">
            <div class="row" style="background-color: lavender">
                <div class="col-sm-12 text-center">
                    <span style="font-size: 24px; color: #FF0000">New Jersey District 1 Little League
                    </span>
                    <br />
                    <span style="font-size: 18px; color: #000066; font-family: Calibri">District Administrator - Chris Graham
                    </span>
                    <br />
                    <span style="font-size: 18px; color: #000066; font-family: Calibri">NJ District Interleague Standings</span><br />
                </div>
            </div>
        </div>
        <br />
<div class="container">
    <div class="row">
        <div class="col-sm-12 text-center">
                <asp:DropDownList ID="dlDivision" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dlDivision_SelectedIndexChanged">
                </asp:DropDownList>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-6 text-center">
            <div class="table-responsive">
                <asp:GridView ID="grdTeams" runat="server" AutoGenerateColumns="false" CssClass="table table-hover" HeaderStyle-VerticalAlign="Bottom" HeaderStyle-HorizontalAlign="Center">
                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="ID" Visible="false" />
                        <asp:BoundField DataField="displayName" HeaderText="Team" />
                        <asp:BoundField DataField="wins" HeaderText="Wins" DataFormatString="{0:#0}" ItemStyle-HorizontalAlign="right" />
                        <asp:BoundField DataField="losses" HeaderText="Losses" DataFormatString="{0:#0}" ItemStyle-HorizontalAlign="right" />
                    </Columns>
                    <AlternatingRowStyle BackColor="beige" />
                </asp:GridView>
            </div>
        </div>
    </div>
 </div>
</asp:Content>
