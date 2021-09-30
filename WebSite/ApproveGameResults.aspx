<%@ Page Language="C#" MasterPageFile="~/masterMain.Master" AutoEventWireup="true" CodeFile="ApproveGameResults.aspx.cs" Inherits="DistrictOne.ApproveGameResults" Title="Approve Game Results" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row" style="background-color: lavender">
            <div class="col-sm-12 text-sm-center">
                <span style="font-size: 24px; color: #FF0000">New Jersey District 1 Little League
                </span>
                <br />
                <span style="font-size: 18px; color: #000066; font-family: Calibri">District Administrator - Chris Graham
                </span>
                <br />
                <span style="font-size: 18px; color: #000066; font-family: Calibri">NJ District One Aprove Results</span>
            </div>
        </div>
    </div>
    <div class="container">
        <div class="row">
            <div class="col-sm-12">
                <asp:Label ID="lblGamesHeader" CssClass="h3" runat="server" Text="Enter the approved scores (results are displayed below game) and click Save to record"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12 text-center">
                <br />
                <asp:CheckBox CssClass="form-control-lg" ID="ckIncludComple" runat="server" AutoPostBack="True" OnCheckedChanged="ckIncludComple_CheckedChanged"
                    Text="Include Completed Games: " TextAlign="Left" />
                <br />
            </div>
        </div>
        <asp:Repeater ID="rptApproveResults" runat="server">
            <ItemTemplate>
                <div class="row border border-primary p-1">
                    <div class="col-sm-12">
                        <div class="form-group row bg-light">
                            <asp:Label CssClass="col-sm-2 col-form-label" ID="lblGameDate" runat="server" Text='<% # Bind("gameDateDisplay") %>'></asp:Label>
                            <asp:Label ID="recordId" runat="server" Text='<% #Bind("id") %>' Visible="false"></asp:Label>
                            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblTeam1" runat="server" Text='<% # Bind("team1") %>'></asp:Label>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblScore1" runat="server" Text="Score: "></asp:Label>
                            <div class="col-sm-1">
                                <asp:TextBox ID="txtScore1" CssClass="form-control" runat="server" Style="text-align: right" MaxLength="2" Text='<% # Bind("score1") %>'></asp:TextBox>
                            </div>
                            <asp:Label CssClass="col-sm-3 col-form-label" runat="server" ID="lblTeam2" Text='<% # Bind("team2") %>'></asp:Label>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblScore2" runat="server" Text="Score: "></asp:Label>
                            <div class="col-sm-1">
                                <asp:TextBox ID="txtScore2" CssClass="form-control" runat="server" Style="text-align: right" MaxLength="2" Text='<% # Bind("score2") %>'></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row bg-success">
                            <div class="col-sm-12">
                                <asp:Label CssClass="col-sm-3" runat="server" Text="Comment: "></asp:Label>
                                <asp:TextBox ID="txtUpdateComment" runat="server" CssClass="col-sm-9"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <asp:GridView ID="grdResults" CssClass="table table-hover" runat="server" AutoGenerateColumns="false" DataSource='<% #Bind("results") %>'>
                                    <AlternatingRowStyle BackColor="Wheat" />
                                    <Columns>
                                        <asp:BoundField DataField="team1" HeaderText="Visitor" />
                                        <asp:BoundField DataField="score1" HeaderText="Score" />
                                        <asp:BoundField DataField="team2" HeaderText="Home" SortExpression="home" />
                                        <asp:BoundField DataField="score2" HeaderText="Score" />
                                        <asp:BoundField DataField="recordedBy" HeaderText="Recorded By" />
                                        <asp:BoundField DataField="phoneContact" HeaderText="Phone" />
                                        <asp:BoundField DataField="comments" HeaderText="Comments" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <br />
        <div class="row">
            <div class="col-sm-12 text-center">
                <asp:Button ID="btnSave" CssClass="btn btn-outline-dark" Text="Save Results" runat="server" OnClick="btnSave_Click" />
            </div>
        </div>
    </div>
 </asp:Content>
