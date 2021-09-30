<%@ Page Language="C#" MasterPageFile="~/masterMain.Master" AutoEventWireup="true" CodeFile="ScheduleImport.aspx.cs" Inherits="ScheduleImport" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <div class="row" style="background-color: lavender">
            <div class="col-sm-12 text-center">
                <span style="font-size: 24px; color: #FF0000">New Jersey District 1 Little League
                </span>
                <br />
                <span style="font-size: 18px; color: #000066; font-family: Calibri">District Administrator - Chris Graham
                </span>
                <br />
                <span style="font-size: 18px; color: #000066; font-family: Calibri">District Pool Game Import Wizard</span><br />
            </div>
        </div>
    </div>
    <br />
    <div class="container">
        <div class="row border border-dark">
            <div class="col-sm-12">
                <asp:MultiView ID="MultiView1" runat="server">
                    <asp:View ID="vwStep1" runat="server">
                        <div class="row">
                            <div class="col-sm-12 text-center p-3">
                                <asp:CheckBox CssClass="form-control-lg" ID="ckPurge" runat="server" TextAlign="Left" Text="Purge Existing Games? " />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-offset-3 col-sm-4">
                                <asp:FileUpload CssClass="form-control-file" ID="gameUpload" runat="server" />
                            </div>
                            <div class="col-sm-2">
                                <asp:RadioButtonList CssClass="custom-radio" ID="rbCategory" runat="server">
                                    <asp:ListItem Value="Pool" Selected="True">Pool</asp:ListItem>
                                    <asp:ListItem Value="Interleague">Interleague</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div class="col-sm-2">
                                <asp:RadioButtonList CssClass="custom-radio" ID="rbDelimiter" runat="server">
                                    <asp:ListItem Value="," Selected="True">comma (,)</asp:ListItem>
                                    <asp:ListItem Value="|">Pipe (|)</asp:ListItem>
                                    <asp:ListItem Value="\t">Tab (\t)</asp:ListItem>
                                    <asp:ListItem Value=";">SemiColon (;)</asp:ListItem>
                                    <asp:ListItem Value=":">Colon (:)</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div class="col-sm-2">
                                <asp:Button CssClass="btn btn-outline-primary" ID="btnStep1Next" runat="server" Text="Next >>" OnClick="btnStep1Next_Click" />
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="vwStep2" runat="server">
                        <div class="row">
                            <div class="col-sm-12 text-center">
                                <table class="table table-bordered table-hover" style="text-align: center">
                                    <asp:Label ID="lblImportData" runat="server" Text="Label"></asp:Label>
                                </table>
                            </div>
                        </div>
                        <div class="row">
                            <div class="offset-2 col-sm-8">
                                <asp:CheckBox CssClass="form-check-label" TextAlign="Left" ID="ckHeader" runat="server" Text="First Row Contains Headers: " />
                            </div>
                        </div>
                        <div class="row">
                            <div class="offset-4 col-sm-2">
                                <span class="h5">Pool Game Field</span> 
                            </div>
                            <div class="col-sm-6">
                                <span class="h5">Import Record Field Number (1 is first field etc...)</span>
                            </div>
                        </div>
                        <br />
                        <div class="form-group row">
                            <span class="offset-4 text-right col-sm-1 col-form-label-lg">Pool</span>
                            <div class="col-sm-4">
                                 <asp:TextBox CssClass="form-text" ID="txtPoolNumber" runat="server"></asp:TextBox>
                                <ajaxToolkit:NumericUpDownExtender ID="poolExtender" Width="75" runat="server" TargetControlID="txtPoolNumber" Minimum="0" Maximum="99"></ajaxToolkit:NumericUpDownExtender>
                             </div>
                        </div>
                        <div class="form-group row">
                            <span class="offset-4 text-right col-sm-1 col-form-label-lg">Game Date</span>
                            <div class="col-sm-4">
                                    <asp:TextBox CssClass="form-text" ID="txtGameDateNumber" runat="server"></asp:TextBox>
                                    <ajaxToolkit:NumericUpDownExtender ID="gameDateExtender" runat="server" Width="75" TargetControlID="txtGameDateNumber" Minimum="0" Maximum="99"></ajaxToolkit:NumericUpDownExtender>
                             </div>
                        </div>
                        <div class="form-group row">
                            <span class="offset-4 text-right col-sm-1 col-form-label-lg">Game Time</span>
                            <div class="col-sm-4">
                                    <asp:TextBox CssClass="form-text" ID="txtGameTimeNumber" runat="server"></asp:TextBox>
                                    <ajaxToolkit:NumericUpDownExtender ID="gameTimeExtender" runat="server" Width="75" TargetControlID="txtGameTimeNumber" Minimum="0" Maximum="99"></ajaxToolkit:NumericUpDownExtender>
                             </div>
                        </div>
                        <div class="form-group row">
                            <span class="offset-4 text-right col-sm-1 col-form-label-lg">Visitor</span>
                            <div class="col-sm-4">
                                    <asp:TextBox CssClass="form-text" ID="txtVisitorNumber" runat="server"></asp:TextBox>
                                    <ajaxToolkit:NumericUpDownExtender ID="visitorExtender" runat="server" Width="75" TargetControlID="txtVisitorNumber" Minimum="0" Maximum="99"></ajaxToolkit:NumericUpDownExtender>
                             </div>
                        </div>
                        <div class="form-group row">
                            <span class="offset-4 text-right col-sm-1 col-form-label-lg">Home</span>
                            <div class="col-sm-4">
                                    <asp:TextBox CssClass="form-text" ID="txtHomeNumber" runat="server"></asp:TextBox>
                                    <ajaxToolkit:NumericUpDownExtender ID="homeExtender" runat="server" Width="75" TargetControlID="txtHomeNumber" Minimum="0" Maximum="99"></ajaxToolkit:NumericUpDownExtender>
                             </div>
                        </div>
                        <div class="form-group row">
                            <span class="offset-4 text-right col-sm-1 col-form-label-lg">Location</span>
                            <div class="col-sm-4">
                                    <asp:TextBox CssClass="form-text" ID="txtLocationNumber" runat="server"></asp:TextBox>
                                    <ajaxToolkit:NumericUpDownExtender ID="locationExtender" runat="server" Width="75" TargetControlID="txtLocationNumber" Minimum="0" Maximum="99"></ajaxToolkit:NumericUpDownExtender>
                             </div>
                        </div>
                        <div class="row">
                            <div class="offset-3 col-sm-3 text-center">
                                <asp:Button CssClass="btn btn-outline-warning" ID="btnBack1" runat="server" OnClick="btnBack3_Click" Text="Back" />
                            </div>
                            <div class="col-sm-3 text-center">
                                <asp:Button CssClass="btn btn-outline-primary" ID="btnStep2Next" runat="server" Text="Next" OnClick="btnStep2Next_Click" />
                            </div>
                        </div>
                         <br />
                     </asp:View>
                    <asp:View ID="vwStep3" runat="server">
                        <div class="container">
                            <div class="row border border-dark">
                                <div class="col-sm-12">
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <asp:GridView CssClass="table table-hover" ID="grdImportedGames" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                CellPadding="5" CellSpacing="5" HorizontalAlign="Center">
                                                <Columns>
                                                    <asp:BoundField DataField="pool" HeaderText="Pool" />
                                                    <asp:BoundField DataField="gameDate" HeaderText="Game Date" />
                                                    <asp:BoundField DataField="team1" HeaderText="Team 1" />
                                                    <asp:BoundField DataField="team2" HeaderText="Team 2" />
                                                    <asp:BoundField DataField="location" HeaderText="Location" />
                                                </Columns>
                                                <PagerSettings Mode="NextPrevious" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="offset-3 col-sm-3 text-center">
                                            <asp:Button CssClass="btn btn-outline-danger" ID="btnBack3" runat="server" Text="Back" OnClick="btnBack3_Click" />
                                        </div>
                                        <div class="col-sm-3 text-center">
                                            <asp:Button CssClass="btn btn-outline-success" ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save Games" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>
    </div>
</asp:Content>