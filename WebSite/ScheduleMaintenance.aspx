<%@ Page Language="C#" MasterPageFile="~/MasterMain.Master" AutoEventWireup="true" CodeFile="ScheduleMaintenance.aspx.cs" Inherits="DistrictOne.ScheduleMaintenance" Title="Schedule Maintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:UpdatePanel ID="updatePanel1" runat="server">
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
                    <span style="font-size: 18px; color: #000066; font-family: Calibri">NJ District One Game Maintenance</span>
                </div>
            </div>
        </div>
        <br />
        <asp:Panel ID="pnlGameDisplay" runat="server">
            <div class="container">
                <div class="row border border-dark p-2">
                    <div class="col-sm-12">
                        <div class="form-group row">
                            <span class="col-sm-1 col-form-label">Division:</span>
                            <div class="col-sm-3">
                                <asp:DropDownList CssClass="dropdown" ID="dlDivision" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dlDivision_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <asp:CheckBoxList ID="ckPools" CssClass="list-group-horizontal" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ckPools_SelectedIndexChanged" RepeatDirection="Horizontal"></asp:CheckBoxList>
                            </div>
                        </div>
                        <div class="row d-flex justify-content-center">
                            <div class="col-sm-3 text-center">
                                <asp:Button CssClass="btn btn-outline-dark" ID="btnAddPool" runat="server" Text="Add Pool Game" OnClick="btnAddPool_Click" />
                            </div>
                            <div class="col-sm-3 text-center">
                                <asp:Button CssClass="btn btn-outline-dark" ID="btnAddBracket" runat="server" OnClick="btnAddBracket_Click" Text="Add Bracket Game" />
                            </div>
                            <div class="col-sm-3 text-center">
                                <asp:Button CssClass="btn btn-outline-danger" ID="btnImport" runat="server" OnClick="btnImport_Click" Text="Import Games" />
                            </div>
                            <div class="col-sm-3 text-center">
                                <asp:Button CssClass="btn btn-outline-info" ID="btnInterleague" runat="server" Text="Add Interleague Game" OnClick="btnAddInterleague_Click" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="table-responsive">
                                <asp:GridView ID="grdSchedule" CssClass="table table-hover table-bordered" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="grdSchedule_SelectedIndexChanged">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Report Results" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton CssClass="btn btn-outline-dark" ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Select"
                                                    Text="Edit" Enabled='<% # Bind("resultAllowed") %>'></asp:LinkButton>
                                                <asp:Label ID="RecordId" runat="server" Text='<% #Bind("id") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="gameDate" HeaderText="Game Date/Time" SortExpression="gameDate"></asp:BoundField>
                                        <asp:BoundField DataField="Division" HeaderText="Division" SortExpression="division"></asp:BoundField>
                                        <asp:BoundField DataField="poolPlay" HeaderText="Pool/Bracket"></asp:BoundField>
                                        <asp:BoundField DataField="visitor" HeaderText="Visitor" SortExpression="visitor" />
                                        <asp:BoundField DataField="home" HeaderText="Home" SortExpression="home" />
                                        <asp:BoundField DataField="location" HeaderText="Location" SortExpression="location" />
                                        <asp:BoundField DataField="updateComment" HeaderText="Last Update Comment"></asp:BoundField>
                                    </Columns>
                                    <AlternatingRowStyle BackColor="#FFC080" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlGameDetail" runat="server" Visible="False">
            <div class="container">
                <div class="row border border-dark p-2">
                    <div class="col-sm-12">
                        <div class="form-group row d-flex align-content-center">
                            <asp:Label CssClass="col-sm-1 form-label" ID="lblPoolBracket" runat="server" Text="Pool"></asp:Label>
                            <div class="col-sm-2">
                                <asp:DropDownList ID="dlPool" CssClass="form-text" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dlPool_SelectedIndexChanged"></asp:DropDownList>
                                <asp:TextBox ID="txtGameNumber" CssClass="form-text w-25" runat="server"></asp:TextBox>
                            </div>
                            <span class="col-sm-1 form-label">Date: </span>
                            <div class="col-sm-3">
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="MM/dd/yyyy" TargetControlID="txtGameDate"></ajaxToolkit:CalendarExtender>
                                <asp:TextBox ID="txtGameDate" CssClass="form-text" runat="server"></asp:TextBox>
                            </div>
                            <span class="col-sm-1 form-label">Time: </span>
                            <div class="col-sm-1">
                                <asp:TextBox ID="txtGameTime" CssClass="form-text" runat="server"></asp:TextBox>
                                <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlToValidate="txtGameTime" ErrorMessage="Invalid Time" Text="*" EmptyValueBlurredText="*" IsValidEmpty="false" EmptyValueMessage="Time is Required" ControlExtender="MaskedEditExtender1"></ajaxToolkit:MaskedEditValidator>
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtGameTime" AcceptAMPM="true" Mask="99:99" MaskType="time"></ajaxToolkit:MaskedEditExtender>
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label ID="lblTeam1" CssClass="col-sm-2" runat="server" Text="Team 1"></asp:Label>
                            <div class="col-sm-4">
                                <asp:DropDownList CssClass="form-text" ID="dlTeam1" runat="server"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rqTeam1" runat="server" ControlToValidate="dlTeam1" ErrorMessage="You need to select a team." InitialValue="-1">*</asp:RequiredFieldValidator>
                            </div>
                            <asp:Label CssClass="col-sm-2" ID="lblTeam2" runat="server" Text="Team 2"></asp:Label>
                            <div class="col-sm-4">
                                <asp:DropDownList CssClass="form-text" ID="dlTeam2" runat="server"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rqTeam2" runat="server" ControlToValidate="dlTeam2" ErrorMessage="You Must Select a Team" InitialValue="-1">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group row">
                            <span class="col-sm-2">Location: </span>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtLocation" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RqLocation" runat="server" ControlToValidate="txtLocation" ErrorMessage="Location is Required">*</asp:RequiredFieldValidator>
                            </div>
                            <span class="col-sm-2">Update Comment: </span>
                            <div class="col-sm-4">
                                <asp:TextBox CssClass="form-text" ID="txtUpdateComment" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row d-flex justify-content-center">
                            <div class="col-sm-4 text-center">
                                <asp:Button CssClass="btn btn-outline-dark" ID="btnSaveGame" runat="server" Text="Save" OnClick="btnSaveGame_Click" />
                            </div>
                            <div class="col-sm-4 text-center">
                                <asp:Button CssClass="btn btn-outline-danger" ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" />
                            </div>
                            <div class="col-sm-4 text-center">
                                <asp:Button CssClass="btn btn-outline-secondary" ID="btnCancelGame" runat="server" Text="Cancel" OnClick="btnCancelGame_Click" CausesValidation="False" />
                            </div>
                        </div>

                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12">
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
                    </div>
                </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
