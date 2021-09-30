<%@ Page Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="true" CodeFile="InterleagueTeamMaintenance.aspx.cs" Inherits="DistrictOne.InterleagueTeamMaintenance" Title="Interleague Team Maintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container">
                <div class="row" style="background-color: lavender">
                    <div class="col-sm-12 text-sm-center">
                        <span style="font-size: 24px; color: #FF0000">New Jersey District 1 Little League
                        </span>
                        <br />
                        <span style="font-size: 18px; color: #000066; font-family: Calibri">District Administrator - Chris Graham
                        </span>
                        <br />
                        <span style="font-size: 18px; color: #000066; font-family: Calibri">NJ Interleague Team Maintenance</span>
                    </div>
                </div>
                </div>
            <div class="container">
                <br />
                <div class="row border border-dark p-2">
                    <div class="col-sm-12">
                        <div class="row">
                            <div class="col-sm-12 text-justify">
                                <br />
                                <span class="h3 text-center">First, select the Division to maintain the teams.  Then, select the League the team is associated with and supply a team name.</span>
                            </div>
                        </div>
                        <br />
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <div class="input-group">
                                    <span class="input-group-text">Division: </span>
                                    <asp:DropDownList CssClass="form-control" ID="dlDivision" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dlDivision_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                             </div>
                        </div>
                    <asp:Panel ID="pnlTeamGrid" runat="server" Visible="false">
                        <div class="row">
                            <div class="col-sm-5">
                                <div class="input-group">
                                    <span class="input-group-text">League</span>
                                    <asp:DropDownList ID="dlLeague" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-5">
                                <div class="input-group">
                                    <span class="input-group-text">Team Name</span>
                                    <asp:TextBox ID="txtTeamName" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div>
                                <div class="col-sm-2">
                                    <asp:Button ID="btnAddTeam" Text="Add" CssClass="btn btn-outline-primary" runat="server" OnClick="btnAddTeam_Click" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="class-col-sm-6 text-center">
                                    <asp:CheckBox ID="ckShowTeamId" runat="server" Text="Show Team No." OnCheckedChanged="ckShowTeamId_changed" AutoPostBack="True" CssClass="form-control align-content-end" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="offset-2 col-sm-8 text-center">
                                <div class="table-responsive">
                                    <asp:GridView CssClass="table table-active table-bordered" ID="grdTeams" runat="server" AutoGenerateColumns="False" EnableModelValidation="True">
                                        <Columns>
                                            <asp:BoundField DataField="recordId" HeaderText="Team Number" Visible="false"></asp:BoundField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRecordID" runat="server" Text='<%#Bind("recordId") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblLeagueId" runat="server" Text='<%#Bind("leagueId") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="league" HeaderText="League"></asp:BoundField>
                                            <asp:TemplateField HeaderText="Name">
                                                <ItemTemplate>
                                                    <asp:TextBox CssClass="form-control" ID="txtName" runat="server" Text='<%# Bind("name") %>' MaxLength="50"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remove?">
                                                <ItemTemplate>
                                                    <asp:CheckBox CssClass="form-control" ID="ckDelete" runat="server" Checked='<%# Bind("deleted") %>'></asp:CheckBox>
                                                    <asp:Label ID="lblError" runat="server" Text="*" ForeColor="red" Font-Bold="true" Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12 text-center">
                                <asp:Button CssClass="btn btn-outline-dark" ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                            </div>
                        </div>
                    </asp:Panel>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

