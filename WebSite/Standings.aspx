<%@ Page Language="C#" MasterPageFile="~/masterMain.master" AutoEventWireup="true" CodeFile="Standings.aspx.cs" Inherits="DistrictOne.Standings" Title="District One Pool Standings" %>

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
                    <span style="font-size: 18px; color: #000066; font-family: Calibri">NJ District Standings</span><br />
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
    <div class="row">
        <div class="col-sm-12 text-center">
            <asp:Label ID="lblDivisionAdvance" runat="server" Font-Bold="true"></asp:Label>
        </div>
    </div>
    <div class="row">
        <asp:Repeater ID="rptTeams" runat="server">
            <ItemTemplate>
                <div class="col-md-6">
                    <div class="card">
                        <div class="card-body text-center">
                            <h4 class="card-title text-center">
                                <asp:Label ID="lblPool" runat="server" Text='<% #Bind("pool")%>'></asp:Label></h4>
                            <p class="card-text text-center">
                                <div class="table-responsive">
                                    <asp:GridView ID="grdTeams" runat="server" DataSource='<% #Bind("teams") %>' AutoGenerateColumns="false" CssClass="table table-hover" HeaderStyle-VerticalAlign="Bottom" HeaderStyle-HorizontalAlign="Center">
                                        <Columns>
                                            <asp:BoundField DataField="id" HeaderText="ID" Visible="false" />
                                            <asp:BoundField DataField="displayName" HeaderText="Team" />
                                            <asp:BoundField DataField="wins" HeaderText="Wins" DataFormatString="{0:#0}" ItemStyle-HorizontalAlign="right" />
                                            <asp:BoundField DataField="losses" HeaderText="Losses" DataFormatString="{0:#0}" ItemStyle-HorizontalAlign="right" />
                                            <asp:BoundField DataField="runsPerInning" HeaderText="RPI" ItemStyle-HorizontalAlign="right" DataFormatString="{0:#.0000}" />
                                            <asp:BoundField DataField="fieldInnings" HeaderText="Total Field Innings" DataFormatString="{0:##0}" ItemStyle-HorizontalAlign="right" />
                                            <asp:BoundField DataField="TotalOpponentScore" HeaderText="Total Runs Let Up" DataFormatString="{0:##0}" ItemStyle-HorizontalAlign="right" />
                                        </Columns>
                                        <AlternatingRowStyle BackColor="beige" />
                                    </asp:GridView>
                                </div>
                            </p>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

    </div>
 </div>
</asp:Content>
